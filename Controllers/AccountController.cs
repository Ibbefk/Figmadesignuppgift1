using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Figmadesign.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Figmadesign.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            _logger.LogInformation("SignIn POST method called");

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model is valid");

                var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} signed in successfully", model.Email);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt");
            }
            else
            {
                _logger.LogWarning("ModelState is invalid");
            }
            return View(model);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            _logger.LogInformation("SignUp POST method called");

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model is valid");

                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    _logger.LogWarning("Email or Password is null or empty");
                    ModelState.AddModelError("", "Email and Password cannot be null or empty");
                    return View(model);
                }

                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} created successfully", model.Email);

                    if (!string.IsNullOrEmpty(model.FirstName))
                    {
                        await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("FirstName", model.FirstName));
                    }
                    if (!string.IsNullOrEmpty(model.LastName))
                    {
                        await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("LastName", model.LastName));
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Error creating user: {Error}", error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                _logger.LogWarning("ModelState is invalid");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var firstName = claims.FirstOrDefault(c => c.Type == "FirstName")?.Value ?? string.Empty;
            var lastName = claims.FirstOrDefault(c => c.Type == "LastName")?.Value ?? string.Empty;

            var model = new UserDetailsViewModel
            {
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = user.PhoneNumber ?? string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(UserDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var claims = await _userManager.GetClaimsAsync(user);

                    var firstNameClaim = claims.FirstOrDefault(c => c.Type == "FirstName");
                    if (firstNameClaim != null)
                    {
                        await _userManager.RemoveClaimAsync(user, firstNameClaim);
                    }
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("FirstName", model.FirstName ?? string.Empty));

                    var lastNameClaim = claims.FirstOrDefault(c => c.Type == "LastName");
                    if (lastNameClaim != null)
                    {
                        await _userManager.RemoveClaimAsync(user, lastNameClaim);
                    }
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("LastName", model.LastName ?? string.Empty));

                    return RedirectToAction("Details");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }
    }
}
