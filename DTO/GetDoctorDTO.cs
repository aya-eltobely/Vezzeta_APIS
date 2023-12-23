using VezetaApi.Models;

namespace VezetaApi.DTO
{
    public class GetDoctorDTO
    {
        public int ImageId { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string SpecializeName { get; set; }
        public Gender Gender { get; set; }

    }
}
