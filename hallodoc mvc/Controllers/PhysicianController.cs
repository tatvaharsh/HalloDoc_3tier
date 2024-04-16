using Microsoft.AspNetCore.Mvc;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using hallocdoc_mvc_Service.Implementation;
using hallocdoc_mvc_Service.Interface;
using HalloDoc.Auth;
using ClosedXML.Excel;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using DocumentFormat.OpenXml.Spreadsheet;
using static Org.BouncyCastle.Crypto.Fips.FipsKdf;

namespace hallodoc_mvc.Controllers
{
    public class PhysicianController : Controller
    {
        private readonly IPhysician_Service _Service;
        private readonly IPDFService _pdfservice;
        private readonly IJwtService _jwtService;

        private readonly IConfiguration _configuration;

        public PhysicianController(IPhysician_Service service, IJwtService jwtService, IConfiguration configuration, IPDFService pdfservice)
        {
            //_context = context;
            _Service = service;
            _jwtService = jwtService;
            _configuration = configuration;
            _pdfservice = pdfservice;
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

            List<ViewDocument> vd = _Service.ViewUploadData(id);
            return View(vd);
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
        public ActionResult deletefilecus(int id, int[] filenames)
        {
            _Service.DeleteCustom(filenames);
            return RedirectToAction(nameof(ViewUpload), new { id = id });
        }


        [HttpPost]
        public IActionResult FileUpload(int id, [FromForm] List<IFormFile> File)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.FileUpload(id, File, phy);
            TempData["success"] = "File Uploaded Successfully!!!";
            return RedirectToAction(nameof(ViewUpload), new { id = id });
        }
        public IActionResult DeleteFile(int id)
        {
            var rq = _Service.DeleteFile(id);
            TempData["success"] = "File Deleted Successfully!!!";
            return RedirectToAction(nameof(ViewUpload), "Physician", new { id = rq });
        }


        public IActionResult SendEmail(int id)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.SendEmail(id, phy);
            TempData["success"] = "Mail Sent Successfully!!!";
            return RedirectToAction(nameof(ViewUpload), "Physician", new { id = id });
        }

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

        public IActionResult Order()
        {
            Order data = _Service.GetOrderData(new hallodoc_mvc_Repository.ViewModel.Order());
            return PartialView("Order", data);


        }

        public IActionResult GetVendor(int Id)
        {
            var phy = _Service.Getvendor(Id);


            return Json(phy);
        }

        public IActionResult Getdetails(int Id)
        {
            var phy = _Service.Getvendordata(Id);

            return Json(phy);
        }

        [HttpPost]
        public IActionResult Order(Order md, int Id)
        {
            if (!ModelState.IsValid)
            {
                Order data = _Service.GetOrderData(new Order());
                md.HealthProfessionalType = data.HealthProfessionalType;
                TempData["error"] = "Enter Valid Data";
                return View(md);
            }
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.OrderPost(md, Id, phy);
            TempData["success"] = "Order Sent Successfully!!!";
            return RedirectToAction(nameof(PhysicianController.PhysicianDashboard));
        }
        public IActionResult Encounter(int id)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            ViewBag.Username = _Service.Phyname(phy);

            Encounter en = _Service.getencounter(id);
            return View(en);
        }
        public IActionResult ChangeEncounter(int id, Encounter model)
        {
            _Service.editencounter(id, model);
            return RedirectToAction("Encounter", new { id = id });
        }
        public IActionResult FinalEncounter(int id) 
        {
            _Service.final(id);
            return RedirectToAction(nameof(PhysicianController.PhysicianDashboard));
        }
        public IActionResult EncounterModal(int Id)
        {
            ModalData md = new()
            {
                requestID = Id,
            };
            return PartialView("EncounterModal",md);
        }
        public IActionResult Housecall(int Id)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.ChangeCallType(Id,phy);
            return RedirectToAction(nameof(PhysicianController.PhysicianDashboard));
        }
        public IActionResult HousecallFinal(int Id)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.ChangeStatus(Id, phy);
            return RedirectToAction(nameof(PhysicianController.PhysicianDashboard));
        }
        public IActionResult Consult(int Id)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.Consult(Id,phy);
            return RedirectToAction(nameof(PhysicianController.PhysicianDashboard));
        }
        public IActionResult DownloadEnc(int Id)
        {
            ModalData md = new()
            {
                requestID = Id,
            };
            return PartialView("DownloadEnc", md);
        }
        public IActionResult DownloadEncDoc(int Id)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");

            Encounter en = _Service.getencounter(Id);
            byte[] pdfdata = _pdfservice.GeneratePDF(en);
            return File(pdfdata, "application/pdf", "MedicalReport.pdf");
        }
        public IActionResult ConcludeCareeeee(int Id)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            List<ViewDocument> vd = _Service.ViewUploadData(Id);
            return View(vd);

        }

        [HttpPost]
        public IActionResult ConcludeCare(int id, [FromForm] string Note)
        {
                int phy = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.AddProviderNote(id, Note,phy);
            if (_Service.GetEncounterStatus(id))
            {
                _Service.ConcludeCare(id, phy);
                TempData["success"] = "Request Concluded Successfully";
                return RedirectToAction(nameof(PhysicianDashboard));
            }
            TempData["error"] = "Finalize Encounter Form to Conclude Care";
            return RedirectToAction(nameof(Encounter), new {id = id});
        }
    }
}
