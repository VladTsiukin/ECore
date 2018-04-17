using ECore.Domain.Entities;
using ECore.Web.BLService;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECore.Web.BLService
{
    public class Training : ITrainingService
    {
        private readonly IMemoryCache _memoryCache;

        private object locker = new object();

        public Training()
        {
        }

        public Training(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        public Card SetNewItemsInCard(Card card, int score)
        {
            try
            {
                if (card == null)
                {
                    throw new NullReferenceException("SetNewItemsInCard()[CARD CAN NOT BE NULL <FAIL>]");
                }

                long intervalInTime = 0;
                long dayMilliseconds = 86400;
                // set new score
                card.UserScore = score;

                lock (locker)
                {
                    // set new group repetition
                    card.GroupRepetition = AlgorithmSM.CalcGroupRepetition(card.GroupRepetition, card.Efactor);
                    // calculate new e-factor
                    card.Efactor = AlgorithmSM.CalcNewEfactor(card.Efactor, score);
                    // set new interval in days (Milliseconds)
                    intervalInTime = (long)AlgorithmSM.CalcInterval(card.GroupRepetition, card.Efactor);
                }
                
                intervalInTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + (intervalInTime * dayMilliseconds);
                card.Interval = intervalInTime;
                // set new interval in DateTimeOffset
                card.DtoInterval = DateTimeOffset.FromUnixTimeMilliseconds(intervalInTime);


                return card;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }


        }

        /// <summary>
        /// Is processing a user answer.
        /// </summary>
        /// <param name="sm2"></param>
        /// <param name="score"></param>
        public bool ProcessingAnswer(int score, string key)
        {
            try
            {
                long unixNow = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                // Get the first card and Remove from the queue
                var queueCard = _memoryCache.Get<Queue<Card>>(key);

                if (queueCard == null)
                {
                    throw new FailTrainingExeption();
                }

                Card card = queueCard.Dequeue();

                if (card == null)
                {
                    throw new FailTrainingExeption();
                }

                if (this.SetNewItemsInCard(card, score) == null) { throw new FailTrainingExeption(); }

                // If the score was below 5, or the card is set to be repeated again
                // In less than a day, reinsert it to the queue.
                if (score < (int)UserScore.Easy || card.DtoInterval.Day == DateTimeOffset.Now.Day)
                {
                    queueCard.Enqueue(card);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
    }


    public enum UserScore
    {
        Bad = 0,
        Again = 1,
        Hard = 2,
        Good = 3,
        Easy = 4,
        Excellent = 5
    }

    public enum GroupRepetition
    {
        NoMemorized = 0,
        AlmostMemorized = 1,
        Memorized = 2
    }

    public class FailTrainingExeption : Exception
    {
        public FailTrainingExeption() :base() { }
        public FailTrainingExeption(string message) : base(message) { }
    }
}
