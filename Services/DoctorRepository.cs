using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VezetaApi.Helper;
using VezetaApi.Interfaces;
using VezetaApi.Models;

namespace VezetaApi.Services
{
    public class DoctorRepository : BaseRepository<Doctor> ,IDoctorRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public DoctorRepository(ApplicationDbContext Context, UserManager<ApplicationUser> _userManager) : base(Context)
        {
            userManager = _userManager;
        }


        public PageResult<Doctor> GetAll(int pagenumber, int pagesize, string? includeProperties, string? search)
        {
            List<Doctor> data = new List<Doctor>();
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
                 

                data = query.Skip(ExcludedData).Take(pagesize).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(d => d.user.FirstName.ToLower().Contains(search.ToLower())).ToList();
                }

                totalCount = Set.ToList().Count;
            }

            catch (Exception)
            {

                throw;
            }

            return new PageResult<Doctor>()
            {
                Data = data,
                PageNumber = pagenumber,
                PageSize = pagesize,
                TotalItem = totalCount
            };
        }


        public Doctor GetById(int id, string includeProperties)
        {
            var doctor = Context.Doctors.AsQueryable();
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                doctor = doctor.Include(includeProp);
            }
            return doctor.Where(d => d.Id == id).FirstOrDefault();
        }

       
    }
}
