using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace VezetaApi.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options ):base( options ) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Oppintment> Oppintments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Times> Times { get; set; }
        public DbSet<Image> Images { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            // Configure Identity-related entities
            builder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            builder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            builder.Entity<IdentityUserClaim<string>>().HasKey(c => c.Id);
            builder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            builder.Entity<IdentityRoleClaim<string>>().HasKey(rc => rc.Id);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////
            

            builder.Entity<ApplicationUser>().HasMany(u => u.Doctors).WithOne(d => d.user);
            builder.Entity<ApplicationUser>().HasMany(u => u.Bookings).WithOne(d => d.user).HasForeignKey(b=>b.userId);


            builder.Entity<Doctor>().HasMany(u => u.Feedbacks).WithOne(d => d.Doctor);
            builder.Entity<Doctor>().HasMany(u => u.Bookings).WithOne(d => d.Doctor).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Doctor>().HasMany(u => u.Oppintments).WithOne(d => d.Doctor);

            builder.Entity<Specialization>().HasMany(u => u.Doctors).WithOne(d => d.Specialization);
            builder.Entity<Specialization>().HasMany(u => u.Bookings).WithOne(d => d.Specialization);

            builder.Entity<Coupon>().HasMany(u => u.users).WithOne(d => d.Coupon).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Coupon>().HasMany(u => u.Bookings).WithOne(d => d.Coupon);

            builder.Entity<Oppintment>().HasOne(u => u.Booking).WithOne(d => d.Oppintment).HasForeignKey<Booking>(d=>d.OppintmentId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Oppintment>().HasMany(u => u.Times).WithOne(d => d.Oppintment);

            builder.Entity<Times>().HasMany(u => u.Booking).WithOne(d => d.Time).OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<Image>().HasOne(u => u.Doctor).WithOne(d => d.Image).HasForeignKey<Doctor>(i => i.ImageId);//.OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Image>().HasOne(u => u.user).WithOne(d => d.UserImage).HasForeignKey<ApplicationUser>(i => i.UserImageId);//.OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Feedback>().HasOne(u => u.AppUser).WithMany(d => d.Feedbacks).HasForeignKey(f=>f.AppUserId).OnDelete(DeleteBehavior.NoAction);





        }
    }
}
