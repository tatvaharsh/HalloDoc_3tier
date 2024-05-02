using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class Payrate
    {
       
        public int PhysicianId { get; set; }

        public int? NigthshiftWeekend { get; set; }

        public int? Shift { get; set; }

        public int? HousecallsNigthsWeekend { get; set; }

        public int? PhoneConsults { get; set; }

        public int? PhoneConsultsNigthsWeekend { get; set; }

        public int? BatchTesting { get; set; }

        public int? Housecall { get; set; }



    }
}
