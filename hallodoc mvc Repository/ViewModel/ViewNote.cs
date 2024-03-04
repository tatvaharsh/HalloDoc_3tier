using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class ViewNote
    {
        public int RequestId { get; set; }

        public string? TransferNotes { get; set; }

        public string? PhysicianNotes { get; set; }

        public string? AdminNotes { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public string? Admincancellationnote { get; set; }

        public string? Patientcancellationnote { get; set; }

        public string? Additonalnote { get; set; }

    }
}
