using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using nts_platform_server.Auth.JWT;
using nts_platform_server.Entities;
using nts_platform_server.Models;

namespace nts_platform_server.Services
{
    public class UserService : IUserService
    {
        private readonly IEfRepository<User> _userRepository;
        private readonly IEfRepository<Role> _roleRepository;
        private readonly IEfRepository<UserProject> _userProjectRepository;

        private readonly ICompanyService _companyService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(IEfRepository<User> userRepository, IEfRepository<Role> roleRepository, IEfRepository<UserProject> userProjectRepository, ICompanyService companyServic, IConfiguration configuration, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _companyService = companyServic;
            _userProjectRepository = userProjectRepository;


            _configuration = configuration;
            _mapper = mapper;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            if(model == null)
            {
                return null;
            }

               var user = _userRepository
                   .Get()
                   .Include(x=> x.Role)
                   .Where(x => x.Email == model.Email)
                   .FirstOrDefault();




            if (user == null)
            {
                // todo: need to add logger
                return null;
            }


            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return null;
            }


            var token = _configuration.GenerateJwtToken(user);



            var authenticate = new AuthenticateResponse(user, token);


            return authenticate;
        }

        public async Task<AuthenticateResponse> Register(UserModelRegister userModel)
        {
            var user = _userRepository
             .GetAll()
             .FirstOrDefault(x => x.Email == userModel.Email);



            var role = _roleRepository.Get().Where(x => x.Title == "guest").FirstOrDefault();

            if(role != null)
            {
                userModel.Role = role;
            }


            if (user == null)
            {
                user = _mapper.Map<User>(userModel);
                user.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);


                if(user.Company != null)
                {

                    var company = await _companyService.Find(user.Company.Name);

                    if(company != null)
                    {
                       user.Company = company;
                    }
                }


                var addedUser = await _userRepository.Add(user);
            }

      
            var response = Authenticate(new AuthenticateRequest
            {
                Email = userModel.Email,
                Password = userModel.Password
            });

            return await Task.FromResult(response);
        }

        public IEnumerable<Object> GetAll()
        {
            /* var users = _userRepository.Get().Include(x => x.Company).Select(e => new {
                 e.FirstName,
                 e.Email,
                 Company = e.Company.Name
             }).ToList();*/

            var users = _userRepository.Get()
            .Include(x => x.Company)
            .Include(x=>x.UserProjects).ThenInclude(x=>x.Project)
            .Include(x=>x.UserProjects).ThenInclude(x=>x.User)
            .Include(x => x.Role)
            //.OrderBy(item => item.Email)
            .Select(e => new {
                e.FirstName,
                e.SecondName,
                e.MiddleName,
                e.Email,
                e.UserProjects,
                Company = e.Company.Name,
                e.Role,
            })
            .ToList();



            foreach(var item in users)
            {
                foreach (var project in item.UserProjects)
                {
                    if(project.Project!= null)
                    {
                        project.Project.UserProjects = null;
                    }
                   
                    project.User.UserProjects = null;
                }
            }

         


            // return _userRepository.GetAll();

            return users;
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public async Task<User> RemoveAsync(string name)
        {
            var check = _userRepository.Get()
                .Include(x=>x.UserProjects)
                .Include(x=>x.Weeks)
                .Where(x => x.Email == name).FirstOrDefault();

            if (check != null)
            {
                var userProject = _userProjectRepository.Get().Where(x => x.User.Email == check.Email).FirstOrDefault();

                if(userProject!= null)
                {
                    var removeUserProject = await _userProjectRepository.Remove(userProject);
                }

                var removedUser = await _userRepository.Remove(check);
                return await Task.FromResult(check);
            }

            return null;
        }



   
        public async Task<Object> Find(string email)
        {
            var check = _userRepository.Get()
                .Include(x => x.Company)
                .Include(x => x.UserProjects).ThenInclude(x => x.Project)
                .Include(x => x.UserProjects).ThenInclude(x => x.User)
                .Include(x => x.Role)
                .Include(x => x.Profile)
                .Where(x => x.Email == email)
                .Select(e => new {
                     e.FirstName,
                     e.SecondName,
                     e.MiddleName,
                     e.Email,
                     e.UserProjects,
                     Company = e.Company.Name,
                     e.Role,
                     e.Profile,
                }).FirstOrDefault();


            if (check != null)
            {
              

                return await Task.FromResult(check);
            }

            return null;
        }

        public async Task<IEnumerable<object>> FindUsersInProjectAsync(string project)
        {
            var check = _userRepository.Get()
               .Include(x => x.Company)
               .Include(x => x.UserProjects).ThenInclude(x => x.Project)
               .Include(x => x.Role)
               .Select(e => new {
                   e.FirstName,
                   e.SecondName,
                   e.MiddleName,
                   e.Email,
                   e.UserProjects,
                   Company = e.Company.Name,
                   e.Role,
               })
               .Where(x => x.UserProjects.Where(s => s.Project.Title == project).Any())
               .ToList();

            if (check != null)
            {
                return await Task.FromResult(check);
            }

            return null;
            
        }

        public async Task<User> ChangeUser(UserModelChange changeUser)
        {

            var check = _userRepository.Get()
                .Include(x => x.Profile)
                .Where(x => x.Email == changeUser.OldUser.Email).FirstOrDefault();


            if (check != null)
            {
                check.FirstName = changeUser.NewUser.FirstName;
                check.SecondName = changeUser.NewUser.SecondName;
                check.MiddleName = changeUser.NewUser.MiddleName;

                


                check.Profile.Sex = changeUser.NewUser.Profile.Sex;
                check.Profile.Date = changeUser.NewUser.Profile.Date;
                check.Profile.PrfSeries = changeUser.NewUser.Profile.PrfSeries;
                check.Profile.PrfNumber = changeUser.NewUser.Profile.PrfNumber;
                check.Profile.PrfDateTaked = changeUser.NewUser.Profile.PrfDateTaked;
                check.Profile.PrfDateBack = changeUser.NewUser.Profile.PrfDateBack;
                check.Profile.PrfCode = changeUser.NewUser.Profile.PrfCode;
                check.Profile.PrfTaked = changeUser.NewUser.Profile.PrfTaked;
                check.Profile.PrfPlaceBorned = changeUser.NewUser.Profile.PrfPlaceBorned;
                check.Profile.PrfPlaceRegistration = changeUser.NewUser.Profile.PrfPlaceRegistration;
                check.Profile.PrfPlaceLived = changeUser.NewUser.Profile.PrfPlaceLived;
                check.Profile.IpNumber = changeUser.NewUser.Profile.IpNumber;
                check.Profile.IpDateTaked = changeUser.NewUser.Profile.IpDateTaked;
                check.Profile.IpDateBack = changeUser.NewUser.Profile.IpDateBack;
                check.Profile.IpCode = changeUser.NewUser.Profile.IpCode;
                check.Profile.IpTaked = changeUser.NewUser.Profile.IpTaked;
                check.Profile.IpPlaceBorned = changeUser.NewUser.Profile.IpPlaceBorned;
                check.Profile.UlmNumber = changeUser.NewUser.Profile.UlmNumber;
                check.Profile.UlmDateTaked = changeUser.NewUser.Profile.UlmDateTaked;
                check.Profile.UlmDateBack = changeUser.NewUser.Profile.UlmDateBack;
                check.Profile.UlmCode = changeUser.NewUser.Profile.UlmCode;
                check.Profile.UlmTaked = changeUser.NewUser.Profile.UlmTaked;
                check.Profile.UlmPlaceBorned = changeUser.NewUser.Profile.UlmPlaceBorned;
                check.Profile.Snils = changeUser.NewUser.Profile.Snils;
                check.Profile.Inn = changeUser.NewUser.Profile.Inn;
                check.Profile.Phone = changeUser.NewUser.Profile.Phone;
                await _userRepository.Save();


                return await Task.FromResult(check); ;
            }


            return null;
        }
    }
}
