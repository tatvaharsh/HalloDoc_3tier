using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class ModalData
    {
        public int requestID { get; set; }

        public int? reason { get; set; }

        public string? note { get; set; }

        public string? PatientName { get; set; }

        public List<Region>? region { get; set; }

        public List<Physician>? Physicians { get; set; }

        public string? SelectedRegion { get; set; }

        public int? SelectedPhysicianName { get; set; }

        public List<CaseTag> CaseTags { get; set; }

        public string? email { get; set; }

        public string? number { get; set; }

        public int? CountPending { get; set; }
        public int? CountNew { get; set; }
        public int? CountUnpaid { get; set; }
        public int? CountClose { get; set; }
        public int? CountActive { get; set; }
        public int? CountConclude { get; set; }


        public int? Token { get; set; }
    }
}
