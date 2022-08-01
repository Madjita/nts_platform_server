using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using nts_platform_server.Entities;
using nts_platform_server.Models;

namespace nts_platform_server.Services
{
   
    public interface IDocHourseService
    {
        Task<object> AddUserHoursInProjectAsync(Week newWeek);
        IEnumerable<Object> GetUserHours(string email);
        IEnumerable<Object> GetAll();
    }

    public class DocHourseService : IDocHourseService
    {
        private readonly IEfRepository<User> _userRepository;
        private readonly IEfRepository<Role> _roleRepository;

        private readonly IEfRepository<DocHour> _docHourRepository;
        private readonly IEfRepository<Week> _weekRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public DocHourseService(IEfRepository<Week> weekRepository, IEfRepository<DocHour> docHourRepository, IEfRepository<User> userRepository, IEfRepository<Role> roleRepository, IConfiguration configuration, IMapper mapper)
        {
            _weekRepository = weekRepository;
            _docHourRepository = docHourRepository;

            _userRepository = userRepository;
            _roleRepository = roleRepository;


            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<object> AddUserHoursInProjectAsync(Week newWeek)
        {
            var chek = _weekRepository.Get()
                .Include(x => x.MoHour)
                .Include(x => x.TuHour)
                .Include(x => x.WeHour)
                .Include(x => x.ThHour)
                .Include(x => x.FrHour)
                .Include(x => x.SaHour)
                .Include(x => x.SuHour)
                .Where(x=> x.Year == newWeek.Year && x.WeHour == newWeek.WeHour )
                //.Where(x => x.Week.NumberWeek == newWeek.NumberWeek && x.Week.Month == newWeek.Month && x.Week.Year == newWeek.Year)
                .ToList();

            if(chek == null)
            {
                var responce = _weekRepository.Add(newWeek);

                return await Task.FromResult(responce);
            }

            return null;
        }

        public IEnumerable<object> GetAll()
        {
            var list = _docHourRepository.Get()
                .Include(x => x.Week)
                .ToList();

            if (list.Count < 1)
            {
                return null;
            }

            return list;
            throw new NotImplementedException();
        }

        public IEnumerable<Object> GetUserHours(string email)
        {
            var list = _docHourRepository.Get()
                .Include(x => x.Week)
               /* .OrderBy(x => x.User.Email)
                .Where(x => x.User.Email == email)
                .Select(e => new {
                    e.Year,
                    e.Week,
                    e.MondayHour,
                    e.TuesdayHour,
                    e.WednesdayHour,
                    e.ThursdayHour,
                    e.FridayHour,
                    e.SaturdayHour,
                    e.SundayHour,
                })*/
                .ToList();

            if(list.Count < 1)
            {
                return null;
            }

            return list;

            //throw new NotImplementedException();
        }
    }
}
