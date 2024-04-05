using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class OnCallModal
    {

        public List<Physician> OnCall { get; set; }

        public List<Physician> OffDuty { get; set; }

        public List<Region> regions { get; set; }

    }
}
