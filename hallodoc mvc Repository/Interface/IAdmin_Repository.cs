using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.Interface
{
    public interface IAdmin_Repository
    {
        IQueryable<Request> GetAdminCode();
        IQueryable<Request> GetAdminConclude();
        IQueryable<Request> GetAdminToclose();
        IQueryable<Request> GetAdminUnpaid();
        IQueryable<Request> GetAdminPending();
        IQueryable<Request> GetAdminStatus();
        List<Request> GetRequest();
        AspNetUser Validate(LoginViewModel model);
        Admin getaspuser(string email);
        List<Region> GetRegion();
        RequestNote? setnotes(int id);
        List<RequestStatusLog?> GetStatusLogsByRequest(int id);
        void save();
        void AddRequestNotes(RequestNote newnotedata);
        //List<Region> GetPhyByRegion(int id, ModalData md);
        List<Physician> GetPhysiciansByRegion(int regionid);
        RequestClient? Cancelname(int id);
        List<CaseTag> GetCases();
        Request GetRequestById(int id);
        void UpdateRequesttbl(Request req);
        void AddRequestStatuslog(RequestStatusLog reqlog);
        Physician GetPhysician(int? transToPhysicianId);
        RequestClient? Blockname(int id);
        RequestClient? GetRequestclient(int id);
        void AddBlockRequest(BlockRequest blreq);
        List<RequestWiseFile> getfile(int id);
        Request? getdetail(int id);
        void AddRequestWiseFiles(RequestWiseFile requestWiseFile);
        RequestWiseFile GetDocumentFile(int id);
        void update_RequestWiseTable(RequestWiseFile df);
        List<RequestWiseFile> GetDocumentList(int id);
        List<HealthProfessionalType> GetHealthprofessionalByType();
        List<HealthProfessional> getvendorbyprofessiontype(int Profession);
        List<HealthProfessional> getdatabyvendorid(int id);
        OrderDetail GetOredrDetail(int id);
        void AddOrderdetails(OrderDetail oredr);

        List<string> GetAspNetRole(int? id);
        RequestClient getagreement(int id);
    }
}
