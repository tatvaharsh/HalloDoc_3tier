using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class AdminInvocing
    {
        public int InvoiceId { get; set; }

        public string PhysicanName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Status { get; set; }

        public List<TimesheetData> Timesheets { get; set; }
        public List<hallodoc_mvc_Repository.DataModels.Reimbursement> Receipts { get; set; }
    }
}
