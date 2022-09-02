using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using nts_platform_server.Algorithms;
using nts_platform_server.Auth.JWT;
using nts_platform_server.Data;
using nts_platform_server.Entities;
using nts_platform_server.Models;
using nts_platform_server.Services;

namespace nts_platform_server
{
    static class DBConnect
    {
        public static string _connectionString;
        public static DbContextOptions<Context> options;
    }


    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        private IServiceCollection _services;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*services.AddDbContext<DataContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));*/


            _services = services;

            services.AddControllers();
          

            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            DBConnect.options = optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)).Options;
            DBConnect._connectionString = Configuration.GetConnectionString("DefaultConnection");


            services.AddDbContext<Context>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IEfRepository<>), typeof(Repository<>));

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserProfile());
            });


            services.AddControllersWithViews()
                        .AddNewtonsoftJson(options =>
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );



            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc();

            services.AddCors();

  
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sailora WEB API API", Version = "v1" });
            });

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IDocHourseService, DocHourseService>();
            services.AddTransient<IProjectService, ProjectService>();

            services.AddCors();
            services.AddControllers();


           //test()

        }


        private void test()
        {
            //test algorithm QuickSort
            int[] inputArray = {
                105, 961, 201, 845, 950,178, 782, 989, 364,984,849, 783, 440, 357, 228,
                703, 322, 604, 381, 413,362, 711,639, 690, 744,649, 278, 731, 642, 760,
                481, 864, 525, 570, 232,959, 219, 269, 776, 728,379, 567, 673, 195, 551,
                95, 440, 296, 258, 631,700, 309, 652, 548, 775,156, 16, 558, 180, 439,
                404, 134, 177, 676, 127,622, 777, 718, 964, 53,423, 457, 469, 678, 715,
                789, 167, 711, 886, 907, 306, 61, 783, 333, 16, 685, 802, 21, 755, 530,
                874, 413, 541, 85, 571, 564, 470, 11, 262, 873, 993, 961, 58, 696, 481,
                455, 264, 488, 120, 964, 211, 210, 261, 750, 646, 889, 295, 900, 126,
                73, 68, 651, 358, 45, 599, 973, 691, 612, 598, 363, 82, 323, 889, 299,
                992, 850, 630, 99, 286, 377, 452, 917, 262, 750, 907, 976, 427, 661,
                363, 338, 715, 771, 445, 305, 468, 139, 989, 90, 850, 614, 43, 688, 874,
                655, 374, 625, 483, 92, 931, 509, 401, 181, 98, 938, 944, 137, 251, 475,
                804, 618, 559, 983, 762, 615, 63, 526, 549, 39, 465, 306, 791, 493, 660,
                723, 96, 897, 132, 775, 825, 379, 508, 340, 135, 242, 825, 442, 59, 20,
                895, 112, 875, 737, 482, 797, 230, 318, 94, 86, 907, 736, 962, 256, 409,
                186, 670, 967, 402, 774, 77, 159, 422, 757, 479, 352, 543, 128, 307, 869,
                274, 485, 392, 712, 851, 260, 211, 637, 683, 353, 342, 818, 761, 505, 707,
                509, 425, 124, 705, 919, 350, 317, 810, 123, 487, 542, 981, 402, 776, 235,
                802, 582, 9, 683, 636, 363, 233, 310, 423, 139, 770, 679, 716, 860, 47, 561,
                574, 475, 904, 673, 535, 971, 157, 976, 969, 976, 981, 284, 83, 634, 811, 829 };

            List<Entities.Profile> profiles = new List<Entities.Profile>();

            foreach (var item in inputArray)
            {
                var o = new Entities.Profile();
                o.Inn = item;
                profiles.Add(o);
            }

            QuickSort<Entities.Profile> quickSort = new QuickSort<Entities.Profile>(profiles, "Inn");

            QuickSort<int> quickSort2 = new QuickSort<int>(inputArray);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Sailora WEB API v1");

            });

            // подключаем CORS
            app.UseCors(x =>
                        x.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .WithExposedHeaders("Content-Disposition")
            );

            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(x => x.MapControllers());


            app.Run(async context =>
            {
                var sb = new StringBuilder();
                sb.Append("<h1>Все сервисы</h1>");
                sb.Append("<table>");
                sb.Append("<tr><th>Тип</th><th>Lifetime</th><th>Реализация</th></tr>");
                foreach (var svc in _services)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{svc.ServiceType.FullName}</td>");
                    sb.Append($"<td>{svc.Lifetime}</td>");
                    sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                context.Response.ContentType = "text/html;charset=utf-8";
                await context.Response.WriteAsync(sb.ToString());
            });

        }
    }
}
