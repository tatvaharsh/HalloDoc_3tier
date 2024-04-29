using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using hallocdoc_mvc_Service.Implementation;
using hallocdoc_mvc_Service.Interface;
using HalloDoc.Auth;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;

namespace hallodoc_mvc.Controllers
{

    public class AdminController : Controller
    {
        private readonly IAdmin_Service _service;

        private readonly IJwtService _jwtService;

        private readonly IConfiguration _configuration;

        private readonly IPDFService _pdf;

        public AdminController(IAdmin_Service service, IJwtService jwtService, IConfiguration configuration, IPDFService pdf)
        {
            //_context = context;
            _service = service;
            _jwtService = jwtService;
            _configuration = configuration;
            _pdf = pdf;
        }

        [CustomAuthorize("Dashboard", "Admin")]
        [Route("/admin/dashboard")]
        public IActionResult Admin_Dashboard()
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");

            if (admin1 != null)
            {
                ViewBag.Username = _service.Adminname(admin1);
                ModalData data = _service.GetAssignData(new ModalData());
                ViewBag.Layout = "_LayAdmin";
                return View(data);
            }
            return RedirectToAction("Admin_Login");
        }

        [CustomAuthorize("Dashboard", "Admin")]
        public IActionResult Admin_DashboardPartial(bool isPartial)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");

            if (admin1 != null)
            {
                if (isPartial == true)
                {

                    ViewBag.Username = _service.Adminname(admin1);
                    ModalData data = _service.GetAssignData(new ModalData());
                    //TempData["success"] = "Login Successfully!!!";
                    //TempData.Clear();
                    ViewBag.Layout = null;
                    return View("Admin_Dashboard", data);
                }
                else
                {
                    ViewBag.Username = _service.Adminname(admin1);
                    ModalData data = _service.GetAssignData(new ModalData());
                    ViewBag.Layout = "_LayAdmin";
                    return View("Admin_Dashboard", data);
                }

            }
            return RedirectToAction("Admin_Login");
        }

        //public IActionResult Admin_Login()
        //{

        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Admin_Login(LoginViewModel model)
        //{   
        //    if (ModelState.IsValid)
        //    {
        //        bool isReg = _service.ValidateUser(model);

        //        if (isReg)
        //        {
        //            var Admin = _service.getAdmin(model.Email);
        //            if (Admin != null)
        //            {
        //                HttpContext.Session.SetInt32("Id", Admin.AdminId);

        //                var token = _jwtService.GenerateJwtToken(model);
        //                Response.Cookies.Append("jwt", token);
        //                ViewBag.Username = Admin.FirstName;
        //               TempData["success"] = "Login Successfully!!!";
        //                return RedirectToAction("Admin_Dashboard");
        //            }
        //        }
        //        TempData["error"] = "Login Failed!!!";


        //    }

        //    return View();
        //}

        //public IActionResult Logout()
        //{


        //    HttpContext.Session.Clear();
        //    HttpContext.Session.Remove("Id");
        //    Response.Cookies.Delete("jwt");
        //    return RedirectToAction("Admin_Login");
        //}

        [CustomAuthorize("Provider Location", "Admin")]
        [Route("/admin/Location")]
        public IActionResult ProviderLocation(bool isPartial)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            ViewBag.Username = _service.Adminname(admin);
            if (isPartial == true)
            {

                return PartialView("ProviderLocation", _service.ProviderLocation());
            }
            else
            {
                ViewBag.Layout = "_LayAdmin";
                return View("ProviderLocation", _service.ProviderLocation());
            }
        }

        [CustomAuthorize("My Profile", "Admin")]
        [Route("/admin/AdminProfile")]
        public IActionResult AdminProfile(bool isPartial)
        {
        
            int admin = (int)HttpContext.Session.GetInt32("Id");
            if (isPartial == true)
            {
                ViewBag.Layout = null;

                return PartialView("Admin_Profile", _service.getprofile(admin));
            }
            else
            {
                ViewBag.Layout = "_LayAdmin";
                return View("Admin_Profile", _service.getprofile(admin));
            }
        }
        [CustomAuthorize("Provider", "Admin")]
        [Route("/admin/Provider-Information")]
        public IActionResult Provider(bool isPartial)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            if (isPartial == true)
            {
                ViewBag.Layout = null;

                return PartialView("Provider", _service.GetRegions());
            }
            else
            {
                ViewBag.Layout = "_LayAdmin";
                return View("Provider", _service.GetRegions());
            }
        }
        [CustomAuthorize("Scheduling", "Admin")]
        [Route("/admin/Scheduling")]
        public IActionResult ProviderScheduling(bool isPartial)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            if (isPartial == true)
            {
                ViewBag.Layout = null;

                return PartialView("Scheduling", _service.GetRegions());
            }
            else
            {
                ViewBag.Layout = "_LayAdmin";
                return View("Scheduling", _service.GetRegions());
            }
        }
        [CustomAuthorize("Partners", "Admin")]
        [Route("/admin/Vendors")]
        public IActionResult Vendor(bool isPartial)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            if (isPartial == true)
            {
                ViewBag.Layout = null;
                return PartialView("Partners", new PartnersCM()
                {
                    //partnersdatas =_service.GetAllHealthProfessionaldata(0),
                    Professions = _service.GetProfession()
                });
            }
            else
            {
                ViewBag.Layout = "_LayAdmin";
                return PartialView("Partners", new PartnersCM()
                {
                    //partnersdatas =_service.GetAllHealthProfessionaldata(0),
                    Professions = _service.GetProfession()
                });

            }
        }
        [CustomAuthorize("Account Access", "Admin")]
        [Route("/admin/AccountAccess")]
        public IActionResult AccountAccess(bool isPartial)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            if (isPartial == true)
            {
                ViewBag.Layout = null;
                return PartialView("AccountAccess", _service.getAccess());
            }
            else
            {
                ViewBag.Layout = "_LayAdmin";
                return View("AccountAccess", _service.getAccess());

            }
        }
        [CustomAuthorize("User Access", "Admin")]
        [Route("/admin/User")]
        public IActionResult Useraccess(bool isPartial)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            if (isPartial == true)
            {
                ViewBag.Layout = null;
                return PartialView("UserAccess");
            }
            else
            {
                ViewBag.Layout = "_LayAdmin";
                return View("UserAccess");

            }
        }

        [Route("/admin/AdminAccount")]
        public IActionResult AdminAccount(bool isPartial)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            if (isPartial == true)
            {
                ViewBag.Layout = null;
                return PartialView("AdminAccount", new CreateAdmin()
                {
                    reg = _service.getreg(),
                    roles = _service.GetRoleOfAdmin(),
                });
            }
            else
            {
                ViewBag.Layout = "_LayAdmin";
                return View("AdminAccount", new CreateAdmin()
                {
                    reg = _service.getreg(),
                    roles = _service.GetRoleOfAdmin(),
                });

            }
        }
        [Route("/admin/PatientHistory")]
        public IActionResult PatientHistory(bool isPartial)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            if (isPartial == true)
            {
                ViewBag.Layout = null;
                return PartialView("PatientHistory");
            }
            else
            {
                ViewBag.Layout = "_LayAdmin";
                return View("PatientHistory");

            }
        }

        [Route("/admin/Emaillog")]
        public IActionResult Emaillog(bool isPartial)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            if (isPartial == true)
            {
                ViewBag.Layout = null;
                return PartialView("EmailLog");
            }
            else
            {
                ViewBag.Layout = "_LayAdmin";
                return View("EmailLog");

            }
        }


        [Route("/admin/Smslog")]
        public IActionResult smslog(bool isPartial)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            if (isPartial == true)
            {
                ViewBag.Layout = null;
                return PartialView("SmsLog");
            }
            else
            {
                ViewBag.Layout = "_LayAdmin";
                return View("SmsLog");

            }
        }
        [Route("/admin/SearchRecord")]
        public IActionResult Searchrecord(bool isPartial)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            if (isPartial == true)
            {

                ViewBag.Layout = null;
                return PartialView("SearchRecord", new AdminRecord()
                {
                    ReqType = _service.GetRequestTypes(),
                });
            }
            else
            {
                ViewBag.Layout = "_LayAdmin";
                return View("SearchRecord", new AdminRecord()
                {
                    ReqType = _service.GetRequestTypes(),
                });
            }
        }

        [Route("/admin/BlockHistory")]
        public IActionResult BlockHistory(bool isPartial)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            if (isPartial == true)
            {

                ViewBag.Layout = null;
                return PartialView("BlockHistory");
            }
            else
            {
                ViewBag.Layout = "_LayAdmin";
                return View("BlockHistory");
            }
        }

        public IActionResult Admin_Forgot()
        {
            return View();
        }
      
        public IActionResult TabChange(int nav, bool isPartial)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            switch (nav)
            {
                case 1:
                    return RedirectToAction(nameof(Admin_DashboardPartial), new { isPartial = isPartial });
                case 2:
                    return RedirectToAction(nameof(ProviderLocation), new { isPartial = isPartial });
                case 3:
                    return RedirectToAction(nameof(AdminProfile), new { isPartial = isPartial });

                case 4:
                    return RedirectToAction(nameof(Provider), new { isPartial = isPartial });
                case 5:
                    return RedirectToAction(nameof(ProviderScheduling), new { isPartial = isPartial });

                case 7:
                    return RedirectToAction(nameof(Vendor), new { isPartial = isPartial });

                case 8:
                    return RedirectToAction(nameof(AccountAccess), new { isPartial = isPartial });
                case 9:
                    return RedirectToAction(nameof(Useraccess), new { isPartial = isPartial });

                case 10:
                    return RedirectToAction(nameof(AdminAccount), new { isPartial = isPartial });

                case 11:
                    return RedirectToAction(nameof(PatientHistory), new { isPartial = isPartial });

                case 12:
                    return RedirectToAction(nameof(Emaillog), new { isPartial = isPartial });

                case 13:
                    return RedirectToAction(nameof(smslog), new { isPartial = isPartial });

                case 14:
                    return RedirectToAction(nameof(Searchrecord), new { isPartial = isPartial });

                case 15:
                    return RedirectToAction(nameof(BlockHistory), new { isPartial = isPartial });
            }
            return View();
        }

        public IActionResult New(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            if (pageid == 0) { pageid = 1; };
            var data = _service.getDashData(requestType, search, requestor, region, pageid);
            ViewBag.page = pageid;
            return PartialView("_Admin_Request_Table", data);
        }
        public IActionResult Pending(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            if (pageid == 0) { pageid = 1; };
            var data = _service.getDashDataPending(requestType, search, requestor, region, pageid);
            //AdminDashboard adminDashboard = new AdminDashboard();
            ViewBag.page = pageid;
            return PartialView("_Admin_Request_Table", data);
        }
        public IActionResult Active(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            if (pageid == 0) { pageid = 1; };
            var data = _service.getDashDataActive(requestType, search, requestor, region, pageid);
            //AdminDashboard adminDashboard = new AdminDashboard();
            ViewBag.page = pageid;
            return PartialView("_Admin_Request_Table", data);
        }

        public IActionResult Conclude(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            if (pageid == 0) { pageid = 1; };
            var data = _service.getDashDataConclude(requestType, search, requestor, region, pageid);
            //AdminDashboard adminDashboard = new AdminDashboard();
            ViewBag.page = pageid;
            return PartialView("_Admin_Request_Table", data);
        }
        public IActionResult ToClose(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            if (pageid == 0) { pageid = 1; };
            var data = _service.getDashDataToclose(requestType, search, requestor, region, pageid);
            //AdminDashboard adminDashboard = new AdminDashboard();
            ViewBag.page = pageid;
            return PartialView("_Admin_Request_Table", data);
        }

        public IActionResult Unpaid(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            if (pageid == 0) { pageid = 1; };
            var data = _service.getDashDataUnpaid(requestType, search, requestor, region, pageid);
            //AdminDashboard adminDashboard = new AdminDashboard();
            ViewBag.page = pageid;
            return PartialView("_Admin_Request_Table", data);
        }

        public IActionResult ViewCase(int Id)
        {
            ViewCase vc = _service.Getcase(Id);
            return View(vc);
        }

        [HttpGet]
        public IActionResult ViewNotes(int Id)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            ViewBag.Username = _service.Adminname(admin);
            ViewNote rn = _service.GetNotes(Id);
            return View(rn);
        }

        [HttpPost]
        public IActionResult ViewNotes(ViewNote model, int Id)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            ViewBag.Username = _service.Adminname(admin);
            var vn = _service.setViewNotesData(model, Id, admin);
            TempData["success"] = "Note Added Successfully!!!";
            return RedirectToAction("ViewNotes", Id);
        }


        public IActionResult AssignCase()
        {
            ModalData data = _service.GetAssignData(new hallodoc_mvc_Repository.ViewModel.ModalData());
            return PartialView("AssignCase", data);
        }

        [HttpPost]
        public IActionResult AssignCase(ModalData md, int Id)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.AssignCase(md, Id, admin);
            TempData["success"] = "Case Assigned Successfully!!!";
            return RedirectToAction(nameof(AdminController.Admin_Dashboard));
        }

        public IActionResult Cancel(int Id)
        {
            var cancelcase = _service.GetCancelCaseData(new hallodoc_mvc_Repository.ViewModel.ModalData(), Id);

            ModalData modalData = new ModalData();
            {
                modalData.PatientName = cancelcase.FirstName + ' ' + cancelcase.LastName;
                modalData.requestID = cancelcase.RequestId;

                modalData.CaseTags = _service.GetCaseReason();
            };

            return PartialView("CancelModal", modalData);
        }

        [HttpPost]
        public IActionResult canclePost(ModalData md, int Id)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.CanclePost(md, Id, admin);
            TempData["success"] = "Case Cancelled Successfully!!!";
            return RedirectToAction(nameof(AdminController.Admin_Dashboard));
        }

        public IActionResult GetPhysician(int Id)
        {
            var phy = _service.GetPhysician(Id);
            ModalData modalData = new ModalData
            {
                Physicians = phy
            };
            return Json(phy);
        }
        public IActionResult Block(ModalData md, int Id)
        {

            var blockcase = _service.GetBlockCaseData(md, Id);

            ModalData modalData = new ModalData();
            {
                modalData.PatientName = blockcase.FirstName + ' ' + blockcase.LastName;
                modalData.requestID = blockcase.RequestId;


            };

            return PartialView("Block", modalData);
        }

        [HttpPost]
        public IActionResult BlockPost(ModalData md, int id)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.BlockPost(md, id, admin);

            return RedirectToAction(nameof(AdminController.Admin_Dashboard));
        }
        public IActionResult ViewUpload(int id)
        {

            List<ViewDocument> vd = _service.ViewUploadData(id);
            return View(vd);
        }

        [HttpPost]
        public ActionResult DownloadFiles([FromBody] string[] filenames)
        {

            System.Diagnostics.Debug.WriteLine(filenames);
            string repositoryPath = Directory.GetCurrentDirectory()+"\\wwwroot\\uplodedfiles";
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
            _service.DeleteCustom(filenames);
            return RedirectToAction(nameof(ViewUpload), new { id = id });
        }


        [HttpPost]
        public IActionResult FileUpload(int id, [FromForm] List<IFormFile> File)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.FileUpload(id, File, admin);
            return RedirectToAction(nameof(ViewUpload), new { id = id });
        }
        public IActionResult DeleteFile(int id)
        {
            var rq = _service.DeleteFile(id);
            return RedirectToAction(nameof(ViewUpload), "Admin", new { id = rq });
        }


        public IActionResult SendEmail(int id)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.SendEmail(id, admin);
            return RedirectToAction(nameof(ViewUpload), "Admin", new { id = id });
        }
        public IActionResult Order()
        {
            Order data = _service.GetOrderData(new hallodoc_mvc_Repository.ViewModel.Order());
            return PartialView("Order", data);


        }

        public IActionResult GetVendor(int Id)
        {
            var phy = _service.Getvendor(Id);


            return Json(phy);
        }

        public IActionResult Getdetails(int Id)
        {
            var phy = _service.Getvendordata(Id);

            return Json(phy);
        }
        [HttpPost]
        public IActionResult Order(Order md, int Id)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.OrderPost(md, Id, admin);
            TempData["success"] = "Order Placed Successfully!!!";
            return RedirectToAction(nameof(AdminController.Admin_Dashboard));
        }
        public IActionResult Transfer()
        {
            ModalData data = _service.GetAssignData(new hallodoc_mvc_Repository.ViewModel.ModalData());
            return PartialView("Transfer", data);
        }

        [HttpPost]
        public IActionResult Transfer(int Id, ModalData md)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.TransferReq(md,Id, admin);
            TempData["success"] = "Case Transffered Successfully!!!";
            return RedirectToAction(nameof(AdminController.Admin_Dashboard));
        }
        public IActionResult Clear()
        {

            return PartialView("Clear");
        }
        [HttpPost]
        public IActionResult Clear(int Id)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.Clear(Id, admin);

            return RedirectToAction(nameof(AdminController.Admin_Dashboard));
        }

        [HttpGet]
        public IActionResult SendAgreement(int Id, int requestType)
        {
            ViewBag.requestType = requestType;

            RequestClient rc = _service.GetAgreementtdata(Id);
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
            int admin = (int)HttpContext.Session.GetInt32("Id");
            var token = _jwtService.GenerateJwtTokenByEmail(md.email);

            _service.SendAgreementMail(Id, md, token, admin);
            return RedirectToAction(nameof(AdminController.Admin_Dashboard));
        }
        public IActionResult Close(int Id)
        {
            Close c = _service.getclosedata(Id);
            return View(c);
        }



        [HttpPost]
        public IActionResult CloseConfirm(int Id, [FromForm] Close model)
        {
            _service.editdata(model, Id);
            return RedirectToAction(nameof(Close), new { Id = Id });
        }
        public IActionResult saavclose(int Id)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.close(Id, admin);
            TempData["success"] = "Case Closed Successfully!!!";
            return RedirectToAction("Admin_Dashboard");
        }
        [HttpGet]
        public IActionResult Agreement(int token, string t)
        {
            if (_jwtService.ValidateJwtToken(t, out JwtSecurityToken jwtSecurityToken))
            {
                ModalData md = _service.cancelmodal(token);
                
               
                    return View(md);
           
                
            }
            return NotFound();

        }

        public IActionResult Agree(int id)
        {
            _service.agreeagreement(id);
            return RedirectToAction("patient_login","Home");
        }
        public IActionResult GetCancel(int id, ModalData md)
        {
            _service.cancelagreement(id, md);
            return RedirectToAction("patient_login", "Home");
        }
        public IActionResult Encounter(int id, Encounter model)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            ViewBag.Username = _service.Adminname(admin1);

            Encounter en = _service.getencounter(id);
            return View(en);
        }
        public IActionResult ChangeEncounter(int id, Encounter model)
        {
            _service.editencounter(id, model);
            return RedirectToAction("Encounter", new { id = id });
        }


        public IActionResult Admin_Profile()
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");

            return View(_service.getprofile(admin));
        }

        public IActionResult Sendlink()
        {

            return PartialView();
        }
        [HttpPost]
        public IActionResult Sendlink(ViewCase model)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            _service.sendlink(model, admin1);
            return RedirectToAction("Admin_Dashboard");
        }
    
        public IActionResult RequestSupport()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult RequestSupport(ModalData model)
        {
            _service.SendEmailToOffDutyProvider(model);
            return RedirectToAction("Admin_Dashboard");
        }
        public IActionResult DownloadAll()
        {
            try
            {
                List<Request> data = GetTableData();
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Data");


                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Date Of Birth";
                worksheet.Cell(1, 3).Value = "Requestor";
                worksheet.Cell(1, 4).Value = "Physician Name";
                worksheet.Cell(1, 5).Value = "Date of Service";
                worksheet.Cell(1, 6).Value = "Requested Date";
                worksheet.Cell(1, 7).Value = "Phone Number";
                worksheet.Cell(1, 8).Value = "Address";
                worksheet.Cell(1, 9).Value = "Notes";
                worksheet.Cell(1, 10).Value = "Status";

                int row = 2;
                foreach (var item in data)
                {
                    var statusClass = "";
                    var dos = "";
                    var notes = "";
                    if (item.RequestTypeId == 1)
                    {
                        statusClass = "Business";
                    }
                    else if (item.RequestTypeId == 4)
                    {
                        statusClass = "Concierge";
                    }
                    else if (item.RequestTypeId == 2)
                    {
                        statusClass = "Patient";
                    }
                    else
                    {
                        statusClass = "Family/Friend";
                    }
                    foreach (var stat in item.RequestStatusLogs)
                    {
                        if (stat.Status == 2)
                        {
                            dos = stat.CreatedDate.ToString("MMMM dd,yyyy");
                            notes = stat.Notes ?? "";
                        }
                    }
                    worksheet.Cell(row, 1).Value = item.RequestClients.FirstOrDefault().FirstName + item.RequestClients.FirstOrDefault().LastName;
                    worksheet.Cell(row, 2).Value = DateTime.Parse($"{item.RequestClients.FirstOrDefault().IntYear}-{item.RequestClients.FirstOrDefault().StrMonth}-{item.RequestClients.FirstOrDefault().IntDate}").ToString("MMMM dd,yyyy");
                    worksheet.Cell(row, 3).Value = statusClass.Substring(0, 1).ToUpper() + statusClass.Substring(1).ToLower() + item.FirstName + item.LastName;
                    worksheet.Cell(row, 4).Value = ("Dr." + item?.Physician == null ? "" : item?.Physician?.FirstName);
                    worksheet.Cell(row, 5).Value = dos;
                    worksheet.Cell(row, 6).Value = item.CreatedDate.ToString("MMMM dd,yyyy");
                    worksheet.Cell(row, 7).Value = item.RequestClients.FirstOrDefault().PhoneNumber + "(Patient)" + (item.RequestTypeId != 4 ? item.PhoneNumber + statusClass.Substring(0, 1).ToUpper() + statusClass.Substring(1).ToLower() : "");
                    worksheet.Cell(row, 8).Value = (item.RequestClients.FirstOrDefault().Address == null ? item.RequestClients.FirstOrDefault().Address + item.RequestClients.FirstOrDefault().Street + item.RequestClients.FirstOrDefault().City + item.RequestClients.FirstOrDefault().State + item.RequestClients.FirstOrDefault().ZipCode : item.RequestClients.FirstOrDefault().Street + item.RequestClients.FirstOrDefault().City + item.RequestClients.FirstOrDefault().State + item.RequestClients.FirstOrDefault().ZipCode);
                    worksheet.Cell(row, 9).Value = item.RequestClients.FirstOrDefault().Notes;
                    worksheet.Cell(row, 10).Value = item.Status;
                    row++;
                }
                worksheet.Columns().AdjustToContents();

                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public List<Request> GetTableData()
        {
            List<Request> data = new List<Request>();
            //var user_id = HttpContext.Session.GetInt32("id");
            //data = _context.Requests.Include(r => r.RequestClient).Where(u => u.UserId == user_id).ToList();
            data = _service.GetRequestDataInList();
            return data;
        }

        public IActionResult CreateReq()
        {
            return View();

        }
        [HttpPost]
        public IActionResult CreateReq(patient_form model)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            if (!ModelState.IsValid)
            {

                return View(model);
            }
            _service.PatientForm(model, admin);

            return RedirectToAction(nameof(Admin_Dashboard));

        }

        public IActionResult Export(string s, int reqtype, int regid, int state)
        {
            var data = _service.Export(s, reqtype, regid, state);
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Data");


            worksheet.Cell(1, 1).Value = "Name";
            worksheet.Cell(1, 2).Value = "Date Of Birth";
            worksheet.Cell(1, 3).Value = "Requestor";
            worksheet.Cell(1, 4).Value = "Physician Name";
            worksheet.Cell(1, 5).Value = "Date of Service";
            worksheet.Cell(1, 6).Value = "Requested Date";
            worksheet.Cell(1, 7).Value = "Phone Number";
            worksheet.Cell(1, 8).Value = "Address";
            worksheet.Cell(1, 9).Value = "Notes";
            worksheet.Cell(1, 10).Value = "Status";

            int row = 2;
            foreach (var item in data)
            {
                var statusClass = "";
                var dos = "";
                var notes = "";
                if (item.RequestTypeId == 1)
                {
                    statusClass = "Business";
                }
                else if (item.RequestTypeId == 4)
                {
                    statusClass = "Concierge";
                }
                else if (item.RequestTypeId == 2)
                {
                    statusClass = "Patient";
                }
                else
                {
                    statusClass = "Family/Friend";
                }
                foreach (var stat in item.RequestStatusLogs)
                {
                    if (stat.Status == 2)
                    {
                        dos = stat.CreatedDate.ToString("MMMM dd,yyyy");
                        notes = stat.Notes ?? "";
                    }
                }
                worksheet.Cell(row, 1).Value = item.RequestClients.FirstOrDefault().FirstName + item.RequestClients.FirstOrDefault().LastName;
                worksheet.Cell(row, 2).Value = DateTime.Parse($"{item.RequestClients.FirstOrDefault().IntYear}-{item.RequestClients.FirstOrDefault().StrMonth}-{item.RequestClients.FirstOrDefault().IntDate}").ToString("MMMM dd,yyyy");
                worksheet.Cell(row, 3).Value = statusClass.Substring(0, 1).ToUpper() + statusClass.Substring(1).ToLower() + item.FirstName + item.LastName;
                worksheet.Cell(row, 4).Value = ("Dr." + item?.Physician == null ? "" : item?.Physician?.FirstName);
                worksheet.Cell(row, 5).Value = dos;
                worksheet.Cell(row, 6).Value = item.CreatedDate.ToString("MMMM dd,yyyy");
                worksheet.Cell(row, 7).Value = item.RequestClients.FirstOrDefault().PhoneNumber + "(Patient)" + (item.RequestTypeId != 4 ? item.PhoneNumber + statusClass.Substring(0, 1).ToUpper() + statusClass.Substring(1).ToLower() : "");
                worksheet.Cell(row, 8).Value = (item.RequestClients.FirstOrDefault().Address == null ? item.RequestClients.FirstOrDefault().Address + item.RequestClients.FirstOrDefault().Street + item.RequestClients.FirstOrDefault().City + item.RequestClients.FirstOrDefault().State + item.RequestClients.FirstOrDefault().ZipCode : item.RequestClients.FirstOrDefault().Street + item.RequestClients.FirstOrDefault().City + item.RequestClients.FirstOrDefault().State + item.RequestClients.FirstOrDefault().ZipCode);
                worksheet.Cell(row, 9).Value = item.RequestClients.FirstOrDefault().Notes;
                worksheet.Cell(row, 10).Value = item.Status;
                row++;
            }
            worksheet.Columns().AdjustToContents();

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
        }

        [HttpPost]
        public IActionResult EditAdminProfile(Profile model, List<int> reg)
        {
            if (model.AdminData.Mobile?.Length != 10)
            {
                TempData["error"] = "Enter Valid MobileNumber";
                return RedirectToAction(nameof(TabChange), new { nav = 3, isPartial = false });
            }

            int admin = (int)HttpContext.Session.GetInt32("Id");
            ViewBag.Username = _service.Adminname(admin);
            _service.editadminprofile(model, admin);
            TempData["success"] = "Profile Changed Successfully!!!";
            return RedirectToAction(nameof(TabChange), new { nav = 3, isPartial = false });
        }

        [HttpPost]
        public IActionResult EditAdminp(Profile model)
        {
            if (model.AdminData.Mobile?.Length != 10)
            {
                TempData["error"] = "Enter Valid MobileNumber";
                return RedirectToAction("Admin_Dashboard");
            }
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.editadminp(model, admin);
            TempData["success"] = "Profile Changed successfully!!!";
            return RedirectToAction(nameof(TabChange), new { nav = 3, isPartial = false });
        }

        [HttpPost]
        public IActionResult ResetPassword(Profile model)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.reset(model, admin);
            TempData["success"] = "Password Changed successfully!!!";
            return RedirectToAction(nameof(TabChange), new { nav = 3, isPartial = false });
        }

        public IActionResult Provider(int region)
        {
            List<Provider> p = _service.Getphysician(region);
            return PartialView("_TablePhysician", p);
        }

        public IActionResult CreatePhysicianAccount()
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            ViewBag.Username = _service.Adminname(admin1);
            var reg = _service.getreg();
            var roles = _service.getrole();
            CreatePhy createPhy = new CreatePhy()
            {
                reg = reg,
                roles = roles,


            };
            return View(createPhy);
        }
        [CustomAuthorize(null,"Admin")]
        public IActionResult EditPhysicianAccount(int id)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            ViewBag.Username = _service.Adminname(admin1);
            var data = _service.getphysiciandata(id);

            return View(data);
        }
        public IActionResult ProviderMenuModal(int id)
        {
            return PartialView(new ModalData() { phyid = id });
        }

        public void ChangeToggle(int id)
        {
            _service.ChangeToggle(id);

        }

        public IActionResult SendEmailOrMessage(int id, ModalData md)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            _service.Sendit(id, md, admin1);
            return RedirectToAction("Provider");
        }

        [HttpPost]
        public IActionResult CreatePhysicianAccount(CreatePhy model)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            if (ModelState.IsValid)
            {
                _service.CreateProvider(model, admin1);
                return RedirectToAction("Admin_Dashboard");
            }
            var reg = _service.getreg();
            var roles = _service.getrole();
            model.reg = reg; model.roles = roles;
            return View(model);
        }



        public IActionResult CreateRole(int check, string RoleName)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            ViewBag.Username = _service.Adminname(admin1);
            RoleModel model = _service.GetMenutbl(check);
            model.RoleName = RoleName;
            return View(model); 
        }
        public IActionResult AssignRole(string RoleName, string[] selectedRoles, int check)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            if(RoleName==null || selectedRoles==null || selectedRoles[0] == null)
            {
                TempData["error"] = "Enter Valid Details";
                return RedirectToAction(nameof(CreateRole));
            }
            _service.AssignRole(RoleName, selectedRoles, check, admin1);
            TempData["success"] = "Role Created successfully!!!";
            return RedirectToAction("TabChange", new { nav = 8 });
        }

        public IActionResult EditRole(int id)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            ViewBag.Username = _service.Adminname(admin1);
            return View(_service.GetRolewiseData(id));
        }
        public IActionResult UpdateRole(RoleModel model)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            _service.UpdateRole(model);
            TempData["success"] = "Role Updated successfully!!!";
            return RedirectToAction("TabChange", new { nav = 8 });
        }
        public IActionResult DeleteRole(int id)
        {
            _service.DeleteRoles(id);
            TempData["success"] = "Role Deleted successfully!!!";
            return RedirectToAction("TabChange", new { nav = 8 });
        }

        [HttpPost]
        public IActionResult EditPhyProfile(int id, [FromForm] CreatePhy model)
        {
            _service.EditPhyProfile(id, model);
            return RedirectToAction("Admin_Dashboard");
        }

        [HttpPost]
        public IActionResult EditPhyInfo(int id, CreatePhy model)
        {
          
            _service.EditPhyInfo(id, model);
            TempData["success"] = "Provider Edited successfully!!!";
            return RedirectToAction(nameof(EditPhysicianAccount),new{Id = id});
        }

        [HttpPost]
        public IActionResult EditPhyMailBillInfo(int id, CreatePhy model)
        {
            _service.EditPhyMailBillInfo(id, model);
            TempData["success"] = "Provider Edited successfully!!!";
            return RedirectToAction(nameof(EditPhysicianAccount), new { Id = id });
        }
        [HttpPost]
        public IActionResult EditPhyProvider(int id, CreatePhy model)
        {
            _service.EditPhyProvider(id, model);
            TempData["success"] = "Provider Edited successfully!!!";
            return RedirectToAction(nameof(EditPhysicianAccount), new { Id = id });
        }
        [HttpPost]
        public IActionResult EditPhyDocs(int id, CreatePhy model)
        {
            _service.EditPhyDocs(id, model);
            TempData["success"] = "Provider Edited successfully!!!";
            return RedirectToAction(nameof(EditPhysicianAccount), new { Id = id });
        }
        public IActionResult DeletePhy(int id)
        {
            _service.DeletePhy(id);
            return RedirectToAction("Admin_Dashboard");
        }



        public IActionResult UserAccess(int region)
        {
            var data = _service.GetUserAccessData(region);
            return PartialView("TableUserAccess", data);
        }


        [HttpPost]
        public IActionResult CreateAdminAccount(CreateAdmin model)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            if (ModelState.IsValid)
            {
                _service.CreateAdmin(model, admin1);
                TempData["success"] = "Admin Created Successfully!!!";
                return RedirectToAction("Admin_Dashboard");
            }

            var reg = _service.getreg();
            var roles = _service.GetRoleOfAdmin();
            model.reg = reg; model.roles = roles;
            TempData["error"] = "Something Went Wrong!!!";
            return RedirectToAction("TabChange", new { nav = 8 });
        }


        public IActionResult Partners(int professionid, string search)
        {
            var Partnersdata = _service.GetAllHealthProfessionaldata(professionid, search);
            return PartialView("TablePartner", new PartnersCM()
            {
                Professions = _service.GetProfession(),
                partnersdatas = Partnersdata,
            });
        }

        public IActionResult AddBusiness()
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            ViewBag.Username = _service.Adminname(admin1);
            PartnersCM partnersCM = new()
            {
                Professions = _service.GetProfession(),
                regions = _service.getreg(),
            };
            return View(partnersCM);
        }

        [HttpPost]
        public IActionResult AddBusiness(PartnersCM model)
        {
            _service.AddBusiness(model);
            TempData["success"] = "Partner Added Successfully!!!";
            return RedirectToAction("Admin_Dashboard");
        }

        public IActionResult EditPartner(int id)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            ViewBag.Username = _service.Adminname(admin1);
            var a = _service.GetPartnerData(id);

            return View(a);
        }

        [HttpPost]
        public IActionResult EditPartner(PartnersCM model, int id)
        {
            _service.EditPartner(model, id);
            TempData["success"] = "Partner Edited successfully!!!";
            return RedirectToAction(nameof(EditPartner),new { id= id});
        }


        public IActionResult DeletePartner(int id)
        {
            _service.DeletePartner(id);
            TempData["success"] = "Partner Deleted successfully!!!";
            return RedirectToAction("Admin_Dashboard");
        }
        public IActionResult CreateShiftModal(int id)
        {

            CreateShift model = new()
            {
                Region = _service.getreg(),

            };

            return PartialView(model);
        }

        public IActionResult GetPhysicianForShift(int id)
        {
            var phy = _service.GetPhysician(id);
            CreateShift model = new CreateShift
            {
                Physicians = phy
            };
            return Json(phy);
        }

        public IActionResult CreateShift(CreateShift model)
        {

            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            bool flag = _service.CreateShift(model, admin1);
            if (!flag)
            {
                TempData["error"] = "Shift has been clashed";
                return RedirectToAction(nameof(TabChange), new { nav = 5, isPartial = false });
            }
            return RedirectToAction(nameof(TabChange), new { nav = 5, isPartial = false });

        }
        public IActionResult Shifttab(int id, int day, int month, int year, int region)
        {
            if (id == 1)
            {
                var a = _service.GetDayWiseData(day, month, year, region);
                return PartialView("DayWiseShift", a);

            }
            else if (id == 2)
            {
                return PartialView("WeekWiseShift", _service.GetWeekWiseData(day, month, year));
            }
            else
            {
                return PartialView("MonthWiseShift", _service.GetMonthWiseData(day, month, year));
            }
        }
        public IActionResult ShiftReview()
        {
            return View(_service.GetRegions());
        }
        public IActionResult ShiftReviews(int region)
        {
            return PartialView("TableShiftData", _service.GetPendingShiftData(region));
        }

        public IActionResult ApproveShift(int[] shiftDetailsId)
        {

            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            _service.ApproveSelectedShift(shiftDetailsId, admin1);

            return Ok();
        }

        public IActionResult DeleteSelectedShift(int[] shiftDetailsId)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");

            _service.DeleteShiftReview(shiftDetailsId, admin1);

            return Ok();
        }
        [HttpGet]
        public IActionResult GetOnCall(int regionid)
        {
            var MdsCallModal = _service.GetOnCallDetails(regionid);
            return PartialView("MdOnCall", MdsCallModal);
        }

        [HttpGet]
        public IActionResult EditShift(int shiftdetailid)
        {
            return PartialView("EditShift", _service.EditShift(shiftdetailid));
        }

        [HttpPost]
        public IActionResult EditShift(EditShift editShift, int shiftdetailid)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            int adminid = _service.GetAspId(admin1);
            _service.UpdateShift(editShift, shiftdetailid, adminid);
            TempData["success"] = "Shift Updated Successfully!!!";
            return RedirectToAction(nameof(TabChange), new { nav = 5, isPartial = false });
        }
        public IActionResult ChangeShiftStatus(int shiftdetailid)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            int adminid = _service.GetAspId(admin1);

            _service.ChangeShiftStatus(shiftdetailid, adminid);
            TempData["success"] = "Shift Updated Successfully!!!";
            return RedirectToAction(nameof(TabChange), new { nav = 5, isPartial = false });
        }
        public IActionResult DeleteShiftViaModal(int shiftdetailid)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            int adminid = _service.GetAspId(admin1);
            _service.DeleteShiftViaModal(shiftdetailid, adminid);
            TempData["success"] = "Shift Updated Successfully!!!";
            return RedirectToAction(nameof(TabChange), new { nav = 5, isPartial = false });
        }
        public IActionResult PatientHistoryTable(string? fname, string? lname, string? email, string? phone, int page)
        {
            if (page == 0) { page = 1; }
            ViewBag.page = page;
            List<PatientHistoryTable> model = _service.PatientHistoryTable(fname, lname, email, phone, page);
            return PartialView("PatientHistoryTable", model);
        }
        public IActionResult PatientRecord(int id)
        {
            List<PatientRecord> model = _service.PatientRecord(id);
            return View("PatientRecord", model);
        }

        public IActionResult SearchRecord(AdminRecord model)
        {
            model.ReqType = _service.GetRequestTypes();

            return PartialView(model);
        }


        [HttpPost]
        public IActionResult _SearchRecordsTable(AdminRecord model, int status, string mobile, string email, string pname, DateTime tdate, DateTime fdate, int reqtype, string searchstr, int page)
        {
            if (page == 0) { page = 1; }
            ViewBag.page = page;
            model = _service.getSearchRecordData(model);


            if (!string.IsNullOrWhiteSpace(searchstr))
            {
                model.Data = model.Data.Where(x => x.FirstName.ToLower().Contains(searchstr.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                model.Data = model.Data.Where(x => x.PhoneNumber.ToLower().Contains(mobile.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                model.Data = model.Data.Where(x => x.Email.ToLower().Contains(email.ToLower())).ToList();
            }

            if (status != 0)
            {
                int[] st = new int[1];
                switch (status)
                {
                    case 1: st = new int[] { 1 }; break;
                    case 2: st = new int[] { 2 }; break;
                    case 3: st = new int[] { 4, 5 }; break;
                    case 4: st = new int[] { 6 }; break;
                    case 5: st = new int[] { 3, 7, 8 }; break;
                    case 6: st = new int[] { 9 }; break;
                }
                model.Data = model.Data.Where(x => st.Any(y => y == x.Request.Status)).ToList();

            }
            if (reqtype != 0)
            {

                model.Data = model.Data.Where(x => x.Request.RequestTypeId == reqtype).ToList();

            }
            if (fdate != DateTime.MinValue)
            {

                model.Data = model.Data.Where(x => x.Request.CreatedDate > fdate).OrderBy(x => x.Request.CreatedDate).ToList();

            }

            if (tdate != DateTime.MinValue)
            {
                model.Data = model.Data.Where(x => x.Request.CreatedDate < tdate).OrderBy(x => x.Request.CreatedDate).ToList();
            }

            if (tdate != DateTime.MinValue && tdate != DateTime.MinValue)
            {
                model.Data = model.Data.Where(x => x.Request.CreatedDate > fdate && x.Request.CreatedDate < tdate).OrderBy(x => x.Request.CreatedDate).ToList();
            }
            if (model.Data.Count != 0)
            {
                model.PgCount = model.Data.Count;
            }
            int size = 10;
            model.Data = model.Data.Skip(page * size - size).Take(size).ToList();


            return PartialView(model);
        }

        public IActionResult DeleteAccountRecord(int id)
        {
            _service.deleteRequest(id)
;
            return RedirectToAction(nameof(TabChange), new { nav = 14, isPartial = false });
        }

        [HttpPost]
        public IActionResult _BlockHistoryTable(string searchstr, DateTime date, string email, string mobile, int page)
        {
            if (page == 0) { page = 1; }
            ViewBag.page = page;
            var a = _service.getBlockHistoryData();
            if (!string.IsNullOrWhiteSpace(searchstr))
            {
                a.blockRequests = a.blockRequests.Where(x => x.Request.RequestClients.First().FirstName.Contains(searchstr, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                a.blockRequests = a.blockRequests.Where(x => x.PhoneNumber.Contains(mobile, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                a.blockRequests = a.blockRequests.Where(x => x.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (date != DateTime.MinValue)
            {
                a.blockRequests = a.blockRequests.Where(x => x.CreatedDate < date).OrderBy(x => x.CreatedDate).ToList();
            }
            if (a.blockRequests.Count() != 0)
            {
                a.PgCount = a.blockRequests.Count();
            }
            int size = 10;
            a.blockRequests = a.blockRequests.Skip(page * size - size).Take(size).ToList();
            return PartialView(a);
        }

        public IActionResult UnblockAccount(int id)
        {
            _service.Unblock(id);
            return RedirectToAction(nameof(TabChange), new { nav = 15, isPartial = false });
        }

        public IActionResult EmailLogs(string name, string email, DateTime createdate, DateTime sentdate, int page)
        {
            if (page == 0) { page = 1; }
            ViewBag.page = page;
            return PartialView("_EmailLogsTable", _service.EmailLogs( name, email, createdate, sentdate, page));
        }


        public IActionResult SmsLogs(string name, string mobile, DateTime createdate, DateTime sentdate, int page)
        {
            if (page == 0) { page = 1; }
            ViewBag.page = page;
            return PartialView("_SmsLogsTable", _service.SmsLog(name, mobile, createdate, sentdate, page));
        }

        public IActionResult ExportRecords(string providername, string patientname, int status, int reqtype, string email, string phone, DateTime fromdate, DateTime todate)
        {
            List<AdminRecord> records = _service.SearchRecords(providername, patientname, status, reqtype, email, phone, fromdate, todate);
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Data");


            worksheet.Cell(1, 1).Value = "Patient Name";
            worksheet.Cell(1, 2).Value = "Request Type";
            worksheet.Cell(1, 3).Value = "Date of Service";
            worksheet.Cell(1, 4).Value = "Email";
            worksheet.Cell(1, 5).Value = "Phone Number";
            worksheet.Cell(1, 6).Value = "Address";
            worksheet.Cell(1, 7).Value = "Zip";
            worksheet.Cell(1, 8).Value = "Status";
            worksheet.Cell(1, 9).Value = "Physician";
            worksheet.Cell(1, 10).Value = "Physician Notes";
            worksheet.Cell(1, 11).Value = "Admin Notes";
            worksheet.Cell(1, 12).Value = "Patient Notes";


            int row = 2;
            foreach (var item in records)
            {
                var statusClass = "";
                if (item.ReqtypeId == 2)
                {
                    statusClass = "Patient";
                }
                else if (item.ReqtypeId == 1)
                {
                    statusClass = "Business";
                }
                else if (item.ReqtypeId == 3)
                {
                    statusClass = "Family";
                }
                else
                {
                    statusClass = "Concierge";
                }

                worksheet.Cell(row, 1).Value = item.PatientName;
                worksheet.Cell(row, 2).Value = statusClass;
                worksheet.Cell(row, 3).Value = item.DateOfService;
                worksheet.Cell(row, 4).Value = item.Email;
                worksheet.Cell(row, 5).Value = item.PhoneNumber;
                worksheet.Cell(row, 6).Value = item.Address;
                worksheet.Cell(row, 7).Value = item.zip;
                worksheet.Cell(row, 8).Value = item.status;
                worksheet.Cell(row, 9).Value = item.ProviderName;
                worksheet.Cell(row, 10).Value = item.PhysicianNote;
                worksheet.Cell(row, 11).Value = item.AdminNote;
                worksheet.Cell(row, 12).Value = item.PatientNote;
                row++;
            }
            worksheet.Columns().AdjustToContents();

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PatientRecords.xlsx");
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
            int admin1 = (int)HttpContext.Session.GetInt32("Id");

            Encounter en = _service.getencounter(Id);
            byte[] pdfdata = _pdf.GeneratePDF(en);
            return File(pdfdata, "application/pdf", "MedicalReport.pdf");
        }



    }
}
