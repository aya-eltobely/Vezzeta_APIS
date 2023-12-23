using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VezetaApi.Models
{
    public class Oppintment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Days Days { get; set; }


        ///
        /// 
        public int? DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        ////
        public Booking Booking { get; set; }


        public List<Times> Times { get; set; }
    }
}
