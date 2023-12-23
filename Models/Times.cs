using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VezetaApi.Models
{
    public class Times
    {
        public int Id { get; set; }

        
        [Required]
        //public DateTime Time { get; set; }
        public string Time { get; set; }


        ///

        public int? OppintmentId { get; set; }
        [ForeignKey("OppintmentId")]
        public Oppintment Oppintment { get; set; }


        public List<Booking> Booking { get; set; }




    }
}
