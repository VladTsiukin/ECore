using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECore.Domain.Entities;
using ECore.Service.CardsServices;
using ECore.Web.BLService;
using ECore.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ECore.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class CardController : Controller
    {
        private readonly ICRUDService<Card> _crudCardService;
        private readonly ICardService<Card> _cardService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<Card> _logger;
        private readonly ITrainingService _trainingService;
        private readonly IMemoryCache _memoryCache;

        private IQueryable<Card> card;

        private Queue<Card> sortedCard;

        public CardController(ICRUDService<Card> crudCardService,
                               UserManager<AppUser> userManager,
                               ICardService<Card> cardService,
                               ILogger<Card> logger, 
                               ITrainingService trainingService,
                               IMemoryCache memoryCache)
        {
            _crudCardService = crudCardService;
            _cardService = cardService;
            _userManager = userManager;
            _logger = logger;
            _trainingService = trainingService;
            _memoryCache = memoryCache;
        }


        /// <summary>
        /// Get current user Identifier from claims.
        /// </summary>
        /// <returns></returns>
        private string GetCurrentUserId()
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                string currentUserId = currentUser?.FindFirst(ClaimTypes.NameIdentifier).Value;

                return currentUserId;
            }
            catch (Exception)
            {
                _logger.LogWarning("GetCurrentUserId() NOT FOUND");
                return String.Empty;
            }

        }

        /// <summary>
        /// Show cards in cards collections.
        /// </summary>
        /// <returns></returns>
        public IActionResult ListCard(int cardsId, string cardsName)
        {

            if(cardsId > 0)
            {
                card = _cardService.GetCardsByCollectionId(cardsId);

                if (card != null)
                {
                    return View(card.ToList());
                }
            }

            return RedirectToAction(nameof(CardsController.AllCards), "Card");
        }

        /// <summary>
        /// Training new and review cards in collection.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ifStart"></param>
        /// <returns></returns>
        public IActionResult Training(string key, int cardsId = 0, string cardsName = "", bool ifStart = false)
        {
            ViewData["key"] = String.Empty;

            string guidKey = String.Empty;

            // if request from 'AllCards'(CardsController)
            if (ifStart)
            {
                if (cardsId <= 0)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                // create sorted cards for repetition one collection.

                sortedCard = new Queue<Card>();
                //HttpContext.Session.Set("queue", sortedCard);

                foreach (var card in _cardService.GetSortedCardsForDay(cardsId))
                {
                    sortedCard.Enqueue(card);
                }

                Card cardCheck;

                if (!(sortedCard.TryPeek(out cardCheck)))
                {
                    return RedirectToAction(nameof(CardController.NotItemForTraining), "Card");
                }

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromHours(1));

                // start Training
                guidKey = Guid.NewGuid().ToString();
                ViewData["key"] = guidKey;
                _memoryCache.Set<Queue<Card>>(guidKey, sortedCard, cacheEntryOptions);

            }

            if (guidKey == String.Empty)
            {
                guidKey = key;
                ViewData["key"] = guidKey;
            }

            var queueResult = _memoryCache.Get<Queue<Card>>(guidKey);

            if (queueResult != null)
            {
                Card cardRes;

                var cardResult = queueResult.TryPeek(out cardRes);

                if (cardResult)
                {
                    return View(cardRes);
                }
            }           

            _memoryCache.Remove(guidKey);
            return RedirectToAction(nameof(CardController.NotItemForTraining), "Card");
        }

        /// <summary>
        /// Processing user score and save result in db.
        /// </summary>
        /// <param name="card"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ProcessingUserAnswer([FromForm] int idCard, int score, string keyQ)
        {

            // update database
            if (idCard > 0)
            {
                // find the card in db we want to change
                var card = await _cardService.GetCardById(idCard);

                if (card != null)
                {
                    // get new card from Training service
                    var newCard = _trainingService.SetNewItemsInCard(card, score);

                    if (newCard != null)
                    {
                        // update new card data in database
                        var result = await _crudCardService.UpdateAsync(newCard);

                        // if success result and success check score for Training
                        if (result && _trainingService.ProcessingAnswer(score, keyQ))
                        {
                            return RedirectToAction(nameof(CardController.Training), "Card",
                                new { key = keyQ, cardsId = 0, cardsName = "", ifStart = false });
                        }
                    }
                }
            }

            return RedirectToAction(nameof(HomeController.Error), "Home");
        }


        /// <summary>
        /// Edit Card.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cardsId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditCard(int id, int cardsId)
        {
            if (id < 0)
            {
                return RedirectToAction(nameof(CardController.ListCard), "Card",
                    new { cardsId = cardsId.ToString(), cardsName = "" });
            }
            else
            {
                Card card = await _cardService.GetCardByIdWithItem(id);

                if (card == null)
                {
                    return RedirectToAction(nameof(CardController.ListCard), "Card",
                        new { cardsId = cardsId.ToString(), cardsName = "" });
                }

                CardEditVM cvm = new CardEditVM
                {
                    Id = card.Id,
                    CardsId = card.CardsId,
                    UserScore = card.UserScore,
                    Efactor = card.Efactor,
                    Item = new ItemVM
                    {
                        Back = card.Item.Back,
                        Face = card.Item.Face
                    }
                };

                return View(cvm);
            }
        }

        /// <summary>
        /// Edit card in db.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCard([FromForm] CardEditVM card)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(CardController.ListCard), "Card",
                       new { cardsId = card.CardsId.ToString(), cardsName = "" });
            }
            else
            {

                Card card_bd = await _cardService.GetCardByIdWithItem(card.Id);

                if (card_bd == null)
                {
                    return RedirectToAction(nameof(CardController.ListCard), "Card",
                        new { cardsId = card.CardsId.ToString(), cardsName = "" });
                }

                card_bd.Item.Back = card.Item.Back;
                card_bd.Item.Face = card.Item.Face;

                bool good = await _crudCardService.UpdateAsync(card_bd);

                if (good)
                {
                    return RedirectToAction(nameof(CardController.ListCard), "Card",
                        new { cardsId = card.CardsId.ToString(), cardsName = "" });
                }
            }

            return RedirectToAction(nameof(HomeController.Error), "Home");
        }

        /// <summary>
        /// If there is nothing to repeat.
        /// </summary>
        /// <returns></returns>
        [HttpGet("NotItemForTraining")]
        public IActionResult NotItemForTraining()
        {
            return View();
        }
    }
}