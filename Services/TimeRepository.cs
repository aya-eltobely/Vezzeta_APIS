using Microsoft.AspNetCore.Identity;
using VezetaApi.Interfaces;
using VezetaApi.Models;

namespace VezetaApi.Services
{
    public class TimeRepository : BaseRepository<Times>, ITimeRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public TimeRepository(ApplicationDbContext Context) : base(Context)
        {
        }

        public Times GetByAppointmentId(int id)
        {
            return Context.Times.Where(t => t.OppintmentId == id).FirstOrDefault();
        }

        public Times GetByTime(string time)
        {
            Times times = Context.Times.FirstOrDefault(o => o.Time == time);
            return times;
        }
    }
}
