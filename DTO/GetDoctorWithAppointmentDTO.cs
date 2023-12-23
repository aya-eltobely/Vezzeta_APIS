
using VezetaApi.Models;

namespace VezetaApi.DTO
{
    public class GetDoctorWithAppointmentDTO : GetDoctorDTO
    {
        public decimal Price { get; set; }
        //public Days Day { get; set; }
        public List<GetAppointmentDTO> appointment { get; set; }

    }
}
