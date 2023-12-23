using VezetaApi.Models;

namespace VezetaApi.DTO
{
    public class AddPatientBookingDTO
    {
        public int TimeId { get; set; }
        public string discoundCodeCoupon { get; set; }
        public int DoctorId { get; set; }
        public string PatientId { get; set; }
        public Days Day { get; set; }
    }
}
