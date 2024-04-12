using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.Interface
{
    public interface IPhysician_Repository
    {
        void Accept(int id);
        ModalData CountState(int admin1);
        List<AdminDashboard> GetActiveData(int phy);
        Physician getaspuser(string email);
        List<AdminDashboard> GetConcludeData(int phy);
        string Getname(int admin1);
        List<AdminDashboard> GetNewData(int phy);
        List<AdminDashboard> GetPendingData(int phy);
        Physician? GetPhysician(int? transToPhysicianId);
        List<Request> GetRequest();
        List<RequestStatusLog> GetStatusLogsByRequest(int id);
        RequestNote setnotes(int id);
    }
}
