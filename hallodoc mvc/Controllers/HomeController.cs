using HalloDoc.Models;
using hallodoc_mvc.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using HalloDocMvc.ViewModel;
using System.Security.Cryptography;

namespace hallodoc_mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly HallodocContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(HallodocContext context)
        {
            _context = context;
        }
/*
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        public IActionResult patient_screen()
        {
            return View();
        }

        public IActionResult patient_login()
        {
            return View();
        }
        /*public async Task<IActionResult> Login([Bind("Email,Passwordhash")] Aspnetuser aspnetuser)
        {

            var user = await _context.Aspnetusers
                .FirstOrDefaultAsync(m => m.Email == aspnetuser.Email && m.Passwordhash == aspnetuser.Passwordhash);
            if (user == null)
            {
                return RedirectToAction(nameof(HomeController.patient_login), "Home");
            }

            return RedirectToAction(nameof(HomeController.submit_screen), "Home");
        }*/
        [HttpPost]
        public IActionResult patient_login(LoginViewModel model)
        {


            if (ModelState.IsValid)
            {
                var user = _context.Aspnetusers.FirstOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    if (model.Passwordhash == user.Passwordhash)
                    {
                        return RedirectToAction("submit_screen");
                    }
                    else
                    {
                        ModelState.AddModelError("Passwordhash", "Incorrect Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Incorrect Username");
                }
            }

            // If we reach here, something went wrong, return the same view with validation errors
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
        public IActionResult PatientCheck(string Email)
        {
            var existingUser = _context.Aspnetusers.SingleOrDefault(u => u.Email == Email);
            bool isValidEmail; 
            if (existingUser == null)
            {
                isValidEmail = false;
            }
            else
            {
                isValidEmail = true;
            }
            return Json(new { isValid = isValidEmail });
        }

        public IActionResult PatientRequestForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PatientRequestForm(patient_form model)
        {

            Aspnetuser aspnetuser = _context.Aspnetusers.FirstOrDefault(u => u.Email == model.Email);

            Debug.WriteLine(aspnetuser);

            if (aspnetuser == null)
            {
                Aspnetuser aspnetuser1 = new Aspnetuser
                {
                    Id = "2",
                    Username = model.FirstName + "_" + model.LastName,
                    Email = model.Email,
                    Passwordhash = model.FirstName,
                    Phonenumber = model.PhoneNumber,
                    Createddate = DateTime.Now,
                };
                _context.Aspnetusers.Add(aspnetuser1);
                _context.SaveChangesAsync();
                aspnetuser = aspnetuser1;
            }

            User user = new User
            {
                Userid = 1,
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                Mobile = model.PhoneNumber,
                Zipcode = model.ZipCode,
                State = model.State,
                City = model.City,
                Street = model.Street,
                Intdate = model.BirthDate.Day,
                Intyear = model.BirthDate.Year,
                Strmonth = (model.BirthDate.Month).ToString(),
                Createddate = DateTime.Now,

                CreatedbyNavigation = aspnetuser,
                Aspnetuser = aspnetuser,
            };
            _context.Users.Add(user);
            _context.SaveChangesAsync();
            Request request = new Request
            {
                Requesttypeid = 2,
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Phonenumber = model.PhoneNumber,
                Email = model.Email,
                Createddate = DateTime.Now,
                Status = 1,
                User = user,
            };

            _context.Requests.Add(request);
            _context.SaveChangesAsync();

            return RedirectToAction(nameof(submit_screen));

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