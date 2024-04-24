using Assignment.Models;
using AssignmentRepository.ViewModel;
using AssignmentService.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAssignmentService _Service;

        public HomeController(ILogger<HomeController> logger,IAssignmentService service)
        {
            _Service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
         
            return View();
        }
        public IActionResult TableData(string search,int page)
        {
            if (page == 0) { page = 1; };
            ViewBag.page = page;
            var a = _Service.GetData(search,page);
            return PartialView("_DataTable",a);
        }
        public IActionResult DeleteData(int id)
        {
            _Service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddUser()
        {
            return PartialView("_AddUser");
        }
        public IActionResult OpenUser(int Id)
        {
            var a = _Service.GetSelectedData(Id);
            return PartialView("_AddUser",a);
        }

        [HttpPost]
        public IActionResult AddUser(UserModel model)
        {
            _Service.AddUser(model);
            return RedirectToAction(nameof(Index));
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