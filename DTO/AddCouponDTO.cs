using System.ComponentModel.DataAnnotations;
using VezetaApi.Models;

namespace VezetaApi.DTO
{
    public class AddCouponDTO
    {
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
    }
}
