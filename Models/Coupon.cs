using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace VezetaApi.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        [Required]
        public string CouponCode { get; set; }

        [Required]
        public int NumOfBooking { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; }

        [Required]
        public int DiscountValue { get; set; }

        [Required]
        public int IsActieve { get; set; }



        /// 
        public List<ApplicationUser> users { get; set; }

        ////


        public List<Booking> Bookings { get; set; }
    }
}
