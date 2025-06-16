using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.DTOs;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecom.Api.Controllers
{
    
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var result = await work.Auth.RegisterAsync(registerDTO);
            if (result != "done")
            {
                return BadRequest(new ResponseAPI(400,result));
            }
            return Ok(new ResponseAPI(200,result));
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            
            var result=await work.Auth.LoginAsync(loginDTO);

            if (result.StartsWith("Plese"))
            {
                return BadRequest(new ResponseAPI(400,result));
            }

            Response.Cookies.Append("token", result,new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(1)
            });
            return Ok(new
            {
                token = result,
                expiration = DateTime.UtcNow.AddDays(1),
            });
        }
        [HttpPost("active-account")]
        public async Task<IActionResult> ActiveAccount(ActiveAccountDTO activeAccountDTO)
        {
            var result = await work.Auth.ActiveAccount(activeAccountDTO);
            if (!result)
            {
                return BadRequest(new ResponseAPI(400));
            }
            
            return Ok(new ResponseAPI(200));
        }
        [HttpGet("send-email-password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var result=await work.Auth.SendEmailForForgetPassword(email);
            if (!result)
            {
                return BadRequest(new ResponseAPI(400));
            }
            return Ok(new ResponseAPI(200));

        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var result = await work.Auth.ResetPassword(resetPasswordDTO);
            if (result == "done")
            {
                return Ok(new ResponseAPI(200));
            }
            return BadRequest(new ResponseAPI(400));
        }

        [Authorize]
        [HttpPut("update-address")]
        public async Task<IActionResult> updateAddress(ShipAddressDTO addressDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var address = mapper.Map<Address>(addressDTO);
            var result = await work.Auth.UpdateAddress(email, address);
            return result ? Ok(new ResponseAPI(200)) : BadRequest();
        }

        [Authorize]
        [HttpGet("get-address-for-user")]
        public async Task<IActionResult> GetUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address=await work.Auth.GetAddress(email);
            var result=mapper.Map<ShipAddressDTO>(address);

            return Ok(result);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append("token", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(-1)
            });

            return Ok(new { message = "Logged out successfully" });
        }
    }
}
