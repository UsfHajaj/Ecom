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
        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        { 
            var saveImageSrc = new List<string>();
            var imageDirectory = Path.Combine("wwwroot","Images", src);
            if (Directory.Exists(imageDirectory) is not true)
            {
                Directory.CreateDirectory(imageDirectory);
            }

            foreach (var item in files)
            {
                if (item.Length>0)
                {
                    var imageName=item.FileName;
                    var imageSrc = $"Images/{src}/{imageName}";
                    var root= Path.Combine(imageDirectory, imageName);
                    using(FileStream stream = new FileStream(root, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                        
                    }
                    saveImageSrc.Add(imageSrc);

                }
            }
            return saveImageSrc;
        }

        public void DeleteImageAsync(string src)
        {
            var info = fileProvider.GetFileInfo(src);
            var root=info.PhysicalPath;
            File.Delete(root);
        }
    }
}
