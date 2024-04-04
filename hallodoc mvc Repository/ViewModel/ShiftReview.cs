using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class ShiftReview
    {
        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public DateTime ShiftDate { get; set; }

        public string Region { get; set; }

        public string ProviderName { get; set; }

        public int shiftdetailid { get; set; } 

        public List<Region> regions { get; set; }


    }
}
