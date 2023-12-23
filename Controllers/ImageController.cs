﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VezetaApi.Models;

namespace VezetaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ImageController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }




        [HttpPost( Name = "UploadImage")]
        public IActionResult UploadImage(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                var image = new Image { FileName = file.FileName, ImageData = memoryStream.ToArray() };

                _dbContext.Images.Add(image);
                _dbContext.SaveChanges();

                //return image.Id;
                return Ok(new { image.Id, image.FileName });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetImage(int id)
        {
            var image = _dbContext.Images.FirstOrDefault(i => i.Id == id);

            if (image == null)
                return NotFound();

            return File(image.ImageData, "image/jpeg"); // Adjust content type based on your image type
        }
    }
}
