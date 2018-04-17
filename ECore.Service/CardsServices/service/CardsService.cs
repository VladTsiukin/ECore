using ECore.Domain.Context;
using ECore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECore.Service.CardsServices.service
{
    public class CardsService : ICardsService<CardsCollection>
    {
        private readonly EcoreDbContext _context;

        public CardsService(EcoreDbContext context)
        {
            _context = context;
        }

        public IQueryable<CardsCollection> GetCardsByUserId(string id)
        {
            return _context.Set<CardsCollection>().Select(c => c).Where(c => c.AppUserId == id);
        }

        Task<CardsCollection> ICardsService<CardsCollection>.GetCardsById(int id)
        {
            return _context.CardsCollection.FindAsync(id);
        }
    }
}
