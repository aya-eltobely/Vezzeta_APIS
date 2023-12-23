using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace VezetaApi.Models
{
    public class ApplicationUser : IdentityUser
    {
       

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


        ////

        public List<Doctor> Doctors { get; set; }


        //

        public int? CouponId { get; set; }
        [ForeignKey("CouponId")]
        public Coupon Coupon { get; set; }

        ////

        public List<Booking> Bookings { get; set; }

        //

        public int? UserImageId { get; set; }
        [ForeignKey("UserImageId")]
        public Image UserImage { get; set; }

        //
        public List<Feedback> Feedbacks { get; set; }

    }
}
