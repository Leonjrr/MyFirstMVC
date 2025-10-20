using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyFirstMVC.Models;

namespace MyFirstMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Role = HttpContext.Session.GetString("Role");
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
