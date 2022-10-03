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
        public DbSet<Role> Role { get; set; }
        public DbSet<Profile> Profile { get; set; }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<ContactProject> ContactProject { get; set; }


        public DbSet<UserProject> UserProject { get; set; }
        public DbSet<Project> Projects { get; set; }


        public DbSet<Week> Week { get; set; }
        public DbSet<DocHour> DocHour { get; set; }

        public DbSet<BusinessTrip> BusinessTrip { get; set; }
        public DbSet<ReportCheck> ReportCheck { get; set; }
        public DbSet<CheckPlane> CheckPlane { get; set; }
        public DbSet<CheckTrain> CheckTrain { get; set; }
        public DbSet<CheckHostel> CheckHostel { get; set; }
        


        public Context(DbContextOptions<Context> options)
        : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add the shadow property to the model
            modelBuilder.Entity<DocHour>().HasOne(p => p.Week);

            modelBuilder.Entity<Week>().HasOne(p => p.MoHour);
            modelBuilder.Entity<Week>().HasOne(p => p.ThHour);
            modelBuilder.Entity<Week>().HasOne(p => p.TuHour);
            modelBuilder.Entity<Week>().HasOne(p => p.WeHour);
            modelBuilder.Entity<Week>().HasOne(p => p.FrHour);
            modelBuilder.Entity<Week>().HasOne(p => p.SaHour);
            modelBuilder.Entity<Week>().HasOne(p => p.SuHour);



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
            });


            modelBuilder.Entity<Profile>().HasData(
           new Profile[]
           {
                new Profile{
                    Id = 1,
                    Sex = false,
                    Date = new DateTime(1994,08,18),
                    PrfSeries = 352,
                    PrfNumber = 5252425,
                    PrfDateTaked = new DateTime(2014,09,03),
                    PrfDateBack = null,
                    PrfCode = 235235,
                    PrfTaked = "Отделом УФМС РОССИИ ПО КРАСНОЯСРКОМУ КРАЮ В СОВЕТСКОМ Р-НЕ Г.КРАСНОЯСРКА",
                    PrfPlaceBorned = "ГОР. МИНСК БЕЛАРУСЬ",
                    PrfPlaceRegistration = "Россия, г. Красняосрк, ул. Урванецва, д. 6А, кв. 345",
                    PrfPlaceLived = "Россия, г. Красняосрк, ул. Урванецва, д. 6А, кв. 35",
                    IpNumber = 1111,
                    IpDateTaked = new DateTime(),
                    IpDateBack = new DateTime(),
                    IpCode = 111,
                    IpTaked = "МВД 24003",
                    IpPlaceBorned = "Гор. КРАСНОЯСРК / RUSSIA",
                    UlmNumber = 111,
                    UlmDateTaked = new DateTime(),
                    UlmDateBack = new DateTime(),
                    UlmCode = 111,
                    UlmTaked = "МВД 345",
                    UlmPlaceBorned = "Гор. КРАСНОЯСРК / RUSSIA",
                    Snils = "1111",
                    Inn = "1111",
                    Phone = "43532352235",
                    PhotoName = "ava",
                },
           });

            modelBuilder.Entity<User>().HasData(
            new User[]
            {
                new User{
                    Id =1,
                    FirstName= "Сергей",
                    SecondName = "Смоглюк",
                    MiddleName = "Юрьевич",
                    Email = "xok",
                    Password = BCrypt.Net.BCrypt.HashPassword("123"),
                    ProfileId = 1,
                    CompanyId = 1,
                    RoleId = 1,
            },
            });



              modelBuilder.Entity<CheckPlane>().HasData(
              new CheckPlane
              {
                  Id = 1,
                  Value = 100,
                  TicketPhotoName = "tiket",
              });

              modelBuilder.Entity<CheckTrain>().HasData(
              new CheckTrain
              {
                  Id = 2,
                  Value = 70,
                  BorderTicketPhotoName = "train",
              });

              modelBuilder.Entity<CheckHostel>().HasData(
              new CheckHostel
              {
                  Id = 3,
                  Value = 50,
                  BillPhotoName = "bill",
              });



              modelBuilder.Entity<ReportCheck>().HasData(
              new ReportCheck[]
              {
                 new ReportCheck
                 {
                     Id = 4,
                     Value = 1,
                     CheckBankPhotoName = "bank"
                 }
              }
              );

            /*
            modelBuilder.Entity<BusinessTrip>().HasData(
            new BusinessTrip[]
            {
               new BusinessTrip
               {
                   Id = 1,
                   UserProjectId = 1,
                   ReportChecks = new System.Collections.Generic.List<ReportCheck>
                   {
                       new CheckPlane
                       {
                           // Id = 1,
                            Value = 100,
                            TicketPhotoName = "tiket",
                       },
                       new CheckTrain
                       {
                           // Id = 2,
                            Value = 70,
                            BorderTicketPhotoName = "train",
                       },
                       new CheckHostel
                       {
                            //Id = 3,
                            Value = 50,
                            BillPhotoName = "bill",
                       },
                       new ReportCheck
                       {
                           //Id = 4,
                           Value = 1,
                           CheckBankPhotoName = "bank"
                       },
                   }
               }
            }
            );*/

        }


        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
