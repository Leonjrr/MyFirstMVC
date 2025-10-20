using Microsoft.AspNetCore.Mvc;
using MyFirstMVC.Models;
using MyFirstMVC.Services;

namespace MyFirstMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController (IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.LoginAsync(model);

            if (result.Success)
            {
                if (result.TwoFactorRequired)
                {
                    //Store Email for 2FA step
                    TempData["Email"] = model.Email;
                    ViewBag.Message = result.Message;

                    return RedirectToAction("TwoFactor");
                }

                else
                {
                    // Store user ID in session
                    HttpContext.Session.SetString("UserId", result.UserId);
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", result.Message ?? "Login failed");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult TwoFactor()
        {
            var email = TempData["Email"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }
            var model = new TwoFactorRequest 
            {
                Email = email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TwoFactor(TwoFactorRequest model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.VerifyTwoFactorAsync(model);
            
            if(result.Success)
            {
                // Store user ID in session
                HttpContext.Session.SetString("UserId", result.UserId);
                HttpContext.Session.SetString("AccessToken", result.AccessToken);
                HttpContext.Session.SetString("Username", result.Username);
                HttpContext.Session.SetString("Role", result.Role);
                HttpContext.Session.SetString("FirstName", result.FirstName);
                HttpContext.Session.SetString("LastName", result.LastName);
                return RedirectToAction("Dashboard", "Home");
            }

            else
            {
                ModelState.AddModelError("", "Invalid OTP code. Please try again.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if(model.NewPassword != model.ConfirmNewPassword)
            {
                ModelState.AddModelError("ConfirmNewPassword", "Passwords do not match");
                return View(model);
            }

            var result = await _authService.ForgotPasswordAsync(model);

            if (result.IsSuccessful)
            {
                TempData["SuccessMessage"] = result.Message ?? "Password reset successfully";
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("", result.Message ?? "Password reset failed");
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


    }
}
