using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using VezetaApi.Models;

namespace VezetaApi.DTO
{
    public class RegisterUserDTO
    {
        //[Required]
        //[JsonIgnore]
        //[BindNever]
        //[IgnoreDataMember]
        //[HiddenInput(DisplayValue = false)]
        //public int ImageId { get; set; }

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
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
