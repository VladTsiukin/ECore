using ECore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECore.Domain.Context
{
    public static class ECoreDbInitializer
    {
        private static readonly Dictionary<int, CardsCollection> cardsCollections = new Dictionary<int, CardsCollection>();
        private static readonly Dictionary<int, Card> cards = new Dictionary<int, Card>();

        public static void Initialize(EcoreDbContext context)
        {
            context.Database.EnsureCreated();

            SeedCardsCollections(context);
        }

        private async static void SeedCardsCollections(EcoreDbContext context)
        {
            var cardsCollections = new[]
            {
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "BLONP", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "BOLID", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "BONAP", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1292 12:00AM"),  Name = "BOTTM", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "BSBEV", AppUserId = "BOTTM" },
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1292 12:00AM"),  Name = "CACTU", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "CENTC", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "CHOPS", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "COMMI", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "CONSH", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "DRACD", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "DUMON", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "EASTC", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1982 12:00AM"),  Name = "ERNSH", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "FAMIA", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1972 12:00AM"),  Name = "FISSA", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 2992 12:00AM"),  Name = "FOLIG", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "FOLKO", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 2992 12:00AM"),  Name = "FRANK", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "FRANR", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "FRANS", AppUserId = "BOTTM" }, 
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 2332 12:00AM"),  Name = "FOLIG", AppUserId = "BOTTM" },
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "FOLKO", AppUserId = "BOTTM" },
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "FRANK", AppUserId = "BOTTM" },
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "FRANR", AppUserId = "BOTTM" },
                new CardsCollection { DateOfCreation =DateTime.Parse("May  1 1992 12:00AM"),  Name = "FRANS", AppUserId = "BOTTM" }
            };

            context.CardsCollection.AddRange(cardsCollections);
            await context.SaveChangesAsync();
        }
    }
}
