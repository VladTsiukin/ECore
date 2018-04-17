using ECore.Domain.Entities;
using ECore.Web.BLService;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECore.Test
{
    public class TrainingTest
    {
        /// <summary>
        /// Check null method SetNewItemsInCards.
        /// </summary>
        [Fact]
        public void SetNewItemsInCardsNullTest()
        {
            // Arrang
            var moq = new Mock<ITrainingService>();
            moq.SetupProperty(t => t.CardQueue);

            // Act
            var card = moq.Setup(t => t.SetNewItemsInCard(GetNewCard(), 5)).Returns(GetNewCard());

            // Assert
            Assert.NotNull(card);
        }


        public Card GetNewCard()
        {
            return new Card
            {
                DtoInterval = DateTimeOffset.Now,
                Efactor = 3.0,
                Item = new Item
                {
                    Back = "back",
                    Face = "face"
                },
                Interval = 0

            };
        }

        public Queue<Card> GetCardQueueWithCard()
        {
            var col = new Queue<Card>();
            col.Enqueue(GetNewCard());

            return col;
        }
    }
}
