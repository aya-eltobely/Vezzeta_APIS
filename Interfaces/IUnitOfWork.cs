using VezetaApi.Models;

namespace VezetaApi.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDoctorRepository Doctor { get; }
        IPatientRepository AppUser { get; }
        IBaseRepository<Coupon> Coupon { get; }
        IBookingRepository Booking { get; }
        IBaseRepository<Oppintment> Oppintment { get; }
        IBaseRepository<Specialization> Specialization { get; set; }
        IBaseRepository<Image> Image { get; set; }
        IBaseRepository<Feedback> Feedback { get; set; }
        ITimeRepository Times { get; set; }



        void Save();
    }
}
