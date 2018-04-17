using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ECore.Domain.Entities;
using ECore.Web.Models.ViewModels;
using ECore.Service.CardsServices;

namespace ECore.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private UserManager<AppUser> _userManager;
        private IUserValidator<AppUser> _userValidator;
        private IPasswordValidator<AppUser> _passwordValidator;
        private IPasswordHasher<AppUser> _passwordHasher;       

        public AdminController(UserManager<AppUser> usrMgr,
                IUserValidator<AppUser> userValid,
                IPasswordValidator<AppUser> passValid,
                IPasswordHasher<AppUser> passwordHash)
        {
            _userManager = usrMgr;
            _userValidator = userValid;
            _passwordValidator = passValid;
            _passwordHasher = passwordHash;
        }

        /// <summary>
        /// Show all users.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {

            if (!User.IsInRole("adminvictestvt"))
            {
                return RedirectToAction("Index", "Claims");
            }

            return View(_userManager.Users);
        }

        /// <summary>
        /// Create user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CreateUser() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserVM model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");

                } else
                {
                    AddErrorsFromResult(result);
                }

            }
            return View(model);
        }

        /// <summary>
        /// User self delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromForm] string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {

                IdentityResult result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(AccountController.Logout), "Account");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Удаление невозможно в данный момент...");
            }
            return RedirectToAction(nameof(ClaimsController.Index), "Claims");
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                if (user.Email.ToLower() == "victestvt@yandex.ru")
                {
                    return RedirectToAction("Index");
                }

                IdentityResult result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index", _userManager.Users);
        }


        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditUser(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                EditUserVM editUser = new EditUserVM
                {
                    Id = user.Id,
                    Email = user.Email
                };

                return View(editUser);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Check hash password, email.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserVM _user)
        {
            if (_user == null)
            {
                ModelState.AddModelError("", "null user 'EditUser'");
                return RedirectToAction("Index");
            }

            if (_user.Email.ToLower() == "victestvt@yandex.ru")
            {
                return RedirectToAction("Index");
            }

            AppUser user = await _userManager.FindByIdAsync(_user.Id);

            if (user != null)
            {
                user.Email = _user.Email;

                IdentityResult validEmail
                    = await _userValidator.ValidateAsync(_userManager, user);

                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;

                if (!string.IsNullOrEmpty(_user.Password))
                {
                    validPass = await _passwordValidator.ValidateAsync(_userManager,
                        user, _user.Password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user,
                            _user.Password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validEmail.Succeeded && validPass == null)
                        || (validEmail.Succeeded
                        && _user.Password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(_user);
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
}