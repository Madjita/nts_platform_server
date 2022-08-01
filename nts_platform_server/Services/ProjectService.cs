using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using nts_platform_server.Entities;
using nts_platform_server.Models;

namespace nts_platform_server.Services
{
    public interface IProjectService
    {
        IEnumerable<Object> GetAll();
        Task<IEnumerable<Object>> AddAsync(ProjectModel newProjectModel);

        Task<IEnumerable<Object>> AddUserProjectAsync(UserProjectModelList newUserProjectModelList);


        Task<Project> RemoveAsync(string name);
        Company GetById(int id);
        Task<Project> Find(string name);
    }

    public class ProjectService : IProjectService
    {
        private readonly IEfRepository<Project> _projectRepository;
        private readonly IEfRepository<User> _userRepository;
        private readonly IEfRepository<UserProject> _userProjectRepository;

        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ProjectService(IEfRepository<Project> projectRepository, IEfRepository<User> userRepository, IEfRepository<UserProject> userProjectRepository, IConfiguration configuration, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _userProjectRepository = userProjectRepository;

            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Object>> AddAsync(ProjectModel newProjectModel)
        {
            var check = _projectRepository.Get().Select(x => x.Title == newProjectModel.Name).FirstOrDefault();

            if (!check)
            {
                var newProject= new Project();
                newProject.Title = newProjectModel.Name;
                newProject.MaxHour = newProjectModel.MaxHours;
                newProject.Status = Status.plan;


                var addedCompany = await _projectRepository.Add(newProject);
                return await Task.FromResult(GetAll());
            }

            return null;
        }

        public async Task<Project> RemoveAsync(string name)
        {
            var check = _projectRepository.Get().Where(x => x.Title == name).FirstOrDefault();

            if (check != null)
            {
                var removedCompany = await _projectRepository.Remove(check);
                return await Task.FromResult(check);
            }

            return null;
        }

        public IEnumerable<Object> GetAll()
        {

            var companies = _projectRepository.Get()
                .OrderBy(x => x.Title)
                .Select(e => new {
                    e.Title,
                    e.ActualHour,
                    e.MaxHour,
                    e.Start,
                    e.End,
                    Status = Enum.GetName(e.Status.GetType(), e.Status),
                    Users = e.UserProjects
                            .OrderBy(item => item.User.FirstName)
                            .Select(x => new {
                                x.User.FirstName,
                                x.User.SecondName,
                                x.User.MiddleName,
                                x.User.Email,
                                x.User.UserProjects,
                                Company = x.User.Company.Name,
                                Role = x.User.Role.Title
                            })
                            .ToList()
                })
                .ToList();



            foreach (var item in companies)
            {
                foreach (var users in item.Users)
                {
                    users.UserProjects.Clear();

                }
            }


            return companies;
        }

        public Company GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Project> Find(string name)
        {
            var project = _projectRepository.Get()
                .OrderBy(x => x.Title)
                .Where(x => x.Title == name)
                .FirstOrDefault();

            return await Task.FromResult(project);
        }

        public async Task<IEnumerable<object>> AddUserProjectAsync(UserProjectModelList newUserProjectModelList)
        {
            List<UserProject> list = new List<UserProject>();


            foreach(var item in newUserProjectModelList.UserProjects)
            {
               var project = _projectRepository.Get()
               .OrderBy(x => x.Title)
               .Where(x => x.Title == item.Project)
               .FirstOrDefault();


                var user = _userRepository.Get()
                    .Where(x => x.Email == item.Email)
                    .FirstOrDefault();

                if(project == null || user == null)
                {
                    return null;
                }

                UserProject userProject = new UserProject()
                {
                    User = user,
                    Project = project
                };


                var chek = _userProjectRepository.Get().Where(x => x.User == userProject.User && x.Project == userProject.Project).FirstOrDefault();

                if(chek != null)
                {
                    continue;
                }


                list.Add(userProject);

                var addNewUserProject = await _userProjectRepository.Add(userProject);
            }


           


            return list;
        }

    }
}
