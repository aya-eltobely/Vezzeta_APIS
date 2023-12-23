using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VezetaApi.Helper;
using VezetaApi.Interfaces;
using VezetaApi.Models;

namespace VezetaApi.Services
{
    public class PatientRepository : BaseRepository<ApplicationUser>,  IPatientRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public PatientRepository(ApplicationDbContext Context,UserManager<ApplicationUser> _userManager) : base(Context)
        {
            userManager = _userManager;
        }


        public PageResult<ApplicationUser> GetAll(int pagenumber, int pagesize, string includeProperties, string search)
        {
            //List<ApplicationUser> data = new List<ApplicationUser>();
            var data = userManager.GetUsersInRoleAsync(WebSiteRoles.SitePatient).GetAwaiter().GetResult();
            int totalCount;
            try
            {
                var query = data.AsQueryable();
                //var query = Set.AsQueryable();

                var ExcludedData = (pagenumber * pagesize) - pagesize;

                if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp); //.Skip(ExcludedData).Take(pagesize);
                    }
                }
                

                data = query.Skip(ExcludedData).Take(pagesize).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(d => d.FirstName.ToLower().Contains(search.ToLower())).ToList();
                }

                totalCount = Set.ToList().Count;
            }

            catch (Exception)
            {

                throw;
            }

            return new PageResult<ApplicationUser>()
            {
                Data = (List<ApplicationUser>)data,
                PageNumber = pagenumber,
                PageSize = pagesize,
                TotalItem = totalCount
            };
        }

        public ApplicationUser GetById(string id)//, string includeProperties)
        {
            var patient = Context.ApplicationUsers.AsQueryable();
            ApplicationUser patient2 = userManager.FindByIdAsync(id).GetAwaiter().GetResult();

            var x = userManager.Users.Where(u =>

                 userManager.IsInRoleAsync(patient2, WebSiteRoles.SitePatient).GetAwaiter().GetResult()
            );

            //foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            //{
            //    patient = patient.Include(includeProp);
            //}
            return patient2;//.Where(d => d.Id == id).FirstOrDefault();
        }

        
    }
}
