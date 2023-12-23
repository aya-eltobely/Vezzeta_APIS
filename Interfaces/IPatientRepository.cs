using VezetaApi.Helper;
using VezetaApi.Models;

namespace VezetaApi.Interfaces
{
    public interface IPatientRepository : IBaseRepository<ApplicationUser>
    {
        PageResult<ApplicationUser> GetAll(int pagenumber, int pagesize, string includeProperties, string search);

        ApplicationUser GetById(string id);//, string includeProperties);
    }
}
