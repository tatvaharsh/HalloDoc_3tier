using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class CreateShift
    {
        public List<Region>? Region { get; set; }

        public List<Physician>? Physicians { get; set; }

        public int? SelectedPhysicianId { get; set; }
        public int SelectedRegionId { get; set; }
        public DateTime ShiftDate { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }   
        public List<int>? Weekday { get; set; }
        public int? Repeat { get; set;}
        public BitArray RepeatToggle { get; set;}
    }
}
