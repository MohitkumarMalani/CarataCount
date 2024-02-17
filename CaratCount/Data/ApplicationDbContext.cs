using System.Net;
using CaratCount.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CaratCount.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser> userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string email = "admin@caratcount.com";
            string password = "Admin@159#";
            string roleName = "Admin";
            string phoneNumber = "1234567890";
            string gstInNo = "22AAAAA0000A1Z5";

            // if role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // if username doesn't exist, create it and add it to role
            if (await userManager.FindByEmailAsync(email) == null)
            {
                ApplicationUser user = new ApplicationUser { UserName= roleName,Email = email, PhoneNumber = phoneNumber, GstInNo = gstInNo };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        public DbSet<GstInDetail> GstInDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GstInDetail>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<ApplicationUser>()
                 .HasMany(u => u.GstInDetails)
                 .WithOne(g => g.User)
                 .HasForeignKey(g => g.UserId)
                 .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
