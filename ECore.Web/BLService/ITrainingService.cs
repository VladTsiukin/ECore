using ECore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECore.Web.BLService
{
    public interface ITrainingService
    {

        Card SetNewItemsInCard(Card card, int score);

        bool ProcessingAnswer(int score, string key);

    }
}
