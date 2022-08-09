using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using nts_platform_server.Entities;
using nts_platform_server.Models;

namespace nts_platform_server.Services
{
    public interface IProjectService
    {
        IEnumerable<Object> GetAll();
        Task<IEnumerable<Object>> AddAsync(ProjectModel newProjectModel);
        Task<IEnumerable<Object>> EditCodeAsync(ProjectEditModel projectEdit);
        Task<IEnumerable<Object>> RemoveCodeAsync(string name);


        Task<IEnumerable<Object>> AddUserProjectAsync(UserProjectModelList newUserProjectModelList);

        Task<IEnumerable<Object>> AddUserProjectHoursAsync(UserProjectModelHours newUserProject);


        
        Company GetById(int id);
        Task<Project> Find(string name);
    }

    public class ProjectService : IProjectService
    {
        private readonly IEfRepository<Project> _projectRepository;
        private readonly IEfRepository<User> _userRepository;
        private readonly IEfRepository<UserProject> _userProjectRepository;
        private readonly IEfRepository<Week> _userWeekRepository;

        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ProjectService(IEfRepository<Week> userWeekRepository,IEfRepository<Project> projectRepository, IEfRepository<User> userRepository, IEfRepository<UserProject> userProjectRepository, IConfiguration configuration, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _userProjectRepository = userProjectRepository;
            _userWeekRepository = userWeekRepository;

            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Object>> AddAsync(ProjectModel newProjectModel)
        {
            var check = _projectRepository.Get().Select(x => x.Title == newProjectModel.NameProject).FirstOrDefault();

            if (!check)
            {
                var newProject= new Project();
                newProject.Code = newProjectModel.Code;
                newProject.Title = newProjectModel.NameProject;
                newProject.MaxHour = newProjectModel.MaxHours;
                newProject.Status = newProjectModel.Status;
                newProject.Description = newProjectModel.Description;

                try
                {
                    newProject.Start = XmlConvert.ToDateTime(newProjectModel.DateStart, XmlDateTimeSerializationMode.Utc);
                    newProject.End = XmlConvert.ToDateTime(newProjectModel.DateStop, XmlDateTimeSerializationMode.Utc);

                   // newProject.Start = DateTimeOffset.TryParse(newProjectModel.DateStart, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTimeOffset result);
                   // newProject.End = DateTimeOffset.TryParse(newProjectModel.DateStop, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var result);

                }
                catch(Exception e)
                {
                    Console.WriteLine("e = ", e.ToString());
                }

               
                newProject.EnginerCreater = _userRepository.Get().Where(x => x.Email == newProjectModel.EngineerCreaterEmail).FirstOrDefault();


                var addedCompany = await _projectRepository.Add(newProject);


                newProject.indexAdd = (int)newProject.Id;

                await _projectRepository.Update(newProject);
                return await Task.FromResult(GetAll());
            }

            return null;
        }

        public async Task<IEnumerable<Object>> RemoveCodeAsync(string Code)
        {
            var check = _projectRepository.Get().Where(x => x.Code == Code).FirstOrDefault();

            if (check != null)
            {
                var context = _userProjectRepository.GetContext();
                //Находим всех пользователей связанных с данным проектом
                var userCheck = _userProjectRepository.Get()
                    .Where(x => x.Project.Id == check.Id)
                    .ToList();


             

                //удаляем всех пользователей связанных с этим проектом
                foreach (var item in userCheck)
                {
                    context.Entry(item).Reference(x => x.Project).Load();
                    context.Entry(item).Reference(x => x.User).Load();
                    context.Entry(item).Collection(x => x.Weeks).Load();


                    //Поиск связанных почасовок
                    foreach(var itemWeek in item.Weeks)
                    {

                        context.Entry(itemWeek).Reference(x => x.MoHour).Load();
                        context.Entry(itemWeek).Reference(x => x.TuHour).Load();
                        context.Entry(itemWeek).Reference(x => x.WeHour).Load();
                        context.Entry(itemWeek).Reference(x => x.ThHour).Load();
                        context.Entry(itemWeek).Reference(x => x.FrHour).Load();
                        context.Entry(itemWeek).Reference(x => x.SaHour).Load();
                        context.Entry(itemWeek).Reference(x => x.SuHour).Load();

                        if(itemWeek.MoHour != null)
                            context.Entry(itemWeek.MoHour).State = EntityState.Deleted;
                        if (itemWeek.TuHour != null)
                            context.Entry(itemWeek.TuHour).State = EntityState.Deleted;
                        if (itemWeek.WeHour != null)
                            context.Entry(itemWeek.WeHour).State = EntityState.Deleted;
                        if (itemWeek.ThHour != null)
                            context.Entry(itemWeek.ThHour).State = EntityState.Deleted;
                        if (itemWeek.FrHour != null)
                            context.Entry(itemWeek.FrHour).State = EntityState.Deleted;
                        if (itemWeek.SaHour != null)
                            context.Entry(itemWeek.SaHour).State = EntityState.Deleted;
                        if (itemWeek.SuHour != null)
                            context.Entry(itemWeek.SuHour).State = EntityState.Deleted;

                        context.Entry(itemWeek).State = EntityState.Deleted;
                       
                    }


                    await _userProjectRepository.Remove(item);
                }



                var removedProject = await _projectRepository.Remove(check);
                return await Task.FromResult(GetAll());
            }

            return null;
        }

        public IEnumerable<Object> GetAll()
        {

            var companies = _projectRepository.Get()
                .Include(x=>x.EnginerCreater)
                .OrderBy(x => x.Title)
                .Select(e => new {
                    e.Code,
                    e.Title,
                    e.ActualHour,
                    e.MaxHour,
                    DateStart = e.Start,
                    DateStop = e.End,
                    e.Description,
                    e.indexAdd,
                    e.EnginerCreater,
                    Status = Enum.GetName(e.Status.GetType(), e.Status),
                    Users = e.UserProjects
                            .OrderBy(item => item.User.FirstName)
                            .Select(x => new {
                                x.User.FirstName,
                                x.User.SecondName,
                                x.User.MiddleName,
                                x.User.Email,
                                //x.User.UserProjects,
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
                    //users.UserProjects.Clear();

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

        public async Task<IEnumerable<object>> EditCodeAsync(ProjectEditModel projectEdit)
        {

            if(projectEdit.NewProjectInfromation == null || projectEdit.OldProjectInfromation == null)
            {
                return null;
            }

            var check = _projectRepository.Get().Where(x => x.Code == projectEdit.OldProjectInfromation.Code).FirstOrDefault();

            //Нашли тот объект который хотим поменять
            if (check != null)
            {
                var newObject = projectEdit.NewProjectInfromation;

                check.Code = newObject.Code;
                check.Title = newObject.NameProject;
                check.MaxHour = newObject.MaxHours;
                check.Status = newObject.Status;
                check.Description = newObject.Description;

                check.Start = XmlConvert.ToDateTime(newObject.DateStart, XmlDateTimeSerializationMode.Utc);
                check.End = XmlConvert.ToDateTime(newObject.DateStop, XmlDateTimeSerializationMode.Utc);


                var editProject = await _projectRepository.Update(check);
                return await Task.FromResult(GetAll());
            }

            return null;
        }

        public async Task<IEnumerable<object>> AddUserProjectHoursAsync(UserProjectModelHours newUserProject)
        {
            //Найти почасовку в системе на данного юзера на данный проект
            var check = _userProjectRepository.Get()
                .Where(
                x => x.User.Email == newUserProject.User.Email &&
                x.Project.Code == newUserProject.Project.Code)
                .Include(x=> x.Project)
                .Include(x=> x.Weeks)
                .FirstOrDefault();

            if (check != null)
            {

                //Если почасовки нашлись проверить есть ли почасовки на данную

                foreach(var week in newUserProject.Weeks)
                {
                    var findWeeks = check.Weeks.Where(x => x.NumberWeek == week.NumberWeek).FirstOrDefault();

                    //Если нашли уже существующую почасовку, то запросить подтвердить изменение у пользователя
                    if(findWeeks != null)
                    {
                        return null; //return await Task.FromResult(1);
                    }
                    else
                    {
                        check.Weeks.Add(week);
                    }
                }


                await _userProjectRepository.Save();

                return await Task.FromResult(GetAll());

            }

            return null;
        }
    }
}
