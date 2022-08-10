using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nts_platform_server.Data;
using nts_platform_server.Entities;

namespace nts_platform_server.Services
{
    public interface IEfRepository<T> where T : BaseEntity
    {
        IQueryable<T> Get();
        List<T> GetAll();
        T GetById(long id);

        Task<long> Add(T entity);
        Task<long> Remove(T entity);
        Task<long> Update(T entity);



        Context GetContext();
        Task<long> Save();
    }
}
