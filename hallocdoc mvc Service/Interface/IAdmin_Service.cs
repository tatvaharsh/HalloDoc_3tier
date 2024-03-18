using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallocdoc_mvc_Service.Interface
{
    public interface IAdmin_Service
    {
        Admin getAdmin(string email);
        ModalData GetAssignData(ModalData md);
        ViewCase Getcase(int reqId);
        List<AdminDashboard> getDashData(int? requestType, string? search, int? requestor, int? region, int pageid);
        List<AdminDashboard> getDashDataActive(int? requestType, string? search, int? requestor, int? region, int pageid);
        List<AdminDashboard> getDashDataConclude(int? requestType, string? search, int? requestor, int? region, int pageid);
        List<AdminDashboard> getDashDataPending(int? requestType, string? search, int? requestor, int? region, int pageid);
        List<AdminDashboard> getDashDataToclose(int? requestType, string? search, int? requestor, int? region, int pageid);
        List<AdminDashboard> getDashDataUnpaid(int? requestType, string? search, int? requestor, int? region, int pageid);
        ViewNote? GetNotes(int id);
        ViewNote? setViewNotesData(ViewNote model, int id,int admin);
        bool ValidateUser(LoginViewModel model);

        List<Physician> GetPhysician(int id);
        RequestClient? GetCancelCaseData(ModalData md, int id);
        List<CaseTag> GetCaseReason();
        void CanclePost(ModalData md, int id,int admin);
        void AssignCase(ModalData md, int id,int admin);
        RequestClient? GetBlockCaseData(ModalData md, int id);
        void BlockPost(ModalData md, int id, int admin);
        List<ViewDocument> ViewUploadData(int id);
        void FileUpload(int id, List<IFormFile> file,int admin);
        void DeleteFile(int id);
        void DeleteAllFiles(int id);
        void SendEmail(int id);
        Order GetOrderData(Order md);
        List<HealthProfessional> Getvendor(int id);
        List<HealthProfessional> Getvendordata(int id);
        void OrderPost(Order md, int id, int admin);
        void Clear(int id, int admin);
        RequestClient GetAgreementtdata(int id);
        void SendAgreementMail(int Id,ModalData md, string token);
        Close getclosedata(int id);
        void editdata(Close model, int id);
        void close(int id,int admin);
        void agreeagreement(int id);
     
        ModalData cancelmodal(int id);
        void cancelagreement(int id, ModalData md);
        Encounter getencounter(int id);
        void editencounter(int id, Encounter model);
        Profile getprofile(int admin);
        List<Request> GetRequestDataInList();
        void sendlink(ViewCase model);
        void PatientForm(patient_form model,int admin);
        List<Request> Export(string s, int reqtype, int regid, int state);
        void editadminprofile(Profile model,int admin, List<int> reg);
        void editadminp(Profile model, int admin);
        string Adminname(int admin1);
    }
}
