using VezetaApi.Models;

namespace VezetaApi.DTO
{
    public class AddAppointmentDTO
    {
        public decimal Price { get; set; }
        public Days Day { get; set; }
        public List<string> Times { get; set; }
    }
}
