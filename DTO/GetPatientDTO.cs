using VezetaApi.Models;

namespace VezetaApi.DTO
{
    public class GetPatientDTO
    {
        public int ImageId { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
    }
}
