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
                return _context.Requests.Include(x=>x.Physician).Include(x => x.RequestClients).Where(x => x.RequestClients != null && x.Status == 1);
        }
        public IQueryable<Request> GetAdminStatus()
        {
          
                return _context.Requests.Include(x=>x.Physician).Include(x => x.RequestClients).Where(x => x.RequestClients != null &&  (x.Status == 4 || x.Status == 5));
        }
        public IQueryable<Request> GetAdminPending()
        {
          
                return _context.Requests.Include(x => x.Physician).Include(x => x.RequestClients).Where(x => x.RequestClients != null && x.Status == 2);
         
        }
        public IQueryable<Request> GetAdminConclude()
        {
         
                return _context.Requests.Include(x => x.Physician).Include(x => x.RequestClients).Where(x => x.RequestClients != null && x.Status == 6);
         
        }
        public IQueryable<Request> GetAdminToclose()
        {
            return _context.Requests.Include(x => x.Physician).Include(x => x.RequestClients).Where(x => x.RequestClients != null && (x.Status == 3 || x.Status == 7 || x.Status == 8));
        }
        public IQueryable<Request> GetAdminUnpaid()
        {
   
                return _context.Requests.Include(x=>x.Physician).Include(x => x.RequestClients).Where(x => x.RequestClients != null && x.Status == 9);
         
        }

        public List<Request> GetRequest()
        {
            return _context.Requests.Include(x => x.RequestClients).Where(x => x.RequestClients != null).ToList();
        }

        public AspNetUser Validate(LoginViewModel model)
        {
            var aspUser = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == model.Passwordhash);

            if (aspUser == null)
            {
                return new AspNetUser();
            }
            else
            {
                return aspUser;
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
            return _context.RequestStatusLogs.Where(x=>x.RequestId==id).ToList() ?? new List<RequestStatusLog>();

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

        public List<HealthProfessionalType> GetHealthprofessionalByType()
        {
            return _context.HealthProfessionalTypes.ToList();
        }

        public List<HealthProfessional> getvendorbyprofessiontype(int Profession)
        {
           return _context.HealthProfessionals.Where(i=>i.Profession==Profession).ToList();
        }

        public List<HealthProfessional> getdatabyvendorid(int id)
        {
            return _context.HealthProfessionals.Where(i => i.VendorId == id).ToList();
        }

        public OrderDetail GetOredrDetail(int id)
        {
           return _context.OrderDetails.FirstOrDefault(i => i.VendorId == id);
        }

        public void AddOrderdetails(OrderDetail oredr)
        {
           _context.OrderDetails.Add(oredr);
            _context.SaveChanges();
        }

        public List<string> GetAspNetRole(int? id)
        {
            return _context.AspNetUsers.Include(x => x.Roles).FirstOrDefault(x => x.Id == id).Roles.Select(x=>x.Name).ToList() ;
        }

        public RequestClient getagreement(int id)
        {
           return _context.RequestClients.FirstOrDefault(x=>x.RequestId== id);
        }

        public void updaterequestclient(RequestClient r)
        {
            _context.RequestClients.Update(r);
            _context.SaveChanges();
        }

        public List<Request> GetCountData()
        {
            return _context.Requests.Include(x => x.RequestClients).ToList();
        }

        public void AddRequestclosed(RequestClosed requestClosed)
        {
            _context.RequestCloseds.Add(requestClosed);
            _context.SaveChanges();
        }

        public Request getreqid(int id)
        {
            return _context.Requests.FirstOrDefault(x => x.RequestId == id);
        }

        public RequestClient getclient(int id)
        {
           return _context.RequestClients.FirstOrDefault(x=>x.RequestId==id);
        }

        public EncounterForm getencounterbyid(int id)
        {
            return _context.EncounterForms.FirstOrDefault(x => x.RequestId == id);
        }

        public void updateEncounterForm(EncounterForm ef)
        {
            _context.EncounterForms.Update(ef);
            _context.SaveChanges();
        }

        public Admin getadminbyadminid(int admin)
        {
          return _context.Admins.FirstOrDefault(x => x.AdminId == admin);
        }

        public List<AdminRegion> getadminreg(int admin)
        {
            return _context.AdminRegions.Where(x=>x.AdminId==admin).ToList();
        }

        public string GetRegionname(int? regionId)
        {
            return _context.Regions.FirstOrDefault(x => x.RegionId == regionId).Name;
        }

        public AspNetUser getuserbyaspid(int aspNetUserId)
        {
            return _context.AspNetUsers.FirstOrDefault(x => x.Id == aspNetUserId);
        }

        public List<Request> getINlist()
        {
            return _context.Requests.Include(r => r.RequestClients).Include(x=>x.Physician).ToList();
        }

        public AspNetUser getAsp(string email)
        {
            return _context.AspNetUsers.FirstOrDefault(u => u.Email == email);
        }

        public void AddAspnetUser(AspNetUser aspnetuser2)
        {
            _context.AspNetUsers.Add(aspnetuser2);
            _context.SaveChanges();
        }

        public Region isRegion(string abbreviation)
        {
            string region2 = abbreviation.ToLower();
            Region region = _context.Regions.FirstOrDefault(r => r.Abbreviation.ToLower() == region2);
            return region;
        }

        public Region AddRegion(Region region)
        { 
            _context.Regions.Add(region);
            _context.SaveChanges();
            return region;
        }

        public void AddRequesttbl(Request request)
        {
            _context.Requests.Add(request);
            _context.SaveChanges();
        }

        public void AddRequestClient(RequestClient requestclient)
        {
            _context.RequestClients.Add(requestclient);
            _context.SaveChanges();
        }

        public void updateadmintbl(Admin a)
        {
            _context.Admins.Update(a);
            _context.SaveChanges();
        }

        public int? GetRegionid(string state)
        {
           return _context.Regions.FirstOrDefault(x=>x.Name.ToLower()== state.ToLower()).RegionId;
        }

        public User getUser(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
