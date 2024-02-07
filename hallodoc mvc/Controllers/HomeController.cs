using hallodoc_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace hallodoc_mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult patient_screen()
        {
            return View();
        }

        public IActionResult patient_login()
        {
            return View();
        }
        public IActionResult forgot_password()
        {
            return View();
        }
        public IActionResult submit_screen()
        {
            return View();
        }
        public IActionResult patient_form()
        {
            return View();
        }
        public IActionResult family_form()
        {
            return View();
        }
        public IActionResult concierge()
        {
            return View();
        }
        public IActionResult business()
        {
            return View();
        }
        public IActionResult create_patient()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}