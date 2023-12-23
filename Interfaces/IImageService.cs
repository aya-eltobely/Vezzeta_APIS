using Microsoft.AspNetCore.Mvc;

namespace VezetaApi.Interfaces
{
    public interface IImageService
    {
        int UploadImage(IFormFile? file);
    }
}
