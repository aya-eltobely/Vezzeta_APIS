using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VezetaApi.Models
{
    public class Specialization
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        ///
        public List<Doctor> Doctors { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}
