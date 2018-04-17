using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECore.Service.CardsServices
{
    public interface ICRUDService<Entity> where Entity : class
    {
        IQueryable<Entity> GetAllQuery();

        Task<Entity> AddAsync(Entity entity);

        Task<bool> UpdateAsync(Entity entity);

        Task<bool> DeleteAsync(Entity entity);
    }
}
