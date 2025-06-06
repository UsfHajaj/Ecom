using Ecom.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Serviecs
{
    public interface IEmailService
    {
        Task SendEmail(EmailDTO email);

    }
}
