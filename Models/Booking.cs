using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace VezetaApi.Models
{
    [Index(nameof(OppintmentId), IsUnique = false)]
    public class Booking
    {

        public int Id { get; set; }

        [Required]
        public RequestStatus RequestStatus { get; set; }



        /// 


        public string? userId { get; set; }
        [ForeignKey("userId")]
        public ApplicationUser user { get; set; }

        ////

        public int? CouponId { get; set; }
        [ForeignKey("CouponId")]
        public Coupon Coupon { get; set; }

        ///// 


        public int? DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }


        ////

        public int? OppintmentId { get; set; }
        [ForeignKey("OppintmentId")]
        public Oppintment Oppintment { get; set; }


        ////

        public int? TimeId { get; set; }
        [ForeignKey("TimeId")]
        public Times Time { get; set; }


        /// 

        public int? SpecializationId { get; set; }
        [ForeignKey("SpecializationId")]
        public Specialization Specialization { get; set; }


    }
}
