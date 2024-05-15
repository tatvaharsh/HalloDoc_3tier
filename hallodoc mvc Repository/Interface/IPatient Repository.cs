using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace hallodoc_mvc_Repository.Interface
{
    public interface IPatient_Repository
    {
        void AddAspnetuser(AspNetUser aspnetuser2);
        void AddBusiness(Business business);
        void AddConcierge(Concierge con);
        Region AddRegion(Region region);
        void AddRequest(Request request);
        void AddRequestBusiness(RequestBusiness reqBus);
        void AddRequestClients(RequestClient requestclient);
        void AddRequestConcierge(RequestConcierge reqCon);
        void AddRequestWiseFiles(RequestWiseFile requestWiseFile);
        ICollection<AspNetRole> AddRole();
        void AddUser(User user);
        List<Admin> admindata();
        AspNetUser AspByUserId(int user1);
        public AspNetUser AspEmail(string email);
        Admin getadmin(int adminID);
        List<Admin> GetAdminData();
        AspNetUser getAspuser(string? email);
        public AspNetUser getAspuserTable(int id);
        List<Request> getcon(int id);
        public List<RequestWiseFile> getFiles();
        Physician getphysician(int providerId);
        RequestClient getRcbyemail(string userName);
        public List<Request> getRequest(int? id);
        Request GetRequestByEmail(string userName);
        RequestClient getrequestclient(int requestId);
        public List<RequestWiseFile> getRequestWiseFile(int id);
       public RequestWiseFile getRequestWiseFileById(int id);
        public User getUser(string email);
       public User getUser(int? user1);
        Region isRegion(string abbreviation);
        ICollection<AspNetRole> RolePatient();
        public void Save();
       public void updateAspnetuserTable(AspNetUser asp);
        void UpdateRequest(Request req);
        public void UpdateUserTable(User req);
        AspNetUser ValidateUser(LoginViewModel model);
    }
}
