using hallocdoc_mvc_Service.Implementation;
using hallocdoc_mvc_Service.Interface;
using hallodoc_mvc_Repository.DataContext;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HalloDoc.Auth;
using NuGet.Protocol;
using NuGet.Common;
using ClosedXML.Excel;
using System.IdentityModel.Tokens.Jwt;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using NuGet.Protocol.Core.Types;

namespace hallodoc_mvc.Controllers
{
   
    public class AdminController : Controller
    {
        private readonly IAdmin_Service _service;

        private readonly IJwtService _jwtService;
        
        private readonly IConfiguration _configuration;



        public AdminController(IAdmin_Service service,IJwtService jwtService, IConfiguration configuration)
        {
            //_context = context;
            _service = service;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        [CustomAuthorize("Admin")]
        public IActionResult Admin_Dashboard()
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
         
            if(admin1 != null)
            {
                ViewBag.Username = _service.Adminname(admin1);
                ModalData data = _service.GetAssignData(new ModalData());
                //TempData["success"] = "Login Successfully!!!";
                //TempData.Clear();
                ViewBag.Layout = "_LayAdmin";
                return View(data); 
            }
            return RedirectToAction("Admin_Login");
        }
        
        [CustomAuthorize("Admin")]
        public IActionResult Admin_DashboardPartial()
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
         
            if(admin1 != null)
            {
                ViewBag.Username = _service.Adminname(admin1);
                ModalData data = _service.GetAssignData(new ModalData());
                //TempData["success"] = "Login Successfully!!!";
                //TempData.Clear();
                ViewBag.Layout = null;
                return PartialView("Admin_Dashboard", data); 
            }
            return RedirectToAction("Admin_Login");
        }

        public IActionResult Admin_Login()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Admin_Login(LoginViewModel model)
        {   
            if (ModelState.IsValid)
            {
                bool isReg = _service.ValidateUser(model);

                if (isReg)
                {
                    var Admin = _service.getAdmin(model.Email);
                    if (Admin != null)
                    {
                        HttpContext.Session.SetInt32("Id", Admin.AdminId);
                       
                        var token = _jwtService.GenerateJwtToken(model);
                        Response.Cookies.Append("jwt", token);
                        ViewBag.Username = Admin.FirstName;
                       TempData["success"] = "Login Successfully!!!";
                        return RedirectToAction("Admin_Dashboard");
                    }
                }
                TempData["error"] = "Login Failed!!!";


            }
               
            return View();
        }

        public IActionResult Logout()
        {
            
     
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Id");
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Admin_Login");
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
                    return RedirectToAction(nameof(Admin_DashboardPartial));
                case 2:
                    return PartialView("ProviderLocation", _service.ProviderLocation());
                case 3: 
                    return PartialView("Admin_Profile", _service.getprofile(admin));  
                case 4: 
                    return PartialView("Provider", _service.GetRegions());
                case 5:
                    if (isPartial == true)
                    {
                        ViewBag.Layout = null;
                        return PartialView("Scheduling");
                    }
                    ViewBag.Layout = "_LayAdmin";
                    return View("Scheduling", _service.GetRegions());
                case 7:
                    return PartialView("Partners", new PartnersCM()
                    {
                        //partnersdatas =_service.GetAllHealthProfessionaldata(0),
                        Professions = _service.GetProfession()
                    });
                case 8:
                    if(isPartial == true)
                    {
                        ViewBag.Layout = null;
                        return PartialView("AccountAccess", _service.getAccess());
                    }
                    ViewBag.Layout = "_LayAdmin";
                    return View("AccountAccess", _service.getAccess());
                case 9:
                    return PartialView("UserAccess");
                case 10:
                    return PartialView("AdminAccount", new CreateAdmin()
                    {
                        reg = _service.getreg(),
                        roles = _service.GetRoleOfAdmin(),
                    }); 
                    




            }
            return View();
        }

        public IActionResult New(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            if (pageid == 0) { pageid = 1; };
            var data = _service.getDashData(requestType,search,requestor,region,pageid);
            ViewBag.page = pageid;
            return PartialView("_Admin_Request_Table",data);
        }
        public IActionResult Pending(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            if (pageid == 0) { pageid = 1; };
            var data = _service.getDashDataPending(requestType, search, requestor, region,pageid);
            //AdminDashboard adminDashboard = new AdminDashboard();
            ViewBag.page = pageid;
            return PartialView("_Admin_Request_Table",data);
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
        public IActionResult ToClose(int? requestType, string? search, int? requestor, int? region,int pageid)
        {
            if (pageid == 0) { pageid = 1; };
            var data = _service.getDashDataToclose(requestType,search,requestor,region, pageid);
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
        public IActionResult ViewNotes(ViewNote model,int Id)
        {
            int admin =(int)HttpContext.Session.GetInt32("Id");
            ViewBag.Username = _service.Adminname(admin);
            var vn= _service.setViewNotesData(model,Id,admin);
            TempData["success"] = "Note Added Successfully!!!";
            return RedirectToAction("ViewNotes",Id);
        }


        public IActionResult AssignCase(ModalData md)
        {
            ModalData data = _service.GetAssignData(md);
            return PartialView("AssignCase",data);
        }

        [HttpPost]
        public IActionResult AssignCase(ModalData md,int Id)
        {
                int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.AssignCase(md,Id,admin);
            return RedirectToAction(nameof(AdminController.Admin_Dashboard));
        }

        public IActionResult Cancel(ModalData md,int Id)
        {
            var cancelcase = _service.GetCancelCaseData(md,Id);

            ModalData modalData = new ModalData();
            {
                modalData.PatientName = cancelcase.FirstName + ' ' + cancelcase.LastName;
                modalData.requestID = cancelcase.RequestId;

                modalData.CaseTags = _service.GetCaseReason();
            };

            return PartialView("CancelModal",modalData);
        }

        [HttpPost]
        public IActionResult canclePost(ModalData md, int Id)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.CanclePost(md,Id,admin);

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
        public IActionResult Block(ModalData md,int Id)
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
            _service.DeleteCustom(filenames);
            return RedirectToAction(nameof(ViewUpload), new { id = id });
        }


        [HttpPost]
        public IActionResult FileUpload(int id, [FromForm] List<IFormFile> File)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.FileUpload(id, File,admin);
            return RedirectToAction(nameof(ViewUpload), new { id = id });
        }
        public IActionResult DeleteFile(int id)
        {
           var rq = _service.DeleteFile(id);
            return RedirectToAction(nameof(ViewUpload), "Admin", new { id = rq });
        }

  
        public IActionResult SendEmail(int id)
        {
            _service.SendEmail(id);
            return RedirectToAction(nameof(ViewUpload), "Admin", new { id = id });
        }
        public IActionResult Order(Order md)
        {
            Order data = _service.GetOrderData(md);
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
            return RedirectToAction(nameof(AdminController.Admin_Dashboard));
        }
        public IActionResult Transfer(ModalData md)
        {
            ModalData data = _service.GetAssignData(md);
            return PartialView("Transfer", data);
        }
    
        [HttpPost]
        public IActionResult Transfer( int Id, ModalData md)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.AssignCase(md, Id, admin);
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
        public IActionResult SendAgreement(int Id ,int requestType)
        {
            ViewBag.requestType = requestType;

            RequestClient rc = _service.GetAgreementtdata(Id);
            ModalData md = new()
            {
                number=rc.PhoneNumber,
                email=rc.Email,
            };
            return PartialView("SendAgreement",md);
        }
        [HttpPost]
        public IActionResult SendAgreement(int Id, ModalData md)
        {
            var token=_jwtService.GenerateJwtTokenByEmail(md.email);

            _service.SendAgreementMail(Id,md,token);
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
             _service.editdata(model,Id); 
            return RedirectToAction(nameof(Close), new {Id = Id});
        }
        public IActionResult saavclose(int Id)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.close(Id, admin);
            TempData["success"] = "Case Closed Successfully!!!";
            return RedirectToAction("Admin_Dashboard");
        }
        public IActionResult Agreement(int token, string t)
        {
            if(_jwtService.ValidateJwtToken(t , out JwtSecurityToken jwtSecurityToken))
            {
                ModalData md = _service.cancelmodal(token);
                return View(md);
            }
            return NotFound();
            
        }

        public IActionResult Agree(int id)
        {
            _service.agreeagreement(id);
            return View();
        }
        public IActionResult GetCancel(int id,ModalData md)
        {
            _service.cancelagreement(id,md);
            return RedirectToAction(nameof(Agreement));
        }
        public IActionResult Encounter(int id,Encounter model)
        {
            ViewBag.Username = HttpContext.Session.GetString("UserName");
            
            Encounter en = _service.getencounter(id);
            return View(en);  
        }
        public IActionResult ChangeEncounter(int id,Encounter model)
        {
             _service.editencounter(id, model);
            return RedirectToAction("Encounter", new {id=id});
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
            //var accountSid = _configuration["Twilio:accountSid"];
            //var authToken = _configuration["Twilio:authToken"];
            //var twilionumber = _configuration["Twilio:twilioNumber"];
       

            //var messageBody = $"Hello {model.FirstName} {model.LastName},\nClick the following link to create new request in our portal,\nhttp://localhost:5198/Home/submit_screen\n\n\nRegards,\nHalloDoc";

            //TwilioClient.Init(accountSid, authToken);

            //var message = MessageResource.Create(
            //    from: new Twilio.Types.PhoneNumber(twilionumber),
            //    body: messageBody,
            //    to: new Twilio.Types.PhoneNumber("+91" + model.PhoneNumber)
            //);

            _service.sendlink(model);
            return RedirectToAction("Admin_Dashboard");
        }
        public IActionResult RequestSupport()
        {
            return PartialView();
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
            _service.PatientForm(model,admin);

            return RedirectToAction(nameof(Admin_Dashboard));

        }

        public IActionResult Export(string s, int reqtype, int regid, int state)
        {
            var data = _service.Export(s,reqtype,regid,state);
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
            if(model.AdminData.Mobile?.Length != 10)
            {
                TempData["error"] = "Enter Valid MobileNumber";
                return RedirectToAction("Admin_Dashboard");
            }

            int admin = (int)HttpContext.Session.GetInt32("Id");
            ViewBag.Username = _service.Adminname(admin);
            _service.editadminprofile(model, admin);
            TempData["success"] = "Profile Changed Successfully!!!";
            return RedirectToAction("Admin_Dashboard");
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
            return RedirectToAction("Admin_Dashboard");
        }

        [HttpPost]
        public IActionResult ResetPassword(Profile model)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.reset(model,admin);
            TempData["success"] = "Password Changed successfully!!!";
            return RedirectToAction("Admin_Dashboard");
        }

        public IActionResult Provider(int region)
        {
            List<Provider> p = _service.Getphysician(region);
            return PartialView("_TablePhysician",p);
        }

        public IActionResult CreatePhysicianAccount()
        {
            var reg = _service.getreg();
            var roles = _service.getrole();
            CreatePhy createPhy = new CreatePhy()
            {
                reg = reg,
                roles = roles,


            };
            return View(createPhy);
        }

        public IActionResult EditPhysicianAccount(int id)
        {
            var data = _service.getphysiciandata(id);
           
            return View(data);
        }
        public IActionResult ProviderMenuModal(int id)
        {
            return PartialView(new ModalData() { phyid=id});
        }

        public void ChangeToggle(int id)
        {
            _service.ChangeToggle(id);

        }

        public IActionResult SendEmailOrMessage(int id, ModalData md)
        {
            _service.Sendit(id, md);
            return RedirectToAction("Provider");
        }

        [HttpPost]
        public IActionResult CreatePhysicianAccount(CreatePhy model)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            if (ModelState.IsValid)
            {
                _service.CreateProvider(model,admin1);
                return RedirectToAction("Admin_Dashboard");
            }
            var reg = _service.getreg();
            var roles = _service.getrole();
            model.reg = reg; model.roles = roles;
            return View(model);
        }


        
        public IActionResult CreateRole(int check)
        {
            RoleModel model = _service.GetMenutbl(check);
            return View(model);
        }
        public IActionResult AssignRole(string RoleName, string[] selectedRoles, int check)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            _service.AssignRole(RoleName, selectedRoles, check, admin1);
            return RedirectToAction("TabChange", new { nav= 8 });
        }

        public IActionResult EditRole(int id)
        {
            return View(_service.GetRolewiseData(id));
        }
        public IActionResult UpdateRole(RoleModel model)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            _service.UpdateRole(model);
            return PartialView("AccountAccess", _service.getAccess());
        }
        public IActionResult DeleteRole(int id)
        {
            _service.DeleteRoles(id);
                return View("AccountAccess", _service.getAccess());
        }

        [HttpPost]
        public IActionResult EditPhyInfo(int id,CreatePhy model) 
        {
            _service.EditPhyInfo(id,model);
            return RedirectToAction("Admin_Dashboard");
        }

        [HttpPost]
        public IActionResult EditPhyMailBillInfo(int id, CreatePhy model)
        {
            _service.EditPhyMailBillInfo(id, model);
            return RedirectToAction("Admin_Dashboard");
        }
        [HttpPost]
        public IActionResult EditPhyProvider(int id, CreatePhy model)
        {
            _service.EditPhyProvider(id, model);
            return RedirectToAction("Admin_Dashboard");
        }
        [HttpPost]
        public IActionResult EditPhyDocs(int id, CreatePhy model)
        {
            _service.EditPhyDocs(id, model);
            return RedirectToAction("Admin_Dashboard");
        }
        public IActionResult DeletePhy(int id)
        {
            _service.DeletePhy(id);
            return RedirectToAction("Admin_Dashboard");
        }

   

        public IActionResult UserAccess(int region)
        {
            var data = _service.GetUserAccessData(region);
            return PartialView("TableUserAccess",data);
        }


        [HttpPost]
        public IActionResult CreateAdminAccount(CreateAdmin model)
        {
            int admin1 = (int)HttpContext.Session.GetInt32("Id");
            if (ModelState.IsValid)
            {
                _service.CreateAdmin(model, admin1);
                return RedirectToAction("Admin_Dashboard");
            }
      
            var reg = _service.getreg();
            var roles = _service.GetRoleOfAdmin();
            model.reg = reg; model.roles = roles;
            return View(model);
        }


        public IActionResult Partners(int professionid,string search)
        {
            var Partnersdata = _service.GetAllHealthProfessionaldata(professionid,search);
            return PartialView("TablePartner", new PartnersCM()
            {
                Professions = _service.GetProfession(),
                partnersdatas = Partnersdata,
            });
        }

        public IActionResult AddBusiness()
        {
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
            return RedirectToAction("Admin_Dashboard");
        }

        public IActionResult EditPartner(int id)
        {
           var a=  _service.GetPartnerData(id);
    
            return View(a);
        }

        [HttpPost]
        public IActionResult EditPartner(PartnersCM model,int id)
        {
            _service.EditPartner(model,id);
            return RedirectToAction("Admin_Dashboard");
        }

   
        public IActionResult DeletePartner(int id)
        {
            _service.DeletePartner(id);
            return RedirectToAction("Admin_Dashboard");
        }
        public IActionResult CreateShiftModal()
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
           bool flag= _service.CreateShift(model, admin1);
            if (!flag)
            {
                TempData["error"] = "Shift has been clashed";
                return RedirectToAction(nameof(TabChange), new { nav = 5, isPartial = false });
            }
            return RedirectToAction(nameof(TabChange), new { nav = 5, isPartial = false });
           
        }
        public IActionResult Shifttab(int id, int day, int month, int year) 
        {
            if(id == 1)
            {
                return PartialView("DayWiseShift", _service.GetDayWiseData(day, month, year));
            
            }
            else if(id == 2)
            {
                return PartialView("WeekWiseShift", _service.GetWeekWiseData(day, month, year));
            }
            else
            {
                return PartialView("MonthWiseShift",_service.GetMonthWiseData(day, month, year));
            }
        }
        public IActionResult ShiftReview()
        {
            return View(_service.GetRegions());
        }
        public IActionResult ShiftReviews(int region)
        {
            return PartialView("TableShiftData",_service.GetPendingShiftData(region));
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
    }
}
