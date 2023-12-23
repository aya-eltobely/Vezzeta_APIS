using Microsoft.AspNetCore.Identity;
using VezetaApi.Interfaces;
using VezetaApi.Models;

namespace VezetaApi.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext Context;
        private readonly UserManager<ApplicationUser> userManager;

        public IDoctorRepository Doctor { get; set; }
        public IPatientRepository AppUser { get; set; }
        public IBaseRepository<Coupon> Coupon { get; set; }
        public IBookingRepository Booking { get; set; }
        public IBaseRepository<Oppintment> Oppintment { get; set; }
        public IBaseRepository<Specialization> Specialization { get; set; }
        public IBaseRepository<Image> Image { get; set; }
        public IBaseRepository<Feedback> Feedback { get; set; }
        public ITimeRepository Times { get; set; }


        public UnitOfWork(ApplicationDbContext context,UserManager<ApplicationUser> _userManager)
        {
            Context = context;
            userManager = _userManager;
            Doctor = new DoctorRepository(Context,userManager);
            AppUser = new PatientRepository(Context, userManager);
            Coupon = new BaseRepository<Coupon>(Context);
            Booking = new BookingRepository(Context);
            Oppintment = new BaseRepository<Oppintment>(Context);
            Specialization = new BaseRepository<Specialization>(Context);
            Image = new BaseRepository<Image>(Context);
            Feedback = new BaseRepository<Feedback>(Context);
            Times = new TimeRepository(Context);
        }


        public void Dispose()
        {
            Context.Dispose();
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        
    }
}
