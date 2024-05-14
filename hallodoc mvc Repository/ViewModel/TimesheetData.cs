using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class TimesheetData
    {
        public int PhysicianId { get; set; }

        public int? InvoiceId { get; set; }

        public bool IsFinalized { get; set; } = false;

        public DateTime Date { get; set; }

        public int OnCallHours { get; set; }

        public int TotalHours { get; set; }

        public bool WeekendHoliday { get; set; }

        public int NumberOfHouseCalls { get; set; }

        public int NumberOfPhoneConsults { get; set; }

        public Payrate Payrate { get; set; } = new();
    }
}
