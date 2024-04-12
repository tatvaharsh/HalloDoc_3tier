﻿using hallocdoc_mvc_Service.Implementation;
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


        private readonly IJwtService _jwtService;

        private readonly IConfiguration _configuration;

        public LoginController(IAdmin_Service admin, IJwtService jwtService, IConfiguration configuration, IPhysician_Service p)
        {
            _service = admin;
            _jwtService = jwtService;
            _configuration = configuration;
            _physervice = p;
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

                if (isReg.Id>0)
                {
                    if(isReg.Roles.First().Id==2)
                    {
                        var Admin = _service.getAdmin(model.Email);
                        if (Admin != null)
                        {
                            HttpContext.Session.SetInt32("Id", Admin.AdminId);
                            model.Id=isReg.Id;
                            var token = _jwtService.GenerateJwtToken(model);
                            Response.Cookies.Append("jwt", token);
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
                            var token = _jwtService.GenerateJwtToken(model);
                            Response.Cookies.Append("jwt", token);
                            ViewBag.Username = physician.FirstName;
                            TempData["success"] = "Login Successfully!!!";
                            return RedirectToAction("PhysicianDashboard", "Physician");
                        }
                    }


                    
                }
                TempData["error"] = "Login Failed!!!";
            }

            return View("~/Views/Admin/Admin_Login.cshtml");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Id");
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Admin_Login");
        }
    }
}
