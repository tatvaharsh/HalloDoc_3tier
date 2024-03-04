using Azure.Core;
using hallocdoc_mvc_Service.Interface;
using hallodoc_mvc_Repository.DataContext;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace hallodoc_mvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdmin_Service _service;

        public AdminController(IAdmin_Service service)
        {
            //_context = context;
            _service = service;
        }
        public IActionResult Admin_Dashboard()
        {
           var admin = HttpContext.Session.GetString("UserName");
            ViewBag.Username = admin;
            return View(); 
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

                if (ModelState.IsValid)
                {
                    bool isReg = _service.ValidateUser(model);

                    if (isReg)
                    {
                        var Admin = _service.getAdmin(model.Email);
                         HttpContext.Session.SetInt32("Id", Admin.AdminId);
                      
                         HttpContext.Session.SetString("UserName", Admin.FirstName);
                        ViewBag.Username = Admin.FirstName;
                        return RedirectToAction("Admin_Dashboard");
                    }
                }

            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Id");
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
            ViewCase vc=_service.Getcase(Id);

            return View(vc);
        }
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


            return View(vn);
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

    }
}
