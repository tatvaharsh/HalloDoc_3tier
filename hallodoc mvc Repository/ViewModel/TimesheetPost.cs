using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class TimesheetPost
    {
        public int PhysicianId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime Date { get; set; }

        public List<int> OnCallHours { get; set; } = new List<int>();

        public List<int> TotalHours { get; set; } = new List<int>();

        public List<int> WeekendHoliday { get; set; } = new List<int>();

        public List<int> NumberOfHouseCalls { get; set; } = new List<int>();

        public List<int> NumberOfPhoneConsults { get; set; } = new List<int>();
    }
}
