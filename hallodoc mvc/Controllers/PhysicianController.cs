using Microsoft.AspNetCore.Mvc;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using hallocdoc_mvc_Service.Implementation;
using hallocdoc_mvc_Service.Interface;
using HalloDoc.Auth;
using ClosedXML.Excel;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;

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
            return RedirectToAction(nameof(PhysicianDashboard));
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

        [HttpPost]
        public IActionResult ViewNotes(ViewNote model, int Id)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            ViewBag.Username = _Service.Phyname(phy);
            var vn = _Service.setViewNotesData(model, Id, phy);
            TempData["success"] = "Note Added Successfully!!!";
            return RedirectToAction("ViewNotes", Id);
        }
        [HttpGet]
        public IActionResult SendAgreement(int Id, int requestType)
        {
            ViewBag.requestType = requestType;

            RequestClient rc = _Service.GetAgreementtdata(Id);
            ModalData md = new()
            {
                number = rc.PhoneNumber,
                email = rc.Email,
            };
            return PartialView("SendAgreement", md);
        }
        [HttpPost]
        public IActionResult SendAgreement(int Id, ModalData md)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            var token = _jwtService.GenerateJwtTokenByEmail(md.email);

            _Service.SendAgreementMail(Id, md, token, phy);
            return RedirectToAction(nameof(PhysicianDashboard));
        }
        public IActionResult ViewUpload(int id)
        {

            ViewDocument vd = _Service.ViewUploadData(id);
            return View(vd);
        }



        //[HttpPost]
        //public ActionResult DownloadFiles([FromBody] string[] filenames)
        //{

        //    System.Diagnostics.Debug.WriteLine(filenames);
        //    string repositoryPath = @"D:\Projects\.net learning\hallo_doc\HalloDoc_MVC\hallodoc mvc\wwwroot\uplodedfiles";
        //    using (MemoryStream zipStream = new MemoryStream())
        //    {
        //        using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        //        {
        //            foreach (string filename in filenames)
        //            {

        //                string filePath = Path.Combine(repositoryPath, filename);
        //                System.Diagnostics.Debug.WriteLine(filePath + "/*/*/*/*/*/*/*/*/*/*/*");
        //                if (System.IO.File.Exists(filePath))
        //                {
        //                    zipArchive.CreateEntryFromFile(filePath, filename);
        //                }
        //                else
        //                {
        //                }
        //            }
        //        }
        //        zipStream.Seek(0, SeekOrigin.Begin);
        //        return File(zipStream.ToArray(), "application/zip", "selected_files.zip");
        //    }
        //}
        //[HttpPost]
        //public ActionResult deletefilecus(int id, int[] filenames)
        //{
        //    _service.DeleteCustom(filenames);
        //    return RedirectToAction(nameof(ViewUpload), new { id = id });
        //}


        //[HttpPost]
        //public IActionResult FileUpload(int id, [FromForm] List<IFormFile> File)
        //{
        //    int admin = (int)HttpContext.Session.GetInt32("Id");
        //    _service.FileUpload(id, File, admin);
        //    return RedirectToAction(nameof(ViewUpload), new { id = id });
        //}
        //public IActionResult DeleteFile(int id)
        //{
        //    var rq = _service.DeleteFile(id);
        //    return RedirectToAction(nameof(ViewUpload), "Admin", new { id = rq });
        //}


        //public IActionResult SendEmail(int id)
        //{
        //    int admin = (int)HttpContext.Session.GetInt32("Id");
        //    _service.SendEmail(id, admin);
        //    return RedirectToAction(nameof(ViewUpload), "Admin", new { id = id });
        //}

        public IActionResult Transfer(int Id)
        {

            return PartialView("Transfer");
        }

        [HttpPost]
        public IActionResult Transfer(int Id,ModalData md)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.Transfer(Id,md,phy);
            return RedirectToAction(nameof(PhysicianDashboard));
        }
    }
}
