using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallocdoc_mvc_Service.Interface
{
    public interface IPhysician_Service
    {
        void AddToPending(int id);
        ModalData CountState(int admin1);
        RequestClient GetAgreementtdata(int id);
        ViewCase Getcase(int id);
        List<AdminDashboard> getDashDataActive(int? requestType, string? search, int? requestor, int? region, int pageid, int phy);
        List<AdminDashboard> getDashDataConclude(int? requestType, string? search, int? requestor, int? region, int pageid, int phy);
        List<AdminDashboard> getDashDataPending(int? requestType, string? search, int? requestor, int? region, int pageid, int phy);
        List<AdminDashboard> GetNewData(int? requestType, string? search, int? requestor, int? region, int pageid,int phy);
        ViewNote GetNotes(int id);
        Physician getPhy(string email);
        string Phyname(int admin1);
        void SendAgreementMail(int id, ModalData md, string token, int phy);
        ViewNote setViewNotesData(ViewNote model, int id, int phy);
        void Transfer(int id, ModalData md,int phy);
        ViewDocument ViewUploadData(int id);
    }
}
