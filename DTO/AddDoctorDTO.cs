using System.ComponentModel.DataAnnotations;
using VezetaApi.Models;

namespace VezetaApi.DTO
{
    public class AddDoctorDTO : RegisterUserDTO
    {
        //public IFormFile Image { get; set; }
        [Required]
        
        public int SpecializationId { get; set; }
    }
}
