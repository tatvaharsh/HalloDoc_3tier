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
        void AddEmaillogtbl(EmailLog emailLog);
        void AddEncounterFormtbl(EncounterForm ef);
        void AddOrderdetails(OrderDetail oredr);
        void AddRequestNotes(RequestNote newnotedata);

        void UpdateRequestNotes(RequestNote newnotedata);

        bool GetEncounterStatus(int id);

        void AddRequestStatusLog(RequestStatusLog req);
        void AddRequesttbl(RequestStatusLog statusLog);
        void AddRequestWiseFiles(RequestWiseFile requestWiseFile);
        ModalData CountState(int admin1);
        List<AdminDashboard> GetActiveData(int phy);
        RequestClient getagreement(int id);
        Physician getaspuser(string email);
        List<AdminDashboard> GetConcludeData(int phy);
        List<HealthProfessional> getdatabyvendorid(int id);
        Request? getdetail(int id);
        RequestWiseFile GetDocumentFile(int id);
        List<RequestWiseFile> GetDocumentList(int id);
        EncounterForm getencounterbyid(int id);
        List<RequestWiseFile> getfile(int id);
        List<HealthProfessionalType> GetHealthprofessionalByType();
        string Getname(int admin1);
        List<AdminDashboard> GetNewData(int phy);
        List<AdminDashboard> GetPendingData(int phy);
        Physician? GetPhysician(int? transToPhysicianId);
        List<Request> GetRequest();
        Request GetRequestById(int id);
        List<RequestStatusLog> GetStatusLogsByRequest(int id);
        ViewDocument getUploaddata(int id);
        List<HealthProfessional> getvendorbyprofessiontype(int id);
        void save();
        RequestNote setnotes(int id);
        void updateEncounterForm(EncounterForm ef);
        void updateRequesttbl(Request a);
        void update_RequestWiseTable(RequestWiseFile df);
        int GetAspId(int phy);
        Physician getphycian(int admin1);
        AspNetUser GetAspNetUser(int v);
        List<Role> getrole();
        List<Region> GetReg();
        List<PhysicianRegion> GetSelectedPhyReg(int admin1);
        void UpdateAspNetUser(AspNetUser asp);
        Scheduling GetMonthData(DateTime date, int phy);
        List<Physician> GetRequestByRegion(int id);
        Physician DayDatabyPhysician(int? selectedPhysicianId);
        void AddShifttbl(Shift s);
        void AddShiftDetails(ShiftDetail detail);
        void AddShiftRegion(ShiftDetailRegion shiftRegion);
        string GetPhysicianName(int phy);
        ShiftDetail GetShiftDetails(int shiftdetailid);
        List<Physician> GetPhyN();
        void Update(ShiftDetail shiftDetail);
        void SmsLogtbl(Smslog smslog);
        AspNetUser getAsp(string email);
        Region isRegion(string abbreviation);
        void AddRegion(Region region);
        void AddRequestTbl(Request request);
        void AddRequestClient(RequestClient requestclient);
        Physician GetShiftData(int admin);
        List<Timesheet> TimeSheets(DateTime start, DateTime end, int phy);
        int ShiftHoursOnDate(int phy, DateTime sheetDate);
        Invoice GetInvoice(DateTime start, int phyid);
        void SaveTable(Invoice invoice);
        void UpdateTable(Invoice isInvoice);
        bool ShowBtn(DateTime date, int phyid);
        DataModels.Reimbursement IsDataAvailable(int invoiceId, DateTime date);
        void AddReimbursement(DataModels.Reimbursement re);
        void UpdateReimbursement(DataModels.Reimbursement reimbursement);
        List<DataModels.Reimbursement> GetReimbursements(int invoiceId);
        DataModels.Reimbursement IsDataAvailablebydate(DateTime date);
        void RemoveData(DataModels.Reimbursement reimbursement);
        void UpdateInvoice(Invoice isInvoice);
        List<DataModels.Reimbursement> GetReimbursementByInvoiceId(int invoiceId);
    }
}
