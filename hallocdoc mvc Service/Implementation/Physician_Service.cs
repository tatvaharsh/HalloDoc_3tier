using hallocdoc_mvc_Service.Interface;
using hallodoc_mvc_Repository.Interface;
using Microsoft.Extensions.Configuration;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using iText.StyledXmlParser.Node;
using Twilio.TwiML.Voice;
using System.Collections;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Web.Helpers;

namespace hallocdoc_mvc_Service.Implementation
{
    public class Physician_Service : IPhysician_Service
    {
        private readonly IPhysician_Repository _Repository;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;

        public Physician_Service(IPhysician_Repository physician, IConfiguration configuration, IJwtService jwtService)
        {
            _Repository = physician;
            _configuration = configuration;
            _jwtService = jwtService;
        }

        public void AddProviderNote(int id, string note, int phy)
        {
            RequestNote requestNote = _Repository.setnotes(id);
            if (requestNote != null)
            {
                requestNote.PhysicianNotes = note;
                _Repository.UpdateRequestNotes(requestNote);

            }
            else
            {
                RequestNote ren = new()
                {
                    RequestId = id,
                    PhysicianNotes = note,
                    CreatedBy = _Repository.GetAspId(phy),
                    CreatedDate = DateTime.Now,
                };
                _Repository.AddRequestNotes(ren);
            }
        }

        public bool GetEncounterStatus(int id)
        {
            return _Repository.GetEncounterStatus(id);
        }

        public void ConcludeCare(int id, int physicianId)
        {
            Request request = _Repository.GetRequestById(id);
            request.Status = 8;
            request.ModifiedDate = DateTime.Now;
            request.LastWellnessDate = DateTime.Now;
            _Repository.updateRequesttbl(request);

            _Repository.AddRequestStatusLog(new RequestStatusLog()
            {
                RequestId = id,
                Status = 8,
                CreatedDate = DateTime.Now
            });
        }

        public void AddToPending(int id)
        {
            _Repository.Accept(id);
        }

        public void ChangeCallType(int id, int phy)
        {
            var a = _Repository.GetRequestById(id);
            a.CallType = 1;
            a.Status = 5;
            a.RequestStatusLogs.Add(new()
            {
                Status = 5,
                PhysicianId = phy,
                CreatedDate = DateTime.Now,
            });
            _Repository.updateRequesttbl(a);

            //RequestStatusLog req = new()
            //{
            //    RequestId = id,
            //    Status = 5,
            //    PhysicianId = phy,
            //    CreatedDate = DateTime.Now,
            //};
            //_Repository.AddRequestStatusLog(req);
        }

        public void ChangeStatus(int id, int phy)
        {
            var a = _Repository.GetRequestById(id);
            a.Status = 6;
            a.RequestStatusLogs.Add(new()
            {
                Status = 6,
                PhysicianId = phy,
                CreatedDate = DateTime.Now,
            });
            _Repository.updateRequesttbl(a);
        }

        public void Consult(int id, int phy)
        {
            var a = _Repository.GetRequestById(id);
            a.CallType = 2;
            a.Status = 6;
            _Repository.updateRequesttbl(a);

            RequestStatusLog req = new()
            {
                RequestId = id,
                Status = 6,
                PhysicianId = phy,
                CreatedDate = DateTime.Now,
            };
            _Repository.AddRequestStatusLog(req);

        }

        public ModalData CountState(int admin1)
        {
            return _Repository.CountState(admin1);
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

        public int DeleteFile(int id)
        {
            RequestWiseFile df = _Repository.GetDocumentFile(id);
            ;
            df.IsDeleted = new System.Collections.BitArray(1, true);
            _Repository.update_RequestWiseTable(df);

            return df.RequestId;
        }

        public void editencounter(int id, Encounter model)
        {
            EncounterForm ef = _Repository.getencounterbyid(id);
            if (ef != null)
            {
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
            else
            {
                EncounterForm ef1 = new();
                ef1.RequestId = id;
                ef1.Date = model.Date;
                ef1.HistoryIllness = model.HistoryIllness;
                ef1.MedicalHistory = model.MedicalHistory;
                ef1.Medications = model.Medications;
                ef1.Procedures = model.Procedures;
                ef1.FollowUp = model.FollowUp;
                ef1.Heent = model.Heent;
                ef1.Cv = model.Cv;
                ef1.Chest = model.Chest;
                ef1.Abd = model.Abd;
                ef1.Extr = model.Extr;
                ef1.Skin = model.Skin;
                ef1.Neuro = model.Neuro;
                ef1.Other = model.Other;
                ef1.Diagnosis = model.Diagnosis;
                ef1.TreatmentPlan = model.TreatmentPlan;
                ef1.MedicationDispensed = model.MedicationDispensed;
                ef1.Medications = model.Medications;
                ef1.Allergies = model.Allergies;
                ef1.Temp = model.Temp;
                ef1.Hr = model.Hr;
                ef1.Rr = model.Rr;
                ef1.BpD = model.BpD;
                ef1.BpS = model.BpS;
                ef1.O2 = model.O2;
                ef1.Pain = model.Pain;
                ef1.Chest = model.Chest;
                _Repository.AddEncounterFormtbl(ef1);
            }


        }

        public void FileUpload(int id, List<IFormFile> file, int phy)
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
                requestWiseFile.PhysicianId = phy;
                _Repository.AddRequestWiseFiles(requestWiseFile);

            }
        }

        public void final(int id)
        {
            EncounterForm ef = _Repository.getencounterbyid(id);
            if (ef != null)
            {
                ef.IsFinalized = new System.Collections.BitArray(1, true);
                _Repository.updateEncounterForm(ef);
            }
            else
            {
                EncounterForm eform = new()
                {
                    RequestId = id,
                    IsFinalized = new System.Collections.BitArray(1, true),
                };
                _Repository.AddEncounterFormtbl(eform);
            }
        }

        public RequestClient GetAgreementtdata(int id)
        {
            return _Repository.getagreement(id);
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

        public List<AdminDashboard> getDashDataActive(int? requestType, string? search, int? requestor, int? region, int pageid, int phy)
        {
            var query = _Repository.GetActiveData(phy);
            if (search != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                pageid = 1;
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3).ToList();
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1).ToList();
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4).ToList();
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2).ToList();
            }
            int size = 2;
            int c = query.Count;
            query = query.Skip(pageid * size - size).Take(size).ToList();
            if (query.Count > 0)
            {
                query.First().PgCount = c;
            }
            return query;
        }

        public List<AdminDashboard> getDashDataConclude(int? requestType, string? search, int? requestor, int? region, int pageid, int phy)
        {
            var query = _Repository.GetConcludeData(phy);
            if (search != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                pageid = 1;
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3).ToList();
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1).ToList();
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4).ToList();
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2).ToList();
            }
            int size = 2;
            int c = query.Count;
            query = query.Skip(pageid * size - size).Take(size).ToList();
            if (query.Count > 0)
            {
                query.First().PgCount = c;
            }
            return query;
        }

        public List<AdminDashboard> getDashDataPending(int? requestType, string? search, int? requestor, int? region, int pageid, int phy)
        {
            var query = _Repository.GetPendingData(phy);
            if (search != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                pageid = 1;
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3).ToList();
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1).ToList();
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4).ToList();
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2).ToList();
            }
            int size = 2;
            int c = query.Count;
            query = query.Skip(pageid * size - size).Take(size).ToList();
            if (query.Count > 0)
            {
                query.First().PgCount = c;
            }
            return query;
        }

        public Encounter getencounter(int id)
        {
            RequestClient rc = _Repository.getagreement(id);


            DateOnly Mydate = new(rc.IntYear.Value, DateOnly.ParseExact(rc.StrMonth, "MMM", CultureInfo.InvariantCulture).Month, rc.IntDate.Value);

            EncounterForm ef = _Repository.getencounterbyid(id);

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
                Encounter en1 = new()
                {

                    Requestid = id,
                    patientData = rc,
                    DOB = Mydate,


                };
                return en1;
            }






        }

        public List<AdminDashboard> GetNewData(int? requestType, string? search, int? requestor, int? region, int pageid, int phy)
        {
            var query = _Repository.GetNewData(phy);
            if (search != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                pageid = 1;
            }

            if (requestor == 3)
            {
                query = query.Where(r => r.RequestTypeId == 3).ToList();
            }

            if (requestor == 1)
            {
                query = query.Where(r => r.RequestTypeId == 1).ToList();
            }

            if (requestor == 4)
            {
                query = query.Where(r => r.RequestTypeId == 4).ToList();
            }

            if (requestor == 2)
            {
                query = query.Where(r => r.RequestTypeId == 2).ToList();
            }
            int size = 2;
            int c = query.Count;
            query = query.Skip(pageid * size - size).Take(size).ToList();
            if (query.Count > 0)
            {
                query.First().PgCount = c;
            }
            return query;
        }

        public ViewNote GetNotes(int id)
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
                PhysicianNotes = notes?.PhysicianNotes,
                Patientcancellationnote = patientcancel,
                AdminNotes = notes?.AdminNotes,
                TransferNotes = transfer,
            };
            return data;
        }

        public Order GetOrderData(Order md)
        {
            List<HealthProfessionalType> r = _Repository.GetHealthprofessionalByType();

            md.HealthProfessionalType = r;

            return md;
        }

        public Physician getPhy(string email)
        {
            return _Repository.getaspuser(email);
        }

        public List<HealthProfessional> Getvendor(int id)
        {

            return _Repository.getvendorbyprofessiontype(id);

        }


        public List<HealthProfessional> Getvendordata(int id)
        {
            return _Repository.getdatabyvendorid(id);
        }

        public void OrderPost(Order md, int id, int phy)
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
                CreatedBy = phy.ToString(),

            };
            _Repository.AddOrderdetails(oredr);
        }

        public string Phyname(int admin1)
        {
            return _Repository.Getname(admin1);
        }

        public void SendAgreementMail(int id, ModalData md, string token, int phy)
        {
            RequestClient rc = _Repository.getagreement(id);


            if (md.email != null)
            {
                var receiver = md.email;
                var subject = "Send Agreement";
                var message = "Tap on link for Send Agreement : http://localhost:5198/Admin/Agreement?t=" + token + "&token=" + id;


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

        public void SendEmail(int id, int phy)
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

        public ViewNote setViewNotesData(ViewNote model, int id, int phy)
        {

            //var notedata = _context.Requestnotes.FirstOrDefault(i => i.Requestid == requestid);
            var notedata = _Repository.setnotes(id);

            if (notedata != null)
            {
                notedata.RequestId = id;

                notedata.PhysicianNotes = model.PhysicianNotes;
                notedata.ModifiedBy = _Repository.GetAspId(phy);
                notedata.ModifiedDate = DateTime.Now;
                _Repository.UpdateRequestNotes(notedata);

            }
            else
            {
                var newnotedata = new RequestNote()
                {
                    RequestId = id,
                    PhysicianNotes = model.PhysicianNotes,
                    CreatedBy = _Repository.GetAspId(phy),
                    CreatedDate = DateTime.Now,
                };

                _Repository.AddRequestNotes(newnotedata);

            }
            return model;

        }

        public void Transfer(int id, ModalData md, int phy)
        {
            RequestStatusLog statusLog = new RequestStatusLog()
            {
                RequestId = id,
                Status = 1,
                PhysicianId = phy,
                Notes = md.note,
                CreatedDate = DateTime.Now,

            };
            _Repository.AddRequesttbl(statusLog);

            Request req = _Repository.GetRequestById(id);
            req.Status = 1;
            req.AcceptedDate = null;
            req.PhysicianId = null;
            _Repository.save();
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
            RequestNote requestNote = _Repository.setnotes(id);
            if (requestNote != null)
            {

                vd.FirstOrDefault().Notes = requestNote.PhysicianNotes;
            }
            return vd;
        }

        public CreatePhy? getphysiciandata(int admin1)
        {
            var p = _Repository.getphycian(admin1);
            var a = _Repository.GetAspNetUser(p.AspNetUserId ?? 0);
            CreatePhy cp = new()
            {
                id = admin1,
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
                SelectedRegions = _Repository.GetSelectedPhyReg(admin1).Select(x => x.RegionId).ToList(),
                isagreement = p.IsAgreementDoc == null ? false : p.IsAgreementDoc[0],
                isbackground = p.IsBackgroundDoc == null ? false : p.IsBackgroundDoc[0],
                ishippa = p.IsTrainingDoc == null ? false : p.IsTrainingDoc[0],
                isnonclosure = p.IsNonDisclosureDoc == null ? false : p.IsNonDisclosureDoc[0],
                islisence = p.IsLicenseDoc == null ? false : p.IsLicenseDoc[0],
            };
            return cp;

        }

        public void changepass(string pass, int p)
        {
            Physician pl = _Repository.GetPhysician(p);
            AspNetUser asp = _Repository.GetAspNetUser((int)pl.AspNetUserId);
            if (asp != null)
            {
                asp.PasswordHash = Crypto.HashPassword(pass);
                _Repository.UpdateAspNetUser(asp);
            }
        }

        public void SendMailToAdmin(int id, string textareas)
        {
            var a = _Repository.GetPhysician(id);
            var b = _Repository.GetAspNetUser((int)a.CreatedBy);
            var receiver = b.Email;
            var subject = "Request For Edit The Physician Account";
            var messages = "Physician Requesting Admin for the Edit the Account" + textareas;

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
                EmailId = a.Email,
                CreateDate = DateTime.Now,
                SentDate = DateTime.Now,

            };
            _Repository.AddEmaillogtbl(emailLog);
        }


        public Provider GetRegions()
        {
            var p = _Repository.GetReg();
            Provider pr = new()
            {
                regions = p,
            };
            return pr;
        }

        public Scheduling GetMonthWiseData(int day, int month, int year, int phy)
        {
            DateTime date = day == 0 ? new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) : new(year, month, day);
            return _Repository.GetMonthData(date, phy);
        }

        public List<Region> getreg()
        {
            return _Repository.GetReg();
        }

        public List<Physician> GetPhy(int id)
        {
            return _Repository.GetRequestByRegion(id).ToList();
        }

        public bool CreateShift(CreateShift shift, int admin)
        {
            Physician data = _Repository.GetShiftData(admin);
            bool flag = true;

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
                PhysicianId = admin,
                StartDate = DateOnly.FromDateTime(shift.ShiftDate),
                IsRepeat = shift.RepeatToggle,
                RepeatUpto = shift.Repeat,
                CreatedBy = _Repository.GetAspId(admin),
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
                RegionId = data.RegionId,
                StartTime = shift.Start,
                EndTime = shift.End,
                Status = 0,//peniding=0 and approve = 1
                IsDeleted = new BitArray(1, false),
            };

            _Repository.AddShiftDetails(detail);



            ShiftDetailRegion shiftRegion = new()
            {
                ShiftDetailId = detail.ShiftDetailId,
                RegionId = (int)data.RegionId,
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
                            RegionId = (int)data.RegionId,
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
                            RegionId = (int)data.RegionId,
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

        public string GetRegByPhy(int phy)
        {
            return _Repository.GetPhysicianName(phy);
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
                Regions = _Repository.GetReg(),
                Physicians = _Repository.GetPhyN(),
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

        public void UpdateShift(EditShift editShift, int shiftdetailid, int phy)
        {
            ShiftDetail shiftDetail = _Repository.GetShiftDetails(shiftdetailid);
            shiftDetail.ShiftDate = editShift.ShftDate;
            shiftDetail.StartTime = editShift.StartTime;
            shiftDetail.EndTime = editShift.EndTime;
            shiftDetail.ModifiedDate = DateTime.Now;
            shiftDetail.ModifiedBy = _Repository.GetAspId(phy);
            _Repository.Update(shiftDetail);
        }

        public void ChangeShiftStatus(int shiftdetailid, int phy)
        {

            ShiftDetail detail = _Repository.GetShiftDetails(shiftdetailid);
            detail.Status = (short)(detail.Status == 1 ? 0 : 1);
            detail.ModifiedBy = _Repository.GetAspId(phy);
            detail.ModifiedDate = DateTime.Now;
            _Repository.Update(detail);
        }

        public void DeleteShiftViaModal(int shiftdetailid, int phy)
        {
            ShiftDetail detail = _Repository.GetShiftDetails(shiftdetailid);
            detail.IsDeleted = new BitArray(1, true);
            detail.ModifiedBy = _Repository.GetAspId(phy);
            detail.ModifiedDate = DateTime.Now;
            _Repository.Update(detail);
        }

        public void sendlink(ViewCase model, int phy)
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
                PhysicianId = phy
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
                PhysicianId = phy

            };
            _Repository.AddEmaillogtbl(emailLog);

        }

        public void PatientForm(patient_form model, int phy)
        {
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

            _Repository.AddRequestTbl(request);

            var aspnetuser1 = _Repository.getAsp(model.Email);

            //send mail//


            if (aspnetuser1 == null)
            {
                var receiver = model.Email;
                var subject = "Create Account";
                var message = "Tap on link for Send Link : http://localhost:5198/Home/create_patient?token=" + _jwtService.GenerateJwtTokenByEmail(receiver);


                var mail = "tatva.dotnet.binalmalaviya@outlook.com";
                var password = "binal@2002";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };
                //complete//

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));


                EmailLog emailLog = new()
                {
                    EmailTemplate = message,
                    SubjectName = subject,
                    SentTries = 1,
                    IsEmailSent = true,

                    EmailId = model.Email,
                    ConfirmationNumber = request.ConfirmationNumber,
                    RequestId = request.RequestId,
                    CreateDate = DateTime.Now,
                    SentDate = DateTime.Now,
                    PhysicianId = phy
                };
                _Repository.AddEmaillogtbl(emailLog);
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
                PhysicianNotes = model.phynotes,
                RequestId = request.RequestId,
                CreatedBy = _Repository.GetAspId(phy),
                CreatedDate = DateTime.Now,
            };

            _Repository.AddRequestNotes(rn);

        }

        public List<TimesheetData> TimesheetData(DateTime date, int phy)
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

        public void AddTimesheets(DateTime date, int phyid, TimesheetPost data)
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

            if (isInvoice.InvoiceId == 0)
            {
                Invoice invoice = new()
                {
                    PhysicianId = phyid,
                    StartDate = date,
                    EndDate = end,
                    Status = 1,
                    CreatedBy = _Repository.GetAspId(phyid),
                };

                int index = 0;
                for (var i = start.Day; i <= end.Day; i++)
                {
                    Timesheet sheet = new()
                    {
                        PhysicianId = phyid,
                        SheetDate = new DateTime(date.Year, date.Month, i),
                        TotalHours = data.TotalHours[index],
                        CreatedBy = _Repository.GetAspId(phyid),
                    };
                    if (data.WeekendHoliday.Contains(i))
                    {
                        sheet.WeekendHoliday = true;
                        sheet.NoHousecallsNight = data.NumberOfHouseCalls[index];
                        sheet.NoPhoneConsultNight = data.NumberOfPhoneConsults[index];
                    }
                    else
                    {
                        sheet.NoHousecalls = data.NumberOfHouseCalls[index];
                        sheet.NoPhoneConsult = data.NumberOfPhoneConsults[index];
                    }
                    invoice.Timesheets.Add(sheet);

                    index++;
                }

                _Repository.SaveTable(invoice);
            }
            else
            {
                int index = 0;
                foreach (var item in isInvoice.Timesheets.OrderBy(X => X.SheetDate).ToList())
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
                }
                _Repository.UpdateTable(isInvoice);
            }
        }

        public bool ShowFinalizeBtn(DateTime date, int phyid)
        {
            return _Repository.ShowBtn(date, phyid);
        }

        public List<hallodoc_mvc_Repository.ViewModel.Reimbursement>? ReimbursementData(int phyid, DateTime date)
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

        public void AddReciet(hallodoc_mvc_Repository.ViewModel.Reimbursement model, DateTime date, int phyid)
        {


            string filename = date.ToShortDateString() + Path.GetExtension(model.FormFile.FileName);
            string path = Directory.GetCurrentDirectory() + "/wwwroot/Reimbursement/" + phyid + "/" + filename;
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using FileStream stream = new(path, FileMode.Create);
            model.FormFile.CopyTo(stream);

            DateTime startDate = new DateTime(date.Year, date.Month, date.Day <= 15 ? 1 : 16);
            Invoice isInvoice = _Repository.GetInvoice(startDate, phyid);
            if (isInvoice.InvoiceId > 0)
            {
                hallodoc_mvc_Repository.DataModels.Reimbursement reimbursement = _Repository.IsDataAvailable(isInvoice.InvoiceId, date);
                if (reimbursement.ReimbursementId > 0)
                {
                    reimbursement.PhysicianId = phyid;
                    reimbursement.InvoiceId = isInvoice.InvoiceId;
                    reimbursement.ReimbursementDate = date;
                    reimbursement.Item = model.Item;
                    reimbursement.Amount = model.Amount;
                    reimbursement.Filename = filename;
                    reimbursement.ModifiedBy = _Repository.GetAspId(phyid);
                    reimbursement.ModifiedDate = DateTime.Now;
                    _Repository.UpdateReimbursement(reimbursement);
                }
                else
                {
                    hallodoc_mvc_Repository.DataModels.Reimbursement re = new()
                    {
                        PhysicianId = phyid,
                        InvoiceId = isInvoice.InvoiceId,
                        ReimbursementDate = date,
                        Item = model.Item,
                        Amount = model.Amount,
                        Filename = filename,
                        CreatedBy = _Repository.GetAspId(phyid),
                        CreatedDate = DateTime.Now,
                    };
                    _Repository.AddReimbursement(re);
                }

            }
        }

        public void EditReciet(hallodoc_mvc_Repository.ViewModel.Reimbursement model, DateTime date, int phyid)
        {
            if (model.FormFile != null)
            {
                string filename = date.ToShortDateString() + Path.GetExtension(model.FormFile.FileName);
                string path = Directory.GetCurrentDirectory() + "/wwwroot/Reimbursement/" + phyid + "/" + filename;
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                model.FormFile.CopyTo(stream);
            }
            DateTime startDate = new DateTime(date.Year, date.Month, date.Day <= 15 ? 1 : 16);
            Invoice isInvoice = _Repository.GetInvoice(startDate, phyid);
            if (isInvoice.InvoiceId > 0)
            {
                hallodoc_mvc_Repository.DataModels.Reimbursement reimbursement = _Repository.IsDataAvailable(isInvoice.InvoiceId, date);
                if (reimbursement.ReimbursementId > 0)
                {
                    reimbursement.PhysicianId = phyid;
                    reimbursement.InvoiceId = isInvoice.InvoiceId;
                    reimbursement.ReimbursementDate = date;
                    reimbursement.Item = model.Item;
                    reimbursement.Amount = model.Amount;
                    reimbursement.Filename = model.Filename != null ? date.ToShortDateString() + Path.GetExtension(model.FormFile.FileName) : reimbursement.Filename;
                    reimbursement.ModifiedBy = _Repository.GetAspId(phyid);
                    reimbursement.ModifiedDate = DateTime.Now;
                    _Repository.UpdateReimbursement(reimbursement);
                }
            }
        }

        public void DeleteReciet(DateTime date, int phyid)
        {
            hallodoc_mvc_Repository.DataModels.Reimbursement reimbursement = _Repository.IsDataAvailablebydate(date);
            if (reimbursement.ReimbursementId > 0)
            {
                _Repository.RemoveData(reimbursement);
            }
        }

        public void FinalizeInvoice(DateTime date, int phyid)
        {
            Invoice isInvoice = _Repository.GetInvoice(date, phyid);
            if (isInvoice.InvoiceId > 0)
            {
                isInvoice.IsFinalized = true;
                _Repository.UpdateInvoice(isInvoice);
            }
        }

        public AdminInvocing? NotApproved(DateTime date, int phyid)
        {
            Invoice isinvoice = _Repository.GetInvoice(date, phyid);
            if (isinvoice != null)
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
    }
}
