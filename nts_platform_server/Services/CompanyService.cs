using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using nts_platform_server.Entities;

namespace nts_platform_server.Services
{
    public interface ICompanyService
    {
        IEnumerable<Object> GetAll();
        Task<IEnumerable<Object>> AddAsync(string name);
        Task<Company> RemoveAsync(string name);
        Company GetById(int id);
        Task<Company> Find(string name);
        Task<Company> UpdateWorkers(string name);

    }

    public class CompanyService : ICompanyService
    {
        private readonly IEfRepository<Company> _companyRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CompanyService(IEfRepository<Company> companyRepository, IConfiguration configuration, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Object>> AddAsync(string name)
        {
            var check = _companyRepository.Get().Select(x => x.Name == name).FirstOrDefault();

            if(!check)
            {
                var newComapny = new Company();
                newComapny.Name = name;

                var addedCompany =  await _companyRepository.Add(newComapny);
                return await Task.FromResult(GetAll());
            }

            return null;
        }

        public async Task<Company> RemoveAsync(string name)
        {
            var check = _companyRepository.Get().Where(x => x.Name == name).FirstOrDefault();

            if (check != null)
            {
                var removedCompany = await  _companyRepository.Remove(check);
                return await Task.FromResult(check);
            }

            return null;
        }

        public IEnumerable<Object> GetAll()
        {

            var companies = _companyRepository.Get()
                //.OrderBy(x => x.Name)
                .Include(x => x.Users)
                .ThenInclude(x => x.Role)
                .Include(x => x.Users)
                .ThenInclude(x => x.Company)
                .Select(e => new {
                    e.Name,
                    e.PersonalCount,
                    Users = e.Users
                            //.OrderBy(item => item.Email)
                            .Select(x => new {
                                x.FirstName,
                                x.SecondName,
                                x.MiddleName,
                                x.Email,
                                x.UserProjects,
                                Company = x.Company.Name,
                                Role = x.Role.Title,
                                //Role = //Enum.GetName(x.Role.GetType(), x.Role)
                            })
                            .ToList()
                })
                .ToList();


            foreach(var item in companies)
            {
                foreach(var user in item.Users)
                {
                    foreach(var userProject in user.UserProjects)
                    {
                        userProject.User.Company = null;
                        userProject.User.UserProjects = null;
                        userProject.User.Role = null;
                    }
                }
            }

            //Enum.GetName(x.Role.GetType(), x.Role)

            return companies;
        }

        public Company GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Company> Find(string name)
        {
            var company = _companyRepository.Get()
                .Include(x=>x.Users)
                .OrderBy(x => x.Name)
                .Where(x => x.Name == name)
                .FirstOrDefault();

            return await Task.FromResult(company);
        }

        public async Task<Company> UpdateWorkers(string name)
        {
            var company = _companyRepository.Get()
                .OrderBy(x => x.Name)
                .Where(x => x.Name == name)
                .FirstOrDefault();

            return await Task.FromResult(company);
        }
    }
}
