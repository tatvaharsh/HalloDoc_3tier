using hallocdoc_mvc_Service.Interface;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.Interface;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace hallocdoc_mvc_Service.Implementation
{
    public class Admin_Service : IAdmin_Service
    {
        private readonly IAdmin_Repository _Repository;

        public Admin_Service(IAdmin_Repository Repository)
        {
            _Repository = Repository;
        }

        public List<AdminDashboard> getDashData(int? requestType, string? search, int? requestor, int? region, int pageid)
        {
            var query = _Repository.GetAdminCode();

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

        public bool ValidateUser(LoginViewModel model)
        {
            AspNetUser asp = _Repository.Validate(model);
            if (asp.Id > 0)
            {
                model.Id = asp.Id;
                return true;
            }
            return false;
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
                notedata.ModifiedBy = admin;
                notedata.ModifiedDate = DateTime.Now;
                _Repository.save();

            }
            else
            {
                var newnotedata = new RequestNote()
                {
                    RequestId = id,
                    AdminNotes = model.AdminNotes,
                    CreatedBy = admin,
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

            req.Status = 2;
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
                RequestId = id.ToString(),
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

        public void SendEmail(int id)
        {
            List<RequestWiseFile> requestWiseFiles = _Repository.GetDocumentList(id);
            Request request = _Repository.GetRequestById(id);
            var receiver = "harsh.mehta8576@gmail.com";
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
                CreatedBy = admin.ToString(),

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

        public void SendAgreementMail(int Id, ModalData md, string token)
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
            ModalData resp = new ModalData();
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
            return new();



        }

        public void editencounter(int id, Encounter model)
        {
            EncounterForm ef = _Repository.getencounterbyid(id);
            ef.Date = model.Date;
            ef.HistoryIllness = model.HistoryIllness;
            ef.MedicalHistory = model.MedicalHistory;
            ef.Medications = model.Medications;
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
            ef.Chest = model.Chest;
            _Repository.updateEncounterForm(ef);

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
                Role = _Repository.GetAspNetRole(admin)[0],
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

        public void sendlink(ViewCase model)
        {


            var receiver = model.Email;
            var subject = "Send Link";
            var message = "Tap on link for Send Link : <!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <style>\r\n        /* Your provided CSS */\r\n        .button-74 {\r\n            background-color: #fbeee0;\r\n            border: 2px solid #422800;\r\n            border-radius: 30px;\r\n            box-shadow: #422800 4px 4px 0 0;\r\n            color: #422800;\r\n            cursor: pointer;\r\n            display: inline-block;\r\n            font-weight: 600;\r\n            font-size: 18px;\r\n            padding: 0 18px;\r\n            line-height: 50px;\r\n            text-align: center;\r\n            text-decoration: none;\r\n            user-select: none;\r\n            -webkit-user-select: none;\r\n            touch-action: manipulation;\r\n        }\r\n\r\n        .button-74:hover {\r\n            background-color: #fff;\r\n        }\r\n\r\n        .button-74:active {\r\n            box-shadow: #422800 2px 2px 0 0;\r\n            transform: translate(2px, 2px);\r\n        }\r\n\r\n        @media (min-width: 768px) {\r\n            .button-74 {\r\n                min-width: 120px;\r\n                padding: 0 25px;\r\n            }\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <a href=\"http://localhost:5198/Home/submit_screen\" class=\"button-74\">Click me!</a>\r\n</body>\r\n</html>\r\n";


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


        }

        public void PatientForm(patient_form model, int admin)
        {


            var aspnetuser1 = _Repository.getAsp(model.Email);
            var user1 = _Repository.getUser(model.Email);
            //send mail//
            var receiver = model.Email;
            var subject = "Send Link";
            var message = "Tap on link for Send Link : http://localhost:5198/Home/create_patient";


            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            //complete//

            client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));


            if (aspnetuser1 == null)
            {
                AspNetUser aspnetuser2 = new AspNetUser
                {

                    UserName = model.FirstName + "_" + model.LastName,
                    Email = model.Email,
                    //PasswordHash = model.Password,
                    PhoneNumber = model.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    PasswordHash = model.Password,
                };
                _Repository.AddAspnetUser(aspnetuser2);
                aspnetuser1 = aspnetuser2;
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

            if (user1 == null)
            {
                User user = new User
                {
                    AspNetUserId = aspnetuser1.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.PhoneNumber,
                    ZipCode = model.ZipCode,
                    State = model.State,
                    City = model.City,
                    RegionId = isRegion.RegionId,
                    Street = model.Street,
                    IntDate = model.BirthDate.Day,
                    IntYear = model.BirthDate.Year,
                    StrMonth = model.BirthDate.ToString("MMM"),
                    CreatedDate = DateTime.Now,

                    CreatedBy = aspnetuser1.Id
                };

                _Repository.AddUser(user);

                user1 = user;
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
                CreatedBy = admin,
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
                asp.PasswordHash = model.user.PasswordHash;
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

        public void Sendit(int id, ModalData md)
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
                PasswordHash = model.Password,
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
                CreatedBy = admin1,
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
            var roles = selectedRoles[0].Split(',');
            Role role = new Role();
            role.Name = roleName;
            role.CreatedDate = DateTime.Now;
            role.AccountType = (short)check;
            role.CreatedBy = admin1.ToString();
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
            p.Photo = model.Photo.FileName;
            p.Signature = model.Signature.FileName;
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
                PasswordHash = model.Password,
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
                CreatedBy = admin1,
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
            model.BusinessContact=hp.BusinessContact;
            model.Street = hp.Address;
            model.City = hp.City;
             model.Email = hp.Email;
            model.Zip = hp.Zip;
            model.RegionId = (Int32)hp.RegionId;
            model.regions = _Repository.GetReg();
            model.Professions = _Repository.GetProfession();
            model.SelectedhealthprofID = hp.Profession??0;
            model.Phonenumber = hp.PhoneNumber;
            model.partnersdatas.Add(new Partnersdata()
            {
                VendorId = hp.VendorId
            });
            return model;
        }

        public void EditPartner(PartnersCM model,int vendorid)
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
            hp1.IsDeleted = new BitArray(1,true);

            _Repository.UpdateHealthProfessiontbl(hp1);
        }
    }
}

