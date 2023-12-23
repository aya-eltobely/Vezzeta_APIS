using VezetaApi.Helper;
using VezetaApi.Models;

namespace VezetaApi.Interfaces
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        PageResult<Booking> GetAll(int pagenumber, int pagesize, string includeProperties, int DocId ,string search=null);

    }
}
