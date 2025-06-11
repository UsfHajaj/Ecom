using Ecom.Core.DTOs;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Ecom.Core.Serviecs;
using Ecom.Core.Sharing;
using Ecom.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Ecom.infrastructure.Repositries
{
    public class AuthRepositry:IAuth
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken generatetoken;
        private readonly AppDbContext _context;

        public AuthRepositry(UserManager<AppUser> userManager,IEmailService emailService,SignInManager<AppUser> signInManager,IGenerateToken generatetoken,AppDbContext context)
        {
            _userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.generatetoken = generatetoken;
            _context = context;
        }
        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            if(registerDTO == null)
            {
                return null;
            }
            var existingUser=await _userManager.FindByNameAsync(registerDTO.UserName);
            if(existingUser is not null)
            {
                return "This User Name Is Already Registered";
            }
            var existingEmail = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingEmail is not null)
            {
                return "This Email Is Already Registered";
            }
            AppUser user = new AppUser()
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                DisplayName=registerDTO.DisplayName

            };
            var result=await _userManager.CreateAsync(user,registerDTO.Password);
            if (result.Succeeded is not true)
            {
                return result.Errors.ToList()[0].Description;
            }
            string token =await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email,token,"active","ActiveEmail","Plese Active Your Email");
            return "done";
        }

        public async Task SendEmail(string email,string code,string component,string subject,string message)
        {
            var result=new EmailDTO(email,
                "yosefhgag23@gmail.com",
                subject,
                EmailStringBody.Send(email,code,component,message));
            await emailService.SendEmail(result);
        }
        public async Task<string> LoginAsync(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return null;
            }
                
            var findUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (findUser == null)
            {
                return null;
            }
            if (!findUser.EmailConfirmed)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(findUser);
                await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Plese Active Your Email");
                return "Plese Confirm Your Email First";
            }
            var result=await signInManager.CheckPasswordSignInAsync(findUser, loginDTO.Password,true);
            if (result.Succeeded)
            {
                return generatetoken.GetAndCreateToken(findUser);
            }
            return "Plese check Your Email And Pasword";
        }

        public async Task<bool> SendEmailForForgetPassword(string email)
        {
            var findUser = await _userManager.FindByEmailAsync(email);
            if(findUser is null)
            {
                return false;
            }
            var token=await _userManager.GeneratePasswordResetTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "reset-password", "Reset Password", "Rest Your Password");
            return true;

        }

        public async Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var findUser = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if(findUser is null)
            {
                return null;
            }
            var result=await _userManager.ResetPasswordAsync(findUser,resetPasswordDTO.Token,resetPasswordDTO.Password);
            if(result.Succeeded)
            {
                return "done";
            }
            return result.Errors.ToList()[0].Description; 
        }

        public async Task<bool> ActiveAccount(ActiveAccountDTO accountDTO)
        {
            var findUser = await _userManager.FindByEmailAsync(accountDTO.Email);
            if( findUser is null)
            {
                return false;
            }
            var result = await _userManager.ConfirmEmailAsync(findUser,accountDTO.Token);
            if (result.Succeeded)
            {
                return true;
            }
            var token=await _userManager.GenerateEmailConfirmationTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Plese Active Your Email");
            return false;
        } 

        public async Task<bool> UpdateAddress(string email, Address address)
        {
            var findUser = await _userManager.FindByEmailAsync(email);
            if (findUser is null)
            {
                return false;
            }
            var Myaddress = await _context.Addresses
                .FirstOrDefaultAsync(m => m.AppUserId == findUser.Id);

            if (Myaddress is null)
            {
                address.AppUserId = findUser.Id;
                await _context.Addresses.AddAsync(address);
            }
            else
            {
                _context.Entry(Myaddress).State = EntityState.Detached;
                address.Id = Myaddress.Id;
                address.AppUserId = Myaddress.AppUserId;
                _context.Addresses.Update(address);

            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Address> GetAddress(string email)
        {
            var findUser = await _userManager.FindByEmailAsync(email);

            var address = await _context.Addresses
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AppUserId == findUser.Id);

            return address;
        }
    }
}
