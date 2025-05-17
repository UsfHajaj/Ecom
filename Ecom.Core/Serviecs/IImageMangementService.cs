using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Serviecs
{
    public interface IImageMangementService
    {
        Task<List<string>> AddImageAsync(IFormFileCollection files,string src);
        void DeleteImageAsync(string src);
    }
}
