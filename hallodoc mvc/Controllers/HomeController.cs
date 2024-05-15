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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Diagnostics;
using Twilio.TwiML.Voice;   

namespace hallodoc_mvc.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IPatient_Service _service;
        private readonly IJwtService _jwtService;


        public HomeController(IPatient_Service service, IJwtService _jwtservice)
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
  
        public IActionResult forgot_password()
        {
            return View();
        }

        [HttpPost]
        public IActionResult forgot_password(Create model)
        {

            if (ModelState.IsValid)
            {
                _service.Forgotmail(model);
                ViewData["Message"] = "Mail Sent Successfully!!!";

            }


            return View(model);

        }
        public IActionResult ResetPassword(string token)
        {

            Response.Cookies.Append("create", token);
            if (_jwtService.ValidateJwtToken(token, out JwtSecurityToken jwtSecurityToken))
            {
                return View();
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult ResetPassword(Create model)
        {
            var cookie = Request.Cookies["create"];
            var email = new JwtSecurityTokenHandler().ReadJwtToken(cookie).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var Email = _service.getAspUser(email);


            if (Email == null)
            {
                ModelState.AddModelError(nameof(model.PasswordHash), "Please enter your valid email!");
                return View();
            }

            if (model.PasswordHash == null || model.CPasswordHash == null)
            {
                ModelState.AddModelError(nameof(model.PasswordHash), "Please enter your credentials!");
                return View();
            }

            if (model.PasswordHash != model.CPasswordHash)
            {
                ModelState.AddModelError(nameof(model.PasswordHash), "Please check your credentials");
                return View();
            }

            _service.Resetpass(model, email);
            return RedirectToAction("patient_login");
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
            _service.editprofile(model, user1);
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

        public IActionResult create_patient(string token)
        {
            Response.Cookies.Append("create", token);
            var email = new JwtSecurityTokenHandler().
                ReadJwtToken(token).
                Claims.
                FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (_jwtService.ValidateJwtToken(token, out JwtSecurityToken jwtSecurityToken))
            {
                Create vm = new()
                {
                    UserName = email
                };
                return View(vm);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult create_patient(Create model)
        {
            var cookie = Request.Cookies["create"];
            var email = new JwtSecurityTokenHandler().
                ReadJwtToken(cookie).
                Claims.
                FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (model.UserName == null || model.PasswordHash == null)
            {
                ModelState.AddModelError(nameof(model.PasswordHash), "Please enter your credentials!");
                return View();
            }

            if (email != model.UserName)
            {
                ModelState.AddModelError(nameof(model.UserName), "Please use your registered email address!");
                return View();
            }

            _service.Update(model);
            return View();
        }

        [CustomAuthorize(null, "Patient")]
        [HttpGet]
        public async Task<IActionResult> PatientDashboard()
        {
            PatientDashboard patientDashboard = new()
            {
                requests = _service.getRequest(HttpContext.Session.GetInt32("Userid")),
                rwfiles = _service.getFiles(),
                adminData = _service.admindata(),
            };
  
            return View(patientDashboard);
        }



        public IActionResult SubmitSomeoneElse()
        {
            return View();
        }


        [HttpGet]
        public IActionResult SubmitForMe(patient_form pf)
        {
            var user1 = (int)HttpContext.Session.GetInt32("Userid");


            _service.SubmitForMe(pf, user1);


            return View(pf);
        }

        [HttpPost]
        public IActionResult ForMe(patient_form req)
        {
            var user1 = (int)HttpContext.Session.GetInt32("Userid");
            _service.ForMe(req, user1);
            return RedirectToAction("PatientDashboard");
        }

        [HttpPost]
        public IActionResult SubmitSomeoneElse(patient_form req)
        {
            var user1 = (int)HttpContext.Session.GetInt32("Userid");
            _service.ForElse(req, user1);
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
                Confirmationnumber = req.FirstOrDefault().ConfirmationNumber,
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

        public IActionResult Access()
        {
            return PartialView("_AccessDenied");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return View(new hallodoc_mvc_Repository.ViewModel.Error()
            {
                Path = exceptionDetails?.Path,
                Message = exceptionDetails?.Error?.Message,
                Stack = exceptionDetails?.Error?.StackTrace,
            });

        }

        public IActionResult Chat(int RequestId, int AdminID, int ProviderId, int FlagId)
        {
            var roleMain = HttpContext.Session.GetInt32("RoleId");
            ChatViewModel model = _service.GetChats(RequestId, AdminID, ProviderId, (int)roleMain, FlagId);
            return PartialView("_ChatArea", model);
        }
    }
}