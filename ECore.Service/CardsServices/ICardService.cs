using ECore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECore.Service.CardsServices
{
    public interface ICardService<T> where T : Card
    {
        IQueryable<T> GetCardsByCollectionId(int id);

        Task<T> GetCardById(int id);

        Task<T> GetCardByIdWithItem(int id);

        IQueryable<T> GetSortedCardsForDay(int cardsId);
    }
}
