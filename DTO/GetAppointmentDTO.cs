using VezetaApi.Models;

namespace VezetaApi.DTO
{
    public class GetAppointmentDTO
    {
        public Days Day { get; set; }
        public List<GetTimeDTO> Times { get; set; }
    }
}
