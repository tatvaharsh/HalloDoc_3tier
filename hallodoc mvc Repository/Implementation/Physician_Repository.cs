using hallodoc_mvc_Repository.DataContext;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.Interface;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.Implementation
{
    public class Physician_Repository : IPhysician_Repository
    {
        private readonly ApplicationDbContext _context;

        public Physician_Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Accept(int id)
        {
            var req = _context.Requests.FirstOrDefault(x=>x.RequestId == id);
            req.AcceptedDate = DateTime.Now;
            _context.SaveChanges();
        }

        public void AddEmaillogtbl(EmailLog emailLog)
        {
            _context.EmailLogs.Add(emailLog);
            _context.SaveChanges();
        }

        public void AddEncounterFormtbl(EncounterForm ef)
        {
            _context.EncounterForms.Add(ef);
            _context.SaveChanges(); 
        }

        public void AddOrderdetails(OrderDetail oredr)
        {
           _context.OrderDetails.Add(oredr);
            _context.SaveChanges(); 
        }

        public void AddRequestNotes(RequestNote newnotedata)
        {
           _context.RequestNotes.Add(newnotedata);
            _context.SaveChanges(); 
        }

        public void UpdateRequestNotes(RequestNote newnotedata)
        {
           _context.RequestNotes.Update(newnotedata);
            _context.SaveChanges(); 
        }

        public bool GetEncounterStatus(int id)
        {
            EncounterForm ef = _context.EncounterForms.FirstOrDefault(x=>x.RequestId == id);
            return ef!=null && ef.IsFinalized[0] == true;
        }


        public void AddRequestStatusLog(RequestStatusLog req)
        {
            _context.RequestStatusLogs.Add(req);
            _context.SaveChanges(); 
        }

        public void AddRequesttbl(RequestStatusLog statusLog)
        {
            _context.RequestStatusLogs.Add(statusLog);
            _context.SaveChanges();
        }

        public void AddRequestWiseFiles(RequestWiseFile requestWiseFile)
        {
            _context.RequestWiseFiles.Add(requestWiseFile);
            _context.SaveChanges(); 
        }

        public ModalData CountState(int admin1)
        {
            return _context.Physicians
                .Where(b=>b.PhysicianId == admin1 && b.IsDeleted == null)
                .Select(bh=>new ModalData
                {
                    CountNew=bh.Requests.Where(x=>x.Status == 2 && x.AcceptedDate == null && x.IsDeleted == null).Count(),
                    CountPending=bh.Requests.Where(x=> x.Status == 2 && x.AcceptedDate != null && x.IsDeleted == null).Count(),
                    CountActive=bh.Requests.Where(x=> (x.Status == 4 || x.Status == 5) && x.IsDeleted == null).Count(),
                    CountConclude=bh.Requests.Where(x=> x.Status == 6 && x.IsDeleted == null).Count(),
                }).First();
        }

        public List<AdminDashboard> GetActiveData(int phy)
        {
            return _context.Requests
              .Where(x => x.PhysicianId == phy && (x.Status == 4||x.Status==5) && x.IsDeleted == null)
              .Select(x => new AdminDashboard
              {
                  Name = x.RequestClients.First().FirstName + " " + x.RequestClients.First().LastName,
                  Address = x.RequestClients.First().Address,
                  Phone = x.RequestClients.First().PhoneNumber ?? "--",
                  RPhone = x.PhoneNumber ?? "--",
                  RequestTypeId = x.RequestTypeId,
                  Status = x.Status,
                  Email = x.RequestClients.First().Email,
                  AcceptedDate = x.AcceptedDate,
                  Id = x.RequestId,
                  isfinal = x.EncounterForms.First().IsFinalized,
                  calltype = x.CallType
              }).ToList();
        }

        public RequestClient getagreement(int id)
        {
            return _context.RequestClients.Include(x => x.Request).FirstOrDefault(x => x.RequestId == id);
        }

        public Physician getaspuser(string email)
        {
            return _context.Physicians.FirstOrDefault(u => u.Email == email);
        }

        public List<AdminDashboard> GetConcludeData(int phy)
        {
            return _context.Requests
               .Where(x => x.PhysicianId == phy && x.Status == 6  && x.IsDeleted == null)
               .Select(x => new AdminDashboard
               {
                   Name = x.RequestClients.First().FirstName + " " + x.RequestClients.First().LastName,
                   Address = x.RequestClients.First().Address,
                   Phone = x.RequestClients.First().PhoneNumber ?? "--",
                   RPhone = x.PhoneNumber ?? "--",
                   RequestTypeId = x.RequestTypeId,
                   Status = x.Status,
                   Email = x.RequestClients.First().Email,
                   AcceptedDate = x.AcceptedDate,
                   isfinal = x.EncounterForms.First().IsFinalized,
                   calltype = x.CallType,
                   Id = x.RequestId,
               }).ToList();
        }

        public List<HealthProfessional> getdatabyvendorid(int id)
        {
            return _context.HealthProfessionals.Where(i => i.VendorId == id).ToList();
        }

        public Request? getdetail(int id)
        {
            return _context.Requests.FirstOrDefault(i => i.RequestId == id);
        }

        public RequestWiseFile GetDocumentFile(int id)
        {
            var file = _context.RequestWiseFiles.Find(id);
            return file;
        }

        public List<RequestWiseFile> GetDocumentList(int id)
        {
            List<RequestWiseFile> reqdoc = _context.RequestWiseFiles.Where(m => m.RequestId == id && m.IsDeleted == null).ToList();
            return reqdoc;
        }

        public EncounterForm getencounterbyid(int id)
        {
            return _context.EncounterForms.FirstOrDefault(x => x.RequestId == id);
        }

        public List<RequestWiseFile> getfile(int id)
        {
            return _context.RequestWiseFiles.Where(i => i.RequestId == id && i.IsDeleted == null).ToList();
        }

        public List<HealthProfessionalType> GetHealthprofessionalByType()
        {
            return _context.HealthProfessionalTypes.ToList();
        }

        public string Getname(int admin1)
        {
            var a =  _context.Physicians.FirstOrDefault(x => x.PhysicianId == admin1);
            return a.FirstName+" "+a.LastName;

        }

        public List<AdminDashboard> GetNewData(int phy)
        {
            return _context.Requests
                .Where(x=>x.PhysicianId==phy && x.Status==2 && x.AcceptedDate == null && x.IsDeleted==null)
                .Select(x=>new AdminDashboard
                {
                    Name = x.RequestClients.First().FirstName+" "+ x.RequestClients.First().LastName,
                    Address = x.RequestClients.First().Address,
                    Phone = x.RequestClients.First().PhoneNumber??"--",
                    RPhone = x.PhoneNumber??"--",
                    RequestTypeId = x.RequestTypeId,
                    Status = x.Status,
                    Email = x.RequestClients.First().Email,
                    Id = x.RequestId

                }).ToList();
        }

        public List<AdminDashboard> GetPendingData(int phy)
        {
            return _context.Requests
              .Where(x => x.PhysicianId == phy && x.Status == 2 && x.AcceptedDate != null && x.IsDeleted == null)
              .Select(x => new AdminDashboard
              {
                  Name = x.RequestClients.First().FirstName + " " + x.RequestClients.First().LastName,
                  Address = x.RequestClients.First().Address,
                  Phone = x.RequestClients.First().PhoneNumber ?? "--",
                  RPhone = x.PhoneNumber ?? "--",
                  RequestTypeId = x.RequestTypeId,
                  Status = x.Status,
                  Email = x.RequestClients.First().Email,
                  AcceptedDate = x.AcceptedDate,
                  Id = x.RequestId,
                  isfinal = x.EncounterForms.First().IsFinalized,
                  calltype = x.CallType
              }).ToList();
        }

        public Physician? GetPhysician(int? transToPhysicianId)
        {
            return _context.Physicians.FirstOrDefault(x => x.PhysicianId == transToPhysicianId);
        }

        public List<Request> GetRequest()
        {
            return _context.Requests.Include(x => x.RequestClients).Where(x => x.RequestClients != null).ToList();
        }

        public Request GetRequestById(int id)
        {
            return _context.Requests.FirstOrDefault(x => x.RequestId == id);
        }

        public List<RequestStatusLog> GetStatusLogsByRequest(int id)
        {
            return _context.RequestStatusLogs.Where(x => x.RequestId == id).ToList() ?? new List<RequestStatusLog>();
        }

        public ViewDocument getUploaddata(int id)
        {
            return _context.Requests
                .Where(x => x.RequestId == id && x.IsDeleted == null)
                .Select(x => new ViewDocument
                {
                    FirstName=x.RequestClients.First().FirstName,
                    Confirmationnumber=x.ConfirmationNumber,
                    RequestId=x.RequestId,
                    AllFiles=x.RequestWiseFiles.Where(x=>x.IsDeleted==null)
                        .Select(x=>new UploadedFiles
                        {
                            FileName=x.FileName,
                            FileId=x.RequestWiseFileId,
                            Uploaddate=x.CreatedDate,
                        }).ToList(),
                }).First();
        }

        public List<HealthProfessional> getvendorbyprofessiontype(int id)
        {
            return _context.HealthProfessionals.Where(i => i.Profession == id).ToList();
        }

        public void save()
        {
           _context.SaveChanges();
        }

        public RequestNote setnotes(int id)
        {
            return _context.RequestNotes.FirstOrDefault(i => i.RequestId == id);
        }

        public void updateEncounterForm(EncounterForm ef)
        {

            _context.EncounterForms.Update(ef);
            _context.SaveChanges();
        }

        public void updateRequesttbl(Request a)
        {
            _context.Requests.Update(a);
            _context.SaveChanges();
        }

        public void update_RequestWiseTable(RequestWiseFile df)
        {
            _context.RequestWiseFiles.Update(df);
            _context.SaveChanges();
        }

        public int GetAspId(int phy)
        {
            return _context.Physicians.FirstOrDefault(x=>x.PhysicianId== phy).AspNetUserId??new();
        }
    }
}
