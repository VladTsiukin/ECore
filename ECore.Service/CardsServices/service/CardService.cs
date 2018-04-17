using ECore.Domain.Context;
using ECore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ECore.Service.CardsServices.service
{
    public class CardService : ICardService<Card>
    {
        private readonly EcoreDbContext _context;

        public CardService(EcoreDbContext context)
        {
            _context = context;

        }

        public Task<Card> GetCardById(int id)
        {
            return _context.Set<Card>().FindAsync(id);
        }

        public IQueryable<Card> GetCardsByCollectionId(int id)
        {
            return _context.Set<Card>().Include("Item").Select(c => c).Where(c => c.CardsId == id);
        }

        public IQueryable<Card> GetSortedCardsForDay(int cardsId)
        {
            try
            {
                return _context.Set<Card>()
                                .Include("Item")
                                .Select(c => c)
                                .Where(c => c.CardsId == cardsId && c.DtoInterval <= DateTimeOffset.Now)
                                .OrderBy(c => c.Interval);
            }
            catch (Exception)
            {
                Console.WriteLine("GetSortedCardsForDay(int cardsId) FAIL");
                return null;
            }
           
        }

        public Task<Card> GetCardByIdWithItem(int id)
        {
            try
            {
                return _context.Set<Card>().Include("Item").FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
