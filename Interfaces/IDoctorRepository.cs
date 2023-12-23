using VezetaApi.Helper;
using VezetaApi.Models;

namespace VezetaApi.Interfaces
{
    public interface IDoctorRepository : IBaseRepository<Doctor>
    {

        PageResult<Doctor> GetAll(int pagenumber, int pagesize, string includeProperties, string search);

        Doctor GetById(int id, string includeProp);

    }
}
