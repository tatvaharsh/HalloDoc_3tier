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
using Twilio.TwiML.Voice;

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

        public IActionResult TabChange(int nav, bool isPartial)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("PhyId");
            switch (nav)
            {
                case 1:
                    return RedirectToAction(nameof(PhysicianDashboard), new { isPartial = isPartial });
                case 2:
                    return RedirectToAction(nameof(MyScheduling), new { isPartial = isPartial });
                case 3:
                    return RedirectToAction(nameof(PhysicianProfile), new { isPartial = isPartial });
                case 4:
                    return RedirectToAction(nameof(Invoicing), new { isPartial = isPartial });
            }
            return View();
        }


        [CustomAuthorize(null, "Physician")]
        public IActionResult PhysicianDashboard(bool isPartial)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("PhyId");
            if (admin1 != null)
            {
                if (isPartial == true)
                {
                    ViewBag.Layout = null;
                    ViewBag.Username = _Service.Phyname(admin1);
                    ModalData MD = _Service.CountState(admin1);
                    return View(MD);

                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/Physician/_LayPhysician.cshtml";
                    ViewBag.Username = _Service.Phyname(admin1);
                    ModalData MD = _Service.CountState(admin1);
                    return View(MD);

                }
            }
            return RedirectToAction("Admin_Login");

        }
        public IActionResult PhysicianProfile(bool isPartial)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("PhyId");

            if (isPartial == true)
            {
                ViewBag.Layout = null;
                ViewBag.Username = _Service.Phyname(admin1);
                var data = _Service.getphysiciandata(admin1);

                return View(data);

            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/Physician/_LayPhysician.cshtml";
                ViewBag.Username = _Service.Phyname(admin1);
                var data = _Service.getphysiciandata(admin1);

                return View(data);

            }


        }
        public IActionResult MyScheduling(bool isPartial)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("PhyId");
            if (isPartial == true)
            {
                ViewBag.Layout = null;

                return PartialView("Scheduling");
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/Physician/_LayPhysician.cshtml";
                return View("Scheduling");
            }
        }
        public IActionResult Invoicing(bool isPartial)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("PhyId");
            if (isPartial == true)
            {
                ViewBag.Layout = null;

                return PartialView("Invoicing");
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/Physician/_LayPhysician.cshtml";
                return View("Invoicing");
            }
        }



        public IActionResult New(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            if (pageid == 0) { pageid = 1; };
            var data = _Service.GetNewData(requestType, search, requestor, region, pageid, phy);
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

        [HttpPost]
        public IActionResult Sendlink(ViewCase model)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.sendlink(model, phy);
            return RedirectToAction(nameof(PhysicianDashboard));
        }

        public IActionResult CreateReq()
        {
            return View();

        }
        [HttpPost]
        public IActionResult CreateReq(patient_form model)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            if (!ModelState.IsValid)
            {

                return View(model);
            }
            _Service.PatientForm(model, phy);

            return RedirectToAction(nameof(PhysicianDashboard));

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
        public IActionResult Transfer(int Id, ModalData md)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.Transfer(Id, md, phy);
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
            return PartialView("EncounterModal", md);
        }
        public IActionResult Housecall(int Id)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.ChangeCallType(Id, phy);
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
            _Service.Consult(Id, phy);
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
            _Service.AddProviderNote(id, Note, phy);
            if (_Service.GetEncounterStatus(id))
            {
                _Service.ConcludeCare(id, phy);
                TempData["success"] = "Request Concluded Successfully";
                return RedirectToAction(nameof(PhysicianDashboard));
            }
            TempData["error"] = "Finalize Encounter Form to Conclude Care";
            return RedirectToAction(nameof(Encounter), new { id = id });
        }
        public IActionResult ChangeProviderPassword([FromForm] string pass)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.changepass(pass, phy);
            TempData["success"] = "Password Changed Successfully";
            return RedirectToAction("Tabchange", new { nav = 3 });
        }
        public IActionResult SendMailToAdmin(int id, string textareas)
        {
            _Service.SendMailToAdmin(id, textareas);
            TempData["success"] = "Mail Send Successfully";
            return RedirectToAction(nameof(PhysicianController.PhysicianDashboard));
        }
        public IActionResult Shifttab(int day, int month, int year)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            return PartialView("~/Views/Shared/Physician/MonthlyShift.cshtml", _Service.GetMonthWiseData(day, month, year, phy));

        }
        public IActionResult CreateShiftModal(int id)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            CreateShift model = new()
            {

                Regionname = _Service.GetRegByPhy(phy),
            };

            return PartialView(model);
        }

        public IActionResult CreateShift(CreateShift model)
        {

            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            bool flag = _Service.CreateShift(model, phy);
            if (!flag)
            {
                TempData["error"] = "Shift has been clashed";
                return RedirectToAction(nameof(TabChange), new { nav = 2, isPartial = false });
            }
            return RedirectToAction(nameof(TabChange), new { nav = 2, isPartial = false });

        }
        [HttpGet]
        public IActionResult EditShift(int shiftdetailid)
        {
            return PartialView("EditShift", _Service.EditShift(shiftdetailid));
        }

        [HttpPost]
        public IActionResult EditShift(EditShift editShift, int shiftdetailid)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");

            _Service.UpdateShift(editShift, shiftdetailid, phy);
            TempData["success"] = "Shift Updated Successfully!!!";
            return RedirectToAction(nameof(TabChange), new { nav = 2, isPartial = false });
        }
        public IActionResult ChangeShiftStatus(int shiftdetailid)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");


            _Service.ChangeShiftStatus(shiftdetailid, phy);
            TempData["success"] = "Shift Updated Successfully!!!";
            return RedirectToAction(nameof(TabChange), new { nav = 2, isPartial = false });
        }
        public IActionResult DeleteShiftViaModal(int shiftdetailid)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");

            _Service.DeleteShiftViaModal(shiftdetailid, phy);
            TempData["success"] = "Shift Updated Successfully!!!";
            return RedirectToAction(nameof(TabChange), new { nav = 2, isPartial = false });
        }
        public IActionResult FinalizeTimesheet(DateTime date)
        {
            int phy = (int)HttpContext.Session.GetInt32("PhyId");
            return PartialView("_FinalizeTimesheet", _Service.TimesheetData(date, phy));
        }


        public IActionResult AddTimeSheet(DateTime date, [FromForm] TimesheetPost data)
        {
            DateTime newdate = new DateTime(date.Year, date.Month, date.Day);
            int phyid = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.AddTimesheets(date, phyid, data);
            return RedirectToAction(nameof(PhysicianController.PhysicianDashboard));
        }

        public IActionResult IsFinalizedBtn(DateTime date)
        {
            int phyid = (int)HttpContext.Session.GetInt32("PhyId");
            if(_Service.ShowFinalizeBtn(date, phyid))
            {
                return Json(new {isFinalized = true});
            }
            return Json(new { isFinalized = false});
        }

        public IActionResult ShowReimburesement(DateTime date)
        {
            int phyid = (int)HttpContext.Session.GetInt32("PhyId");
            return PartialView("_Reimbursement",_Service.ReimbursementData(phyid,date));
        }
        [HttpPost]
        public IActionResult SubmitReciet(DateTime date, [FromForm]hallodoc_mvc_Repository.ViewModel.Reimbursement model)
        {
            int phyid = (int)HttpContext.Session.GetInt32("PhyId");
            _Service.AddReciet(model,date,phyid);
            return RedirectToAction(nameof(PhysicianController.PhysicianDashboard));
        }
    }
}
