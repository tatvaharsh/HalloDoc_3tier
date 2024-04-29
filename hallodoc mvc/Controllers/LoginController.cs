using hallocdoc_mvc_Service.Implementation;
using hallocdoc_mvc_Service.Interface;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hallodoc_mvc.Controllers
{
    public class LoginController : Controller
    {
         private readonly IAdmin_Service _service;
        private readonly IPhysician_Service _physervice;
        private readonly IPatient_Service _Service;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public LoginController(IAdmin_Service admin, IJwtService jwtService, IConfiguration configuration, IPhysician_Service p, IPatient_Service Service)
        {
            _service = admin;
            _jwtService = jwtService;
            _configuration = configuration;
            _physervice = p;
            _Service = Service;
        }


        public IActionResult Admin_Login()
        {

            return View("~/Views/Admin/Admin_Login.cshtml");
        }

        [HttpPost]
        public IActionResult Admin_Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                AspNetUser isReg = _service.ValidateUser(model);

                if (isReg.Id > 0)
                {
                    try
                    {
                        if (isReg.Roles.First().Id == 2)
                        {
                            var Admin = _service.getAdmin(model.Email);
                            if (Admin != null)
                            {
                                HttpContext.Session.SetInt32("Id", Admin.AdminId);
                                model.Id = isReg.Id;
                                var (token, menus) = _jwtService.GenerateJwtToken(model, "Admin");
                                Response.Cookies.Append("jwt", token);
                                HttpContext.Session.SetString("menus", menus);
                                ViewBag.Username = Admin.FirstName;
                                TempData["success"] = "Login Successfully!!!";
                                return RedirectToAction("Admin_Dashboard", "Admin");
                            }
                        }
                        else if (isReg.Roles.First().Id == 3)
                        {
                            Physician physician = _physervice.getPhy(model.Email);
                            if (physician != null)
                            {
                                HttpContext.Session.SetInt32("PhyId", physician.PhysicianId);
                                model.Id = isReg.Id;
                                var (token, menus) = _jwtService.GenerateJwtToken(model, "Provider");
                                Response.Cookies.Append("jwt", token);
                                HttpContext.Session.SetString("menus", menus);
                                ViewBag.Username = physician.FirstName;
                                TempData["success"] = "Login Successfully!!!";
                                return RedirectToAction("PhysicianDashboard", "Physician");
                            }
                        }
                        else
                        {
                            var user = _Service.getUser(model.Email);
                            HttpContext.Session.SetInt32("Userid", user.UserId);
                            HttpContext.Session.SetString("Username", user.FirstName + " " + user.LastName);
                            model.Id = isReg.Id;
                            var (token, menus) = _jwtService.GenerateJwtToken(model, "Patient");
                            Response.Cookies.Append("jwt", token);
                            HttpContext.Session.SetString("menus", menus);
                            ViewBag.username = user.FirstName + " " + user.LastName;
                            TempData["success"] = "Login Successfully!!!";
                            return RedirectToAction("PatientDashboard", "Home");
                        }

                    }
                    catch
                    {
                        TempData["error"] = "Invalid Creadentails!!!";
                    }




                }
                TempData["error"] = "Login Failed!!!";
            }

            return View("~/Views/Home/patient_login.cshtml");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Id");
            Response.Cookies.Delete("jwt");
            return RedirectToAction("patient_login", "Home");
        }
    }
}
