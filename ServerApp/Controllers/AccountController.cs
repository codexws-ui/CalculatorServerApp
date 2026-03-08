using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Data;
using ServerApp.Models;
using System.Text.Json;

namespace ServerApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly CalculatorDB db;
        private readonly SignInManager<IdentityUser> SignInManager;
        private readonly UserManager<IdentityUser> UserManager;
        private readonly IUserStore<IdentityUser> UserStore;
        private readonly ILogger<AccountController> Logger;

        public AccountController(
            CalculatorDB _db,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<AccountController> logger)
        {
            db = _db;
            UserManager = userManager;
            UserStore = userStore;
            SignInManager = signInManager;
            Logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string? returnUrl = null)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);   // Clear the existing external cookie to ensure a clean login process

            TempData["ReturnUrl"] = returnUrl ??= Url.Content("~/");
            TempData["ExternalLogins"] = JsonSerializer.Serialize((await SignInManager.GetExternalAuthenticationSchemesAsync()).ToList());

            return View("Login", new LoginVM());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginVM login)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            string returnUrl = (string?)TempData.Peek("ReturnUrl") ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                login.Email = login.Email.Trim();
                login.Password = login.Password.Trim();

                var result = await SignInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    Logger.LogInformation("User logged in.");

                    return Redirect(returnUrl);
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = login.RememberMe });
                }

                if (result.IsLockedOut)
                {
                    Logger.LogWarning("User account locked out.");

                    return RedirectToPage("./Lockout");
                }

                ModelState.AddModelError(string.Empty, "Incorrect Login");
            }

            return View("Login", login);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Register(string? returnUrl = null)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["ReturnUrl"] = returnUrl ??= Url.Content("~/");
            TempData["ExternalLogins"] = JsonSerializer.Serialize((await SignInManager.GetExternalAuthenticationSchemesAsync()).ToList());

            return View("Register", new RegisterVM());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterVM registerVM, string? number = null)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            string returnUrl = (string?)TempData.Peek("ReturnUrl") ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                registerVM.Email = registerVM.Email.Trim();
                registerVM.Password = registerVM.Password.Trim();

                var user = CreateUser();

                await UserStore.SetUserNameAsync(user, registerVM.Email, CancellationToken.None);

                var result = await UserManager.CreateAsync(user, registerVM.Password);

                if (result.Succeeded)
                {
                    Logger.LogInformation("User created a new account with password.");

                    await db.SaveChangesAsync();
                    await SignInManager.SignInAsync(user, isPersistent: false);

                    return Redirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("Register", registerVM);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Logout()
        {
            HttpContext context = Request.HttpContext;

            if (User.Identity?.Name != null)
            {
                Logger.LogInformation($"User '{User.Identity.Name}' logged out.");
            }

            await SignInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
