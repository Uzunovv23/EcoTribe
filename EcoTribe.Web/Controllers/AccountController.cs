using EcoTribe.BusinessObjects.Domain.Models;
using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Net;

namespace EcoTribe.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IVolunteerService _volunteerService;
        private readonly IEmailService _emailService;
        private readonly UrlEncoder _urlEncoder;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IVolunteerService volunteerService,
            IEmailService emailService,
            UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _volunteerService = volunteerService;
            _emailService = emailService;
            _urlEncoder = urlEncoder;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && !user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "You need to confirm your email before logging in.");
                return View(model);
            }

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

                var volunteer = new VolunteerInputModel
                {
                    UserId = user.Id,
                    Name = model.Name,
                    City = model.City,
                    Email = model.Email,
                    Skills = model.Skills,
                    PreferredEvents = model.PreferredEvents,
                    Number = model.Number,
                    Instagram = model.Instagram,
                    Facebook = model.Facebook
                };

                _volunteerService.Create(volunteer);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = UrlEncoder.Default.Encode(token);

                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    new { userId = user.Id, token = encodedToken },
                    protocol: HttpContext.Request.Scheme);

                await _emailService.SendEmail(user.Email, "Confirm your email",
                    $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>");

                return View("RegistrationSuccess");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest("Invalid email confirmation request.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var decodedToken = WebUtility.UrlDecode(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
                return View("ConfirmEmail"); 

            return BadRequest("Email confirmation failed.");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
