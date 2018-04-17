using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using ECore.Service.CardsServices;
using ECore.Domain.Entities;
using ECore.Web.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ECore.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class CardsController : Controller
    {
        private readonly ICRUDService<CardsCollection> _crudCardsService;
        private readonly ICRUDService<Card> _crudCardService;
        private readonly ICardsService<CardsCollection> _cardsService;
        private readonly ICardService<Card> _cardService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<CardsController> _logger;

        IQueryable<CardsCollection> cards;

        public CardsController(ICRUDService<CardsCollection> crudCardsService,
                               ICardsService<CardsCollection> cardsService,
                               ICRUDService<Card> crudCardService,
                               UserManager<AppUser> userManager,
                               ICardService<Card> cardService,
                               ILogger<CardsController> logger)
        {
            _cardService = cardService;
            _crudCardService = crudCardService;
            _crudCardsService = crudCardsService;
            _cardsService = cardsService;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Show cards collections by sort.
        /// </summary>
        /// <returns></returns>
        [Route("{sort = null}")]
        public IActionResult AllCards(SortState? sort = null)
        {
            try
            {
                string id = GetCurrentUserId();

                if (id == String.Empty)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }

                ViewData["Date"] = SortState.Date;
                ViewData["Name"] = SortState.Name;

                switch (sort)
                {
                    case SortState.Name:
                        cards = _cardsService.GetCardsByUserId(id)
                                             .OrderBy(c => c.Name);
                        break;
                    case SortState.Date:
                        cards = _cardsService.GetCardsByUserId(id)
                                             .OrderBy(c => c.DateOfCreation);
                        break;
                    default:
                        cards = _cardsService.GetCardsByUserId(id);
                        break;
                }

                return View(cards.ToList());
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
            
        }

        
        /// <summary>
        /// Get current user id
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
        /// Create cards collection.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCards([FromForm] string name)
        {
            if (ModelState.IsValid)
            {
                string id = GetCurrentUserId();

                // if user id not found => redirect to login view
                if (id == String.Empty)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }

                CardsCollection cardsCollection = new CardsCollection
                {
                    AppUserId = id,
                    DateOfCreation = DateTimeOffset.Now,
                    Name = name
                };

                var result = await _crudCardsService.AddAsync(cardsCollection);

                if (result != null)
                {                    
                    return RedirectToAction("AllCards");
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

            }
            return RedirectToAction("AllCards");
        }


        /// <summary>
        /// Add card to cards collection.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddCard(int cardsId, string cardsName)
        {
            ViewData["cardsId"] = cardsId;
            ViewData["cardsName"] = cardsName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCard(CardVM model, int idCards)
        {
            string id = GetCurrentUserId();

            // if user id not found => redirect to login view
            if (id == String.Empty)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            if (ModelState.IsValid)
            {
                Card card = new Card
                {
                    Interval = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    GroupRepetition = 0,
                    Efactor = 3.0,
                    DtoInterval = DateTimeOffset.Now,
                    UserScore = 0,
                    CardsId = idCards,
                    Item = new Item
                    {
                        Face = model.Item.Face,
                        Back = model.Item.Back
                    }
                };

                var result = await _crudCardService.AddAsync(card);

                if (result != null)
                {
                    return RedirectToAction("AllCards");
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

            }
            return View(model);
        }


        /// <summary>
        /// Delete cards collection.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id, string name)
        {

            if (name.ToLower() == "default")
            {
                return RedirectToAction("AllCards");
            }

            CardsCollection cardsVM = await _cardsService.GetCardsById(id);

            if (cardsVM != null)
            {
                Task<bool> result = _crudCardsService.DeleteAsync(cardsVM);

                if (result.Result == true)
                {
                    return RedirectToAction("AllCards");
                }
                else
                {
                    ModelState.AddModelError("", "Collection Not Delete");
                }
            }
            else
            {
                ModelState.AddModelError("", "Collection Not Found");
            }
            return View("AllCards");
        }
       

        /// <summary>
        /// Show errors.
        /// </summary>
        /// <param name="result"></param>
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }



    }


    /// <summary>
    /// For sorting cards collections.
    /// </summary>
    public enum SortState
    {
        Name,
        Date
    }
}