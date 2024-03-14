using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class Profile
    {
        public Admin AdminData { get; set; }

        public List<Regiondetails> Reg { get; set; }

        public String Role { get; set; }
        public AspNetUser user { get; set; }

        public String State { get; set; }

        public List<Region>? region { get; set; }
    }
}
