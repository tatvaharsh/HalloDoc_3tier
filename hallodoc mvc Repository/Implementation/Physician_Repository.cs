using hallodoc_mvc_Repository.DataContext;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.Interface;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Charts;
using Syncfusion.EJ2.Layouts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.Implementation
{
    public class Physician_Repository : IPhysician_Repository
    {
        private readonly ApplicationDbContext _context;

        public Physician_Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Accept(int id)
        {
            var req = _context.Requests.FirstOrDefault(x => x.RequestId == id);
            req.Status = 2;
            _context.SaveChanges();
        }

        public void AddEmaillogtbl(EmailLog emailLog)
        {
            _context.EmailLogs.Add(emailLog);
            _context.SaveChanges();
        }

        public void AddEncounterFormtbl(EncounterForm ef)
        {
            _context.EncounterForms.Add(ef);
            _context.SaveChanges();
        }

        public void AddOrderdetails(OrderDetail oredr)
        {
            _context.OrderDetails.Add(oredr);
            _context.SaveChanges();
        }

        public void AddRequestNotes(RequestNote newnotedata)
        {
            _context.RequestNotes.Add(newnotedata);
            _context.SaveChanges();
        }

        public void UpdateRequestNotes(RequestNote newnotedata)
        {
            _context.RequestNotes.Update(newnotedata);
            _context.SaveChanges();
        }

        public bool GetEncounterStatus(int id)
        {
            EncounterForm ef = _context.EncounterForms.FirstOrDefault(x => x.RequestId == id);
            return ef != null && ef.IsFinalized[0] == true;
        }


        public void AddRequestStatusLog(RequestStatusLog req)
        {
            _context.RequestStatusLogs.Add(req);
            _context.SaveChanges();
        }

        public void AddRequesttbl(RequestStatusLog statusLog)
        {
            _context.RequestStatusLogs.Add(statusLog);
            _context.SaveChanges();
        }

        public void AddRequestWiseFiles(RequestWiseFile requestWiseFile)
        {
            _context.RequestWiseFiles.Add(requestWiseFile);
            _context.SaveChanges();
        }

        public ModalData CountState(int admin1)
        {
            return _context.Physicians
                .Where(b => b.PhysicianId == admin1 && b.IsDeleted == null)
                .Select(bh => new ModalData
                {
                    CountNew = bh.Requests.Where(x => x.Status == 1 && x.IsDeleted == null).Count(),
                    CountPending = bh.Requests.Where(x => x.Status == 2 && x.IsDeleted == null).Count(),
                    CountActive = bh.Requests.Where(x => (x.Status == 4 || x.Status == 5) && x.IsDeleted == null).Count(),
                    CountConclude = bh.Requests.Where(x => x.Status == 6 && x.IsDeleted == null).Count(),
                }).First();
        }

        public List<AdminDashboard> GetActiveData(int phy)
        {
            return _context.Requests
              .Where(x => x.PhysicianId == phy && (x.Status == 4 || x.Status == 5) && x.IsDeleted == null)
              .Select(x => new AdminDashboard
              {
                  Name = x.RequestClients.First().FirstName + " " + x.RequestClients.First().LastName,
                  Address = x.RequestClients.First().Address,
                  Phone = x.RequestClients.First().PhoneNumber ?? "--",
                  RPhone = x.PhoneNumber ?? "--",
                  RequestTypeId = x.RequestTypeId,
                  Status = x.Status,
                  Email = x.RequestClients.First().Email,
                  AcceptedDate = x.AcceptedDate,
                  Id = x.RequestId,
                  isfinal = x.EncounterForms.First().IsFinalized,
                  DateOfBirth = new(x.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(x.RequestClients.FirstOrDefault().StrMonth, "MMM").Month, x.RequestClients.FirstOrDefault().IntDate.Value),
                  calltype = x.CallType
              }).ToList();
        }

        public RequestClient getagreement(int id)
        {
            return _context.RequestClients.Include(x => x.Request).FirstOrDefault(x => x.RequestId == id);
        }

        public Physician getaspuser(string email)
        {
            return _context.Physicians.FirstOrDefault(u => u.Email == email);
        }

        public List<AdminDashboard> GetConcludeData(int phy)
        {
            return _context.Requests
               .Where(x => x.PhysicianId == phy && x.Status == 6 && x.IsDeleted == null)
               .Select(x => new AdminDashboard
               {
                   Name = x.RequestClients.First().FirstName + " " + x.RequestClients.First().LastName,
                   Address = x.RequestClients.First().Address,
                   Phone = x.RequestClients.First().PhoneNumber ?? "--",
                   RPhone = x.PhoneNumber ?? "--",
                   RequestTypeId = x.RequestTypeId,
                   Status = x.Status,
                   Email = x.RequestClients.First().Email,
                   AcceptedDate = x.AcceptedDate,
                   isfinal = x.EncounterForms.First().IsFinalized,
                   DateOfBirth = new(x.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(x.RequestClients.FirstOrDefault().StrMonth, "MMM").Month, x.RequestClients.FirstOrDefault().IntDate.Value),
                   calltype = x.CallType,
                   Id = x.RequestId,
               }).ToList();
        }

        public List<HealthProfessional> getdatabyvendorid(int id)
        {
            return _context.HealthProfessionals.Where(i => i.VendorId == id).ToList();
        }

        public Request? getdetail(int id)
        {
            return _context.Requests.FirstOrDefault(i => i.RequestId == id);
        }

        public RequestWiseFile GetDocumentFile(int id)
        {
            var file = _context.RequestWiseFiles.Find(id);
            return file;
        }

        public List<RequestWiseFile> GetDocumentList(int id)
        {
            List<RequestWiseFile> reqdoc = _context.RequestWiseFiles.Where(m => m.RequestId == id && m.IsDeleted == null).ToList();
            return reqdoc;
        }

        public EncounterForm getencounterbyid(int id)
        {
            return _context.EncounterForms.FirstOrDefault(x => x.RequestId == id);
        }

        public List<RequestWiseFile> getfile(int id)
        {
            return _context.RequestWiseFiles.Where(i => i.RequestId == id && i.IsDeleted == null).ToList();
        }

        public List<HealthProfessionalType> GetHealthprofessionalByType()
        {
            return _context.HealthProfessionalTypes.ToList();
        }

        public string Getname(int admin1)
        {
            var a = _context.Physicians.FirstOrDefault(x => x.PhysicianId == admin1);
            return a.FirstName + " " + a.LastName;

        }

        public List<AdminDashboard> GetNewData(int phy)
        {

            return _context.Requests
            .Where(x => x.PhysicianId == phy && x.Status == 1 && x.IsDeleted == null)
            .Select(x => new AdminDashboard
            {
                Name = x.RequestClients.First().FirstName + " " + x.RequestClients.First().LastName,
                Address = x.RequestClients.First().Address,
                Phone = x.RequestClients.First().PhoneNumber ?? "--",
                RPhone = x.PhoneNumber ?? "--",
                RequestTypeId = x.RequestTypeId,
                Status = x.Status,
                Email = x.RequestClients.First().Email,
                Id = x.RequestId,
                DateOfBirth = new(x.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(x.RequestClients.FirstOrDefault().StrMonth, "MMM").Month, x.RequestClients.FirstOrDefault().IntDate.Value),

            }).ToList();
        }

        public List<AdminDashboard> GetPendingData(int phy)
        {
            return _context.Requests
              .Where(x => x.PhysicianId == phy && x.Status == 2 && x.IsDeleted == null)
              .Select(x => new AdminDashboard
              {
                  Name = x.RequestClients.First().FirstName + " " + x.RequestClients.First().LastName,
                  Address = x.RequestClients.First().Address,
                  Phone = x.RequestClients.First().PhoneNumber ?? "--",
                  RPhone = x.PhoneNumber ?? "--",
                  RequestTypeId = x.RequestTypeId,
                  Status = x.Status,
                  Email = x.RequestClients.First().Email,
                  AcceptedDate = x.AcceptedDate,
                  Id = x.RequestId,
                  DateOfBirth = new(x.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(x.RequestClients.FirstOrDefault().StrMonth, "MMM").Month, x.RequestClients.FirstOrDefault().IntDate.Value),
                  isfinal = x.EncounterForms.First().IsFinalized,
                  calltype = x.CallType
              }).ToList();
        }

        public Physician? GetPhysician(int? transToPhysicianId)
        {
            return _context.Physicians.FirstOrDefault(x => x.PhysicianId == transToPhysicianId);
        }

        public List<Request> GetRequest()
        {
            return _context.Requests.Include(x => x.RequestClients).Where(x => x.RequestClients != null).ToList();
        }

        public Request GetRequestById(int id)
        {
            return _context.Requests.FirstOrDefault(x => x.RequestId == id);
        }

        public List<RequestStatusLog> GetStatusLogsByRequest(int id)
        {
            return _context.RequestStatusLogs.Where(x => x.RequestId == id).ToList() ?? new List<RequestStatusLog>();
        }

        public ViewDocument getUploaddata(int id)
        {
            return _context.Requests
                .Where(x => x.RequestId == id && x.IsDeleted == null)
                .Select(x => new ViewDocument
                {
                    FirstName = x.RequestClients.First().FirstName,
                    Confirmationnumber = x.ConfirmationNumber,
                    RequestId = x.RequestId,
                    AllFiles = x.RequestWiseFiles.Where(x => x.IsDeleted == null)
                        .Select(x => new UploadedFiles
                        {
                            FileName = x.FileName,
                            FileId = x.RequestWiseFileId,
                            Uploaddate = x.CreatedDate,
                        }).ToList(),
                }).First();
        }

        public List<HealthProfessional> getvendorbyprofessiontype(int id)
        {
            return _context.HealthProfessionals.Where(i => i.Profession == id).ToList();
        }

        public void save()
        {
            _context.SaveChanges();
        }

        public RequestNote setnotes(int id)
        {
            return _context.RequestNotes.FirstOrDefault(i => i.RequestId == id);
        }

        public void updateEncounterForm(EncounterForm ef)
        {

            _context.EncounterForms.Update(ef);
            _context.SaveChanges();
        }

        public void updateRequesttbl(Request a)
        {
            _context.Requests.Update(a);
            _context.SaveChanges();
        }

        public void update_RequestWiseTable(RequestWiseFile df)
        {
            _context.RequestWiseFiles.Update(df);
            _context.SaveChanges();
        }

        public int GetAspId(int phy)
        {
            return _context.Physicians.FirstOrDefault(x => x.PhysicianId == phy).AspNetUserId ?? new();
        }

        public Physician getphycian(int admin1)
        {

            return _context.Physicians.FirstOrDefault(x => x.PhysicianId == admin1);


        }

        public AspNetUser? GetAspNetUser(int v)
        {

            return _context.AspNetUsers.FirstOrDefault(x => x.Id == v);

        }

        public List<Role> getrole()
        {
            return _context.Roles.ToList();
        }
        public List<Region> GetReg()
        {
            return _context.Regions.ToList();
        }
        public List<PhysicianRegion> GetSelectedPhyReg(int id)
        {
            return _context.PhysicianRegions.Where(x => x.PhysicianId == id).ToList();
        }

        public void UpdateAspNetUser(AspNetUser asp)
        {
            _context.AspNetUsers.Update(asp);
            _context.SaveChanges();
        }

        public Scheduling GetMonthData(DateTime date, int phy)
        {

            return _context.Physicians.Where(x => x.PhysicianId == phy)
                .Select(x => new Scheduling()
                {
                    Physicians = x,
                    CurrentDate = date,
                    shifts = x.Shifts.SelectMany(x => x.ShiftDetails
                            .Where(x => x.ShiftDate.Month == date.Month && x.IsDeleted == new System.Collections.BitArray(1, false)))
                            .ToList(),
                }).First();
        }


        public List<Physician> GetRequestByRegion(int id)
        {
            return _context.Physicians.Where(x => x.RegionId == id).ToList();
        }

        public Physician DayDatabyPhysician(int? selectedPhysicianId)
        {
            return _context.Physicians.Include(s => s.Shifts).ThenInclude(s => s.ShiftDetails).FirstOrDefault(s => s.PhysicianId == selectedPhysicianId) ?? new();
        }

        public void AddShifttbl(Shift s)
        {
            _context.Shifts.Add(s);
            _context.SaveChanges();
        }

        public void AddShiftDetails(ShiftDetail detail)
        {
            _context.ShiftDetails.Add(detail);
            _context.SaveChanges();
        }

        public void AddShiftRegion(ShiftDetailRegion shiftRegion)
        {
            _context.ShiftDetailRegions.Add(shiftRegion);
            _context.SaveChanges();
        }

        public string GetPhysicianName(int phy)
        {
            return _context.Physicians.Include(y => y.Region).FirstOrDefault(x => x.PhysicianId == phy)!.Region!.Name;
        }

        public ShiftDetail GetShiftDetails(int shiftdetailid)
        {
            return _context.ShiftDetails.Include(x => x.Shift).FirstOrDefault(x => x.ShiftDetailId == shiftdetailid);
        }

        public List<Physician> GetPhyN()
        {
            return _context.Physicians.Where(x => x.IsDeleted == null).ToList();
        }

        public void Update(ShiftDetail shiftDetail)
        {
            _context.ShiftDetails.Update(shiftDetail);
            _context.SaveChanges();
        }

        public void SmsLogtbl(Smslog smslog)
        {
            _context.Smslogs.Add(smslog);
            _context.SaveChanges();

        }

        public AspNetUser getAsp(string email)
        {
            return _context.AspNetUsers.FirstOrDefault(u => u.Email == email);
        }

        public Region isRegion(string abbreviation)
        {
            string region2 = abbreviation.ToLower();
            Region region = _context.Regions.FirstOrDefault(r => r.Abbreviation.ToLower() == region2);
            return region;
        }

        public void AddRegion(Region region)
        {
            _context.Regions.Add(region);
            _context.SaveChanges();

        }

        public void AddRequestTbl(Request request)
        {
            _context.Requests.Add(request);
            _context.SaveChanges();
        }

        public void AddRequestClient(RequestClient requestclient)
        {
            _context.RequestClients.Add(requestclient); _context.SaveChanges();
        }

        public Physician GetShiftData(int admin)
        {
            return _context.Physicians.Include(s => s.Shifts).ThenInclude(s => s.ShiftDetails).FirstOrDefault(s => s.PhysicianId == admin) ?? new();
        }

        public List<Timesheet> TimeSheets(DateTime start, DateTime end, int phyid)
        {
            return _context.Timesheets.Where(x => x.PhysicianId == phyid && (x.SheetDate.Date >= start.Date && x.SheetDate.Date <= end.Date)).ToList();
        }

        public int ShiftHoursOnDate(int phyid, DateTime sheetDate)
        {
            return _context.ShiftDetails
                .Where(x => x.Shift.PhysicianId == phyid && x.ShiftDate.Date == sheetDate.Date)
                .Select(x => new
                {
                    duraion = x.EndTime - x.StartTime
                }).Sum(x => (int)x.duraion.TotalHours);
        }

        public Invoice GetInvoice(DateTime start, int phyid)
        {
            return _context.Invoices.Include(x=>x.Timesheets).FirstOrDefault(x => x.StartDate.Date == start.Date && x.PhysicianId == phyid) ?? new();
        }

        public void SaveTable(Invoice invoice)
        {
           _context.Invoices.Add(invoice);
            _context.SaveChanges();
        }

        public void UpdateTable(Invoice isInvoice)
        {
            _context.Invoices.Update(isInvoice);
            _context.SaveChanges();
        }

        public bool ShowBtn(DateTime date, int phyid)
        {
            return _context.Invoices.Any(x=>x.PhysicianId==phyid && x.IsFinalized==true && x.StartDate.Date==date.Date);
        }

        public DataModels.Reimbursement IsDataAvailable(int invoiceId, DateTime date)
        {
            return _context.Reimbursements.FirstOrDefault(x => x.InvoiceId == invoiceId && x.ReimbursementDate == date) ?? new();
        }

        public void AddReimbursement(DataModels.Reimbursement re)
        {
            _context.Reimbursements.Add(re);
            _context.SaveChanges();     
        }

        public void UpdateReimbursement(DataModels.Reimbursement reimbursement)
        {
            _context.Reimbursements.Update(reimbursement);
            _context.SaveChanges();
        }

        public List<DataModels.Reimbursement> GetReimbursements(int invoiceId)
        {
            return _context.Reimbursements.Where(x => x.InvoiceId == invoiceId).ToList();
        }
    }
}
