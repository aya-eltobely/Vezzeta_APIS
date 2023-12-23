using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VezetaApi.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }


        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        /// 

        
        public string? userId { get; set; }
        [ForeignKey("userId")]
        public ApplicationUser user  { get; set; }




        public int? SpecializationId { get; set; }
        [ForeignKey("SpecializationId")]
        public Specialization Specialization { get; set; }



        public List<Feedback> Feedbacks { get; set; }


        ////

        public List<Oppintment> Oppintments { get; set; }



        public List<Booking> Bookings { get; set; }



        //public int? ImageId { get; set; }
        //[ForeignKey("Image")]
        //public Image Image { get; set; }


    }
}
