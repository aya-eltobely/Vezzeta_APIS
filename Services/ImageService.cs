using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VezetaApi.Interfaces;
using VezetaApi.Models;

namespace VezetaApi.Services
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext Context;

        public ImageService(ApplicationDbContext _Context)
        {
            Context = _Context;
        }

        public int UploadImage(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return 0;

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                var image = new Image { FileName = file.FileName, ImageData = memoryStream.ToArray() };

                Context.Images.Add(image);
                Context.SaveChanges();

                return image.Id;
            }
        }



    }
}
