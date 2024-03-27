using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO.Compression;
using hallodoc_mvc_Repository.DataContext;
using hallocdoc_mvc_Service.Interface;
using System.Net.Mail;
using System.Net;
using System.Globalization;
using HalloDoc.Auth;
using hallocdoc_mvc_Service.Implementation;

namespace hallodoc_mvc.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IPatient_Service _service;
        private readonly IJwtService _jwtService;


        public HomeController(IPatient_Service service,IJwtService _jwtservice)
        {
            //_context = context;
            _service = service;
            _jwtService = _jwtservice;
        }
        public IActionResult patient_screen()
        {
            return View();
        }

        public IActionResult patient_login()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult patient_login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                bool isReg = _service.ValidateUser(model);

                if (isReg)
                {
                    var user = _service.getUser(model.Email);
                    HttpContext.Session.SetInt32("Userid", user.UserId);
                    HttpContext.Session.SetString("Username", user.FirstName + " " + user.LastName);
                    var token = _jwtService.GenerateJwtToken(model);
                    Response.Cookies.Append("jwt", token);
                    ViewBag.username = user.FirstName + " " + user.LastName;
                    return RedirectToAction("PatientDashboard");
                }
            }

            return View();

           
        }

        public IActionResult forgot_password()
        {
            return View();
        }

        [HttpPost]
        public IActionResult forgot_password(Create model)
        {
            _service.Forgot(model);
            if (ModelState.IsValid)
            {
                _service.Forgotmail(model);
                ViewData["Message"] = "Mail Sent Successfully!!!";

            }


            return View(model);


        }
        public IActionResult ResetPassword()
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
            //var existingUser = _context.AspNetUsers.SingleOrDefault(u => u.Email == Email);
            var existingUser = _service.getUser(Email);
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


        [HttpGet]
        public IActionResult PatientProfile()
        {

            var user1 = HttpContext.Session.GetInt32("Userid");
            var req = _service.getUser(user1);
            DateOnly Mydate = new(req.IntYear.Value, DateOnly.ParseExact(req.StrMonth, "MMM", CultureInfo.InvariantCulture).Month, req.IntDate.Value);

            List<PatientProfile> pp = new List<PatientProfile>();
            pp.Add(new PatientProfile
            {
                user = req,
                BirthDate = Mydate,
            });
            return View(pp);
        }
        public IActionResult editProfile(PatientProfile model)
        {
            var user1 = (int)HttpContext.Session.GetInt32("Userid");
            _service.editprofile(model,user1);
            HttpContext.Session.SetString("Username", model.FirstName + " " + model.LastName);
            return RedirectToAction("PatientProfile");
        }


        [HttpPost]
        public async Task<IActionResult> patient_form([FromForm] patient_form model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _service.PatientForm(model);

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
            _service.FamilyForm(req);

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
            _service.ConciergeForm(req);
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
            _service.Business(req);
           
            return RedirectToAction(nameof(HomeController.submit_screen), "Home");
        }

        public IActionResult create_patient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult create_patient(Create model)
        {
            _service.Update(model);

           

            return View();
        }

        [CustomAuthorize("Patient")]
        [HttpGet]
        public async Task<IActionResult> PatientDashboard()
        {

            if (HttpContext.Session.GetInt32("Userid") == null)
            {
                return RedirectToAction(nameof(patient_login));
            }

            var req = _service.getRequest(HttpContext.Session.GetInt32("Userid"));
            //var req = _context.Requests.Where(u => u.UserId == HttpContext.Session.GetInt32("Userid")).ToList();

            ViewBag.data = req;


            ViewBag.rwfiles = _service.getFiles(); /*_context.RequestWiseFiles.ToList();*/




            return View();
        }



        public IActionResult SubmitSomeoneElse()
        {
            return View();
        }


        [HttpGet]
        public IActionResult SubmitForMe(patient_form pf)
        {
            var user1 =(int) HttpContext.Session.GetInt32("Userid");
       

            _service.SubmitForMe(pf,user1);

        
            return View(pf);
        }

        [HttpPost]
        public IActionResult ForMe(patient_form req)
        {
            var user1 = (int)HttpContext.Session.GetInt32("Userid");
            _service.ForMe(req,user1);
            return RedirectToAction("PatientDashboard");
        }





        [HttpGet]
        public async Task<IActionResult> ViewDocument(int id)
        {
            
            ViewBag.username = HttpContext.Session.GetString("Username");
            //ViewBag.reqwfiles = _context.RequestWiseFiles.Where(u => u.RequestId == id).ToList();
            ViewBag.reqwfiles = _service.getRequestWiseFile(id);
            List<Request> req = _service.getRequestcon(id);

            ViewDocument viewDocument1 = new()
            {
                RequestId = id,
                Confirmationnumber=req.FirstOrDefault().ConfirmationNumber,
            };
            ViewDocument viewDocument = viewDocument1;

            return View(viewDocument);
        }

        [HttpPost]
        public ActionResult DownloadFiles([FromBody] string[] filenames)
        {

            System.Diagnostics.Debug.WriteLine(filenames);
            string repositoryPath = @"D:\Projects\.net learning\hallo_doc\HalloDoc_MVC\hallodoc mvc\wwwroot\uplodedfiles";
            using (MemoryStream zipStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (string filename in filenames)
                    {

                        string filePath = Path.Combine(repositoryPath, filename);
                        System.Diagnostics.Debug.WriteLine(filePath + "/*/*/*/*/*/*/*/*/*/*/*");
                        if (System.IO.File.Exists(filePath))
                        {
                            zipArchive.CreateEntryFromFile(filePath, filename);
                        }
                        else
                        {
                        }
                    }
                }
                zipStream.Seek(0, SeekOrigin.Begin);
                return File(zipStream.ToArray(), "application/zip", "selected_files.zip");
            }
        }


        [HttpPost]
        public IActionResult FileUpload(int id, [FromForm] List<IFormFile> File)
        {
            _service.FileUpload(id, File);
            return RedirectToAction(nameof(ViewDocument), new { id = id });
        }

        public IActionResult Download(int id)
        {
            //var file = _context.RequestWiseFiles.Find(id);
            var file = _service.getRequestWiseFileById(id);
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