using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECore.Domain.Context;

namespace ECore.Service.CardsServices.service
{
    public class CRUDService<T> : ICRUDService<T> where T : class
    {

        private readonly EcoreDbContext _context;

        public CRUDService(EcoreDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(T entity)
        {          
            try
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR DELETE ASYNC");
                return false;
            }
        }

        public IQueryable<T> GetAllQuery()
        {
            return _context.Set<T>().AsQueryable<T>();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR UPDATE ASYNC");
                return false;
            }
        }
    }
}
