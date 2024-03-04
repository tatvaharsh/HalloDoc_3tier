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
    public class Admin_Repository : IAdmin_Repository
    {
        private readonly ApplicationDbContext _context;

        public Admin_Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Request> GetAdminCode()
        {
                return _context.Requests.Include(x => x.RequestClients).Where(x => x.RequestClients != null && x.Status == 1);
     
        }
        public IQueryable<Request> GetAdminStatus()
        {
          
                return _context.Requests.Include(x => x.RequestClients).Where(x => x.RequestClients != null &&  (x.Status == 4 || x.Status == 5));
        }
        public IQueryable<Request> GetAdminPending()
        {
          
                return _context.Requests.Include(x => x.RequestClients).Where(x => x.RequestClients != null && x.Status == 2);
         
        }
        public IQueryable<Request> GetAdminConclude()
        {
         
                return _context.Requests.Include(x => x.RequestClients).Where(x => x.RequestClients != null && x.Status == 6);
         
        }
        public IQueryable<Request> GetAdminToclose()
        {
            return _context.Requests.Include(x => x.RequestClients).Where(x => x.RequestClients != null && (x.Status == 3 || x.Status == 7 || x.Status == 8));
        }
        public IQueryable<Request> GetAdminUnpaid()
        {
   
                return _context.Requests.Include(x => x.RequestClients).Where(x => x.RequestClients != null && x.Status == 9);
         
        }

        public List<Request> GetRequest()
        {
            return _context.Requests.Include(x => x.RequestClients).Where(x => x.RequestClients != null).ToList();
        }

        public bool Validate(LoginViewModel model)
        {
            var aspUser = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == model.Passwordhash);

            if (aspUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Admin getaspuser(string email)

        {
           return _context.Admins.FirstOrDefault(u => u.Email == email);
        }

        public List<Region> GetRegion()
        {
            return _context.Regions.ToList();
        }

        public RequestNote? setnotes(int id)
        {
            return _context.RequestNotes.FirstOrDefault(i => i.RequestId == id);
        }

        public List<RequestStatusLog?> GetStatusLogsByRequest(int id)
        {
            return _context.RequestStatusLogs.Where(x=>x.RequestId==id).ToList();

        }

        public void save()
        {
            _context.SaveChanges();
        }

        public void AddRequestNotes(RequestNote newnotedata)
        {
            _context.RequestNotes.Add(newnotedata);
            _context.SaveChanges();
        }

        public List<Physician> GetPhysiciansByRegion(int regionid)
        {
            return _context.Physicians.Where(x=>x.RegionId == regionid).ToList();
        }

        public RequestClient? Cancelname(int id)
        {
           return _context.RequestClients.FirstOrDefault(i => i.RequestId == id);
        }

        public List<CaseTag> GetCases()
        {
             var cases =_context.CaseTags.ToList();
            return cases;
        }

        public Request GetRequestById(int id)
        {
            return _context.Requests.FirstOrDefault(i => i.RequestId == id);
        }

        public void UpdateRequesttbl(Request req)
        {
            _context.Requests.Update(req);
            _context.SaveChanges();

        }

        public void AddRequestStatuslog(RequestStatusLog reqlog)
        {

            _context.RequestStatusLogs.Add(reqlog);
            _context.SaveChanges();

        }

        public Physician GetPhysician(int? transToPhysicianId)
        {
            return _context.Physicians.FirstOrDefault(x => x.PhysicianId == transToPhysicianId);
        }

        public RequestClient? Blockname(int id)
        {
            return _context.RequestClients.FirstOrDefault(i => i.RequestId == id);
        }

        public RequestClient? GetRequestclient(int id)
        {
           return _context.RequestClients.FirstOrDefault(x=> x.RequestId == id);
        }

        public void AddBlockRequest(BlockRequest blreq)
        {
            _context.BlockRequests.Add(blreq);
            _context.SaveChanges();
        }

        public List<RequestWiseFile> getfile(int id)
        {
            return _context.RequestWiseFiles.Where(i => i.RequestId == id && i.IsDeleted==null).ToList();
        }

        public Request? getdetail(int id)
        {
            return _context.Requests.FirstOrDefault(i => i.RequestId == id);
        }

        public void AddRequestWiseFiles(RequestWiseFile requestWiseFile)
        {
            _context.RequestWiseFiles.Add(requestWiseFile);
            _context.SaveChanges();
        }

        public RequestWiseFile GetDocumentFile(int id)
        {
            var file = _context.RequestWiseFiles.Find(id)
;
            return file;
        }

        public void update_RequestWiseTable(RequestWiseFile df)
        {
            _context.RequestWiseFiles.Update(df);
            _context.SaveChanges();
        }

        public List<RequestWiseFile> GetDocumentList(int id)
        {
            List<RequestWiseFile> reqdoc = _context.RequestWiseFiles.Where(m => m.RequestId == id && m.IsDeleted == null).ToList();
            return reqdoc;
        }


    }
}
