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

namespace hallocdoc_mvc_Service.Implementation
{
    public class Physician_Service : IPhysician_Service
    {
        private readonly IPhysician_Repository _Repository;

        public Physician_Service(IPhysician_Repository physician)
        {
            _Repository = physician;
       
        }

        public void AddToPending(int id)
        {
            _Repository.Accept(id);
        }

        public ModalData CountState(int admin1)
        {
            return _Repository.CountState(admin1);
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
                Patientcancellationnote = patientcancel,
                AdminNotes = notes?.AdminNotes,
                TransferNotes = transfer,
            };
            return data;
        }

        public Physician getPhy(string email)
        {
            return _Repository.getaspuser(email);
        }

        public string Phyname(int admin1)
        {
            return _Repository.Getname(admin1);
        }
    }
}
