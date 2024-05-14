using hallocdoc_mvc_Service.Interface;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.Interface;
using hallodoc_mvc_Repository.ViewModel;
using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Helpers;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Voice;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace hallocdoc_mvc_Service.Implementation
{
    public class Admin_Service : IAdmin_Service
    {
        private readonly IAdmin_Repository _Repository;

        private readonly IConfiguration _configuration;

        private readonly IJwtService _jwtService;
        public Admin_Service(IAdmin_Repository Repository, IConfiguration configuration, IJwtService jwtService)
        {
            _Repository = Repository;
            _configuration = configuration;
            _jwtService = jwtService;
        }


        public List<AdminDashboard> getDashData(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            var query = _Repository.GetNewData();

            if (search != null)
            {
                query = query.Where(r => (bool)r.RequestClients.FirstOrDefault().FirstName.ToLower().Contains(search.ToLower()) || (bool)r.RequestClients.FirstOrDefault().LastName.ToLower().Contains(search.ToLower()));
                pageid = 1;
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3);
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1);
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4);
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2);
            }
            if (region != null && region != -1)
            {
                query = query.Where(r => r.RequestClients.FirstOrDefault().RegionId == region);
            }
            int size = 2;
            var a = query.Skip(pageid * size - size).Take(size).ToList();
            int c = query.Count();
            var dashData = new List<AdminDashboard>();





            foreach (var item in a)
            {
                List<RequestStatusLog?> requestStatusLog = _Repository.GetStatusLogsByRequest(item.RequestId);
                List<string>? transfer = new();

                requestStatusLog.ForEach(x =>
                {
                    Physician? phy = _Repository.GetPhysician(x.TransToPhysicianId);
                    if (phy != null)
                    {
                        transfer.Add("Admin transferred to Dr : " + phy?.FirstName + " on " + x.CreatedDate.ToString("dd/MM/yyyy") + " at " + x.CreatedDate.ToString("HH: mm:ss: tt") + " " + x.Notes);
                    }
                    else if (x.PhysicianId != null)
                    {
                        transfer.Add("Physician changed to status" + " " + x.Status + " " + x.Notes);
                    }
                    else
                    {
                        transfer.Add("Admin changed to status" + " " + x.Status + " " + x.Notes);
                    }
                });

                DateOnly Mydate = new(item.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(item.RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, item.RequestClients.FirstOrDefault().IntDate.Value);
                dashData.Add(new AdminDashboard
                {
                    Id = item.RequestId,
                    LName = item.RequestClients.FirstOrDefault()?.LastName,
                    FName = item.RequestClients.FirstOrDefault()?.FirstName,
                    Email = item.RequestClients.FirstOrDefault()?.Email,
                    Name = item.RequestClients.FirstOrDefault()?.FirstName,
                    RPhone = item.PhoneNumber,
                    DateOfBirth = Mydate,
                    Requestor = item.FirstName + ' ' + item.LastName,
                    RequestDate = item.CreatedDate,
                    Phone = item.RequestClients.FirstOrDefault()?.PhoneNumber,
                    Address = item.RequestClients.FirstOrDefault()?.Street + ", " + item.RequestClients.FirstOrDefault()?.City + ", " + item.RequestClients.FirstOrDefault()?.State,
                    Notes = transfer,
                    RequestTypeId = item.RequestTypeId,
                    Status = item.Status,
                    Physician = item.Physician?.FirstName,
                    PgCount = c,
                    ChatWith = item.PhysicianId ?? 0,
                });
            }

            return dashData;
        }

        public List<AdminDashboard> getDashDataActive(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            var query = _Repository.GetAdminStatus();

            if (search != null)
            {
                query = query.Where(r => (bool)r.RequestClients.FirstOrDefault().FirstName.ToLower().Contains(search.ToLower()) || (bool)r.RequestClients.FirstOrDefault().LastName.ToLower().Contains(search.ToLower()));
                pageid = 1;
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3);
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1);
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4);
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2);
            }
            if (region != null && region != -1)
            {
                query = query.Where(r => r.RequestClients.FirstOrDefault().RegionId == region);

            }
            var a = query.Skip(pageid * 2 - 2).Take(2).ToList();
            int c = query.Count();
            var dashData = new List<AdminDashboard>();

            foreach (var item in a)
            {
                DateOnly Mydate = new(item.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(item.RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, item.RequestClients.FirstOrDefault().IntDate.Value);
                dashData.Add(new AdminDashboard
                {
                    Id = item.RequestId,
                    LName = item.RequestClients.FirstOrDefault()?.LastName,
                    FName = item.RequestClients.FirstOrDefault()?.FirstName,
                    Email = item.RequestClients.FirstOrDefault()?.Email,
                    Name = item.RequestClients.FirstOrDefault()?.FirstName,
                    DateOfBirth = Mydate,
                    RPhone = item.PhoneNumber,
                    Requestor = item.FirstName + ' ' + item.LastName,
                    RequestDate = item.CreatedDate,
                    Phone = item.RequestClients.FirstOrDefault()?.PhoneNumber,
                    Address = item.RequestClients.FirstOrDefault()?.Street + ", " + item.RequestClients.FirstOrDefault()?.City + ", " + item.RequestClients.FirstOrDefault()?.State,
                    //Notes = item.RequestClients.FirstOrDefault()?.Notes,
                    RequestTypeId = item.RequestTypeId,
                    Status = item.Status,
                    Physician = item.Physician?.FirstName,
                    PgCount = c,
                    calltype = item.CallType,
                    isfinal = item.EncounterForms.FirstOrDefault()?.IsFinalized ?? new BitArray(1, false),
                    ChatWith = item.PhysicianId ?? 0,
                });
            }

            return dashData;
        }
        public List<AdminDashboard> getDashDataPending(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            var query = _Repository.GetAdminPending();

            if (search != null)
            {
                query = query.Where(r => (bool)r.RequestClients.FirstOrDefault().FirstName.ToLower().Contains(search.ToLower()) || (bool)r.RequestClients.FirstOrDefault().LastName.ToLower().Contains(search.ToLower()));
                pageid = 1;
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3);
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1);
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4);
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2);
            }
            if (region != null && region != -1)
            {
                query = query.Where(r => r.RequestClients.FirstOrDefault().RegionId == region);

            }
            var a = query.Skip(pageid * 2 - 2).Take(2).ToList();
            int c = query.Count();
            var dashData = new List<AdminDashboard>();



            foreach (var item in a)
            {

                List<RequestStatusLog?> requestStatusLog = _Repository.GetStatusLogsByRequest(item.RequestId);
                List<string>? transfer = new();

                requestStatusLog.ForEach(x =>
                {
                    Physician? phy = _Repository.GetPhysician(x.TransToPhysicianId);
                    if (phy != null)
                    {
                        transfer.Add("Admin transferred to Dr : " + phy?.FirstName + " on " + x.CreatedDate.ToString("dd/MM/yyyy") + " at " + x.CreatedDate.ToString("HH: mm:ss: tt") + " " + x.Notes);
                    }
                    else
                    {
                        transfer.Add("Admin changed to status" + " " + x.Notes);
                    }
                });


                DateOnly Mydate = new(item.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(item.RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, item.RequestClients.FirstOrDefault().IntDate.Value);
                dashData.Add(new AdminDashboard
                {
                    Id = item.RequestId,
                    LName = item.RequestClients.FirstOrDefault()?.LastName,
                    FName = item.RequestClients.FirstOrDefault()?.FirstName,
                    Email = item.RequestClients.FirstOrDefault()?.Email,
                    RPhone = item.PhoneNumber,
                    Physician = item.Physician?.FirstName,
                    Name = item.RequestClients.FirstOrDefault()?.FirstName,
                    DateOfBirth = Mydate,
                    Requestor = item.FirstName + ' ' + item.LastName,
                    RequestDate = item.CreatedDate,
                    Phone = item.RequestClients.FirstOrDefault()?.PhoneNumber,
                    Address = item.RequestClients.FirstOrDefault()?.Street + ", " + item.RequestClients.FirstOrDefault()?.City + ", " + item.RequestClients.FirstOrDefault()?.State,
                    Notes = transfer,
                    RequestTypeId = item.RequestTypeId,
                    Status = item.Status,
                    PgCount = c,
                    ChatWith = item.PhysicianId ?? 0,
                });
            }

            return dashData;
        }
        public List<AdminDashboard> getDashDataConclude(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            var query = _Repository.GetAdminConclude();

            if (search != null)
            {
                query = query.Where(r => (bool)r.RequestClients.FirstOrDefault().FirstName.ToLower().Contains(search.ToLower()) || (bool)r.RequestClients.FirstOrDefault().LastName.ToLower().Contains(search.ToLower()));
                pageid = 1;
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3);
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1);
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4);
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2);
            }
            if (region != null && region != -1)
            {
                query = query.Where(r => r.RequestClients.FirstOrDefault().RegionId == region);

            }
            var a = query.Skip(pageid * 2 - 2).Take(2).ToList();
            int c = query.Count();
            var dashData = new List<AdminDashboard>();

            foreach (var item in a)
            {
                DateOnly Mydate = new(item.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(item.RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, item.RequestClients.FirstOrDefault().IntDate.Value);
                dashData.Add(new AdminDashboard
                {
                    Id = item.RequestId,
                    LName = item.RequestClients.FirstOrDefault()?.LastName,
                    FName = item.RequestClients.FirstOrDefault()?.FirstName,
                    Email = item.RequestClients.FirstOrDefault()?.Email,
                    Name = item.RequestClients.FirstOrDefault()?.FirstName,
                    DateOfBirth = Mydate,
                    RPhone = item.PhoneNumber,

                    Requestor = item.FirstName + ' ' + item.LastName,
                    RequestDate = item.CreatedDate,
                    Phone = item.RequestClients.FirstOrDefault()?.PhoneNumber,
                    Address = item.RequestClients.FirstOrDefault()?.Street + ", " + item.RequestClients.FirstOrDefault()?.City + ", " + item.RequestClients.FirstOrDefault()?.State,
                    //Notes = item.RequestClients.FirstOrDefault()?.Notes,
                    RequestTypeId = item.RequestTypeId,
                    Status = item.Status,
                    Physician = item.Physician?.FirstName,
                    PgCount = c,
                    calltype = item.CallType,
                    isfinal = item.EncounterForms.FirstOrDefault()?.IsFinalized ?? new BitArray(1, false),
                    ChatWith = item.PhysicianId ?? 0,
                });
            }

            return dashData;
        }
        public List<AdminDashboard> getDashDataToclose(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            var query = _Repository.GetAdminToclose();

            if (search != null)
            {
                query = query.Where(r => (bool)r.RequestClients.FirstOrDefault().FirstName.ToLower().Contains(search.ToLower()) || (bool)r.RequestClients.FirstOrDefault().LastName.ToLower().Contains(search.ToLower()));
                pageid = 1;
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3);
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1);
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4);
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2);
            }
            if (region != null && region != -1)
            {
                query = query.Where(r => r.RequestClients.FirstOrDefault().RegionId == region);

            }
            var a = query.Skip(pageid * 2 - 2).Take(2).ToList();
            int c = query.Count();
            var dashData = new List<AdminDashboard>();

            foreach (var item in a)
            {

                List<RequestStatusLog?> requestStatusLog = _Repository.GetStatusLogsByRequest(item.RequestId);
                List<string>? transfer = new();

                requestStatusLog.ForEach(x =>
                {
                    Physician? phy = _Repository.GetPhysician(x.TransToPhysicianId);
                    if (phy != null)
                    {
                        transfer.Add("Admin transferred to Dr : " + phy?.FirstName + " on " + x.CreatedDate.ToString("dd/MM/yyyy") + " at " + x.CreatedDate.ToString("HH: mm:ss: tt") + " " + x.Notes);
                    }
                    else
                    {
                        transfer.Add("Admin changed to status" + " " + x.Status);
                    }
                });
                DateOnly Mydate = new(item.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(item.RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, item.RequestClients.FirstOrDefault().IntDate.Value);
                dashData.Add(new AdminDashboard
                {
                    Id = item.RequestId,
                    LName = item.RequestClients.FirstOrDefault()?.LastName,
                    FName = item.RequestClients.FirstOrDefault()?.FirstName,
                    Email = item.RequestClients.FirstOrDefault()?.Email,
                    Name = item.RequestClients.FirstOrDefault()?.FirstName,
                    DateOfBirth = Mydate,
                    Region = _Repository.GetRegionname(item.RequestClients.FirstOrDefault().RegionId),
                    RPhone = item.PhoneNumber,
                    Requestor = item.FirstName + ' ' + item.LastName,
                    RequestDate = item.CreatedDate,
                    Phone = item.RequestClients.FirstOrDefault()?.PhoneNumber,
                    Address = item.RequestClients.FirstOrDefault()?.Street + ", " + item.RequestClients.FirstOrDefault()?.City + ", " + item.RequestClients.FirstOrDefault()?.State,
                    Notes = transfer,
                    RequestTypeId = item.RequestTypeId,
                    Status = item.Status,
                    Physician = item.Physician?.FirstName,
                    PgCount = c,
                    calltype = item.CallType,
                    isfinal = item.EncounterForms.FirstOrDefault()?.IsFinalized ?? new BitArray(1, false),
                    ChatWith = item.PhysicianId ?? 0,
                });
            }

            return dashData;
        }
        public List<AdminDashboard> getDashDataUnpaid(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            var query = _Repository.GetAdminUnpaid();

            if (search != null)
            {
                query = query.Where(r => (bool)r.RequestClients.FirstOrDefault().FirstName.ToLower().Contains(search.ToLower()) || (bool)r.RequestClients.FirstOrDefault().LastName.ToLower().Contains(search.ToLower()));
                pageid = 1;
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3);
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1);
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4);
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2);
            }
            if (region != null && region != -1)
            {
                query = query.Where(r => r.RequestClients.FirstOrDefault().RegionId == region);

            }

            var a = query.Skip(pageid * 2 - 2).Take(2).ToList();
            int c = query.Count();
            var dashData = new List<AdminDashboard>();

            foreach (var item in a)
            {
                DateOnly Mydate = new(item.RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(item.RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, item.RequestClients.FirstOrDefault().IntDate.Value);
                dashData.Add(new AdminDashboard
                {
                    Id = item.RequestId,
                    LName = item.RequestClients.FirstOrDefault()?.LastName,
                    FName = item.RequestClients.FirstOrDefault()?.FirstName,
                    Email = item.RequestClients.FirstOrDefault()?.Email,
                    Name = item.RequestClients.FirstOrDefault()?.FirstName,
                    DateOfBirth = Mydate,
                    RPhone = item.PhoneNumber,
                    Requestor = item.FirstName + ' ' + item.LastName,
                    RequestDate = item.CreatedDate,
                    Phone = item.RequestClients.FirstOrDefault()?.PhoneNumber,
                    Address = item.RequestClients.FirstOrDefault()?.Street + ", " + item.RequestClients.FirstOrDefault()?.City + ", " + item.RequestClients.FirstOrDefault()?.State,
                    //Notes = item.RequestClients.FirstkOrDefault()?.Notes,
                    RequestTypeId = item.RequestTypeId,
                    Status = item.Status,
                    Physician = item.Physician?.FirstName,
                    PgCount = c,
                    calltype = item.CallType,
                    ChatWith = item.PhysicianId ?? 0,
                    isfinal = item.EncounterForms.FirstOrDefault()?.IsFinalized ?? new BitArray(1, false),
                });
            }

            return dashData;
        }

        public ViewCase Getcase(int reqId)
        {
            var requestData = _Repository.GetRequest().Where(x => x.RequestId == reqId);
            //var requestData = _context.Requests.FirstOrDefault(r => r.Requestid == ReqId);

            DateOnly date = new DateOnly(requestData.FirstOrDefault().RequestClients.FirstOrDefault().IntYear.Value, DateOnly.ParseExact(requestData.FirstOrDefault().RequestClients.FirstOrDefault().StrMonth, "MMM", CultureInfo.InvariantCulture).Month, requestData.FirstOrDefault().RequestClients.FirstOrDefault().IntDate.Value);


            ViewCase ViewCase = new()
            {
                ReqId = reqId,
                FirstName = requestData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.FirstName,
                LastName = requestData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.LastName,
                PhoneNumber = requestData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.PhoneNumber,
                Email = requestData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.Email,
                PatientNotes = requestData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.Notes,
                BirthDate = date,
                AddressORBusinessName = requestData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.Address,
                Status = requestData.FirstOrDefault().Status,
                ReqType = requestData.FirstOrDefault().RequestTypeId,
                ConfirmationNO = requestData.FirstOrDefault()?.ConfirmationNumber,

                // Room = requestData.roo,
                // PatientRegion = requestData.
            };

            return ViewCase;

        }

        public AspNetUser ValidateUser(LoginViewModel model)
        {
            AspNetUser asp = _Repository.Validate(model);
            if (asp.Id > 0)
            {
                if (Crypto.VerifyHashedPassword(asp.PasswordHash, model.Passwordhash))
                {
                    return asp;
                }
            }
            return new();
        }

        public Admin getAdmin(string email)
        {
            return _Repository.getaspuser(email);
        }

        public ModalData GetAssignData(ModalData md)
        {


            List<Region> r = _Repository.GetRegion();
            md.region = r;


            List<Request> countdata = _Repository.GetCountData();

            var num = countdata.GroupBy(x => x.Status).Select(x => new
            {
                status = x.Key,
                c = x.Count(),
            });

            int a = num.FirstOrDefault(x => x.status == 1)?.c ?? 0;
            int b = num.FirstOrDefault(x => x.status == 2)?.c ?? 0;

            int c = num.FirstOrDefault(x => x.status == 4)?.c ?? 0;
            c += num.FirstOrDefault(x => x.status == 5)?.c ?? 0;

            int d = num.FirstOrDefault(x => x.status == 6)?.c ?? 0;

            int e = num.FirstOrDefault(x => x.status == 3)?.c ?? 0;
            e += num.FirstOrDefault(x => x.status == 7)?.c ?? 0;
            e += num.FirstOrDefault(x => x.status == 8)?.c ?? 0;

            int f = num.FirstOrDefault(x => x.status == 9)?.c ?? 0;

            md.CountNew = a;
            md.CountPending = b;
            md.CountActive = c;
            md.CountConclude = d;
            md.CountClose = e;
            md.CountUnpaid = f;

            return md;
        }

        public ViewNote? setViewNotesData(ViewNote model, int id, int admin)
        {
            //var notedata = _context.Requestnotes.FirstOrDefault(i => i.Requestid == requestid);
            var notedata = _Repository.setnotes(id);

            if (notedata != null)
            {
                notedata.RequestId = id;
                notedata.AdminNotes = model.AdminNotes;
                notedata.ModifiedBy = _Repository.getAspid(admin);
                notedata.ModifiedDate = DateTime.Now;
                _Repository.save();

            }
            else
            {
                var newnotedata = new RequestNote()
                {
                    RequestId = id,
                    AdminNotes = model.AdminNotes,
                    CreatedBy = _Repository.getAspid(admin),
                    CreatedDate = DateTime.Now,
                };

                _Repository.AddRequestNotes(newnotedata);

            }
            return model;
        }

        public ViewNote? GetNotes(int id)
        {
            var notes = _Repository.setnotes(id);

            var cancelNote = string.Empty;
            var patientcancel = "";

            var requestStatusLog = _Repository.GetStatusLogsByRequest(id);
            List<string?> transfer = new();

            requestStatusLog.ForEach(x =>
            {
                Physician? phy = _Repository.GetPhysician(x.TransToPhysicianId);
                transfer.Add("Admin transferred to Dr : " + phy?.FirstName + " on " + x.CreatedDate.ToString("dd/MM/yyyy") + " at " + x.CreatedDate.ToString("HH: mm:ss: tt") + " " + x.Notes);
            });

            var log = requestStatusLog.FirstOrDefault(x => x.Status == 3);
            if (log != null)
            {
                cancelNote = log.Notes;
            }

            var log1 = requestStatusLog.FirstOrDefault(x => x.Status == 7);
            if (log1 != null)
            {
                patientcancel = log1.Notes;
            }

            ViewNote data = new ViewNote()
            {
                RequestId = id,
                Admincancellationnote = cancelNote,
                Patientcancellationnote = patientcancel,
                AdminNotes = notes?.AdminNotes,
                TransferNotes = transfer,
                PhysicianNotes = notes?.PhysicianNotes
            };
            return data;
        }

        //public ModalData GetAssignPhysician(int id,ModalData md)
        //{
        //    List<Region> r = _Repository.GetPhyByRegion(id,md);
        //    md.region = r;
        //    return md;
        //}

        public List<Physician> GetPhysician(int id)
        {
            return _Repository.GetPhysiciansByRegion(id).ToList();
        }

        public RequestClient? GetCancelCaseData(ModalData md, int id)
        {
            var canceldata = _Repository.Cancelname(id);


            var AdminCancelCaseModel = new RequestClient()
            {
                RequestId = id,
                FirstName = canceldata.FirstName,
                LastName = canceldata.LastName
            };
            return AdminCancelCaseModel;
        }

        public List<CaseTag> GetCaseReason()
        {
            return _Repository.GetCases();
        }

        public void CanclePost(ModalData md, int id, int admin)
        {
            Request req = _Repository.GetRequestById(id);

            req.Status = 3;
            req.CaseTag = md.reason;
            req.ModifiedDate = DateTime.Now;
            _Repository.UpdateRequesttbl(req);

            RequestStatusLog reqlog = new()
            {
                RequestId = id,
                Status = 3,
                AdminId = admin,
                Notes = md.note,
                CreatedDate = DateTime.Now,

            };
            _Repository.AddRequestStatuslog(reqlog);
        }

        public void AssignCase(ModalData md, int id, int admin)
        {
            Request req = _Repository.GetRequestById(id);

            req.Status = 1;
            req.PhysicianId = md.SelectedPhysicianName;
            req.ModifiedDate = DateTime.Now;
            _Repository.UpdateRequesttbl(req);

            RequestStatusLog reqlog = new()
            {
                RequestId = id,
                Status = 1,
                TransToPhysicianId = md.SelectedPhysicianName,
                AdminId = admin,
                Notes = md.note,
                CreatedDate = DateTime.Now,
            };
            _Repository.AddRequestStatuslog(reqlog);





        }

        public RequestClient? GetBlockCaseData(ModalData md, int id)
        {
            var blockdata = _Repository.Blockname(id);


            var AdminCancelCaseModel = new RequestClient()
            {
                RequestId = id,
                FirstName = blockdata.FirstName,
                LastName = blockdata.LastName
            };
            return AdminCancelCaseModel;
        }

        public void BlockPost(ModalData md, int id, int admin)
        {

            Request req = _Repository.GetRequestById(id);

            req.Status = 10;
            
            req.ModifiedDate = DateTime.Now;
            _Repository.UpdateRequesttbl(req);

            RequestStatusLog reqlog = new()
            {
                RequestId = id,
                Status = 10,
                AdminId = admin,
                Notes = md.note,
                CreatedDate = DateTime.Now,

            };
            _Repository.AddRequestStatuslog(reqlog);
            RequestClient? rc = _Repository.GetRequestclient(id);
            BlockRequest blreq = new()
            {
                PhoneNumber = rc.PhoneNumber,
                Email = rc.Email,
                Reason = md.note,
                RequestId = id,
                CreatedDate = DateTime.Now

            };
            _Repository.AddBlockRequest(blreq);
        }

        public List<ViewDocument> ViewUploadData(int id)
        {
            List<RequestWiseFile> rwf = _Repository.getfile(id);
            Request? req = _Repository.getdetail(id);

            List<ViewDocument> vd = new();

            if (rwf.Count != 0)
            {



                foreach (var item in rwf)
                {
                    vd.Add(new ViewDocument
                    {
                        FirstName = req.FirstName,
                        Confirmationnumber = req.ConfirmationNumber,
                        RequestId = req.RequestId,
                        FileName = item.FileName,
                        FileId = item.RequestWiseFileId,
                        Uploaddate = item.CreatedDate,


                    });
                }
            }
            else
            {
                vd.Add(new ViewDocument
                {
                    FirstName = req.FirstName,
                    Confirmationnumber = req.ConfirmationNumber,
                    RequestId = req.RequestId,



                });
            }
            return vd;
        }

        public void FileUpload(int id, List<IFormFile> file, int admin)
        {
            foreach (IFormFile files in file)
            {
                string filename = files.FileName;
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\uplodedfiles\\", filename);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    files.CopyToAsync(stream).Wait();
                }


                RequestWiseFile requestWiseFile = new RequestWiseFile();
                requestWiseFile.FileName = filename;
                requestWiseFile.RequestId = id;
                requestWiseFile.DocType = 1;
                requestWiseFile.AdminId = admin;
                _Repository.AddRequestWiseFiles(requestWiseFile);

            }
        }

        public int DeleteFile(int id)
        {
            RequestWiseFile df = _Repository.GetDocumentFile(id)
;
            df.IsDeleted = new System.Collections.BitArray(1, true);
            _Repository.update_RequestWiseTable(df);

            return df.RequestId;
        }

        void IAdmin_Service.DeleteAllFiles(int id)
        {
            List<RequestWiseFile> reqdoc = _Repository.GetDocumentList(id);
            foreach (var item in reqdoc)
            {
                item.IsDeleted = new System.Collections.BitArray(1, true);
                _Repository.update_RequestWiseTable(item);

            }
        }

        public void SendEmail(int id, int admin)
        {
            List<RequestWiseFile> requestWiseFiles = _Repository.GetDocumentList(id);
            Request request = _Repository.GetRequestById(id);
            var receiver = request.Email;
            var subject = "Documents of Request " + request.ConfirmationNumber?.ToUpper();
            var message = "Find the Files uploaded for your request in below:";
            var mailMessage = new MailMessage(from: "tatva.dotnet.binalmalaviya@outlook.com", to: receiver, subject, message);

            foreach (var file in requestWiseFiles)
            {
                var filePath = "D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\uplodedfiles\\" + file.FileName;
                if (File.Exists(filePath))
                {
                    byte[] fileContent;
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        fileContent = new byte[fileStream.Length];
                        fileStream.Read(fileContent, 0, (int)fileStream.Length);
                    }
                    var attachment = new Attachment(new MemoryStream(fileContent), file.FileName);
                    mailMessage.Attachments.Add(attachment);
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }
            }

            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";

            var client = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            client.SendMailAsync(mailMessage);


            EmailLog emailLog = new()
            {
                EmailTemplate = message,
                SubjectName = subject,
                SentTries = 1,
                IsEmailSent = true,
                EmailId = request.Email,
                CreateDate = DateTime.Now,
                SentDate = DateTime.Now,
                RequestId = request.RequestId,

            };
            _Repository.AddEmaillogtbl(emailLog);
        }

        public Order GetOrderData(Order md)
        {

            List<HealthProfessionalType> r = _Repository.GetHealthprofessionalByType();

            md.HealthProfessionalType = r;

            return md;
        }

        public List<HealthProfessional> Getvendor(int id)
        {

            return _Repository.getvendorbyprofessiontype(id);

        }

        public List<HealthProfessional> Getvendordata(int id)
        {
            return _Repository.getdatabyvendorid(id);
        }

        public void OrderPost(Order md, int id, int admin)
        {

            OrderDetail oredr = new()
            {
                VendorId = md.SelectedVendorId,
                RequestId = id,
                FaxNumber = md.Fax,
                Email = md.Email,
                BusinessContact = md.Email,
                Prescription = md.Detail,
                NoOfRefill = md.refill,
                CreatedDate = DateTime.Now,
                CreatedBy = _Repository.getAspid(admin).ToString(),

            };
            _Repository.AddOrderdetails(oredr);
        }

        public void Clear(int id, int admin)
        {
            Request req = _Repository.GetRequestById(id);

            req.Status = 11;

            req.ModifiedDate = DateTime.Now;
            _Repository.UpdateRequesttbl(req);

            RequestStatusLog reqlog = new()
            {
                RequestId = id,
                Status = 11,
                AdminId = admin,

                CreatedDate = DateTime.Now,

            };
            _Repository.AddRequestStatuslog(reqlog);
        }

        public RequestClient GetAgreementtdata(int id)
        {
            return _Repository.getagreement(id);
        }

        public void SendAgreementMail(int Id, ModalData md, string token, int admin)
        {
            RequestClient rc = _Repository.getagreement(Id);


            if (md.email != null)
            {
                var receiver = md.email;
                var subject = "Send Agreement";
                var message = "Tap on link for Send Agreement : http://localhost:5198/Admin/Agreement?t=" + token + "&token=" + Id;


                var mail = "tatva.dotnet.binalmalaviya@outlook.com";
                var password = "binal@2002";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));


                EmailLog emailLog = new()
                {
                    EmailTemplate = message,
                    SubjectName = subject,
                    SentTries = 1,
                    IsEmailSent = true,

                    EmailId = md.email,
                    ConfirmationNumber = rc.Request.ConfirmationNumber,
                    RequestId = rc.RequestId,
                    CreateDate = DateTime.Now,
                    SentDate = DateTime.Now,

                };
                _Repository.AddEmaillogtbl(emailLog);

            }




        }

        public Close getclosedata(int id)
        {
            List<RequestWiseFile> rwf = _Repository.getfile(id);
            Request? req = _Repository.getdetail(id);
            var r = _Repository.getagreement(id);
            DateOnly Mydate = new(r.IntYear.Value, DateOnly.ParseExact(r.StrMonth, "MMM", CultureInfo.InvariantCulture).Month, r.IntDate.Value);

            Close c = new()
            {
                FirstName = req.FirstName,
                Confirmationnumber = req.ConfirmationNumber,
                RequestId = req.RequestId,
                //FileName = item.FileName,
                //FileId = item.RequestWiseFileId,
                //Uploaddate = item.CreatedDate,

                Fname = r.FirstName,
                Lname = r.LastName,
                Email = r.Email,
                BirthDate = Mydate,
                Phone = r.PhoneNumber,
            };

            List<ViewDocument> viewDocuments = new List<ViewDocument>();

            if (rwf.Count != 0)
            {
                foreach (var item in rwf)
                {
                    viewDocuments.Add(new ViewDocument
                    {
                        FirstName = req.FirstName,
                        Confirmationnumber = req.ConfirmationNumber,
                        RequestId = req.RequestId,
                        FileName = item.FileName,
                        FileId = item.RequestWiseFileId,
                        Uploaddate = item.CreatedDate,
                    });
                }
            }
            else
            {
                viewDocuments.Add(new ViewDocument
                {
                    FirstName = req.FirstName,
                    Confirmationnumber = req.ConfirmationNumber,
                    RequestId = req.RequestId,
                });
            }

            c.viewDocuments = viewDocuments;

            return c;
        }

        public void editdata(Close model, int id)
        {
            var r = _Repository.getagreement(id);


            r.Email = model.Email;
            r.PhoneNumber = model.Phone;


            _Repository.updaterequestclient(r);

        }

        public void close(int id, int admin)
        {
            Request req = _Repository.GetRequestById(id);

            req.Status = 9;
            req.ModifiedDate = DateTime.Now;
            _Repository.UpdateRequesttbl(req);

            RequestStatusLog reqlog = new()
            {
                RequestId = id,
                Status = 9,
                AdminId = admin,
                CreatedDate = DateTime.Now,
            };
            _Repository.AddRequestStatuslog(reqlog);

            RequestClosed requestClosed = new()
            {
                RequestId = id,
                RequestStatusLogId = reqlog.RequestStatusLogId
            };
            _Repository.AddRequestclosed(requestClosed);
        }

        public void agreeagreement(int id)
        {
            Request req = _Repository.getreqid(id);
            req.Status = 4;
            _Repository.UpdateRequesttbl(req);

        }

        public ModalData cancelmodal(int id)
        {
            RequestClient rc = _Repository.getclient(id);
            ModalData resp = new();
            resp.PatientName = rc.FirstName + " " + rc.LastName;
            resp.Token = id;
            
            
            return resp;
        }

        public void cancelagreement(int id, ModalData md)
        {
            Request req = _Repository.GetRequestById(id);

            req.Status = 7;

            req.ModifiedDate = DateTime.Now;
            _Repository.UpdateRequesttbl(req);

            RequestStatusLog reqlog = new()
            {
                RequestId = id,
                Status = 7,
                Notes = md.note,
                CreatedDate = DateTime.Now,

            };
            _Repository.AddRequestStatuslog(reqlog);

        }

        public Encounter getencounter(int id)
        {
            RequestClient rc = _Repository.getagreement(id);

            EncounterForm ef = _Repository.getencounterbyid(id);
            DateOnly Mydate = new(rc.IntYear.Value, DateOnly.ParseExact(rc.StrMonth, "MMM", CultureInfo.InvariantCulture).Month, rc.IntDate.Value);
            if (ef != null)
            {


                Encounter en = new()
                {

                    Requestid = id,
                    patientData = rc,
                    DOB = Mydate,
                    Date = ef.Date,
                    HistoryIllness = ef.HistoryIllness,
                    MedicalHistory = ef.MedicalHistory,
                    Medications = ef.Medications,
                    Allergies = ef.Allergies,
                    Temp = ef.Temp,
                    Hr = ef.Hr,
                    Rr = ef.Rr,
                    BpS = ef.BpS,
                    BpD = ef.BpD,
                    O2 = ef.O2,
                    Pain = ef.Pain,
                    Heent = ef.Heent,
                    Cv = ef.Cv,
                    Chest = ef.Chest,
                    Abd = ef.Abd,
                    Extr = ef.Extr,
                    Skin = ef.Skin,
                    Neuro = ef.Neuro,
                    Other = ef.Other,
                    Diagnosis = ef.Diagnosis,
                    TreatmentPlan = ef.TreatmentPlan,
                    MedicationDispensed = ef.MedicationDispensed,
                    Procedures = ef.Procedures,
                    FollowUp = ef.FollowUp,

                };

                return en;
            }
            else
            {
                Encounter ent = new()
                {
                    Requestid = id,
                    patientData = rc,
                    DOB = Mydate,
                };

                return ent;
            }
            return new();



        }

        public void editencounter(int id, Encounter model)
        {
            EncounterForm ef = _Repository.getencounterbyid(id);
            if (ef != null)
            {
                ef.Date = model.Date;
                ef.HistoryIllness = model.HistoryIllness;
                ef.MedicalHistory = model.MedicalHistory;

                ef.Procedures = model.Procedures;
                ef.FollowUp = model.FollowUp;
                ef.Heent = model.Heent;
                ef.Cv = model.Cv;
                ef.Chest = model.Chest;
                ef.Abd = model.Abd;
                ef.Extr = model.Extr;
                ef.Skin = model.Skin;
                ef.Neuro = model.Neuro;
                ef.Other = model.Other;
                ef.Diagnosis = model.Diagnosis;
                ef.TreatmentPlan = model.TreatmentPlan;
                ef.MedicationDispensed = model.MedicationDispensed;
                ef.Medications = model.Medications;
                ef.Allergies = model.Allergies;
                ef.Temp = model.Temp;
                ef.Hr = model.Hr;
                ef.Rr = model.Rr;
                ef.BpD = model.BpD;
                ef.BpS = model.BpS;
                ef.O2 = model.O2;
                ef.Pain = model.Pain;

                _Repository.updateEncounterForm(ef);
            }
            else
            {
                EncounterForm enf = new()
                {
                    RequestId = id,

                    Date = DateTime.Now,
                    HistoryIllness = model.HistoryIllness,
                    MedicalHistory = model.MedicalHistory,
                    Medications = model.Medications,
                    Procedures = model.Procedures,
                    Heent = model.Heent,
                    Cv = model.Cv,
                    Chest = model.Chest,
                    Abd = model.Abd,
                    Extr = model.Extr,
                    Skin = model.Skin,
                    Neuro = model.Neuro,
                    Other = model.Other,
                    Diagnosis = model.Diagnosis,
                    TreatmentPlan = model.TreatmentPlan,
                    MedicationDispensed = model.MedicationDispensed,
                    IsFinalized = new BitArray(1,false),
                    Allergies = model.Allergies,
                    Temp = model.Temp,
                    Hr = model.Hr,
                    Rr = model.Rr,
                    BpD = model.BpD,
                    BpS = model.BpS,
                    O2 = model.O2,
                    Pain = model.Pain,
                    FollowUp = model.FollowUp,

                };
                _Repository.AddEncounterForm(enf);
            }


        }

        public Profile getprofile(int admin)
        {
            Admin a = _Repository.getadminbyadminid(admin);
            List<AdminRegion> dsa = _Repository.getadminreg(admin);
            List<Region> r = _Repository.GetRegion();
            List<Regiondetails> rd = new();
            foreach (AdminRegion region in dsa)
            {
                rd.Add(new Regiondetails
                {
                    Regionid = region.RegionId,
                    Regionname = _Repository.GetRegionname(region.RegionId),
                });
            }
            Profile pf = new()
            {
                AdminData = a,
                region = r,
                Role = _Repository.GetRoleOfUser(admin),
                user = _Repository.getuserbyaspid(a.AspNetUserId),
                State = _Repository.GetRegionname(a.RegionId),
                Reg = rd,
            };
            return pf;
        }

        public List<Request> GetRequestDataInList()
        {
            return _Repository.getINlist();
        }

        public void sendlink(ViewCase model, int admin1)
        {
            var accountSid = _configuration["Twilio:accountSid"];
            var authToken = _configuration["Twilio:authToken"];
            var twilionumber = _configuration["Twilio:twilioNumber"];


            var messageBody = $"Hello {model.FirstName} {model.LastName},\nClick the following link to create new request in our portal,\nhttp://localhost:5198/Home/submit_screen\n\n\nRegards,\nHalloDoc";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                from: new Twilio.Types.PhoneNumber(twilionumber),
                body: messageBody,
                to: new Twilio.Types.PhoneNumber("+91" + model.PhoneNumber)
            );


            Smslog smslog = new()
            {
                Smstemplate = messageBody,
                MobileNumber = model.PhoneNumber,
                CreateDate = DateTime.Now,
                SentDate = DateTime.Now,
                SentTries = 1,
                IsSmssent = true,
            };
            _Repository.SmsLogtbl(smslog);





            var receiver = model.Email;
            var subject = "Send Link";
            var messages = "Tap on link for Send Link : <!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <style>\r\n        /* Your provided CSS */\r\n        .button-74 {\r\n            background-color: #fbeee0;\r\n            border: 2px solid #422800;\r\n            border-radius: 30px;\r\n            box-shadow: #422800 4px 4px 0 0;\r\n            color: #422800;\r\n            cursor: pointer;\r\n            display: inline-block;\r\n            font-weight: 600;\r\n            font-size: 18px;\r\n            padding: 0 18px;\r\n            line-height: 50px;\r\n            text-align: center;\r\n            text-decoration: none;\r\n            user-select: none;\r\n            -webkit-user-select: none;\r\n            touch-action: manipulation;\r\n        }\r\n\r\n        .button-74:hover {\r\n            background-color: #fff;\r\n        }\r\n\r\n        .button-74:active {\r\n            box-shadow: #422800 2px 2px 0 0;\r\n            transform: translate(2px, 2px);\r\n        }\r\n\r\n        @media (min-width: 768px) {\r\n            .button-74 {\r\n                min-width: 120px;\r\n                padding: 0 25px;\r\n            }\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <a href=\"http://localhost:5198/Home/submit_screen\" class=\"button-74\">Click me!</a>\r\n</body>\r\n</html>\r\n";


            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, messages)
            {
                IsBodyHtml = true
            });

            EmailLog emailLog = new()
            {
                EmailTemplate = messages,
                SubjectName = subject,
                SentTries = 1,
                IsEmailSent = true,
                EmailId = model.Email,
                CreateDate = DateTime.Now,
                SentDate = DateTime.Now,

            };
            _Repository.AddEmaillogtbl(emailLog);

        }

        public void PatientForm(patient_form model, int admin)
        {


            var aspnetuser1 = _Repository.getAsp(model.Email);

            //send mail//


            if (aspnetuser1 == null)
            {
                var receiver = model.Email;
                var subject = "Send Link";
                var message = "Tap on link for Send Link : http://localhost:5198/Home/create_patient?token="+_jwtService.GenerateJwtTokenByEmail(receiver);


                var mail = "tatva.dotnet.binalmalaviya@outlook.com";
                var password = "binal@2002";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };
                //complete//

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
            }


            Region region = new Region
            {
                Name = model.State,
                Abbreviation = model.State.Substring(0, 3),
            };
            Region isRegion = _Repository.isRegion(region.Abbreviation);

            if (isRegion == null)
            {
                isRegion = region;
                _Repository.AddRegion(region);
            }



            Request request = new Request
            {
                RequestTypeId = 2,

                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                CreatedDate = DateTime.Now,
                Status = 1,

            };

            _Repository.AddRequesttbl(request);





            RequestClient requestclient = new RequestClient
            {

                RequestId = request.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Location = model.City,
                Address = model.Street,
                RegionId = isRegion.RegionId,
                IntDate = model.BirthDate.Day,
                StrMonth = model.BirthDate.ToString("MMM"),
                IntYear = model.BirthDate.Year,
                Street = model.Street,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,

            };

            _Repository.AddRequestClient(requestclient);

            RequestNote rn = new()
            {
                AdminNotes = model.adminnote,
                RequestId = request.RequestId,
                CreatedBy = _Repository.getAspid(admin),
                CreatedDate = DateTime.Now,
            };

            _Repository.AddRequestNotes(rn);
        }

        public List<Request> Export(string s, int reqtype, int regid, int state)
        {
            Dictionary<int, int> mapping = new()
            {
                 { 1, 1 }, { 2, 2 }, { 3, 5 }, { 4, 3 }, { 5, 3 }, { 6, 4 }, { 7, 5 }, { 8, 5 }, { 9, 6 },{10,0},{11,0}
            };

            var query = _Repository.getINlist();

            query = query.Where(x => mapping[x.Status] == state).ToList();

            if (s != null)
            {
                query = query.Where(r => (bool)r.RequestClients.FirstOrDefault().FirstName.Contains(s) || (bool)r.RequestClients.FirstOrDefault().LastName.Contains(s)).ToList();
            }

            if (reqtype != 0)
            {
                query = query.Where(r => r.RequestTypeId == reqtype).ToList();
            }

            if (regid != 0)
            {
                query = query.Where(r => r.RequestClients.FirstOrDefault().RegionId == regid).ToList();

            }

            return query;
        }

        public void editadminprofile(Profile model, int admin)
        {
            Admin a = _Repository.getadminbyadminid(admin);
            if (model.AdminData != null)
            {
                a.FirstName = model.AdminData.FirstName;
                a.LastName = model.AdminData.LastName;
                a.Email = model.AdminData.Email;
                a.Mobile = model.AdminData.Mobile;
                _Repository.updateadmintbl(a);
            }
            if (model.SelectedRegions?.Count > 0)
            {
                _Repository.deletereg(admin);
                foreach (var ritem in model.SelectedRegions)
                {
                    _Repository.AddRegionbyid(ritem, admin);
                }
            }
        }

        public void editadminp(Profile model, int admin)
        {
            Admin a = _Repository.getadminbyadminid(admin);
            a.Address1 = model.AdminData.Address1;
            a.Address2 = model.AdminData.Address2;
            a.City = model.AdminData.City;
            a.Zip = model.AdminData.Zip;
            a.Mobile = model.AdminData.Mobile;
            a.RegionId = _Repository.GetRegionid(model.State);
            _Repository.updateadmintbl(a);
        }

        public string Adminname(int admin1)
        {
            return _Repository.Adminname(admin1);
        }

        public void DeleteCustom(int[] filenames)
        {
            foreach (var item in filenames)
            {
                var rwf = _Repository.GetDocumentFile(item);
                rwf.IsDeleted = new System.Collections.BitArray(1, true);
                _Repository.update_RequestWiseTable(rwf);
            }
        }

        public void reset(Profile model, int a)
        {
            Admin ad = _Repository.getadminbyadminid(a);
            AspNetUser asp = _Repository.getuserbyaspid(ad.AspNetUserId);

            if (asp.Id > 0)
            {
                asp.PasswordHash = Crypto.HashPassword(model.user.PasswordHash);
                _Repository.UpdateAsp(asp);
            }
        }

        public Provider GetRegions()
        {
            var p = _Repository.GetRegion();
            Provider pr = new()
            {
                regions = p,
            };
            return pr;
        }

        public List<Provider> Getphysician(int region)
        {
            List<Physician> p = new();
            List<Provider> providers = new();
            p = _Repository.getphysician();
            //Role roles = new();
            //roles = _Repository.Getrolebyroleid(p.FirstOrDefault().RoleId);

            if (region > 0)
            {
                p = _Repository.GetPhysiciansByRegion(region);
            }

            //Provider pr = new()
            //{

            //    physicians = p,

            //};
            foreach (var item in p)
            {
                bool ischeck;

                PhysicianNotification phynoti = _Repository.phynoti(item.PhysicianId);
                if (phynoti.PhysicianId > 0)
                {
                    ischeck = phynoti.IsNotificationStopped;
                }
                else
                {

                    ischeck = false;

                }

                providers.Add(new Provider
                {
                    PhyId = item.PhysicianId,
                    Name = item.FirstName + " " + item.LastName,
                    Role = _Repository.Getrolebyroleid(item.RoleId),
                    Status = item.Status,
                    Notification = ischeck,
                });

            }
            return providers;

        }

        public List<PhysicianLocation> ProviderLocation()
        {
            return _Repository.GetPhyLocation();
        }

        public void ChangeToggle(int phyid)
        {
            PhysicianNotification pl = _Repository.phynoti(phyid);

            if (pl.Id > 0)
            {
                pl.IsNotificationStopped = !pl.IsNotificationStopped;
                //Update
                _Repository.updatephynoti(pl);
            }
            else
            {
                PhysicianNotification p = new()
                {
                    PhysicianId = phyid,
                    IsNotificationStopped = true,
                };
                //Add p
                _Repository.addPhynoti(p);
            }

        }

        public void Sendit(int id, ModalData md, int admin1)
        {
            Physician p = _Repository.GetPhysician(id);
            if (p != null && md.MessageType == 2)
            {
                var receiver = p.Email;
                var subject = "Send Link";
                var message = "Tap on Message which Admin wants you to send : " + md.note;


                var mail = "tatva.dotnet.binalmalaviya@outlook.com";
                var password = "binal@2002";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message)
                {
                    IsBodyHtml = true
                });

                EmailLog emailLog = new()
                {
                    EmailTemplate = message,
                    SubjectName = subject,
                    SentTries = 1,
                    IsEmailSent = true,
                    EmailId = p.Email,
                    PhysicianId = p.PhysicianId,
                    CreateDate = DateTime.Now,
                    SentDate = DateTime.Now,

                };
                _Repository.AddEmaillogtbl(emailLog);
            }
            else if (p != null && md.MessageType == 1)
            {

                var accountSid = _configuration["Twilio:accountSid"];
                var authToken = _configuration["Twilio:authToken"];
                var twilionumber = _configuration["Twilio:twilioNumber"];


                var messageBody = $"Tap on Message which Admin wants you to send :  " + md.note + "\n\n\nRegards,\nHalloDoc";

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    from: new Twilio.Types.PhoneNumber(twilionumber),
                    body: messageBody,
                    to: new Twilio.Types.PhoneNumber("+91" + "9081818576")
                );


                Smslog smslog = new()
                {
                    Smstemplate = messageBody,
                    MobileNumber = p.Mobile,
                    CreateDate = DateTime.Now,
                    SentDate = DateTime.Now,
                    SentTries = 1,
                    IsSmssent = true,
                    PhysicianId = p.PhysicianId,
                };
                _Repository.SmsLogtbl(smslog);
            }

            else
            {
                var receiver = p.Email;
                var subject = "Send Link";
                var message = "Tap on Message which Admin wants you to send : " + md.note;


                var mail = "tatva.dotnet.binalmalaviya@outlook.com";
                var password = "binal@2002";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message)
                {
                    IsBodyHtml = true
                });

                EmailLog emailLog = new()
                {
                    EmailTemplate = message,
                    SubjectName = subject,
                    SentTries = 1,
                    IsEmailSent = true,
                    EmailId = p.Email,
                    PhysicianId = p.PhysicianId,
                    CreateDate = DateTime.Now,
                    SentDate = DateTime.Now,

                };
                _Repository.AddEmaillogtbl(emailLog);

                var accountSid = _configuration["Twilio:accountSid"];
                var authToken = _configuration["Twilio:authToken"];
                var twilionumber = _configuration["Twilio:twilioNumber"];


                var messageBody = $"\"Tap on Message which Admin wants you to send : \" + md.note\n\n\nRegards,\nHalloDoc";

                TwilioClient.Init(accountSid, authToken);

                var message1 = MessageResource.Create(
                    from: new Twilio.Types.PhoneNumber(twilionumber),
                    body: messageBody,
                    to: new Twilio.Types.PhoneNumber("+91" + "9081818576")
                );


                Smslog smslog = new()
                {
                    Smstemplate = messageBody,
                    MobileNumber = p.Mobile,
                    CreateDate = DateTime.Now,
                    SentDate = DateTime.Now,
                    SentTries = 1,
                    IsSmssent = true,
                    PhysicianId = p.PhysicianId,
                };
                _Repository.SmsLogtbl(smslog);

            }
        }

        public List<Region> getreg()
        {
            var t = _Repository.GetReg();
            return t;

        }

        public List<Role> getrole()
        {
            var y = _Repository.getrole();
            return y;
        }

        public void CreateProvider(CreatePhy model, int admin1)
        {

            AspNetUser asp = new AspNetUser()
            {
                UserName = "MD." + model.Firstname + "." + model.Lastname,
                PasswordHash = Crypto.HashPassword(model.Password),
                Email = model.email,
                PhoneNumber = model.phone,
                CreatedDate = DateTime.Now,
                Roles = _Repository.PhycianRoles(),
            };
            _Repository.AddAspnetUser(asp);


            Physician phy = new Physician()
            {
                AspNetUserId = asp.Id,
                FirstName = model.Firstname,
                LastName = model.Lastname,
                Email = model.email,
                Mobile = model.phone,
                MedicalLicense = model.medicallicence,

                AdminNotes = model.Adminnote,

                IsAgreementDoc = model.AgreementDoc != null ? new BitArray(1, true) : new BitArray(1, false),
                IsBackgroundDoc = model.BackgroundDoc != null ? new BitArray(1, true) : new BitArray(1, false),
                IsTrainingDoc = model.HIPAA != null ? new BitArray(1, true) : new BitArray(1, false),
                IsNonDisclosureDoc = model.NonDisclosureDoc != null ? new BitArray(1, true) : new BitArray(1, false),
                IsLicenseDoc = model.LicenseDoc != null ? new BitArray(1, true) : new BitArray(1, false),
                Photo = model?.Photo?.FileName ?? null,
                Address1 = model.address1,
                Address2 = model.address2,
                City = model.city,
                RegionId = model.SelectedStateId,
                Zip = model.zipcode,
                AltPhone = model.alterphone,
                CreatedBy = _Repository.getAspid(admin1),
                CreatedDate = DateTime.Now,
                Status = 1,
                BusinessName = model.Businessname,
                BusinessWebsite = model.Businesswebsite,

                RoleId = model.SelectedRoleId,
                Npinumber = model.npi,
                //licence
                //signature
                //iscredential
                //istokengenerate
                //syncemail
            };
            _Repository.AddPhysician(phy);

            foreach (var item in model.SelectedRegions)
            {
                PhysicianRegion reg = new()
                {
                    PhysicianId = phy.PhysicianId,
                    RegionId = item,
                };
                _Repository.AddPhysicianRegion(reg);

            }

            PhysicianLocation pl = new()
            {
                PhysicianId = phy.PhysicianId,
                Latitude = model?.lat ?? 0,
                Longitude = model?.log ?? 0,
                CreatedDate = DateTime.Now,
                PhysicianName = phy.FirstName,
                Address = model.address1
            };
            _Repository.AddPhyLocation(pl);

            if (model.Photo != null)
            {
                string filename = "Photo" + Path.GetExtension(model.Photo?.FileName);
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\PhysicianDoc\\" + phy.PhysicianId + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                model.Photo?.CopyTo(stream);
            }
            if (model.HIPAA != null)
            {
                string filename = "HIPAA" + Path.GetExtension(model.HIPAA?.FileName);
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\PhysicianDoc\\" + phy.PhysicianId + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                model.HIPAA?.CopyTo(stream);
            }
            if (model.AgreementDoc != null)
            {
                string filename = "AgreementDoc" + Path.GetExtension(model.AgreementDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\PhysicianDoc\\" + phy.PhysicianId + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                model.AgreementDoc?.CopyTo(stream);
            }
            if (model.BackgroundDoc != null)
            {
                string filename = "BackgroundDoc" + Path.GetExtension(model.BackgroundDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\PhysicianDoc\\" + phy.PhysicianId + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                model.BackgroundDoc?.CopyTo(stream);
            }
            if (model.NonDisclosureDoc != null)
            {
                string filename = "NonDisclosureDoc" + Path.GetExtension(model.NonDisclosureDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\PhysicianDoc\\" + phy.PhysicianId + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                model.NonDisclosureDoc?.CopyTo(stream);
            }



        }

        public RoleModel getAccess()
        {
            var a = _Repository.getroletbl();
            RoleModel roleModel = new RoleModel()
            {
                rolesofphy = a
            };
            return roleModel;
        }

        public RoleModel GetMenutbl(int value)
        {
            List<Menu> menus = _Repository.getmenutbl(value);
            RoleModel rm = new RoleModel();
            rm.menu = menus;
            rm.SelectedRole = value;

            return rm;
        }

        public void AssignRole(string roleName, string[] selectedRoles, int check, int admin1)
        {
            if (check == 3)
            {
                Role role1 = new Role();
                role1.Name = roleName;
                role1.CreatedDate = DateTime.Now;
                role1.AccountType = (short)check;
                role1.CreatedBy = _Repository.getAspid(admin1).ToString();
                role1.IsDeleted = new BitArray(1, false);
                _Repository.AddRoletbl(role1);
            }
            else
            {
                var roles = selectedRoles[0].Split(',');
                Role role = new Role();
                role.Name = roleName;
                role.CreatedDate = DateTime.Now;
                role.AccountType = (short)check;
                role.CreatedBy = _Repository.getAspid(admin1).ToString();
                role.IsDeleted = new BitArray(1, false);
                _Repository.AddRoletbl(role);

                foreach (string item in roles)
                {
                    RoleMenu rolemenu = new RoleMenu();
                    rolemenu.RoleId = role.RoleId;
                    rolemenu.MenuId = Int32.Parse(item);
                    _Repository.AddRoleMenutbl(rolemenu);
                }
            }


        }

        void IAdmin_Service.UpdateRole(RoleModel model)
        {
            _Repository.RemoveRoleMenu(model.RoleId);
            foreach (var item in model.RoleIds)
            {
                RoleMenu rolemenu = new RoleMenu();
                rolemenu.RoleId = model.RoleId;
                rolemenu.MenuId = item;
                _Repository.AddRoleMenu(rolemenu);
            }
        }

        public RoleModel GetRolewiseData(int id)
        {
            var role = _Repository.GetDataFromRoles(id);
            RoleModel model = new RoleModel();
            model.rolemenus = _Repository.GetDataFromRoleMenu(id);
            model.menu = _Repository.GetMenuDataWithCheckwise(role.AccountType);
            model.RoleName = role.Name;
            model.RoleId = role.RoleId;
            model.SelectedRole = role.AccountType;
            return model;
        }

        public CreatePhy getphysiciandata(int id)
        {
            var p = _Repository.getphycian(id);
            var a = _Repository.GetAspNetUser(p.AspNetUserId ?? 0);
            CreatePhy cp = new()
            {
                id = id,
                Firstname = p.FirstName,
                Lastname = p.LastName,
                email = p.Email,
                phone = p.Mobile,
                medicallicence = p.MedicalLicense,
                npi = p.Npinumber,
                address1 = p.Address1,
                address2 = p.Address2,
                city = p.City,
                SelectedStateId = p.RegionId,
                SelectedRoleId = p.RoleId,
                zipcode = p.Zip,
                alterphone = p.AltPhone,
                Businessname = p.BusinessName,
                Businesswebsite = p.BusinessWebsite,
                Adminnote = p.AdminNotes,
                Username = a.UserName,
                Password = a.PasswordHash,
                roles = _Repository.getrole(),
                reg = _Repository.GetReg(),
                pic = p.Photo,
                SignatureCheck = p.Signature,
                SelectedRegions = _Repository.GetSelectedPhyReg(id).Select(x => x.RegionId).ToList(),
                isagreement = p.IsAgreementDoc == null ? false : p.IsAgreementDoc[0],
                isbackground = p.IsBackgroundDoc == null ? false : p.IsBackgroundDoc[0],
                ishippa = p.IsTrainingDoc == null ? false : p.IsTrainingDoc[0],
                isnonclosure = p.IsNonDisclosureDoc == null ? false : p.IsNonDisclosureDoc[0],
                islisence = p.IsLicenseDoc == null ? false : p.IsLicenseDoc[0],
            };
            return cp;

        }

        public void EditPhyInfo(int id, CreatePhy model)
        {
            var p = _Repository.getphycian(id);
            var a = _Repository.GetAspNetUser(p.AspNetUserId ?? 0);
            p.FirstName = model.Firstname;
            p.LastName = model.Lastname;
            p.Email = model.email;
            p.Mobile = model.phone;
            p.MedicalLicense = model.medicallicence;
            p.Npinumber = model.npi;
            p.SyncEmailAddress = model.syncemail;
            _Repository.UpdatePhytbl(p);

            a.UserName = "MD." + model.Firstname + "." + model.Lastname;
            a.Email = model.email;
            a.PhoneNumber = model.phone;
            _Repository.UpdateAsp(a);

            List<PhysicianRegion> physicianRegions = model.SelectedRegions.Select(x => new PhysicianRegion()
            {
                PhysicianId = p.PhysicianId,
                RegionId = x
            }).ToList();
            _Repository.RemovePhyRegion(p.PhysicianId);
            _Repository.AddPhyRegions(physicianRegions);
        }

        public void EditPhyMailBillInfo(int id, CreatePhy model)
        {
            var p = _Repository.getphycian(id);
            p.Address1 = model.address1;
            p.Address2 = model.address2;
            p.City = model.city;
            p.RegionId = model.SelectedStateId;
            p.Zip = model.zipcode;
            p.AltPhone = model.alterphone;
            _Repository.UpdatePhytbl(p);

        }

        public void EditPhyProvider(int id, CreatePhy model)
        {
            var p = _Repository.getphycian(id);
            p.BusinessName = model.Businessname;
            p.BusinessWebsite = model.Businesswebsite;
            p.AdminNotes = model.Adminnote;
            if (model.Photo != null)
            {
            p.Photo = model.Photo.FileName;

            }
            if(model.Signature != null)
            {

            p.Signature = model.Signature.FileName;
            }
            if (model.Photo != null)
            {
                string filename = "Photo" + Path.GetExtension(model.Photo?.FileName);
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\PhysicianDoc\\" + p.PhysicianId + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                model.Photo?.CopyTo(stream);
            }
            if (model.Signature != null)
            {
                string filename = "Signature" + Path.GetExtension(model.Signature?.FileName);
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\PhysicianDoc\\" + p.PhysicianId + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                model.Signature?.CopyTo(stream);
            }
            _Repository.UpdatePhytbl(p);

        }

        public void EditPhyDocs(int id, CreatePhy model)
        {
            var p = _Repository.getphycian(id);
            p.IsAgreementDoc = p.IsAgreementDoc[0] == true ? p.IsAgreementDoc : model.AgreementDoc != null ? new BitArray(1, true) : new BitArray(1, false);
            p.IsBackgroundDoc = p.IsBackgroundDoc[0] == true ? p.IsBackgroundDoc : model.BackgroundDoc != null ? new BitArray(1, true) : new BitArray(1, false);
            p.IsTrainingDoc = p.IsTrainingDoc[0] == true ? p.IsTrainingDoc : model.HIPAA != null ? new BitArray(1, true) : new BitArray(1, false);
            p.IsNonDisclosureDoc = p.IsNonDisclosureDoc[0] == true ? p.IsNonDisclosureDoc : model.NonDisclosureDoc != null ? new BitArray(1, true) : new BitArray(1, false);
            p.IsLicenseDoc = p.IsLicenseDoc[0] == true ? p.IsLicenseDoc : model.LicenseDoc != null ? new BitArray(1, true) : new BitArray(1, false);
            _Repository.UpdatePhytbl(p);

            if (model.HIPAA != null)
            {
                string filename = "HIPAA" + Path.GetExtension(model.HIPAA?.FileName);
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\PhysicianDoc\\" + p.PhysicianId + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                model.HIPAA?.CopyTo(stream);
            }
            if (model.AgreementDoc != null)
            {
                string filename = "AgreementDoc" + Path.GetExtension(model.AgreementDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\PhysicianDoc\\" + p.PhysicianId + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                model.AgreementDoc?.CopyTo(stream);
            }
            if (model.BackgroundDoc != null)
            {
                string filename = "BackgroundDoc" + Path.GetExtension(model.BackgroundDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\PhysicianDoc\\" + p.PhysicianId + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                model.BackgroundDoc?.CopyTo(stream);
            }
            if (model.NonDisclosureDoc != null)
            {
                string filename = "NonDisclosureDoc" + Path.GetExtension(model.NonDisclosureDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\PhysicianDoc\\" + p.PhysicianId + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                model.NonDisclosureDoc?.CopyTo(stream);
            }
            if (model.LicenseDoc != null)
            {
                string filename = "LicenseDoc" + Path.GetExtension(model.LicenseDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\PhysicianDoc\\" + p.PhysicianId + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                model.LicenseDoc?.CopyTo(stream);
            }
        }

        public void DeletePhy(int id)
        {
            var p = _Repository.getphycian(id);
            p.IsDeleted = new BitArray(1, true);
            _Repository.UpdatePhytbl(p);
        }

        public void DeleteRoles(int id)
        {
            var role = _Repository.GetDataFromRoles(id);
            role.IsDeleted = new BitArray(1, true);
            _Repository.UpdateRoletbl(role);
        }

        public List<UserAccess> GetUserAccessData(int region)
        {
            //return _Repository.GetAdminAndPhysicianData(region);

            List<AspNetUser> asp = _Repository.getallusers();

            if (region != 0)
            {
                asp = asp.Where(x => x.Roles.Where(x => x.Id == region).Any()).ToList();
            }

            List<UserAccess> ua = new();

            foreach (var user in asp)
            {
                ua.Add(new()
                {
                    Username = user.AdminAspNetUsers.Count != 0 ? user.AdminAspNetUsers.First().FirstName + " " + user.AdminAspNetUsers.First().LastName :
                    user.PhysicianAspNetUsers.Count != 0 ? user.PhysicianAspNetUsers.First().FirstName + " " + user.PhysicianAspNetUsers.First().LastName : "",
                    Phonenumber = user.AdminAspNetUsers.Count != 0 ? user.AdminAspNetUsers.FirstOrDefault()?.Mobile : user.PhysicianAspNetUsers.Count != 0 ? user.PhysicianAspNetUsers.FirstOrDefault()?.Mobile : "",
                    accountType = user.Roles.First().Name,
                    Status = user.AdminAspNetUsers.Count != 0 ? user.AdminAspNetUsers.First().Status : user.PhysicianAspNetUsers.Count != 0 ? user.PhysicianAspNetUsers.First().Status : 0,
                    phyid = user.Roles.First().Name == "Admin" ? _Repository.GetAdminByasp(user.Id) : _Repository.GetPhyByAsp(user.Id),
                    openrequest = user.PhysicianAspNetUsers.Count != 0 ? user.PhysicianAspNetUsers.First().Requests.Where(x=>x.Status<8).Count():_Repository.GetRequest().Count(X=>X.Status<=8),
                });
            }
            return ua;
        }

        public List<Role> GetRoleOfAdmin()
        {
            var a = _Repository.GetAminRoles();
            return a;
        }

        public void CreateAdmin(CreateAdmin model, int admin1)
        {

            AspNetUser asp = new AspNetUser()
            {
                UserName = model.Firstname + " " + model.Lastname,
                PasswordHash = Crypto.HashPassword(model.Password),
                Email = model.email,
                PhoneNumber = model.phone,
                CreatedDate = DateTime.Now,
                Roles = _Repository.AdminRoles(),
            };
            _Repository.AddAspnetUser(asp);

            Admin AD = new Admin()
            {
                AspNetUserId = asp.Id,
                FirstName = model.Firstname,
                LastName = model.Lastname,
                Email = model.email,
                Mobile = model.phone,
                Address1 = model.address1,
                Address2 = model.address2,
                City = model.city,
                RegionId = model.SelectedStateId,
                Zip = model.zipcode,
                AltPhone = model.alterphone,
                CreatedBy = _Repository.getAspid(admin1),
                CreatedDate = DateTime.Now,
                Status = 1,
                RoleId = model.SelectedRoleId
            };
            _Repository.AddAdmintbl(AD);

            foreach (var item in model.SelectedRegions)
            {
                AdminRegion reg = new()
                {
                    AdminId = AD.AdminId,
                    RegionId = item,
                };
                _Repository.AddAdminRegiontbl(reg);

            }


        }

        public List<HealthProfessionalType> GetProfession()
        {
            var professions = _Repository.GetProfession();
            return professions;
        }

        public List<Partnersdata> GetAllHealthProfessionaldata(int p, string search)
        {
            List<HealthProfessional> hp = new();

            hp = _Repository.GetHealthProfession();


            if (p != 0)
            {
                hp = _Repository.GetHealthProfessionByProfession(p);
            }
            if (search != null)
            {
                hp = hp.Where(r => r.VendorName.ToLower().Contains(search.ToLower())).ToList();
            }

            List<Partnersdata> data = new List<Partnersdata>();
            hp.ForEach(item =>
            {
                data.Add(new Partnersdata()
                {
                    VendorId = item.VendorId,
                    VendorName = item.VendorName,
                    ProfessionName = _Repository.Profession(item.Profession),
                    VendorEmail = item.Email,
                    FaxNo = item.FaxNumber,
                    PhoneNo = item.PhoneNumber,
                    Businesscontact = item.BusinessContact,
                });
            });
            return data;

        }

        public void AddBusiness(PartnersCM model)
        {



            HealthProfessional hp = new HealthProfessional()
            {
                VendorName = model.BusinessName,
                FaxNumber = model.FAXNumber,
                PhoneNumber = model.Phonenumber,
                Email = model.Email,
                BusinessContact = model.BusinessContact,
                State = _Repository.GetRegionname(model.RegionId),
                Zip = model.Zip,
                City = model.City,
                Address = model.Street,
                CreatedDate = DateTime.Now,
                Profession = model.SelectedhealthprofID,
                RegionId = model.RegionId,

            };

            _Repository.AddHealthProfessiontbl(hp);


        }

        public PartnersCM GetPartnerData(int vendorid)
        {
            PartnersCM model = new();
            HealthProfessional hp = _Repository.GetData(vendorid);
            model.BusinessName = hp.VendorName;
            model.FAXNumber = hp.FaxNumber;
            model.BusinessContact = hp.BusinessContact;
            model.Street = hp.Address;
            model.City = hp.City;
            model.Email = hp.Email;
            model.Zip = hp.Zip;
            model.RegionId = (Int32)hp.RegionId;
            model.regions = _Repository.GetReg();
            model.Professions = _Repository.GetProfession();
            model.SelectedhealthprofID = hp.Profession ?? 0;
            model.Phonenumber = hp.PhoneNumber;
            model.partnersdatas.Add(new Partnersdata()
            {
                VendorId = hp.VendorId
            });
            return model;
        }

        public void EditPartner(PartnersCM model, int vendorid)
        {
            HealthProfessional hp1 = _Repository.GetData(vendorid);

            hp1.VendorName = model.BusinessName;
            hp1.FaxNumber = model.FAXNumber;
            hp1.PhoneNumber = model.Phonenumber;
            hp1.Email = model.Email;
            hp1.BusinessContact = model.BusinessContact;
            hp1.State = _Repository.GetRegionname(model.RegionId);
            hp1.Zip = model.Zip;
            hp1.City = model.City;
            hp1.Address = model.Street;
            hp1.CreatedDate = DateTime.Now;
            hp1.Profession = model.SelectedhealthprofID;
            hp1.RegionId = model.RegionId;


            _Repository.UpdateHealthProfessiontbl(hp1);

        }

        public void DeletePartner(int id)
        {
            HealthProfessional hp1 = _Repository.GetData(id);
            hp1.IsDeleted = new BitArray(1, true);

            _Repository.UpdateHealthProfessiontbl(hp1);
        }
        public bool CreateShift(CreateShift shift, int admin)
        {
            Physician data = _Repository.DayDatabyPhysician(shift.SelectedPhysicianId);
            bool flag = true;
            //if((shift.Start.Hour==0 && shift.Start.Minute==0 && shift.End.Hour==0 && shift.End.Minute==0) || (shift.Start.Hour >= shift.End.Hour))
            //{
            //    flag = false;
            //}
            foreach (var item in data.Shifts)
            {
                foreach (var item2 in item.ShiftDetails)
                {
                    DateOnly date = DateOnly.FromDateTime(item2.ShiftDate);
                    DateOnly date2 = DateOnly.FromDateTime(shift.ShiftDate);

                    if (date == date2 && shift.Start.Hour < item2.EndTime.Hour && shift.End.Hour > item2.StartTime.Hour)
                    {
                        flag = false;
                    }

                }
            }
            if (!flag)
            {
                return false;
            }
            Shift s = new()
            {
                PhysicianId = shift.SelectedPhysicianId ?? 0,
                StartDate = DateOnly.FromDateTime(shift.ShiftDate),
                IsRepeat = shift.RepeatToggle,
                RepeatUpto = shift.Repeat,
                CreatedBy = _Repository.getAspid(admin),
                CreatedDate = DateTime.Now
            };
            if (shift.Weekday != null)
            {
                string days = "0000000";
                StringBuilder daysofweek = new(days);
                foreach (var i in shift.Weekday)
                {
                    daysofweek[i] = '1';
                }
                s.WeekDays = daysofweek.ToString();
            }

            _Repository.AddShifttbl(s);

            ShiftDetail detail = new()
            {
                ShiftId = s.ShiftId,
                ShiftDate = shift.ShiftDate,
                RegionId = shift.SelectedRegionId,
                StartTime = shift.Start,
                EndTime = shift.End,
                Status = 0,//peniding=0 and approve = 1
                IsDeleted = new BitArray(1, false),
            };

            _Repository.AddShiftDetails(detail);



            ShiftDetailRegion shiftRegion = new()
            {
                ShiftDetailId = detail.ShiftDetailId,
                RegionId = shift.SelectedRegionId,
                IsDeleted = new BitArray(1, false),
            };
            _Repository.AddShiftRegion(shiftRegion);


            int currentday = (int)shift.ShiftDate.DayOfWeek;

            while (shift.Repeat != 0 && s.WeekDays != null)
            {
                for (var i = 0; i < 7; i++)
                {
                    if (s.WeekDays[i] == '1')
                    {
                        int toAdd = i - currentday;
                        if (toAdd < 0) { toAdd += 7; }

                        ShiftDetail detail1 = new()
                        {
                            ShiftId = s.ShiftId,
                            ShiftDate = shift.ShiftDate.AddDays(toAdd),
                            RegionId = shift.SelectedRegionId,
                            StartTime = shift.Start,
                            EndTime = shift.End,
                            Status = 0,
                            IsDeleted = new BitArray(1, false),
                        };


                        bool flag2 = true;
                        foreach (var item in data.Shifts)
                        {
                            foreach (var item2 in item.ShiftDetails)
                            {
                                DateOnly date = DateOnly.FromDateTime(item2.ShiftDate);
                                DateOnly date2 = DateOnly.FromDateTime(detail1.ShiftDate);

                                if (date == date2 && detail1.StartTime.Hour < item2.EndTime.Hour && detail1.EndTime.Hour > item2.StartTime.Hour)
                                {
                                    flag2 = false;
                                }

                            }
                        }
                        if (!flag2)
                        {
                            return false;
                        }


                        _Repository.AddShiftDetails(detail1);

                        ShiftDetailRegion shiftRegion1 = new()
                        {
                            ShiftDetailId = detail1.ShiftDetailId,
                            RegionId = shift.SelectedRegionId,
                            IsDeleted = new BitArray(1, false),
                        };
                        _Repository.AddShiftRegion(shiftRegion1);
                    }
                }
                shift.Repeat--;
                shift.ShiftDate = shift.ShiftDate.AddDays(7);
            };
            return true;
        }

        public List<Physician> GetPhyTbl()
        {
            return _Repository.getphysician();
        }

        public List<Scheduling> GetDayWiseData(int day, int month, int year, int region)
        {
            List<Physician> data = _Repository.DayData();
            List<Scheduling> schedulings = new List<Scheduling>();

            DateTime date = day == 0 ? new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) : new(year, month, day);

            if (region != 0)
            {
                data = data.Where(x => x.RegionId == region).ToList();
            }

            if (data.Count != 0)
            {
                foreach (var item in data)
                {
                    Scheduling s = new()
                    {
                        Physicians = item,
                    };
                    if (item.Shifts.Count > 0)
                    {
                        foreach (var shifts in item.Shifts)
                        {
                            foreach (var shiftdetails in shifts.ShiftDetails)
                            {
                                if (shiftdetails.ShiftDate == date)
                                {
                                    ShiftDetail detail = new()
                                    {
                                        ShiftDetailId = shifts.ShiftDetails.First(X => X.ShiftDate == date).ShiftDetailId,
                                        StartTime = shifts.ShiftDetails.First(X => X.ShiftDate == date).StartTime,
                                        EndTime = shifts.ShiftDetails.First(X => X.ShiftDate == date).EndTime,
                                        ShiftDate = shifts.ShiftDetails.First(X => X.ShiftDate == date).ShiftDate,
                                        Status = shifts.ShiftDetails.First(X => X.ShiftDate == date).Status,
                                    };
                                    s.shifts.Add(detail);
                                    
                                }
                            }

                        }
                    }
                    schedulings.Add(s);
                    schedulings.FirstOrDefault().CurrentDate = date;

                }
            }

            if (schedulings.Count == 0)
            {
                schedulings.Add(new()
                {
                    CurrentDate = date
                });
            }
            return schedulings;
        }

        public List<Scheduling>? GetWeekWiseData(int day, int month, int year)
        {
            List<Physician> data = _Repository.DayData();
            List<Scheduling> schedulings = new List<Scheduling>();
            DateTime date = day == 0 ? new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) : new(year, month, day);
            date = date.AddDays(-(int)date.DayOfWeek);

            foreach (var item in data)
            {
                Scheduling s = new()
                {
                    Physicians = item,

                };
                if (item.Shifts.Count > 0)
                {
                    foreach (var shifts in item.Shifts)
                    {
                        foreach (var shiftDetail in shifts.ShiftDetails)
                        {

                            if ((shiftDetail.ShiftDate - date).TotalDays < 7 && (shiftDetail.ShiftDate - date).TotalDays >= 0) //  Any(x => (x.ShiftDate - date).TotalDays<7))
                            {
                                ShiftDetail detail = new()
                                {
                                    ShiftDetailId = shiftDetail.ShiftDetailId,
                                    StartTime = shiftDetail.StartTime,
                                    EndTime = shiftDetail.EndTime,
                                    ShiftDate = shiftDetail.ShiftDate,
                                };
                                s.shifts.Add(detail);
                            }
                        }
                    }
                }

                s.shifts = s.shifts.GroupBy(x => x.ShiftDate).Select(x => new ShiftDetail
                {
                    ShiftDate = x.Key,
                    ShiftDetailId = x.Sum(x => (x.EndTime.Hour - x.StartTime.Hour)),
                }).ToList();

                schedulings.Add(s);
                schedulings.FirstOrDefault().CurrentDate = date;

            }
            return schedulings;

        }

        public List<Scheduling>? GetMonthWiseData(int day, int month, int year)
        {
            List<Physician> data = _Repository.DayData();
            List<Scheduling> schedulings = new();

            DateTime date = day == 0 ? new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) : new(year, month, day);

            foreach (var item in data)
            {
                Scheduling s = new()
                {
                    Physicians = item,
                };
                if (item.Shifts.Count > 0)
                {
                    foreach (var shifts in item.Shifts)
                    {
                        foreach (var detail in shifts.ShiftDetails)
                        {
                            if (detail.ShiftDate.Month == date.Month && detail.ShiftDate.Year == date.Year)
                            {
                                ShiftDetail detail1 = new()
                                {
                                    ShiftDetailId = detail.ShiftDetailId,
                                    StartTime = detail.StartTime,
                                    EndTime = detail.EndTime,
                                    ShiftDate = detail.ShiftDate,
                                    Status = detail.Status,
                                };
                                s.shifts.Add(detail1);
                            }

                        }
                    }
                }
                schedulings.Add(s);
                schedulings.FirstOrDefault().CurrentDate = date;

            }
            return schedulings;

        }

        public List<ShiftReview>? GetPendingShiftData(int region)
        {


            List<Physician> physicians = _Repository.DayDataPending();
            List<ShiftReview> shiftReviews = new();
            if (region != 0)
            {
                physicians = physicians.Where(x => x.RegionId == region).ToList();
            };
            foreach (var item in physicians)
            {
                if (item.Shifts.Count > 0)
                {
                    foreach (var shifts in item.Shifts)
                    {
                        foreach (var detail in shifts.ShiftDetails)
                        {

                            ShiftReview sr = new()
                            {
                                ShiftDate = detail.ShiftDate,
                                StartTime = detail.StartTime,
                                EndTime = detail.EndTime,
                                ProviderName = item.FirstName,
                                Region = _Repository.GetRegionname(item.RegionId),
                                shiftdetailid = detail.ShiftDetailId,
                                regions = _Repository.GetRegion(),

                            };
                            shiftReviews.Add(sr);
                        }
                    }
                }
            }

            return shiftReviews;
        }

        public void ApproveSelectedShift(int[] shiftDetailsId, int admin1)
        {
            foreach (var shiftId in shiftDetailsId)
            {
                var shift = _Repository.ChangeShift(shiftId);
                shift.Status = 1;
                shift.ModifiedDate = DateTime.Now;
                shift.ModifiedBy = _Repository.getAspid(admin1);
            }
            _Repository.UpdateShiftDetails();
        }


        public void DeleteShiftReview(int[] shiftDetailsId, int admin1)
        {
            foreach (var shiftId in shiftDetailsId)
            {
                var shift = _Repository.Shiftdetials(shiftId);
                shift.IsDeleted = new BitArray(1, true);
                shift.ModifiedDate = DateTime.Now;
                shift.ModifiedBy = _Repository.getAspid(admin1);

            }
            _Repository.UpdateShiftDetails();
        }

        public OnCallModal GetOnCallDetails(int regionId)
        {
            var currentTime = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute);
            BitArray deletedBit = new BitArray(new[] { false });

            var onDutyQuery = _Repository.onduty(regionId, currentTime, deletedBit);


            var offDutyQuery = _Repository.offduty(regionId, currentTime, deletedBit);
            var onCallModal = new OnCallModal
            {
                OnCall = onDutyQuery,
                OffDuty = offDutyQuery,
                regions = _Repository.GetRegion(),
            };

            return onCallModal;
        }

        public EditShift EditShift(int shiftdetailid)
        {
            ShiftDetail shiftDetail = _Repository.GetShiftDetails(shiftdetailid);
            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            int Hour = DateTime.Now.Hour;
            bool flag = true;
            if (DateOnly.FromDateTime(shiftDetail.ShiftDate) <= date && (shiftDetail.StartTime.Hour < Hour || DateOnly.FromDateTime(shiftDetail.ShiftDate) < date))
            {
                flag = false;
            }

            EditShift edit = new()
            {
                Regions = _Repository.GetRegion(),
                Physicians = _Repository.getphysician(),
                SelectedRegion = shiftDetail.RegionId ?? 0,
                SelectedPhy = shiftDetail.Shift.PhysicianId,
                ShftDate = shiftDetail.ShiftDate,
                StartTime = shiftDetail.StartTime,
                Status = shiftDetail.Status,
                EndTime = shiftDetail.EndTime,
                ShiftDetailId = shiftdetailid,
                isEditable = flag,
            };
            return edit;



        }

        public void UpdateShift(EditShift editShift, int shiftdetailid, int adminid)
        {
            ShiftDetail shiftDetail = _Repository.GetShiftDetails(shiftdetailid);
            shiftDetail.ShiftDate = editShift.ShftDate;
            shiftDetail.StartTime = editShift.StartTime;
            shiftDetail.EndTime = editShift.EndTime;
            shiftDetail.ModifiedDate = DateTime.Now;
            shiftDetail.ModifiedBy = _Repository.getAspid(adminid);
            _Repository.Update(shiftDetail);
        }

        public int GetAspId(int admin1)
        {
            return _Repository.getAspid(admin1);
        }

        public void ChangeShiftStatus(int shiftdetailid, int adminid)
        {
            ShiftDetail detail = _Repository.GetShiftDetails(shiftdetailid);
            detail.Status = (short)(detail.Status == 1 ? 0 : 1);
            detail.ModifiedBy = _Repository.getAspid(adminid);
            detail.ModifiedDate = DateTime.Now;
            _Repository.Update(detail);
        }

        public void DeleteShiftViaModal(int shiftdetailid, int adminid)
        {
            ShiftDetail detail = _Repository.GetShiftDetails(shiftdetailid);
            detail.IsDeleted = new BitArray(1, true);
            detail.ModifiedBy = _Repository.getAspid(adminid);
            detail.ModifiedDate = DateTime.Now;
            _Repository.Update(detail);
        }

        public List<PatientHistoryTable> PatientHistoryTable(string? fname, string? lname, string? email, string? phone, int page)
        {
            IQueryable<PatientHistoryTable> tabledata = _Repository.GetPatientHistoryTable(fname, lname, email, phone, page);
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
            var count = tabledata.Count();
            int size = 10;
            List<PatientHistoryTable> list = tabledata.Skip(page * size - size).Take(size).ToList();
            if (count != 0)
            {
                foreach (var item in list)
                {
                    item.PgCount = count;
                };
            }

            return list;
        }

        public List<PatientRecord> PatientRecord(int id)
        {
            List<Request> requests = _Repository.GetAllRequestsByAid(id);
            List<PatientRecord> records = new List<PatientRecord>();
            foreach (Request request in requests)
            {
                Physician physician = new Physician();
                if (request.PhysicianId != null)
                {
                    physician = _Repository.getphycian((int)request.PhysicianId);
                }
                PatientRecord record = new PatientRecord
                {
                    rid = request.RequestId,
                    Name = request.FirstName + " " + request.LastName,
                    createdDate = request.CreatedDate.ToString("MMM dd, yyyy"),
                    conNo = request.ConfirmationNumber ?? "-",
                    phyName = physician.FirstName == null && physician.LastName == null ? "-" : "Dr. " + physician.FirstName + " " + physician.LastName,
                    concludeDate = request.Status == 6 && request.ModifiedDate != null ? request.ModifiedDate.Value.ToString("MMM dd, yyyy") : "-",
                    status = _Repository.GetStatus(request.Status) ?? "-",
                    docNo = _Repository.GetNumberOfDocsByRid(request.RequestId),
                };
                records.Add(record);
            }
            return records;
        }

        public List<RequestType>? GetRequestTypes()
        {

            return _Repository.getRequestTypeList();

        }

        public AdminRecord getSearchRecordData(AdminRecord model)
        {
            model.Data = _Repository.getRequestClientList();
            model.ReqNotes = _Repository.getRequestNotesList();
            model.ReqType = _Repository.getRequestTypeList();
            model.phy = _Repository.getphysician();
            return model;
        }

        public void deleteRequest(int id)
        {
            Request r = _Repository.GetRequestById(id);

            r.IsDeleted = new BitArray(1, true);
            r.ModifiedDate = DateTime.Now;
            _Repository.UpdateRequesttbl(r);
        }

        public AdminRecord? getBlockHistoryData()
        {
            AdminRecord model = new();
            model.blockRequests = _Repository.getBlockData();
            return model;
        }

        public void Unblock(int id)
        {
            BlockRequest req = _Repository.getBlockRequestById(id);
            req.IsActive = new BitArray(1, true);
            req.ModifiedDate = DateTime.Now;
            _Repository.updateBlockRequest(req);


            Request r = _Repository.GetRequestById(id);
            r.Status = 1;
            r.ModifiedDate = DateTime.Now;
            _Repository.UpdateRequesttbl(r);



        }

        public List<Emaillogs>? EmailLogs(string name, string email, DateTime createdate, DateTime sentdate, int page)
        {

            List<Emaillogs> logs = _Repository.EmailLogs();

            if (name != null) { logs = logs.Where(x => x.Recipient.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList(); }
            if (email != null) { logs = logs.Where(x => x.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList(); }
            if (createdate != DateTime.MinValue) { logs = logs.Where(x => x.CreatedDate.Date == createdate).ToList(); }
            if (sentdate != DateTime.MinValue) { logs = logs.Where(x => x.SentDate.HasValue && x.SentDate.Value.Date == sentdate).ToList(); }
            if (logs.Count() != 0)
            {
                foreach (Emaillogs log in logs)
                {
                    log.PgCount = logs.Count();
                };
            }
            int size = 10;
            logs = logs.Skip(page * size - size).Take(size).ToList();
            return logs;

        }

        public List<Emaillogs>? SmsLog( string name, string mobile, DateTime createdate, DateTime sentdate, int page)
        {
            List<Emaillogs> logs = _Repository.SmsLogs();

            if (name != null) { logs = logs.Where(x => x.Recipient.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList(); }
            if (mobile != null) { logs = logs.Where(x => x.Mobile.Contains(mobile, StringComparison.OrdinalIgnoreCase)).ToList(); }
            if (createdate != DateTime.MinValue) { logs = logs.Where(x => x.CreatedDate.Date == createdate).ToList(); }
            if (sentdate != DateTime.MinValue) { logs = logs.Where(x => x.SentDate.HasValue && x.SentDate.Value.Date == sentdate).ToList(); }
            if (logs.Count() != 0)
            {
                foreach (Emaillogs log in logs)
                {
                    log.PgCount = logs.Count();
                };
            }
            int size = 10;
            logs = logs.Skip(page * size - size).Take(size).ToList();
            return logs;
        }

        public List<AdminRecord> SearchRecords(string providername, string patientname, int status, int reqtype, string email, string phone, DateTime fromdate, DateTime todate)
        {
            List<AdminRecord> records = _Repository.SearchRecords();

            if (providername != null) { records = records.Where(x => x.ProviderName.Contains(providername, StringComparison.OrdinalIgnoreCase)).ToList(); }
            if (patientname != null) { records = records.Where(x => x.PatientName.Contains(patientname, StringComparison.OrdinalIgnoreCase)).ToList(); }
            if (email != null) { records = records.Where(x => x.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList(); }
            if (phone != null) { records = records.Where(x => x.PhoneNumber.Contains(phone, StringComparison.OrdinalIgnoreCase)).ToList(); }
            if (status != 0) { records = records.Where(x => x.status == status).ToList(); }
            if (reqtype != 0) { records = records.Where(x => x.ReqtypeId == reqtype).ToList(); }

            return records;
        }

        public void SendEmailToOffDutyProvider(ModalData model)
        {
            var a = _Repository.GetOffDuty();

            foreach (var obj in a)
            {
                var receiver = obj.Email;
                var subject = "Approved the Pending Request";
                var message = "Please Approve The Pending Shifts of the Patients";


                var mail = "tatva.dotnet.binalmalaviya@outlook.com";
                var password = "binal@2002";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));


                EmailLog emailLog = new()
                {
                    EmailTemplate = message,
                    SubjectName = subject,
                    SentTries = 1,
                    IsEmailSent = true,
                    EmailId = obj.Email,
                    CreateDate = DateTime.Now,
                    SentDate = DateTime.Now,

                };
                _Repository.AddEmaillogtbl(emailLog);

            }
        }

        public void EditPhyProfile(int id, CreatePhy model)
        {
            var phy = _Repository.getphycian(id);
            var asp = _Repository.GetAspNetUser((int)phy.AspNetUserId);
            if (model.SelectedRoleId != null)
            {
                phy.RoleId = model.SelectedRoleId;
                _Repository.UpdatePhytbl(phy);
            }
            if(model.Password!= null)
            {
                asp.PasswordHash = Crypto.HashPassword(model.Password);
                _Repository.UpdateAsp(asp);
            }
        }

        public void TransferReq(ModalData md, int id, int admin)
        {
            Request req = _Repository.GetRequestById(id);

            req.Status = 1;
            req.AcceptedDate = null;
            req.PhysicianId = md.SelectedPhysicianName;
            req.ModifiedDate = DateTime.Now;
            _Repository.UpdateRequesttbl(req);

            RequestStatusLog reqlog = new()
            {
                RequestId = id,
                Status = 2,
                TransToPhysicianId = md.SelectedPhysicianName,
                AdminId = admin,
                Notes = md.note,
                CreatedDate = DateTime.Now,
            };
            _Repository.AddRequestStatuslog(reqlog);
        }

        public void EditPayrate(int adminid, int phyid, int nightshiftweekend, int shift, int housecallnight, int phoneconsults, int phoneconsultsnight, int batchtesting, int housecall)
        {
            PhysicianPayrate payrate = _Repository.GetPhysicianPayrate(phyid);

            if (payrate.PayrateId > 0)
            {
                payrate.NigthshiftWeekend = nightshiftweekend > 0 ? nightshiftweekend : payrate.NigthshiftWeekend;
                payrate.Shift = shift > 0 ? shift : payrate.Shift;
                payrate.HousecallsNigthsWeekend = housecallnight > 0 ? housecallnight : payrate.HousecallsNigthsWeekend;
                payrate.PhoneConsults = phoneconsults > 0 ? phoneconsults : payrate.PhoneConsults;
                payrate.PhoneConsultsNigthsWeekend = phoneconsultsnight > 0 ? phoneconsultsnight : payrate.PhoneConsultsNigthsWeekend;
                payrate.BatchTesting = batchtesting > 0 ? batchtesting : payrate.BatchTesting;
                payrate.Housecall = housecall > 0 ? housecall : payrate.Housecall;
                payrate.ModifiedBy = _Repository.getAspid(adminid);

                _Repository.UpdateTable(payrate);
            }
            else
            {
                PhysicianPayrate newPayrate = new()
                {
                    PhysicianId = phyid,
                    NigthshiftWeekend = nightshiftweekend,
                    Shift = shift,
                    HousecallsNigthsWeekend = housecallnight,
                    PhoneConsults = phoneconsults,
                    PhoneConsultsNigthsWeekend = phoneconsultsnight,
                    BatchTesting = batchtesting,
                    Housecall = housecall,
                    CreatedBy = _Repository.getAspid(adminid),
                };
                _Repository.AddPayrateTbl(newPayrate);
            }
        }

        public Payrate Payrate(int phyid)
        {
            PhysicianPayrate payrate = _Repository.GetPhysicianPayrate(phyid);
            if (payrate.PayrateId > 0)
            {
                return new()
                {
                    PhysicianId = phyid,
                    NigthshiftWeekend = payrate.NigthshiftWeekend,
                    Shift = payrate.Shift,
                    HousecallsNigthsWeekend = payrate.HousecallsNigthsWeekend,
                    PhoneConsults = payrate.PhoneConsults,
                    PhoneConsultsNigthsWeekend = payrate.PhoneConsultsNigthsWeekend,
                    BatchTesting = payrate.BatchTesting,
                    Housecall = payrate.Housecall,
                };
            }
            return new()
            {
                PhysicianId = phyid,
            };
        }

        public ModalData? Addphy()
        {
            ModalData model = new()
            {
                Physicians = _Repository.getphysician()
            };
            return model;
        }

        public List<TimesheetData>? TimesheetData(DateTime date, int adminid,int phy)
        {
            DateTime start = date;
            DateTime end = new();
            List<TimesheetData> data = new();

            if (date.Day == 1)
            {
                end = date.AddDays(14);
            }
            else
            {
                end = date.AddDays(DateTime.DaysInMonth(date.Year, date.Month) - date.Day);
            }

            List<Timesheet> timesheets = _Repository.TimeSheets(start, end, phy).OrderBy(x => x.SheetDate).ToList();
            if (timesheets.Count > 0)
            {
                foreach (var timesheet in timesheets)
                {
                    data.Add(new TimesheetData()
                    {
                        PhysicianId = phy,
                        InvoiceId = timesheet.InvoiceId,
                        Date = timesheet.SheetDate,
                        OnCallHours = _Repository.ShiftHoursOnDate(phy, timesheet.SheetDate),
                        TotalHours = timesheet.TotalHours ?? 0,
                        WeekendHoliday = timesheet.WeekendHoliday ?? false,
                        NumberOfHouseCalls = timesheet.WeekendHoliday == true ? timesheet.NoHousecallsNight ?? 0 : timesheet.NoHousecalls ?? 0,
                        NumberOfPhoneConsults = timesheet.WeekendHoliday == true ? timesheet.NoPhoneConsultNight ?? 0 : timesheet.NoPhoneConsult ?? 0,
                    });
                }
            }
            else
            {
                for (int i = start.Day; i <= end.Day; i++)
                {
                    data.Add(new TimesheetData()
                    {
                        PhysicianId = phy,
                        Date = new DateTime(date.Year, date.Month, i),
                        OnCallHours = _Repository.ShiftHoursOnDate(phy, new DateTime(date.Year, date.Month, i)),
                    });
                }
            }

            return data;
        }

        public List<hallodoc_mvc_Repository.ViewModel.Reimbursement>? ReimbursementData(int phyid, DateTime date, int adminid)
        {
            DateTime start = date;
            DateTime end = new();
            if (date.Day == 1)
            {
                end = date.AddDays(14);
            }
            else
            {
                end = date.AddDays(DateTime.DaysInMonth(date.Year, date.Month) - date.Day);
            }

            Invoice isInvoice = _Repository.GetInvoice(start, phyid);
            List<hallodoc_mvc_Repository.ViewModel.Reimbursement> reimbursements = new();
            if (isInvoice.InvoiceId > 0)
            {
                List<hallodoc_mvc_Repository.DataModels.Reimbursement> reimbursementss = _Repository.GetReimbursements(isInvoice.InvoiceId);
                for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
                {
                    reimbursements.Add(new hallodoc_mvc_Repository.ViewModel.Reimbursement
                    {
                        PhysicianId = phyid,
                        Date = i,
                        Item = reimbursementss.FirstOrDefault(x => x.ReimbursementDate.Date == i.Date)?.Item,
                        Filename = reimbursementss.FirstOrDefault(x => x.ReimbursementDate.Date == i.Date)?.Filename,
                        Amount = reimbursementss.FirstOrDefault(x => x.ReimbursementDate.Date == i.Date)?.Amount ?? 0,

                    });
                }
            }
            else
            {
                for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
                {
                    reimbursements.Add(new hallodoc_mvc_Repository.ViewModel.Reimbursement
                    {
                        PhysicianId = phyid,
                        Date = i,

                    });
                }
            }
            return reimbursements;
        }

        public AdminInvocing NotApproved(DateTime date, int phyid, int adminid)
        {
            Invoice isinvoice = _Repository.CheckInvoice(date, phyid);
            if(isinvoice != null)
            {
                if (isinvoice.Status == 1)
                {
                    return new AdminInvocing()
                    {
                        InvoiceId = isinvoice.InvoiceId,
                        StartDate = isinvoice.StartDate,
                        EndDate = isinvoice.EndDate,
                        Status = isinvoice.Status,

                    };
                }
                else
                {
                    DateTime start = date;
                    DateTime end = new();
                    List<TimesheetData> data = new();

                    if (date.Day == 1)
                    {
                        end = date.AddDays(14);
                    }
                    else
                    {
                        end = date.AddDays(DateTime.DaysInMonth(date.Year, date.Month) - date.Day);
                    }

                    List<Timesheet> timesheets = _Repository.TimeSheets(start, end, phyid).OrderBy(x => x.SheetDate).ToList();
                    if (timesheets.Count > 0)
                    {
                        foreach (var timesheet in timesheets)
                        {
                            data.Add(new TimesheetData()
                            {
                                PhysicianId = phyid,
                                InvoiceId = timesheet.InvoiceId,
                                Date = timesheet.SheetDate,
                                OnCallHours = _Repository.ShiftHoursOnDate(phyid, timesheet.SheetDate),
                                TotalHours = timesheet.TotalHours ?? 0,
                                WeekendHoliday = timesheet.WeekendHoliday ?? false,
                                NumberOfHouseCalls = timesheet.WeekendHoliday == true ? timesheet.NoHousecallsNight ?? 0 : timesheet.NoHousecalls ?? 0,
                                NumberOfPhoneConsults = timesheet.WeekendHoliday == true ? timesheet.NoPhoneConsultNight ?? 0 : timesheet.NoPhoneConsult ?? 0,

                            });
                        }
                    }
                    else
                    {
                        for (int i = start.Day; i <= end.Day; i++)
                        {
                            data.Add(new TimesheetData()
                            {
                                PhysicianId = phyid,
                                Date = new DateTime(date.Year, date.Month, i),
                                OnCallHours = _Repository.ShiftHoursOnDate(phyid, new DateTime(date.Year, date.Month, i)),
                            });
                        }
                    }
                    AdminInvocing model = new AdminInvocing();
                    model.Timesheets = data;
                    model.Receipts = _Repository.GetReimbursementByInvoiceId(isinvoice.InvoiceId);

                    model.Status = 2;
                    return model;
                }
            }            
            else 
            {
                Physician? physician = _Repository.GetPhysician(phyid);
                string phyname = physician?.FirstName + " " + physician?.LastName;
                return new AdminInvocing()
                {
                    PhysicanName = phyname,
                    Status = 0,
                };
            }
            
       

        }
        public List<TimesheetData> GetTimesheet(int invoiceid)
        {
            List<TimesheetData> data = new();
            List<Timesheet> timesheets = _Repository.GetTimesheetByInvoiceId(invoiceid);
            PhysicianPayrate payrate = _Repository.GetPhysicianPayrate(timesheets.First().PhysicianId);

            foreach (var timesheet in timesheets)
            {
                data.Add(new TimesheetData()
                {
                    PhysicianId = timesheet.PhysicianId,
                    InvoiceId = timesheet.InvoiceId,
                    Date = timesheet.SheetDate,
                    OnCallHours = _Repository.ShiftHoursOnDate(timesheet.PhysicianId, timesheet.SheetDate),
                    TotalHours = timesheet.TotalHours ?? 0,
                    WeekendHoliday = timesheet.WeekendHoliday ?? false,
                    NumberOfHouseCalls = timesheet.WeekendHoliday == true ? timesheet.NoHousecallsNight ?? 0 : timesheet.NoHousecalls ?? 0,
                    NumberOfPhoneConsults = timesheet.WeekendHoliday == true ? timesheet.NoPhoneConsultNight ?? 0 : timesheet.NoPhoneConsult ?? 0,
                });
            }

            data.FirstOrDefault().Payrate.Shift = payrate.Shift;
            data.FirstOrDefault().Payrate.NigthshiftWeekend = payrate.NigthshiftWeekend;
            data.FirstOrDefault().Payrate.Housecall = payrate.Housecall;
            data.FirstOrDefault().Payrate.PhoneConsults = payrate.PhoneConsults;
            data.FirstOrDefault().Payrate.HousecallsNigthsWeekend = payrate.HousecallsNigthsWeekend;
            data.FirstOrDefault().Payrate.PhoneConsultsNigthsWeekend = payrate.PhoneConsultsNigthsWeekend;

            return data;
        }

        public List<Receipts> Reimbursement(int invoiceid)
        {
            List<Receipts> allReceipts = new();
            Invoice invoice = _Repository.GetInvoiceByInvoiceId(invoiceid);
            foreach (var item in invoice.Timesheets)
            {
                Receipts receipt = new();
                if (invoice.Reimbursements.Select(x => x.ReimbursementDate).Contains(item.SheetDate))
                {
                    receipt.ReceiptDate = item.SheetDate;
                    receipt.PhysicianId = invoice.Reimbursements.FirstOrDefault(x => x.ReimbursementDate == item.SheetDate)?.PhysicianId ?? 0;
                    receipt.Item = invoice.Reimbursements.FirstOrDefault(x => x.ReimbursementDate == item.SheetDate)?.Item ?? "";
                    receipt.Amount = invoice.Reimbursements.FirstOrDefault(x => x.ReimbursementDate == item.SheetDate)?.Amount ?? 0;
                    receipt.FileName = invoice.Reimbursements.FirstOrDefault(x => x.ReimbursementDate == item.SheetDate)?.Filename ?? "";
                }
                else
                {
                    receipt.ReceiptDate = item.SheetDate;
                    receipt.Item = "-";
                    receipt.FileName = "-";
                }
                allReceipts.Add(receipt);
            }
            return allReceipts.OrderBy(x => x.ReceiptDate).ToList();
        }

        public void ApproveTimesheet(int invoiceid, string description, int bonus, int adminid)
        {
            Invoice invoice = _Repository.GetInvoiceByInvoiceId(invoiceid);
            invoice.Description = description;
            invoice.Bonus = bonus;
            invoice.Status = 2;
            invoice.ApprovedDate = DateTime.Now;
            invoice.ApprovedBy = _Repository.getAspid(adminid);
            _Repository.UpdateInvoice(invoice);
        }

        public void UpdateTimesheet(int invoiceid, TimesheetPost data, int adminid)
        {
            List<Timesheet> timesheets = _Repository.GetTimesheetByInvoiceId(invoiceid);

            int index = 0;
            foreach (var item in timesheets)
            {
                item.TotalHours = data.TotalHours[index];
                if (data.WeekendHoliday.Contains(item.SheetDate.Day))
                {
                    item.WeekendHoliday = true;
                    item.NoHousecallsNight = data.NumberOfHouseCalls[index];
                    item.NoPhoneConsultNight = data.NumberOfPhoneConsults[index];
                    item.NoHousecalls = 0;
                    item.NoPhoneConsult = 0;
                }
                else
                {
                    item.NoHousecalls = data.NumberOfHouseCalls[index];
                    item.NoPhoneConsult = data.NumberOfPhoneConsults[index];
                    item.NoHousecallsNight = 0;
                    item.NoPhoneConsultNight = 0;
                    item.WeekendHoliday = null;
                }
                index++;

                item.ModifiedBy = _Repository.getAspid(adminid);
                item.ModifiedDate = DateTime.Now;
                _Repository.UpdateTable(item);
            }
        }
    }
}

