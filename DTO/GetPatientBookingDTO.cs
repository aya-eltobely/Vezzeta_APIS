using VezetaApi.Models;

namespace VezetaApi.DTO
{
    public class GetPatientBookingDTO
    {
        public int DoctorImageId { get; set; }
        public string DoctorName { get; set; }
        public string SpecializationName { get; set; }
        public Days Day { get; set; }
        public string Time { get; set; }
        public decimal Price { get; set; }
        public string DiscoundCode { get; set; }
        public decimal FinalPrice { get; set; }
        public RequestStatus Status { get; set; }
    }
}
