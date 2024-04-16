using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace hallocdoc_mvc_Service.Interface
{
    public interface IPhysician_Service
    {
        void AddProviderNote(int id, string note,int phy);

        bool GetEncounterStatus(int id);

        void ConcludeCare(int id, int physicianId);
        void AddToPending(int id);
        void ChangeCallType(int id, int phy);
        void ChangeStatus(int id, int phy);
        void Consult(int id,int phy);
        ModalData CountState(int admin1);
        void DeleteCustom(int[] filenames);
        int DeleteFile(int id);
        void editencounter(int id, Encounter model);
        void FileUpload(int id, List<IFormFile> file, int phy);
        void final(int id);
        RequestClient GetAgreementtdata(int id);
        ViewCase Getcase(int id);
        List<AdminDashboard> getDashDataActive(int? requestType, string? search, int? requestor, int? region, int pageid, int phy);
        List<AdminDashboard> getDashDataConclude(int? requestType, string? search, int? requestor, int? region, int pageid, int phy);
        List<AdminDashboard> getDashDataPending(int? requestType, string? search, int? requestor, int? region, int pageid, int phy);
        Encounter getencounter(int id);
        List<AdminDashboard> GetNewData(int? requestType, string? search, int? requestor, int? region, int pageid,int phy);
        ViewNote GetNotes(int id);
        Order GetOrderData(Order md);
        Physician getPhy(string email);
        List<HealthProfessional> Getvendor(int id);
        List<HealthProfessional> Getvendordata(int id);
        void OrderPost(Order md, int id, int phy);
        string Phyname(int admin1);
        void SendAgreementMail(int id, ModalData md, string token, int phy);
        void SendEmail(int id, int phy);
        ViewNote setViewNotesData(ViewNote model, int id, int phy);
        void Transfer(int id, ModalData md,int phy);
        List<ViewDocument> ViewUploadData(int id);
    }
}
