using System.ComponentModel.DataAnnotations;

namespace VezetaApi.DTO
{
    public class LoginUseDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
