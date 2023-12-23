using VezetaApi.Models;

namespace VezetaApi.DTO
{
    public class PatientsRequestsDTO
    {
        public int DoctorImageId { get; set; }
        public string DoctorName { get; set; }
        public string SpecalizeName { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; } //after discount
        public string discoundCode { get; set; }
        public string status { get; set; }

    }
}
