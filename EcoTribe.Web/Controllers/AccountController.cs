using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace EcoTribe.Web.Controllers
{
    public class AccountController : Controller
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IVolunteerService _volunteerService;

        public AccountController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IVolunteerService volunteerService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _volunteerService = volunteerService;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVolunteerInputModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
        

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                model.Latitude = Convert.ToDecimal(model.Latitude, CultureInfo.InvariantCulture);
                model.Longitude = Convert.ToDecimal(model.Longitude, CultureInfo.InvariantCulture);

                var volunteer = new VolunteerInputModel
                {
                    Name = model.Name,
                    City = model.City,
                    Email = model.Email,
                    Skills = model.Skills,
                    PreferredEvents = model.PreferredEvents,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Number = model.Number,
                    Instagram = model.Instagram,
                    Facebook = model.Facebook
                };

                _volunteerService.Create(volunteer);

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

