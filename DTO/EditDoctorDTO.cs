using System.ComponentModel.DataAnnotations;
using VezetaApi.Models;

namespace VezetaApi.DTO
{
    public class EditDoctorDTO
    {
        [Required]
        public int ImageId { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z]+$")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z]+$")]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public int SpecializationId { get; set; }
    }
}
