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

            // if role doesn't exist, create it
            if (await roleManager.FindByNameAsync("User") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // if username doesn't exist, create it and add it to role
            if (await userManager.FindByEmailAsync(email) == null)
            {
                ApplicationUser user = new ApplicationUser { UserName = roleName, Email = email, PhoneNumber = phoneNumber, GstInNo = gstInNo };
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
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<DiamondPacket> DiamondPackets { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<ProcessPrice> ProcessPrices { get; set; }
        public DbSet<DiamondPacketProcess> DiamondPacketProcesses { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<GstInDetail>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<Address>()
            .HasKey(a => a.Id);

            modelBuilder.Entity<DiamondPacket>()
            .HasKey(dp => dp.Id);

            modelBuilder.Entity<Employee>()
            .HasKey(e => e.Id);

            modelBuilder.Entity<Process>()
            .HasKey(p => p.Id);

            modelBuilder.Entity<ProcessPrice>()
            .HasKey(pp => pp.Id);  
            
            modelBuilder.Entity<DiamondPacketProcess>()
            .HasKey(dpp => dpp.Id);     
            
            modelBuilder.Entity<Invoice>()
            .HasKey(i => i.Id);

          modelBuilder.Entity<InvoiceItem>()
            .HasKey(i => i.Id);

     
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.GstInDetail)
                    .WithOne(g => g.User)
                .HasForeignKey<ApplicationUser>(a => a.GstInDetailId)
                .IsRequired(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Clients)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Employees)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Processes)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);   
            
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Invoices)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.GstInDetail)
                .WithOne(g => g.Client)
                .HasForeignKey<Client>(c => c.GstInDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Invoices)
                .WithOne(i => i.Client)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GstInDetail>()
                .HasOne(g => g.Address)
                .WithOne(a => a.GstInDetail)
                .HasForeignKey<GstInDetail>(g => g.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DiamondPacket>()
                .Property(dp => dp.CaratWeight)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<DiamondPacket>()
                .HasOne(dp => dp.Client)
                .WithMany(c => c.DiamondPackets)
                .HasForeignKey(dp => dp.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProcessPrice>()
            .HasOne(p => p.Process)
            .WithMany(p => p.ProcessPrices)
            .HasForeignKey(p => p.ProcessId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProcessPrice>()
               .Property(p => p.UserCost)
               .HasColumnType("decimal(10,2)"); 

            modelBuilder.Entity<ProcessPrice>()
               .Property(p => p.ClientCharge)
               .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<DiamondPacket>()
              .HasMany(dp => dp.DiamondPacketProcesses)
              .WithOne(dpp => dpp.DiamondPacket)
              .HasForeignKey(dpp => dpp.DiamondPacketId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
              .HasMany(e => e.DiamondPacketProcesses)
              .WithOne(dpp => dpp.Employee)
              .HasForeignKey(dpp => dpp.EmployeeId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DiamondPacketProcess>()
               .HasOne(dpp => dpp.Process)
               .WithMany() 
               .HasForeignKey(dp => dp.ProcessId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DiamondPacketProcess>()
              .HasOne(dpp => dpp.ProcessPrice)
              .WithMany()
              .HasForeignKey(dpp => dpp.ProcessPriceId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DiamondPacketProcess>()
               .Property(dpp => dpp.FinalCaratWeight)
               .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Invoice>()
               .HasMany(i => i.InvoiceItems)
               .WithOne(i => i.Invoice)
               .HasForeignKey(i => i.InvoiceId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InvoiceItem>()
               .HasOne(i => i.DiamondPacket)
               .WithOne()
               .HasForeignKey<InvoiceItem>(i => i.DiamondPacketId)
               .OnDelete(DeleteBehavior.NoAction);

        }


    }
}
