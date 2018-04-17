using ECore.Domain.Entities;
using ECore.Service.CardsServices;
using ECore.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECore.Test
{
    public class CardsControllerTest
    {
        
        [Fact]
        public void CheckModelCardsTest()
        {
            // Arrange
            var mock = new Mock<ICardsService<CardsCollection>>();

            mock.Setup(c => c.GetCardsByUserId("123$")).Returns(GetIQueryableCollection());

            var controller = new CardsController(null, mock.Object, null, null, null, null);

            // Act
            var result = controller.AllCards();

            // Assert
            var res = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<CardsCollection>>(res.Model);
        }

        [Fact]
        public void GetAllCardsTest()
        {
            // Arrange
            var mock = new Mock<ICardsService<CardsCollection>>();

            mock.Setup(c => c.GetCardsByUserId("123$")).Returns(GetIQueryableCollection());

            var controller = new CardsController(null, mock.Object, null, null, null, null);

            // Act
            var result = controller.AllCards();

            // Assert
            Assert.IsType<ViewResult>(result);
        }


        public IQueryable<CardsCollection> GetIQueryableCollection()
        {
            List<CardsCollection> cards = new List<CardsCollection>();

            cards.Add(new CardsCollection
            {
                Id = 3,
                Name = "CardsOne",
                DateOfCreation = DateTimeOffset.Now,
                AppUserId = "123$"               
            });

            return cards.AsQueryable();
        }


    }

    
}
