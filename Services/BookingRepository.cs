using Microsoft.EntityFrameworkCore;
using VezetaApi.Helper;
using VezetaApi.Interfaces;
using VezetaApi.Models;

namespace VezetaApi.Services
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public PageResult<Booking> GetAll(int pagenumber, int pagesize, string includeProperties, int DocId, string search = null)
        {
            List<Booking> data = new List<Booking>();
            //var data = userManager.GetUsersInRoleAsync(WebSiteRoles.SiteDoctor).GetAwaiter().GetResult();

            int totalCount;
            try
            {
                var query = Set.AsQueryable();

                var ExcludedData = (pagenumber * pagesize) - pagesize;

                if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp); //.Skip(ExcludedData).Take(pagesize);
                    }
                }

                data = query.Where(b => b.DoctorId == DocId).ToList(); //update


                data = data.Skip(ExcludedData).Take(pagesize).ToList();


                //data = query.Skip(ExcludedData).Take(pagesize).ToList();

                //data = data.Where(b => b.DoctorId == DocId).ToList(); //update

                //if (! (search == DateTime.MinValue))
                if (!string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(d => d.Time.Time == search).ToList();
                }

                totalCount = Set.ToList().Count;
            }

            catch (Exception)
            {

                throw;
            }

            return new PageResult<Booking>()
            {
                Data = data,
                PageNumber = pagenumber,
                PageSize = pagesize,
                TotalItem = totalCount
            };
        }

       
    }
}
