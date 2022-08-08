using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nts_platform_server.Entities;

namespace nts_platform_server.Data
{
    public class Context : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }


        public DbSet<Contact> Contact { get; set; }
        public DbSet<ContactProject> ContactProject { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserProject> UserProject { get; set; }

        public DbSet<Week> Week { get; set; }
        public DbSet<DocHour> DocHour { get; set; }


        public Context(DbContextOptions<Context> options)
        : base(options)
        {
           // Database.EnsureDeleted();
           // Database.EnsureCreated();
        }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add the shadow property to the model
            modelBuilder.Entity<DocHour>()
                .HasOne(p=> p.Week);


            modelBuilder.Entity<Week>()
               .HasOne(p => p.MoHour);

            modelBuilder.Entity<Week>()
               .HasOne(p => p.ThHour);

            modelBuilder.Entity<Week>()
              .HasOne(p => p.TuHour);

            modelBuilder.Entity<Week>()
             .HasOne(p => p.WeHour);

            modelBuilder.Entity<Week>()
             .HasOne(p => p.FrHour);

            modelBuilder.Entity<Week>()
            .HasOne(p => p.SaHour);

            modelBuilder.Entity<Week>()
            .HasOne(p => p.SuHour);



            /*
           modelBuilder.Entity<Role>().HasData(
           new Role[]
           {
                new Role{Id =1,Title= "admin"},
                new Role{Id =2,Title= "engineer"},
                new Role{Id =3,Title= "guest"}
           });

          modelBuilder.Entity<Company>().HasData(
          new Company[]
          {
                new Company{Id =1,Name= "NTS"},
          });*/

        }



        /*
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }*/
    }
}
