using Ecom.Core.DTOs;
using Ecom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task SendEmail(string email, string code, string component, string subject, string message);
        Task<string> LoginAsync(LoginDTO loginDTO);
        Task<bool> SendEmailForForgetPassword(string email);
        Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO);
        Task<bool> ActiveAccount(ActiveAccountDTO accountDTO);
        Task<bool> UpdateAddress(string email, Address address);
        Task<Address> GetAddress(string email);


    }
}
