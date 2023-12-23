using VezetaApi.Models;

namespace VezetaApi.Interfaces
{
    public interface ITimeRepository : IBaseRepository<Times>
    {
        Times GetByTime(string time);
        Times GetByAppointmentId(int id);
    }
}
