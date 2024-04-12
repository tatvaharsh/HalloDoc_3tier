using Microsoft.AspNetCore.Mvc;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using hallocdoc_mvc_Service.Implementation;
using hallocdoc_mvc_Service.Interface;
using HalloDoc.Auth;
using ClosedXML.Excel;
using System.IdentityModel.Tokens.Jwt;

namespace hallodoc_mvc.Controllers
{
    public class PhysicianController : Controller
    {
        private readonly IPhysician_Service _Service;

        private readonly IJwtService _jwtService;

        private readonly IConfiguration _configuration;

        public PhysicianController(IPhysician_Service service, IJwtService jwtService, IConfiguration configuration)
        {
            //_context = context;
            _Service = service;
            _jwtService = jwtService;
            _configuration = configuration;
        }


        [CustomAuthorize("Physician")]
        public IActionResult PhysicianDashboard()
        {
            int admin1 = (int)HttpContext.Session.GetInt32("PhyId");

            if (admin1 != null)
            {
                ViewBag.Username = _Service.Phyname(admin1);
                //ModalData data = _service.GetAssignData(new ModalData());
                //TempData["success"] = "Login Successfully!!!";
                //TempData.Clear();
                //ViewBag.Layout = "_LayAdmin";
                ModalData MD = _Service.CountState(admin1);
                return View(MD);
            }
            return RedirectToAction("Admin_Login");
        }


        public IActionResult New(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            if (pageid == 0) { pageid = 1; };
            var data = _Service.GetNewData(requestType, search, requestor, region, pageid,phy);
            ViewBag.page = pageid;
            return PartialView("~/Views/Shared/Physician/_Physician_Request_Table.cshtml", data);
        }
        public IActionResult Pending(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            if (pageid == 0) { pageid = 1; };
            var data = _Service.getDashDataPending(requestType, search, requestor, region, pageid, phy);
            //AdminDashboard adminDashboard = new AdminDashboard();
            ViewBag.page = pageid;
            return PartialView("~/Views/Shared/Physician/_Physician_Request_Table.cshtml", data);
        }
        public IActionResult Active(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            if (pageid == 0) { pageid = 1; };
            var data = _Service.getDashDataActive(requestType, search, requestor, region, pageid, phy);
            //AdminDashboard adminDashboard = new AdminDashboard();
            ViewBag.page = pageid;
            return PartialView("~/Views/Shared/Physician/_Physician_Request_Table.cshtml", data);
        }

        public IActionResult Conclude(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            if (pageid == 0) { pageid = 1; };
            var data = _Service.getDashDataConclude(requestType, search, requestor, region, pageid, phy);
            //AdminDashboard adminDashboard = new AdminDashboard();
            ViewBag.page = pageid;
            return PartialView("~/Views/Shared/Physician/_Physician_Request_Table.cshtml", data);
        }
        public IActionResult Sendlink()
        {
            return PartialView();
        }

        public IActionResult Accept(int id) 
        {
            _Service.AddToPending(id);
            return View("PhysicianDashboard");
        }
        public IActionResult ViewCase(int Id)
        {
            ViewCase vc = _Service.Getcase(Id);
            return View(vc);
        }
        [HttpGet]
        public IActionResult ViewNotes(int Id)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            ViewBag.Username = _Service.Phyname(phy);
            ViewNote rn = _Service.GetNotes(Id);
            return View(rn);
        }

        //[HttpPost]
        //public IActionResult ViewNotes(ViewNote model, int Id)
        //{
        //    int phy = (int)HttpContext.Session.GetInt32("PhyId");
        //    ViewBag.Username = _Service.Phyname(phy);
        //    var vn = _Service.setViewNotesData(model, Id, phy);
        //    TempData["success"] = "Note Added Successfully!!!";
        //    return RedirectToAction("ViewNotes", Id);
        //}
    }
}
