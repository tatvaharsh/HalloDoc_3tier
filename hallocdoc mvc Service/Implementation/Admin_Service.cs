using hallocdoc_mvc_Service.Interface;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.Interface;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace hallocdoc_mvc_Service.Implementation
{
    public class Admin_Service : IAdmin_Service
    {
        private readonly IAdmin_Repository _Repository;

        public Admin_Service(IAdmin_Repository Repository)
        {
            _Repository = Repository;
        }

        public List<AdminDashboard> getDashData(int? requestType, string? search, int? requestor, int? region)
        {
            var query  = _Repository.GetAdminCode();

            if (search != null)
            {
                query = query.Where(r => (bool)r.RequestClients.FirstOrDefault().FirstName.Contains(search) || (bool)r.RequestClients.FirstOrDefault().LastName.Contains(search));
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3);
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1);
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4);
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2);
            }
            if (region != null && region != -1)
            {
                query = query.Where(r => r.RequestClients.FirstOrDefault().RegionId == region);

            }

            var dashData = new List<AdminDashboard>();

           

            foreach(var item in query)
            {
                DateOnly Mydate = new(item.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(item.RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, item.RequestClients.FirstOrDefault().IntDate.Value);
                dashData.Add(new AdminDashboard
                {
                    Id = item.RequestId,
                    LName=item.RequestClients.FirstOrDefault()?.LastName,
                    FName=item.RequestClients.FirstOrDefault()?.FirstName,
                    Email=item.RequestClients.FirstOrDefault()?.Email,
                    Name = item.RequestClients.FirstOrDefault()?.FirstName,
                    RPhone=item.PhoneNumber,
                    DateOfBirth=Mydate,
                    Requestor = item.FirstName+' '+item.LastName,
                    RequestDate = item.CreatedDate,
                    Phone = item.RequestClients.FirstOrDefault()?.PhoneNumber,
                    Address = item.RequestClients.FirstOrDefault()?.Street + ", " + item.RequestClients.FirstOrDefault()?.City + ", " + item.RequestClients.FirstOrDefault()?.State,
                    //Notes = item.RequestClients.FirstOrDefault()?.Notes,
                    RequestTypeId = item.RequestTypeId,
                    Status = item.Status,
                });
            }

            return dashData;
        }

        public List<AdminDashboard> getDashDataActive(int? requestType, string? search, int? requestor, int? region)
        {
            var query = _Repository.GetAdminStatus();

            if (search != null)
            {
                query = query.Where(r => (bool)r.RequestClients.FirstOrDefault().FirstName.Contains(search) || (bool)r.RequestClients.FirstOrDefault().LastName.Contains(search));
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3);
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1);
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4);
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2);
            }
            if (region != null && region != -1)
            {
                query = query.Where(r => r.RequestClients.FirstOrDefault().RegionId == region);

            }

            var dashData = new List<AdminDashboard>();

            foreach (var item in query)
            {
                DateOnly Mydate = new(item.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(item.RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, item.RequestClients.FirstOrDefault().IntDate.Value);
                dashData.Add(new AdminDashboard
                {
                    Id = item.RequestId,
                    Name = item.RequestClients.FirstOrDefault()?.FirstName,
                    DateOfBirth=Mydate,
                    Requestor = item.FirstName,
                    RequestDate = item.CreatedDate,
                    Phone = item.RequestClients.FirstOrDefault()?.PhoneNumber,
                    Address = item.RequestClients.FirstOrDefault()?.Street + ", " + item.RequestClients.FirstOrDefault()?.City + ", " + item.RequestClients.FirstOrDefault()?.State,
                    //Notes = item.RequestClients.FirstOrDefault()?.Notes,
                    RequestTypeId = item.RequestTypeId,
                    Status = item.Status,
                });
            }

            return dashData;
        }
        public List<AdminDashboard> getDashDataPending(int? requestType, string? search, int? requestor, int? region)
        {
            var query = _Repository.GetAdminPending();

            if (search != null)
            {
                query = query.Where(r => (bool)r.RequestClients.FirstOrDefault().FirstName.Contains(search) || (bool)r.RequestClients.FirstOrDefault().LastName.Contains(search));
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3);
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1);
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4);
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2);
            }
            if (region != null && region != -1)
            {
                query = query.Where(r => r.RequestClients.FirstOrDefault().RegionId == region);

            }

            var dashData = new List<AdminDashboard>();

            

            foreach (var item in query)
            {

                //List<RequestStatusLog?> requestStatusLog = _Repository.GetStatusLogsByRequest(item.RequestId);
                //List<string?> transfer = new();

                //requestStatusLog.ForEach(x =>
                //{
                //    Physician? phy = _Repository.GetPhysician(x.TransToPhysicianId);
                //    transfer.Add("Admin transferred to Dr : " + phy?.FirstName + " on " + x.CreatedDate.ToString("dd/MM/yyyy") + " at " + x.CreatedDate.ToString("HH: mm:ss: tt") + " " + x.Notes);
                //});


                DateOnly Mydate = new(item.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(item.RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, item.RequestClients.FirstOrDefault().IntDate.Value);
                dashData.Add(new AdminDashboard
                {
                    Id = item.RequestId,
                    Name = item.RequestClients.FirstOrDefault()?.FirstName,
                    DateOfBirth=Mydate,
                    Requestor = item.FirstName,
                    RequestDate = item.CreatedDate,
                    Phone = item.RequestClients.FirstOrDefault()?.PhoneNumber,
                    Address = item.RequestClients.FirstOrDefault()?.Street + ", " + item.RequestClients.FirstOrDefault()?.City + ", " + item.RequestClients.FirstOrDefault()?.State,
                    //Notes = transfer,
                    RequestTypeId = item.RequestTypeId,
                    Status = item.Status,
                });
            }

            return dashData;
        }
        public List<AdminDashboard> getDashDataConclude(int? requestType, string? search, int? requestor, int? region)
        {
            var query = _Repository.GetAdminConclude();

            if (search != null)
            {
                query = query.Where(r => (bool)r.RequestClients.FirstOrDefault().FirstName.Contains(search) || (bool)r.RequestClients.FirstOrDefault().LastName.Contains(search));
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3);
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1);
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4);
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2);
            }
            if (region != null && region != -1)
            {
                query = query.Where(r => r.RequestClients.FirstOrDefault().RegionId == region);

            }

            var dashData = new List<AdminDashboard>();

            foreach (var item in query)
            {
                DateOnly Mydate = new(item.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(item.RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, item.RequestClients.FirstOrDefault().IntDate.Value);
                dashData.Add(new AdminDashboard
                {
                    Id = item.RequestId,
                    Name = item.RequestClients.FirstOrDefault()?.FirstName,
                    DateOfBirth=Mydate,
                    Requestor = item.FirstName,
                    RequestDate = item.CreatedDate,
                    Phone = item.RequestClients.FirstOrDefault()?.PhoneNumber,
                    Address = item.RequestClients.FirstOrDefault()?.Street + ", " + item.RequestClients.FirstOrDefault()?.City + ", " + item.RequestClients.FirstOrDefault()?.State,
                    //Notes = item.RequestClients.FirstOrDefault()?.Notes,
                    RequestTypeId = item.RequestTypeId,
                    Status = item.Status,
                });
            }

            return dashData;
        }
        public List<AdminDashboard> getDashDataToclose(int? requestType, string? search, int? requestor, int? region)
        {
            var query = _Repository.GetAdminToclose();

            if (search != null)
            {
                query = query.Where(r => (bool)r.RequestClients.FirstOrDefault().FirstName.Contains(search) || (bool)r.RequestClients.FirstOrDefault().LastName.Contains(search));
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3);
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1);
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4);
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2);
            }
            if (region != null && region != -1)
            {
                query = query.Where(r => r.RequestClients.FirstOrDefault().RegionId == region);

            }

            var dashData = new List<AdminDashboard>();

            foreach (var item in query)
            {
                DateOnly Mydate = new(item.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(item.RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, item.RequestClients.FirstOrDefault().IntDate.Value);
                dashData.Add(new AdminDashboard
                {
                    Id = item.RequestId,
                    Name = item.RequestClients.FirstOrDefault()?.FirstName,
                    DateOfBirth=Mydate,
                    Requestor = item.FirstName,
                    RequestDate = item.CreatedDate,
                    Phone = item.RequestClients.FirstOrDefault()?.PhoneNumber,
                    Address = item.RequestClients.FirstOrDefault()?.Street + ", " + item.RequestClients.FirstOrDefault()?.City + ", " + item.RequestClients.FirstOrDefault()?.State,
                    //Notes = item.RequestClients.FirstOrDefault()?.Notes,
                    RequestTypeId = item.RequestTypeId,
                    Status = item.Status,
                });
            }

            return dashData;
        }
        public List<AdminDashboard> getDashDataUnpaid(int? requestType, string? search, int? requestor, int? region)
        {
            var query = _Repository.GetAdminUnpaid();

            if (search != null)
            {
                query = query.Where(r => (bool)r.RequestClients.FirstOrDefault().FirstName.Contains(search) || (bool)r.RequestClients.FirstOrDefault().LastName.Contains(search));
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3);
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1);
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4);
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2);
            }
            if (region != null && region != -1)
            {
                query = query.Where(r => r.RequestClients.FirstOrDefault().RegionId == region);

            }

            var dashData = new List<AdminDashboard>();

            foreach (var item in query)
            {
                DateOnly Mydate = new(item.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(item.RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, item.RequestClients.FirstOrDefault().IntDate.Value);
                dashData.Add(new AdminDashboard
                {
                    Id= item.RequestId,
                    Name = item.RequestClients.FirstOrDefault()?.FirstName,
                    DateOfBirth=Mydate,
                    Requestor = item.FirstName,
                    RequestDate = item.CreatedDate,
                    Phone = item.RequestClients.FirstOrDefault()?.PhoneNumber,
                    Address = item.RequestClients.FirstOrDefault()?.Street + ", " + item.RequestClients.FirstOrDefault()?.City + ", " + item.RequestClients.FirstOrDefault()?.State,
                    //Notes = item.RequestClients.FirstkOrDefault()?.Notes,
                    RequestTypeId = item.RequestTypeId,
                    Status = item.Status,
                });
            }

            return dashData;
        }

        public ViewCase Getcase(int reqId)
        {
            var requestData = _Repository.GetRequest().Where(x=>x.RequestId==reqId);
            //var requestData = _context.Requests.FirstOrDefault(r => r.Requestid == ReqId);

            DateOnly date = new DateOnly(requestData.FirstOrDefault().RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(requestData.FirstOrDefault().RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, requestData.FirstOrDefault().RequestClients.FirstOrDefault().IntDate.Value);


            ViewCase ViewCase = new()
                {
                    FirstName = requestData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.FirstName,
                    LastName = requestData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.LastName,
                    PhoneNumber = requestData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.PhoneNumber,
                    Email = requestData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.Email,
                    PatientNotes = requestData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.Notes,
                    BirthDate = date,
                    AddressORBusinessName = requestData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.Address,
                    Status = requestData.FirstOrDefault().Status,
                    ReqType = requestData.FirstOrDefault().RequestTypeId,
                    ConfirmationNO = requestData.FirstOrDefault()?.ConfirmationNumber,
                            
                    // Room = requestData.roo,
                    // PatientRegion = requestData.
                };

                return ViewCase;
            
        }

        public bool ValidateUser(LoginViewModel model)
        {
            AspNetUser asp = _Repository.Validate(model);
            if (asp.Id>0)
            {
                model.Id=asp.Id;
                return true;  
            }
            return false;
        }

        public Admin getAdmin(string email)
        {
            return _Repository.getaspuser(email);
        }

        public ModalData GetAssignData(ModalData md)
        {
            List<Region> r = _Repository.GetRegion();
            md.region = r;
           
            return md;
        }

        public ViewNote? setViewNotesData(ViewNote model, int id, int admin)
        {
            //var notedata = _context.Requestnotes.FirstOrDefault(i => i.Requestid == requestid);
            var notedata = _Repository.setnotes(id);

            if (notedata != null)
            {
                notedata.RequestId = id;
                notedata.AdminNotes = model.AdminNotes;
                notedata.ModifiedBy = admin;
                notedata.ModifiedDate = DateTime.Now;
                _Repository.save();
                
            }
            else
            {
                var newnotedata = new RequestNote()
                {
                    RequestId = id,
                    AdminNotes = model.AdminNotes,
                    CreatedBy = admin,
                    CreatedDate = DateTime.Now,
                };

                _Repository.AddRequestNotes(newnotedata);
              
            }
            return model;
        }

        public ViewNote? GetNotes(int id)
        {
            var notes = _Repository.setnotes(id);

            var cancelNote = string.Empty;

            var requestStatusLog = _Repository.GetStatusLogsByRequest(id);
            List<string?> transfer = new();
            
            requestStatusLog.ForEach(x =>
            {
                Physician? phy = _Repository.GetPhysician(x.TransToPhysicianId);
                transfer.Add("Admin transferred to Dr : " + phy?.FirstName + " on " + x.CreatedDate.ToString("dd/MM/yyyy") + " at " + x.CreatedDate.ToString("HH: mm:ss: tt") + " " + x.Notes);
            });
            
            var log = requestStatusLog.FirstOrDefault(x => x.Status == 3);
            if(log != null)
            {
            cancelNote = log.Notes;
            }
            ViewNote data = new ViewNote()
            {
                RequestId = id,
                Admincancellationnote = cancelNote,
                AdminNotes = notes?.AdminNotes,
                TransferNotes = transfer,
                
            };
            return data;
        }

        //public ModalData GetAssignPhysician(int id,ModalData md)
        //{
        //    List<Region> r = _Repository.GetPhyByRegion(id,md);
        //    md.region = r;
        //    return md;
        //}

        public List<Physician> GetPhysician(int id)
        {
            return _Repository.GetPhysiciansByRegion(id).ToList();
        }

        public RequestClient? GetCancelCaseData(ModalData md, int id)
        {
            var canceldata = _Repository.Cancelname(id);
            

            var AdminCancelCaseModel = new RequestClient()
            {
                RequestId = id,
                FirstName = canceldata.FirstName,
                LastName= canceldata.LastName
            };
            return AdminCancelCaseModel;
        }

        public List<CaseTag> GetCaseReason()
        {
           return _Repository.GetCases();
        }

        public void CanclePost(ModalData md, int id, int admin)
        {
            Request req =_Repository.GetRequestById(id);

            req.Status = 3;
            req.CaseTag = md.reason;
            req.ModifiedDate=DateTime.Now;
            _Repository.UpdateRequesttbl(req);

            RequestStatusLog reqlog = new()
            {
                RequestId=id,
                Status = 3,
                AdminId=admin,
                Notes=md.note,
                CreatedDate=DateTime.Now,
                 
            };
            _Repository.AddRequestStatuslog(reqlog);
        }

        public void AssignCase(ModalData md, int id,int admin)
        {
            Request req = _Repository.GetRequestById(id);

            req.Status = 2;
            req.PhysicianId = md.SelectedPhysicianName;
            req.ModifiedDate = DateTime.Now;
            _Repository.UpdateRequesttbl(req);

            RequestStatusLog reqlog = new()
            {
                RequestId = id,
                Status = 2,
                TransToPhysicianId = md.SelectedPhysicianName,
                AdminId = admin,
                Notes = md.note,
                CreatedDate = DateTime.Now,
            };
            _Repository.AddRequestStatuslog(reqlog);

           

           

        }

        public RequestClient? GetBlockCaseData(ModalData md, int id)
        {
            var blockdata = _Repository.Blockname(id);


            var AdminCancelCaseModel = new RequestClient()
            {
                RequestId = id,
                FirstName = blockdata.FirstName,
                LastName = blockdata.LastName
            };
            return AdminCancelCaseModel;
        }

        public void BlockPost(ModalData md, int id, int admin)
        {

            Request req = _Repository.GetRequestById(id);

            req.Status = 10;
            req.ModifiedDate = DateTime.Now;
            _Repository.UpdateRequesttbl(req);

            RequestStatusLog reqlog = new()
            {
                RequestId = id,
                Status = 10,
                AdminId = admin,
                Notes = md.note,
                CreatedDate = DateTime.Now,

            };
            _Repository.AddRequestStatuslog(reqlog);
            RequestClient? rc = _Repository.GetRequestclient(id);
            BlockRequest blreq = new()
            {
                PhoneNumber = rc.PhoneNumber,
                Email=rc.Email,
                Reason=md.note,
                RequestId=id.ToString(),
                CreatedDate=DateTime.Now
                
            };
            _Repository.AddBlockRequest(blreq);
        }

        public List<ViewDocument> ViewUploadData(int id)
        {
            List<RequestWiseFile> rwf = _Repository.getfile(id);
            Request? req = _Repository.getdetail(id);
            
            List<ViewDocument> vd = new();
           
            if (rwf.Count!=0)
            {

          

            foreach (var item in rwf)
            {
                vd.Add(new ViewDocument
                {
                    FirstName = req.FirstName,
                    Confirmationnumber = req.ConfirmationNumber,
                    RequestId = req.RequestId,
                    FileName = item.FileName,
                    FileId = item.RequestWiseFileId,
                    Uploaddate=item.CreatedDate,
                 
                    
                });
            }
            }
            else
            {
                vd.Add(new ViewDocument
                {
                    FirstName = req.FirstName,
                    Confirmationnumber = req.ConfirmationNumber,
                    RequestId = req.RequestId,
               


                });
            }
            return vd;
        }

        public void FileUpload(int id, List<IFormFile> file,int admin)
        {
            foreach (IFormFile files in file)
            {
                string filename = files.FileName;
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\uplodedfiles\\", filename);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    files.CopyToAsync(stream).Wait();
                }


                RequestWiseFile requestWiseFile = new RequestWiseFile();
                requestWiseFile.FileName = filename;
                requestWiseFile.RequestId = id;
                requestWiseFile.DocType = 1;
                requestWiseFile.AdminId= admin;
                _Repository.AddRequestWiseFiles(requestWiseFile);

            }
        }

        public void DeleteFile(int id)
        {
            RequestWiseFile df = _Repository.GetDocumentFile(id)
;
            df.IsDeleted = new System.Collections.BitArray(1,true);
            _Repository.update_RequestWiseTable(df);
        }

        void IAdmin_Service.DeleteAllFiles(int id)
        {
            List<RequestWiseFile> reqdoc = _Repository.GetDocumentList(id);
            foreach (var item in reqdoc)
            {
                item.IsDeleted = new System.Collections.BitArray(1, true);
                _Repository.update_RequestWiseTable(item);

            }
        }

        public void SendEmail(int id)
        {
            List<RequestWiseFile> requestWiseFiles = _Repository.GetDocumentList(id);
            Request request = _Repository.GetRequestById(id);
            var receiver = "harsh.mehta8576@gmail.com";
            var subject = "Documents of Request " + request.ConfirmationNumber?.ToUpper();
            var message = "Find the Files uploaded for your request in below:";
            var mailMessage = new MailMessage(from: "tatva.dotnet.binalmalaviya@outlook.com", to: receiver, subject, message);

            foreach (var file in requestWiseFiles)
            {
                var filePath = "D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\uplodedfiles\\" + file.FileName;
                if (File.Exists(filePath))
                {
                    byte[] fileContent;
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        fileContent = new byte[fileStream.Length];
                        fileStream.Read(fileContent, 0, (int)fileStream.Length);
                    }
                    var attachment = new Attachment(new MemoryStream(fileContent), file.FileName);
                    mailMessage.Attachments.Add(attachment);
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }
            }

            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";

            var client = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            client.SendMailAsync(mailMessage);
        
        }

        public Order GetOrderData(Order md)
        {

            List<HealthProfessionalType> r = _Repository.GetHealthprofessionalByType();
          
            md.HealthProfessionalType = r;
          
            return md;
        }

        public List<HealthProfessional> Getvendor(int id)
        {

           return _Repository.getvendorbyprofessiontype(id);

        }

        public List<HealthProfessional> Getvendordata(int id)
        {
            return _Repository.getdatabyvendorid(id);
        }

        public void OrderPost(Order md, int id, int admin)
        {
           
            OrderDetail oredr = new()
            {
                VendorId=md.SelectedVendorId,
                RequestId=id,
                FaxNumber=md.Fax,
                Email=md.Email,
                BusinessContact=md.Email,
                Prescription=md.Detail,
                NoOfRefill=md.refill,
                CreatedDate=DateTime.Now,
                CreatedBy=admin.ToString(),

            };
            _Repository.AddOrderdetails(oredr);
        }

        public void Clear(int id, int admin)
        {
            Request req = _Repository.GetRequestById(id);

            req.Status = 11;

            req.ModifiedDate = DateTime.Now;
            _Repository.UpdateRequesttbl(req);

            RequestStatusLog reqlog = new()
            {
                RequestId = id,
                Status = 11,
                AdminId = admin,
   
                CreatedDate = DateTime.Now,

            };
            _Repository.AddRequestStatuslog(reqlog);
        }

        public RequestClient GetAgreementtdata(int id)
        {
            return _Repository.getagreement(id);
        }

        public void SendAgreementMail(int Id)
        {
            RequestClient rc = _Repository.getagreement(Id);

            if (rc.Email != null)
            {
                var receiver = rc.Email;
                var subject = "Send Agreement";
                var message = "Tap on link for Send Agreement";


                var mail = "tatva.dotnet.binalmalaviya@outlook.com";
                var password = "binal@2002";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));



            }
        }
    }
}
