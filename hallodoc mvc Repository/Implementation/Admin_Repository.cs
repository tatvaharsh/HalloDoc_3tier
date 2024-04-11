﻿using hallodoc_mvc_Repository.DataContext;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.Interface;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Charts;
using Syncfusion.EJ2.CircularGauge;
using Syncfusion.EJ2.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
            return _context.Requests.Include(x => x.Physician).Include(x => x.RequestClients).Where(x => x.RequestClients != null && x.Status == 1);
        }
        public IQueryable<Request> GetAdminStatus()
        {

            return _context.Requests.Include(x => x.Physician).Include(x => x.RequestClients).Where(x => x.RequestClients != null && (x.Status == 4 || x.Status == 5));
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

            return _context.Requests.Include(x => x.Physician).Include(x => x.RequestClients).Where(x => x.RequestClients != null && x.Status == 9);

        }

        /// <summary>
        /// Get request by id
        /// </summary>
        /// <returns></returns>
        public List<Request> GetRequest()
        {
            return _context.Requests.Include(x => x.RequestClients).Where(x => x.RequestClients != null).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"> </param>
        /// <returns></returns>
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
            return _context.RequestStatusLogs.Where(x => x.RequestId == id).ToList() ?? new List<RequestStatusLog>();

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
            return _context.Physicians.Where(x => x.RegionId == regionid).ToList();
        }

        public RequestClient? Cancelname(int id)
        {
            return _context.RequestClients.FirstOrDefault(i => i.RequestId == id);
        }

        public List<CaseTag> GetCases()
        {
            var cases = _context.CaseTags.ToList();
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
            return _context.RequestClients.FirstOrDefault(x => x.RequestId == id);
        }

        public void AddBlockRequest(BlockRequest blreq)
        {
            _context.BlockRequests.Add(blreq);
            _context.SaveChanges();
        }

        public List<RequestWiseFile> getfile(int id)
        {
            return _context.RequestWiseFiles.Where(i => i.RequestId == id && i.IsDeleted == null).ToList();
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
            var file = _context.RequestWiseFiles.Find(id);
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
            return _context.HealthProfessionals.Where(i => i.Profession == Profession).ToList();
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
            return _context.AspNetUsers.Include(x => x.Roles).FirstOrDefault(x => x.Id == id).Roles.Select(x => x.Name).ToList();
        }

        public RequestClient getagreement(int id)
        {
            return _context.RequestClients.Include(x => x.Request).FirstOrDefault(x => x.RequestId == id);
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
            return _context.RequestClients.FirstOrDefault(x => x.RequestId == id);
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
            return _context.AdminRegions.Where(x => x.AdminId == admin).ToList();
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
            return _context.Requests.Include(r => r.RequestClients).Include(x => x.Physician).ToList();
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
            return _context.Regions.FirstOrDefault(x => x.Name.ToLower() == state.ToLower()).RegionId;
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

        public bool IsAdminRegion(int ritem, int a)
        {
            if (_context.AdminRegions.FirstOrDefault(x => x.AdminId == a && x.RegionId == ritem) != null)
            {
                return true;
            }
            return false;
        }

        public void deletereg(int admin)
        {
            var employeeToDelete = _context.AdminRegions.Where(e => e.AdminId == admin).ToList();
            foreach (var a in employeeToDelete)
            {
                _context.AdminRegions.Remove(a);
                _context.SaveChanges();
            }
        }

        public void AddRegionbyid(int ritem, int admin)
        {
            _context.AdminRegions.Add(new AdminRegion { RegionId = ritem, AdminId = admin });
            _context.SaveChanges();
        }

        public string Adminname(int admin1)
        {
            return _context.Admins.FirstOrDefault(x => x.AdminId == admin1).FirstName;
        }

        public void UpdateAsp(AspNetUser asp)
        {
            _context.AspNetUsers.Update(asp);
            _context.SaveChanges();
        }

        public List<Physician> getphysician()
        {
            return _context.Physicians.AsEnumerable().Where(x => x.IsDeleted == null).ToList();
        }

        public List<PhysicianLocation> GetPhyLocation()
        {
            return _context.PhysicianLocations.ToList();
        }

        public string Getrolebyroleid(int? roleId)
        {
            return _context.Roles.FirstOrDefault(x => x.RoleId == roleId).Name;
        }

        public PhysicianNotification phynoti(int physicianId)
        {
            return _context.PhysicianNotifications.FirstOrDefault(x => x.PhysicianId == physicianId) ?? new();
        }

        public void updatephynoti(PhysicianNotification pl)
        {
            _context.PhysicianNotifications.Update(pl);
            _context.SaveChanges();
        }

        public void addPhynoti(PhysicianNotification p)
        {
            _context.PhysicianNotifications.Add(p);
            _context.SaveChanges();
        }

        public List<Region> GetReg()
        {
            return _context.Regions.ToList();
        }

        public List<Role> getrole()
        {
            return _context.Roles.ToList();
        }

        public void AddPhysician(Physician phy)
        {
            _context.Physicians.Add(phy);
            _context.SaveChanges();
        }

        public void AddPhysicianRegion(PhysicianRegion reg)
        {
            _context.PhysicianRegions.Add(reg);
            _context.SaveChanges();
        }

        public ICollection<AspNetRole> PhycianRoles()
        {
            return _context.AspNetRoles.Where(x => x.Id == 3).ToList();
        }

        public List<Role> getroletbl()
        {
            return _context.Roles.AsEnumerable().Where(x => !x.IsDeleted[0]).ToList();
        }

        public List<Menu> getmenutbl(int value)
        {
            if (value == 0)
            {
                return _context.Menus.ToList();
            }
            else
            {
                return _context.Menus.Where(x => x.AccountType == value).ToList();
            }


        }

        public void AddRoletbl(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        public void AddRoleMenutbl(RoleMenu rolemenu)
        {
            _context.RoleMenus.Add(rolemenu);
            _context.SaveChanges();
        }

        void IAdmin_Repository.RemoveRoleMenu(int roleId)
        {
            var menu = _context.RoleMenus.Where(m => m.RoleId == roleId).ToList();
            foreach (var item in menu)
            {
                _context.RoleMenus.Remove(item);
            }
        }

        public void AddRoleMenu(RoleMenu rolemenu)
        {
            _context.RoleMenus.Add(rolemenu);
            _context.SaveChanges();
        }



        Role IAdmin_Repository.GetDataFromRoles(int? id)
        {
            return _context.Roles.FirstOrDefault(m => m.RoleId == id);
        }

        public List<RoleMenu> GetDataFromRoleMenu(int id)
        {
            return _context.RoleMenus.Where(x => x.RoleId == id).ToList();
        }

        public List<Menu> GetMenuDataWithCheckwise(short accountType)
        {
            return _context.Menus.Where(m => m.AccountType == accountType).ToList();
        }

        public Physician getphycian(int id)
        {
            return _context.Physicians.FirstOrDefault(x => x.PhysicianId == id);
        }

        public AspNetUser? GetAspNetUser(int id)
        {
            return _context.AspNetUsers.FirstOrDefault(x => x.Id == id);
        }

        public List<PhysicianRegion> GetSelectedPhyReg(int id)
        {
            return _context.PhysicianRegions.Where(x => x.PhysicianId == id).ToList();
        }

        public void UpdatePhytbl(Physician p)
        {
            _context.Physicians.Update(p);
            _context.SaveChanges();
        }

        public void RemovePhyRegion(int physicianId)
        {
            List<PhysicianRegion> physicianRegions = _context.PhysicianRegions.Where(x => x.PhysicianId == physicianId).ToList();

            _context.PhysicianRegions.RemoveRange(physicianRegions);
            _context.SaveChanges();
        }
        public void AddPhyRegions(List<PhysicianRegion> physicianRegions)
        {
            _context.PhysicianRegions.AddRange(physicianRegions);
            _context.SaveChanges();
        }

        public void UpdateRoletbl(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }


        public List<AspNetUser> getallusers()
        {
            return _context.AspNetUsers.Where(x => x.Roles.Where(x => x.Id == 2 || x.Id == 3).Any()).Include(x => x.AdminAspNetUsers).Include(x => x.PhysicianAspNetUsers).Include(x => x.Roles).ToList();
        }

        public List<Role> GetAminRoles()
        {
            return _context.Roles.Where(x => x.AccountType == 1).ToList();
        }

        public ICollection<AspNetRole> AdminRoles()
        {
            return _context.AspNetRoles.Where(x => x.Id == 2).ToList();
        }

        public void AddAdmintbl(Admin aD)
        {
            _context.Admins.Add(aD);
            _context.SaveChanges();
        }

        public void AddAdminRegiontbl(AdminRegion reg)
        {
            _context.AdminRegions.Add(reg);
            _context.SaveChanges();
        }

        public void AddPhyLocation(PhysicianLocation pl)
        {
            _context.PhysicianLocations.Add(pl);
            _context.SaveChanges();
        }

        public List<HealthProfessionalType> GetProfession()
        {
            return _context.HealthProfessionalTypes.ToList();
        }

        public List<HealthProfessional> GetHealthProfession()
        {
            return _context.HealthProfessionals.AsEnumerable().Where(x => x.IsDeleted == null).ToList();
        }

        public string Profession(int? profession)
        {
            return _context.HealthProfessionalTypes.FirstOrDefault(x => x.HealthProfessionalId == profession).ProfessionName;

        }

        public List<HealthProfessional> GetHealthProfessionByProfession(int p)
        {
            return _context.HealthProfessionals.Where(x => x.Profession == p).ToList();
        }

        public void AddHealthProfessiontbl(HealthProfessional hp)
        {
            _context.HealthProfessionals.Add(hp);
            _context.SaveChanges();
        }

        public HealthProfessional GetData(int vendorid)
        {
            return _context.HealthProfessionals.FirstOrDefault(x => x.VendorId == vendorid);
        }

        public List<HealthProfessionalType> GetProfessionbyid(int? profession)
        {
            return _context.HealthProfessionalTypes.Where(x => x.HealthProfessionalId == profession).ToList();
        }

        public void UpdateHealthProfessiontbl(HealthProfessional hp)
        {
            _context.HealthProfessionals.Update(hp);
            _context.SaveChanges();
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

        public List<Physician> DayData()
        {
            return _context.Physicians.Include(x => x.Shifts).ThenInclude(x => x.ShiftDetails.Where(x => x.IsDeleted == new BitArray(1, false))).ToList();
        }

        public void UpdateShiftDetails()
        {
            _context.SaveChanges();
        }

        public ShiftDetail ChangeShift(int shiftId)
        {
            return _context.ShiftDetails.FirstOrDefault(i => i.ShiftDetailId == shiftId);
        }

        public ShiftDetail Shiftdetials(int shiftId)
        {
            return _context.ShiftDetails.FirstOrDefault(i => i.ShiftDetailId == shiftId);
        }



        public Physician DayDatabyPhysician(int? selectedPhysicianId)
        {
            return _context.Physicians.Include(s => s.Shifts).ThenInclude(s => s.ShiftDetails).FirstOrDefault(s => s.PhysicianId == selectedPhysicianId) ?? new();
        }

        public List<Physician> onduty(int regionid, TimeOnly currentTime, BitArray deletedBit)
        {
            return _context.ShiftDetails
                 .Include(sd => sd.Shift.Physician)
             .Where(sd => (regionid == 0 || sd.RegionId == regionid) &&
             sd.ShiftDate.Date == DateTime.Today &&
                              currentTime >= sd.StartTime &&
                              currentTime < sd.EndTime &&
                              sd.IsDeleted.Equals(deletedBit))
                 .Select(sd => sd.Shift.Physician)
                 .Distinct()
                 .ToList();
        }

        public List<Physician> offduty(int regionId, TimeOnly currentTime, BitArray deletedBit)
        {
            var a = _context.Physicians
                 .Include(p => p.PhysicianRegions)
                 .Where(p => (regionId == 0 || p.RegionId == regionId) &&
                             !_context.ShiftDetails.Any(sd => sd.Shift.PhysicianId == p.PhysicianId &&
                                                                sd.ShiftDate.Date == DateTime.Today &&
                                                                currentTime >= sd.StartTime &&
                                                                currentTime <= sd.EndTime &&
                                                                sd.IsDeleted.Equals(deletedBit)) && p.IsDeleted == null)
                 .ToList();

            //a = a.Where(x => x.RegionId == regionId).ToList();
            return a;
        }

        public ShiftDetail GetShiftDetails(int shiftdetailid)
        {
            return _context.ShiftDetails.Include(x => x.Shift).FirstOrDefault(x => x.ShiftDetailId == shiftdetailid);
        }

        public void Update(ShiftDetail shiftDetail)
        {
            _context.ShiftDetails.Update(shiftDetail);
            _context.SaveChanges();
        }

        public int getAspid(int admin1)
        {
            return _context.Admins.FirstOrDefault(x => x.AdminId == admin1).AspNetUserId;
        }

        public IQueryable<PatientHistoryTable> GetPatientHistoryTable(string? fname, string? lname, string? email, string? phone)
        {
            IQueryable<PatientHistoryTable> tabledata =
                                                       from u in _context.Users

                                                       select new PatientHistoryTable
                                                       {
                                                           aspId = u.UserId,
                                                           Firstname = u.FirstName ?? "-",
                                                           Lastname = u.LastName ?? "-",
                                                           Email = u.Email ?? "-",
                                                           phone = u.Mobile ?? "-",
                                                           Address = (u.Street ?? "") + " , " + (u.City ?? "") + " , " + (u.State ?? ""),
                                                       };
            if (!string.IsNullOrEmpty(fname))
            {
                tabledata = tabledata.Where(e => e.Firstname.ToLower().Contains(fname.ToLower()));
            }
            if (!string.IsNullOrEmpty(lname))
            {
                tabledata = tabledata.Where(e => e.Lastname.ToLower().Contains(lname.ToLower()));
            }
            if (!string.IsNullOrEmpty(email))
            {
                tabledata = tabledata.Where(e => e.Email.ToLower().Contains(email.ToLower()));
            }
            if (!string.IsNullOrEmpty(phone))
            {
                tabledata = tabledata.Where(e => e.phone.Contains(phone));
            }
            return tabledata;
        }

        public List<Request> GetAllRequestsByAid(int id)
        {

            return _context.Requests.Where(r => r.UserId == id && r.IsDeleted != new BitArray(1, true) && r.Status != 11).ToList();

        }

        public int GetNumberOfDocsByRid(int requestid)
        {
            return _context.RequestWiseFiles.Where(f => f.RequestId == requestid && f.IsDeleted == null).Count();
        }

        public string? GetStatus(short status)
        {
            switch (status)
            {
                case 1:
                    return "Unassigned";

                case 2:
                    return "Accepted";

                case 3:
                    return "MDEnRoute";

                case 4:
                    return "MDEnRoute";

                case 5:
                    return "Conclude";

                case 6:
                    return "Cancelled";

                case 7:
                    return "CancelledByPatient";

                case 8:
                    return "Closed";

                default:
                    return "Unpaid";


            }
        }

        public List<RequestType>? getRequestTypeList()
        {
            return _context.RequestTypes.ToList();
        }

        public List<RequestClient> getRequestClientList()
        {
            return _context.RequestClients.Include(e => e.Request).Where(e => e.Request.IsDeleted == null).ToList();
        }

        public List<RequestNote> getRequestNotesList()
        {
            return _context.RequestNotes.ToList();
        }

        public List<BlockRequest> getBlockData()
        {
            return _context.BlockRequests.Include(e => e.Request).ThenInclude(m => m.RequestClients).ToList();
        }

        public BlockRequest getBlockRequestById(int id)
        {
            return _context.BlockRequests.FirstOrDefault(x => x.RequestId == id);
        }

        public void updateBlockRequest(BlockRequest req)
        {
            _context.BlockRequests.Update(req);
            _context.SaveChanges();
        }

        public void AddEmaillogtbl(EmailLog emailLog)
        {
            _context.EmailLogs.Add(emailLog);
            _context.SaveChanges();
        }

        public List<Emaillogs> EmailLogs()
        {

            return _context.EmailLogs
                .Select(x => new Emaillogs()
                {
                    EmailLogId = x.EmailLogId,
                    Recipient = x.Admin != null ? x.Admin.FirstName + " " + x.Admin.LastName :
                               x.Physician != null ? x.Physician.FirstName + " " + x.Physician.LastName :
                               x.Request != null ? x.Request.RequestClients.First().FirstName + " " + x.Request.RequestClients.First().LastName :
                               "-",
                    Email = x.EmailId,
                    CreatedDate = x.CreateDate,
                    SentDate = x.SentDate,
                    SentTries = x.SentTries,
                    IsSent = x.IsEmailSent,
                    ConfirmationNumber = x.ConfirmationNumber,
                }).ToList();

        }

        public List<Emaillogs> SmsLogs()
        {
            return _context.Smslogs
          .Select(x => new Emaillogs()
          {
              EmailLogId = x.SmslogId,
              Recipient = x.Admin != null ? x.Admin.FirstName + " " + x.Admin.LastName :
                         x.Physician != null ? x.Physician.FirstName + " " + x.Physician.LastName :
                         x.Request != null ? x.Request.RequestClients.First().FirstName + " " + x.Request.RequestClients.First().LastName :
                         "-",
              Mobile = x.MobileNumber,
              RoleName = x.Role.Name??"-",
              CreatedDate = x.CreateDate,
              SentDate = x.SentDate,
              SentTries = x.SentTries,
              IsSent = x.IsSmssent,
              ConfirmationNumber = x.ConfirmationNumber,
          }).ToList();
        }

        public void SmsLogtbl(Smslog smslog)
        {
            _context.Smslogs.Add(smslog);
            _context.SaveChanges();
        }

        public List<AdminRecord> SearchRecords()
        {
            return _context.Requests
         .Where(x => x.IsDeleted == null)
         .Select(x => new AdminRecord()
         {
             RequestId = x.RequestId,
             PatientName = x.RequestClients.First().FirstName + " " + x.RequestClients.First().LastName,
             Email = x.RequestClients.First().Email,
             PhoneNumber = x.RequestClients.First().PhoneNumber ?? "-",
             Address = x.RequestClients.First().Street + ", " + x.RequestClients.First().City + ", " + x.RequestClients.First().State ?? "",
             zip = x.RequestClients.First().ZipCode ?? "-",
             status = x.Status,
             ReqtypeId = x.RequestTypeId,
             ProviderName = x.Physician.FirstName + " " + x.Physician.LastName,
             PhysicianNote = x.RequestNotes.First().PhysicianNotes,
             AdminNote = x.RequestNotes.First().AdminNotes,
             PatientNote = x.RequestClients.First().Notes,
            
         }).ToList();
        }
    }
}
