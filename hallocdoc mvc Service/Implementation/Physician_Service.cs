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

namespace hallocdoc_mvc_Service.Implementation
{
    public class Physician_Service : IPhysician_Service
    {
        private readonly IPhysician_Repository _Repository;

        public Physician_Service(IPhysician_Repository physician)
        {
            _Repository = physician;

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
                notedata.ModifiedBy = phy;
                notedata.ModifiedDate = DateTime.Now;
                _Repository.save();

            }
            else
            {
                var newnotedata = new RequestNote()
                {
                    RequestId = id,
                    PhysicianNotes = model.PhysicianNotes,
                    CreatedBy = phy,
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
    }
}
