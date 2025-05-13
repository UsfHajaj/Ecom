using Ecom.Core.Serviecs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries.Services
{
    public class ImageMangementService : IImageMangementService
    {
        private readonly IFileProvider fileProvider;

        public ImageMangementService(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }
        public Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            throw new NotImplementedException();
        }

        public Task DeleteImageAsync(string src)
        {
            throw new NotImplementedException();
        }
    }
}
