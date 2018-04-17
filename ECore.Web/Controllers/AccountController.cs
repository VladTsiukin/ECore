using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECore.Domain.Entities;
using ECore.Service.CardsServices;
using ECore.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECore.Web.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private ICRUDService<CardsCollection> _iCRUDService;
        private ILogger<AccountController> _logger;

        public AccountController(UserManager<AppUser> userMgr,
                SignInManager<AppUser> signInMgr,
                RoleManager<IdentityRole> roleManager,
                ICRUDService<CardsCollection> iCRUDService,
                ILogger<AccountController> logger)
        {
            _userManager = userMgr;
            _signInManager = signInMgr;
            _roleManager = roleManager;
            _iCRUDService = iCRUDService;
            _logger = logger;
            //ECore.Domain.Context.ECoreIdentityInitializer.EnsurePopulated(userMgr, roleManager).Wait();
        }

        /// <summary>
        /// Login for admin
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM details,
                string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                            await _signInManager.PasswordSignInAsync(
                                user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginVM.Email),
                    "Неверный пароль");
            }
            return View(details);
        }

        /// <summary>
        /// Logout from app
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Log in with google
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult GoogleLogin(string returnUrl)
        {
            string redirectUrl = Url.Action("CallbackResponse", "Account",
                new { ReturnUrl = returnUrl });
            var properties = _signInManager
                .ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        /// <summary>
        /// Log in with facebook
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult FacebookLogin(string returnUrl)
        {
            string redirectUrl = Url.Action("CallbackResponse", "Account",
                new { ReturnUrl = returnUrl });
            var properties = _signInManager
                .ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);
        }
         
        /// <summary>
        /// Responce from google or facebook.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> CallbackResponse(string returnUrl = "/")
        {

            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _logger.LogError("CallbackResponse() ERROR: ExternalLoginInfo = NULL");
                return RedirectToAction(nameof(Login));
            }
            var result = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                string nameDirty = "  " + Guid.NewGuid().ToString();

                AppUser user = new AppUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName =
                        info.Principal.FindFirst(ClaimTypes.Name).Value + nameDirty,
                    EmailConfirmed = true
                };

                user.CardsCollections.Add(new CardsCollection
                {
                    AppUserId = user.Id,
                    Name = "Default",
                    DateOfCreation = DateTimeOffset.Now,
                    Cards = SetDefaultCard()
                });

                // add user in db
                IdentityResult identResult = await _userManager.CreateAsync(user);

                if (identResult.Succeeded)
                {
                    // add info
                    List<Claim> userClaims = info.Principal.Claims.ToList();
                    userClaims.Add( new Claim("Provider", info.ProviderDisplayName));
                    
                    identResult = await _userManager.AddLoginAsync(user, info);
                    IdentityResult identityClaims = await _userManager.AddClaimsAsync(user, userClaims);

                    if (identResult.Succeeded && identityClaims.Succeeded)
                    {                     
                        await _signInManager.SignInAsync(user, false);
                        return Redirect(returnUrl);
                    }
                }

                return AccessDenied();
            }
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        private ICollection<Card> SetDefaultCard()
        {
            List<Card> card = new List<Card> {
                new Card
                {
                    Interval = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    GroupRepetition = 0,
                    Efactor = 2.5,
                    DtoInterval = DateTimeOffset.Now,
                    UserScore = 0,
                    Item = new Item
                    {
                        Face = "PEACE",
                        Back = "МИР"
                    }
                },
                new Card
                {
                    Interval = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    GroupRepetition = 0,
                    Efactor = 2.5,
                    DtoInterval = DateTimeOffset.Now,
                    UserScore = 0,
                    Item = new Item
                    {
                        Face = "LABOR",
                        Back = "ТРУД"
                    }
                },
                new Card
                {
                    Interval = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    GroupRepetition = 0,
                    Efactor = 2.5,
                    DtoInterval = DateTimeOffset.Now,
                    UserScore = 0,
                    Item = new Item
                    {
                        Face = "MAY",
                        Back = "МАЙ"
                    }
                }
            };

            return card;
        }
    }
}
