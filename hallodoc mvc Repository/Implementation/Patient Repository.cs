using hallodoc_mvc_Repository.DataContext;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.Interface;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.Implementation
{
    public class Patient_Repository : IPatient_Repository
    {
        private readonly ApplicationDbContext _context;

        public Patient_Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddAspnetuser(AspNetUser aspnetuser2)
        {
            _context.AspNetUsers.Add(aspnetuser2);
            _context.SaveChanges();
        }

        public void AddBusiness(Business business)
        {
            _context.Businesses.Add(business);
            _context.SaveChanges();
        }

        public void AddConcierge(Concierge con)
        {
            _context.Concierges.Add(con);
            _context.SaveChanges();
        }

        public Region AddRegion(Region region)
        {
            _context.Regions.Add(region);
            _context.SaveChanges();
            return region;
        }

        public void AddRequest(Request request)
        {
            _context.Requests.Add(request);
            _context.SaveChanges();
        }

        public void AddRequestBusiness(RequestBusiness reqBus)
        {
            _context.RequestBusinesses.Add(reqBus);
            _context.SaveChanges();
        }

        public void AddRequestClients(RequestClient requestclient)
        {
            _context.RequestClients.Add(requestclient);
            _context.SaveChanges();
        }

        public void AddRequestConcierge(RequestConcierge reqCon)
        {
            _context.RequestConcierges.Add(reqCon);
            _context.SaveChanges();
        }

        public void AddRequestWiseFiles(RequestWiseFile requestWiseFile)
        {
            _context.RequestWiseFiles.Add(requestWiseFile);
            _context.SaveChanges();
        }

        public ICollection<AspNetRole> AddRole()
        {
            return _context.AspNetRoles.Where(x => x.Id == 1).ToList();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public AspNetUser AspByUserId(int user1)
        {
            return _context.Users.Include(x => x.AspNetUser).FirstOrDefault(x => x.UserId == user1).AspNetUser;
        }

        public AspNetUser AspEmail(string email)
        {
            return _context.AspNetUsers.FirstOrDefault(u => u.Email == email);
        }

        public AspNetUser getAspuser(string? email)
        {
            return _context.AspNetUsers.FirstOrDefault(e => e.Email == email);
        }

        public List<Request> getcon(int id)
        {
            return _context.Requests.Where(u => u.RequestId == id).ToList();
        }

        public List<RequestWiseFile> getFiles()
        {
            return _context.RequestWiseFiles.ToList();
        }

        public RequestClient getRcbyemail(string userName)
        {
            return _context.RequestClients.FirstOrDefault(x => x.Email == userName);
        }

        public List<Request> getRequest(int? id)
        {
            return _context.Requests.Include(x => x.Physician).Where(u => u.UserId == id).ToList();
        }

        public Request GetRequestByEmail(string userName)
        {
            return _context.Requests.FirstOrDefault(r => r.Email == userName);
        }

        public List<RequestWiseFile> getRequestWiseFile(int id)
        {
            return _context.RequestWiseFiles.Where(u => u.RequestId == id).ToList();
        }

        public RequestWiseFile getRequestWiseFileById(int id)
        {
            return _context.RequestWiseFiles.Find(id);
        }

        public User getUser(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User getUser(int? user1)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == user1);
        }

        public Region isRegion(string abbreviation)
        {
            string region2 = abbreviation.ToLower();
            Region region = _context.Regions.FirstOrDefault(r => r.Abbreviation.ToLower() == region2);
            return region;
        }

        public ICollection<AspNetRole> RolePatient()
        {
            return _context.AspNetRoles.Where(x => x.Id == 1).ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void updateAspnetuserTable(AspNetUser asp)
        {
            _context.AspNetUsers.Update(asp);

        }

        public void UpdateRequest(Request req)
        {
            _context.Requests.Update(req);
            _context.SaveChanges();
        }

        public void UpdateUserTable(User req)
        {
            _context.Users.Update(req);
        }

        public AspNetUser ValidateUser(LoginViewModel model)
        {
            AspNetUser isReg = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == model.Passwordhash);
            if (isReg == null)
            {
                return new AspNetUser();
            }
            else
            {
                return isReg;
            }
        }

        AspNetUser IPatient_Repository.getAspuserTable(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            return _context.AspNetUsers.FirstOrDefault(u => u.Id == user.AspNetUserId);
        }
    }
}
