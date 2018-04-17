using ECore.Domain.Entities;
using ECore.Service.CardsServices;
using ECore.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ECore.Test
{
    public class CardControllerTest
    {

        int cardsiId = 5;

        [Fact]
        public void GetAllCardsTest()
        {
            // Arrange
            var mock = new Mock<ICardService<Card>>();            

            mock.Setup(c => c.GetCardsByCollectionId(cardsiId)).Returns(GetIQueryableCard());

            var controller = new CardController(null, null, mock.Object, null, null);

            // Act
            var result = controller.ListCard(cardsiId, "");

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        /// <summary>
        /// Check is the object IEnumerable <Card>
        /// </summary>
        [Fact]
        public void CheckModelCardsTest()
        {
            // Arrange
            var mock = new Mock<ICardService<Card>>();

            mock.Setup(c => c.GetSortedCardsForDay(cardsiId)).Returns(GetIQueryableCard());

            var controller = new CardController(null, null, mock.Object, null, null);

            // Act
            var result = controller.ListCard(cardsiId, "");

            // Assert
            var res = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Card>>(res.Model);
        }

        public IQueryable<Card> GetIQueryableCard()
        {
            List<Card> cards = new List<Card>();

            cards.Add(new Card
            {
                CardsId = 5,
                DtoInterval = new DateTimeOffset(new DateTime(2018, 01, 01)),
                Efactor = 3.0,
                Item = new Item
                {
                    Back = "back",
                    Face = "face"
                }
            });

            return cards.AsQueryable();
        }

    }
}
