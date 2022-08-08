using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nts_platform_server.Data;
using nts_platform_server.Entities;

namespace nts_platform_server.Services
{
    public class Repository<T> : IEfRepository<T> where T : BaseEntity
    {
        private readonly Context _context;

        public Repository(Context context)
        {
            _context = context;
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>();
        }


        public Context GetContext()
        {
            return _context;
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(long id)
        {
            var result = _context.Set<T>().FirstOrDefault(x => x.Id == id);

            if (result == null)
            {
                //todo: need to add logger
                return null;
            }

            return result;
        }

        public async Task<long> Add(T entity)
        {
            var result = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task<long> Remove(T entity)
        {
            var result =  _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task<long> Update(T entity)
        {
            var result = _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task<long> Save()
        {
            await _context.SaveChangesAsync();
            return 1;
        }
    }
}
