using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class EditShift
    {
        public bool isEditable { get; set; } = true;
        public List<Region> Regions { get; set; } = new();

        public List<Physician> Physicians { get; set; } = new();

        public int SelectedRegion { get; set; }

        public int SelectedPhy { get; set; }

        public int ShiftDetailId { get; set; }

        public int Status { get; set; }

        public DateTime ShftDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
    }
}
