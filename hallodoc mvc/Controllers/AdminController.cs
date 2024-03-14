
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

namespace hallodoc_mvc.Controllers
{
   
    public class AdminController : Controller
    {
        private readonly IAdmin_Service _service;

        private readonly IJwtService _jwtService;

        public AdminController(IAdmin_Service service,IJwtService jwtService)
        {
            //_context = context;
            _service = service;
            _jwtService = jwtService;
        }

        [CustomAuthorize("Admin")]
        public IActionResult Admin_Dashboard(ModalData md)
        {
            
           var admin = HttpContext.Session.GetString("UserName");
            if(admin != null)
            {
            ViewBag.Username = admin;
                ModalData data = _service.GetAssignData(md);
                TempData["success"] = "Login Successfully!!!";
                //TempData.Clear();
                return View(data); 
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
                        HttpContext.Session.SetString("UserName", Admin.FirstName);
                        var token = _jwtService.GenerateJwtToken(model);
                        Response.Cookies.Append("jwt", token);
                        ViewBag.Username = Admin.FirstName;
                        return RedirectToAction("Admin_Dashboard");
                    }
                }

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

        public IActionResult New(int? requestType, string? search, int? requestor, int? region)
        {
           
            var data = _service.getDashData(requestType,search,requestor,region);
            return PartialView("_Admin_Request_Table",data);
        }
        public IActionResult Pending(int? requestType, string? search, int? requestor, int? region)
        {
            var data = _service.getDashDataPending(requestType, search, requestor, region);
            //AdminDashboard adminDashboard = new AdminDashboard();
            return PartialView("_Admin_Request_Table",data);
        }
        public IActionResult Active(int? requestType, string? search, int? requestor, int? region)
        {
            var data = _service.getDashDataActive(requestType, search, requestor, region);
            //AdminDashboard adminDashboard = new AdminDashboard();
            return PartialView("_Admin_Request_Table", data);
        }

        public IActionResult Conclude(int? requestType, string? search, int? requestor, int? region)
        {
            var data = _service.getDashDataConclude(requestType, search, requestor, region);
            //AdminDashboard adminDashboard = new AdminDashboard();
            return PartialView("_Admin_Request_Table", data);
        }
        public IActionResult ToClose(int? requestType, string? search, int? requestor, int? region)
        {
            var data = _service.getDashDataToclose(requestType,search,requestor,region);
            //AdminDashboard adminDashboard = new AdminDashboard();
            return PartialView("_Admin_Request_Table", data);
        }
     
        public IActionResult Unpaid(int? requestType, string? search, int? requestor, int? region)
        {
            var data = _service.getDashDataUnpaid(requestType, search, requestor, region);
            //AdminDashboard adminDashboard = new AdminDashboard();
            return PartialView("_Admin_Request_Table", data);
        }

        public IActionResult ViewCase(int Id)
        {
            var admin = HttpContext.Session.GetString("UserName");
            ViewBag.Username = admin;

            ViewCase vc =_service.Getcase(Id);

            return View(vc);
        }

        [HttpGet]
        public IActionResult ViewNotes(int Id)
        {

            ViewNote rn = _service.GetNotes(Id);
            return View(rn);
        }

        [HttpPost]
        public IActionResult ViewNotes(ViewNote model,int Id)
        {
            int admin =(int)HttpContext.Session.GetInt32("Id");
            var vn= _service.setViewNotesData(model,Id,admin);

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
        public IActionResult FileUpload(int id, [FromForm] List<IFormFile> File)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.FileUpload(id, File,admin);
            return RedirectToAction(nameof(ViewUpload), new { id = id });
        }
        public IActionResult DeleteFile(int id)
        {
            _service.DeleteFile(id);
            return RedirectToAction(nameof(Admin_Dashboard));
        }

        public IActionResult DeleteAllFiles(int id)
        {
            _service.DeleteAllFiles(id);
            return RedirectToAction(nameof(Admin_Dashboard));

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
          
            return View();
        }
        [HttpPost]
        public IActionResult Sendlink(ViewCase model)
        {
            _service.sendlink(model);
            return RedirectToAction("Admin_Dashboard");
        }
        public IActionResult RequestSupport()
        {
            return View();
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
                row++;
            }
            worksheet.Columns().AdjustToContents();

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
        }

        [HttpPost]
        public IActionResult EditAdminProfile(Profile model) 
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.editadminprofile(model,admin);
            return RedirectToAction("Admin_profile");
        }

        [HttpPost]
        public IActionResult EditAdminp(Profile model)
        {
            int admin = (int)HttpContext.Session.GetInt32("Id");
            _service.editadminp(model, admin);
            return RedirectToAction("Admin_profile");
        }


    }
}
