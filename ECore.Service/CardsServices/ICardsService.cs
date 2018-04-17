using ECore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECore.Service.CardsServices
{
    public interface ICardsService<T> where T : CardsCollection
    {
        IQueryable<T> GetCardsByUserId(string id);

        Task<T> GetCardsById(int id);
    }
}
