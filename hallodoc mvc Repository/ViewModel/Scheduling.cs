using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class Scheduling
    {
        public Physician Physicians { get; set; } = null!;

        public List<ShiftDetail>? shifts { get; set; } = new();

        public DateTime CurrentDate { get; set; }
    }
}
