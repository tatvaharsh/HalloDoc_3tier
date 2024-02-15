
using HalloDoc.Models;
using hallodoc_mvc.Models;
using HalloDocMvc.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
                var user = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.Email);
                var userobj = _context.Users.FirstOrDefault(u => u.AspNetUserId == user.Id);
                if (user != null)
                {
                    if (model.Passwordhash == user.PasswordHash)
                    {
                        HttpContext.Session.SetInt32("Userid", userobj.UserId);
                        HttpContext.Session.SetString("Username", userobj.FirstName + " " + userobj.LastName);
                        return RedirectToAction("PatientDashboard");
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
            var existingUser = _context.AspNetUsers.SingleOrDefault(u => u.Email == Email);
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

        [HttpPost]
        public async Task<IActionResult> patient_form([FromForm] patient_form model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var aspnetuser1 = await _context.AspNetUsers.FirstOrDefaultAsync(u => u.Email == model.Email);
            var user1 = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            /* Debug.WriteLine(aspnetuser);*/

            if (aspnetuser1 == null)
            {
                AspNetUser aspnetuser2 = new AspNetUser
                {

                    UserName = model.FirstName + "_" + model.LastName,
                    Email = model.Email,
                    //PasswordHash = model.Password,
                    PhoneNumber = model.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    PasswordHash = model.Password,
                };
                _context.AspNetUsers.Add(aspnetuser2);
                await _context.SaveChangesAsync();
                aspnetuser1 = aspnetuser2;
            }

            if (user1 == null)
            {
                User user = new User
                {
                    AspNetUserId = aspnetuser1.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.PhoneNumber,
                    ZipCode = model.ZipCode,
                    State = model.State,
                    City = model.City,
                    Street = model.Street,
                    IntDate = model.BirthDate.Day,
                    IntYear = model.BirthDate.Year,
                    StrMonth = (model.BirthDate.Month).ToString("MMM"),
                    CreatedDate = DateTime.Now,
                    CreatedBy = aspnetuser1.Id
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                user1 = user;
            }




            Request request = new Request
            {
                RequestTypeId = 2,
                UserId = user1.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                CreatedDate = DateTime.Now,
                Status = 1,

            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();


            RequestClient requestclient = new RequestClient
            {

                RequestId = request.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Location = model.City,
                Address = model.Street,

                IntDate = model.BirthDate.Day,
                StrMonth = model.BirthDate.Month.ToString(),
                IntYear = model.BirthDate.Year,
                Street = model.Street,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,

            };

            _context.RequestClients.Add(requestclient);
            await _context.SaveChangesAsync();


            foreach (IFormFile files in model.File)
            {
                string filename = model.FirstName + model.LastName + files.FileName;
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\uplodedfiles\\", filename);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    files.CopyToAsync(stream).Wait();
                }


                RequestWiseFile requestWiseFile = new RequestWiseFile();
                requestWiseFile.FileName = filename;
                requestWiseFile.RequestId = request.RequestId;
                requestWiseFile.DocType = 1;
                _context.RequestWiseFiles.Add(requestWiseFile);
                _context.SaveChanges();
            }


            return RedirectToAction(nameof(submit_screen));

        }


        public IActionResult family_form()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> family_form([FromForm] FamilyReqModel req)
        {
            if (!ModelState.IsValid)
            {
                return View(req);
            }

            AspNetUser aspuser = _context.AspNetUsers.FirstOrDefault(m => m.Email == req.Email);
            User usertbl = _context.Users.FirstOrDefault(m => m.Email == req.Email);

            if (aspuser == null)
            {
                AspNetUser aspNetUser = new AspNetUser
                {
                    UserName = req.FirstName,
                    PasswordHash = req.Password,
                    Email = req.Email,
                    PhoneNumber = req.Mobile,
                };
                _context.AspNetUsers.Add(aspNetUser);
                _context.SaveChanges();
                aspuser = aspNetUser;
            }

            if (usertbl == null)
            {
                User user = new()
                {
                    AspNetUserId = aspuser.Id,
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    Email = req.Email,
                    Mobile = req.Mobile,
                    ZipCode = req.ZipCode,
                    State = req.State,
                    City = req.City,
                    Street = req.Street,
                    Status = 1,
                    CreatedBy = aspuser.Id,
                    IntDate = req.DOB.Day,
                    IntYear = req.DOB.Year,
                    StrMonth = req.DOB.ToString("MMM"),
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                usertbl = user;
            }

            Request reqobj = new()
            {
                RequestTypeId = 3,
                UserId = usertbl.UserId,
                FirstName = req.FamFirstName,
                LastName = req.FamLastName,
                Email = req.FamEmail,
                PhoneNumber = req.FamMobile,
                Status = 1,
            };
            _context.Requests.Add(reqobj);
            _context.SaveChanges();

            Region region = new Region
            {
                Name = req.City,
            };
            _context.Regions.Add(region);
            _context.SaveChanges();


            RequestClient rc = new RequestClient
            {
                RequestId = reqobj.RequestId,
                FirstName = req.FirstName,
                LastName = req.LastName,
                PhoneNumber = req.Mobile,
                Location = req.Room,
                Address = req.Street + ", " + req.City + ", " + req.State,
                Notes = req.Symptoms,
                Email = req.Email,
                RegionId = region.RegionId,
                IntDate = req.DOB.Day,
                IntYear = req.DOB.Year,
                StrMonth = req.DOB.ToString("MMM"),
                Street = req.Street,
                City = req.City,
                State = req.State,
                ZipCode = req.ZipCode,
            };
            _context.RequestClients.Add(rc);
            _context.SaveChanges();

            return RedirectToAction(nameof(HomeController.submit_screen), "Home");
        }

        [HttpGet]
        public IActionResult concierge_form()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> concierge_form([FromForm] ConciergeReqModel req)
        {
            if (!ModelState.IsValid)
            {
                return View(req);
            }

            AspNetUser aspuser = _context.AspNetUsers.FirstOrDefault(m => m.Email == req.Email);
            User usertbl = _context.Users.FirstOrDefault(m => m.Email == req.Email);

            if (aspuser == null)
            {
                AspNetUser aspNetUser = new AspNetUser
                {
                    UserName = req.FirstName,
                    PasswordHash = req.Password,
                    Email = req.Email,
                    PhoneNumber = req.Mobile,
                };
                _context.AspNetUsers.Add(aspNetUser);
                _context.SaveChanges();
                aspuser = aspNetUser;
            }

            if (usertbl == null)
            {
                User user = new User
                {
                    AspNetUserId = aspuser.Id,
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    Email = req.Email,
                    Mobile = req.Mobile,
                    Status = 1,
                    IntDate = req.DOB.Day,
                    IntYear = req.DOB.Year,
                    StrMonth = req.DOB.ToString("MMM"),
                    CreatedBy = aspuser.Id
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                usertbl = user;
            }


            Request reqobj = new Request
            {
                RequestTypeId = 4,
                UserId = usertbl.UserId,
                FirstName = req.ConFirstName,
                LastName = req.ConLastName,
                Email = req.ConEmail,
                PhoneNumber = req.ConMobile,
                Status = 1,
            };
            _context.Requests.Add(reqobj);
            _context.SaveChanges();


            Region region = new Region
            {
                Name = req.City,
            };
            _context.Regions.Add(region);
            _context.SaveChanges();


            RequestClient rc = new RequestClient
            {
                RequestId = reqobj.RequestId,
                FirstName = req.FirstName,
                LastName = req.LastName,
                PhoneNumber = req.Mobile,
                Location = req.Room,
                Address = req.Street + ", " + req.City + ", " + req.State,
                Notes = req.Symptoms,
                Email = req.Email,
                RegionId = region.RegionId,
                IntDate = req.DOB.Day,
                IntYear = req.DOB.Year,
                StrMonth = req.DOB.ToString("MMM"),
                Street = req.Street,
                City = req.City,
                State = req.State,
                ZipCode = req.ZipCode,
            };
            _context.RequestClients.Add(rc);
            _context.SaveChanges();


            Concierge con = new Concierge
            {
                ConciergeName = req.ConFirstName + " " + req.ConLastName,
                Address = req.Property,
                Street = req.Street,
                City = req.City,
                ZipCode = req.ZipCode,
                State = req.State,
                RegionId = region.RegionId,
            };
            _context.Concierges.Add(con);
            _context.SaveChanges();


            RequestConcierge reqCon = new RequestConcierge
            {
                RequestId = reqobj.RequestId,
                ConciergeId = con.ConciergeId,
            };
            _context.RequestConcierges.Add(reqCon);
            _context.SaveChanges();

            return RedirectToAction(nameof(HomeController.submit_screen), "Home");
        }

        public IActionResult business()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> business([FromForm] BusinessReqModel req)
        {
            if (!ModelState.IsValid)
            {
                return View(req);
            }

            AspNetUser aspuser = _context.AspNetUsers.FirstOrDefault(m => m.Email == req.Email);
            User usertbl = _context.Users.FirstOrDefault(m => m.Email == req.Email);

            if (aspuser == null)
            {
                AspNetUser aspNetUser = new AspNetUser
                {
                    UserName = req.FirstName,
                    PasswordHash = req.Password,
                    Email = req.Email,
                    PhoneNumber = req.Mobile,
                };
                _context.AspNetUsers.Add(aspNetUser);
                _context.SaveChanges();
                aspuser = aspNetUser;
            }

            if (usertbl == null)
            {
                User user = new User
                {
                    AspNetUserId = aspuser.Id,
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    Email = req.Email,
                    Mobile = req.Mobile,
                    ZipCode = req.ZipCode,
                    State = req.State,
                    City = req.City,
                    Street = req.Street,
                    Status = 1,
                    CreatedBy = aspuser.Id,
                    IntDate = req.DOB.Day,
                    IntYear = req.DOB.Year,
                    StrMonth = req.DOB.ToString("MMM"),
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                usertbl = user;
            }

            Request reqobj = new Request
            {
                RequestTypeId = 1,
                UserId = usertbl.UserId,
                FirstName = req.BusFirstName,
                LastName = req.BusLastName,
                Email = req.BusEmail,
                PhoneNumber = req.BusMobile,
                Status = 1,
            };
            _context.Requests.Add(reqobj);
            _context.SaveChanges();

            Region region = new Region
            {
                Name = req.City,
            };
            _context.Regions.Add(region);
            _context.SaveChanges();


            RequestClient rc = new RequestClient
            {
                RequestId = reqobj.RequestId,
                FirstName = req.FirstName,
                LastName = req.LastName,
                PhoneNumber = req.Mobile,
                Location = req.Room,
                Address = req.Street + ", " + req.City + ", " + req.State,
                Notes = req.Symptoms,
                Email = req.Email,
                RegionId = region.RegionId,
                IntDate = req.DOB.Day,
                IntYear = req.DOB.Year,
                StrMonth = req.DOB.ToString("MMM"),
                Street = req.Street,
                City = req.City,
                State = req.State,
                ZipCode = req.ZipCode,
            };
            _context.RequestClients.Add(rc);
            _context.SaveChanges();

            Business business = new Business
            {
                Name = req.BusFirstName,
                Address1 = req.Property,
                PhoneNumber = req.BusMobile,
                RegionId = region.RegionId,
            };
            _context.Businesses.Add(business);
            _context.SaveChanges();


            RequestBusiness reqBus = new RequestBusiness
            {
                RequestId = reqobj.RequestId,
                BusinessId = business.BusinessId,
            };
            _context.RequestBusinesses.Add(reqBus);
            _context.SaveChanges();

            return RedirectToAction(nameof(HomeController.submit_screen), "Home");
        }

        public IActionResult create_patient()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> PatientDashboard()
        {

            if (HttpContext.Session.GetInt32("Userid") == null)
            {
                return RedirectToAction(nameof(patient_login));
            }

            var req = _context.Requests.Where(u => u.UserId == HttpContext.Session.GetInt32("Userid")).ToList();
            ViewBag.username = HttpContext.Session.GetString("Username");
            ViewBag.data = req;
            //ViewBag.username = HttpContext.Session.GetString("username");

            //return  _context.Requests!=null?
            //    View(_context.Requests.ToList())
            //    :
            //    Problem("EntitySet is emapty")
            //    ;
            ViewBag.rwfiles = _context.RequestWiseFiles.ToList();




            return View();
        }

        public IActionResult SubmitSomeoneElse()
        {
            return View();
        }

        public IActionResult SubmitForMe()
        {

            return View();
        }
        [HttpGet]
        public IActionResult PatientProfile()
        {
            var user1 = HttpContext.Session.GetInt32("Userid");
            var req = _context.Users.FirstOrDefault(u => u.UserId == user1);

            List<PatientProfile> pp = new List<PatientProfile>();
            pp.Add(new PatientProfile
            {
                user = req,
            });
            return View(pp);
        }

        public IActionResult editProfile(PatientProfile model)
        {
            var user1 = HttpContext.Session.GetInt32("Userid");
            var req = _context.Users.FirstOrDefault(u => u.UserId == user1);
            var asp = _context.AspNetUsers.FirstOrDefault(u => u.Id == req.AspNetUserId);

            req.FirstName = model.FirstName;
            req.LastName = model.LastName;
            req.Email = model.Email;
            req.Mobile = model.PhoneNumber;
            req.StrMonth = model.BirthDate.Month.ToString();
            req.IntYear = model.BirthDate.Year;
            req.IntDate = model.BirthDate.Day;
            req.Street = model.Street;
            req.City = model.City;
            req.State = model.State;
            req.ZipCode = model.ZipCode;
            _context.Users.Update(req);
            _context.SaveChanges();

            asp.UserName = model.FirstName;
            asp.Email = model.Email;
            asp.PhoneNumber = model.PhoneNumber;
            _context.AspNetUsers.Update(asp);
            _context.SaveChanges();

            return RedirectToAction("PatientProfile");
        }

        [HttpGet]
        public async Task<IActionResult> ViewDocument(int id)
        {
            ViewBag.username = HttpContext.Session.GetString("Username");
            ViewBag.reqwfiles = _context.RequestWiseFiles.Where(u => u.RequestId == id).ToList();


            return View();
        }

        public IActionResult Download(int id)
        {
            var file = _context.RequestWiseFiles.Find(id);
            var filepath = "D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\uplodedfiles\\" + file.FileName;
            var bytes = System.IO.File.ReadAllBytes(filepath);
            return File(bytes, "aplication/octet-stream", file.FileName);
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