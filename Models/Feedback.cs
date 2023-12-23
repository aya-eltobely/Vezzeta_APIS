using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace VezetaApi.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }


        /// 
        public int? DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }


        //
        /// 
        public string? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public ApplicationUser AppUser { get; set; }
    }
}
