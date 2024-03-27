using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class Provider
    {
        public List<Region> regions { get; set; }

        public List<Physician> physicians { get; set; }

        public short? Status { get; set; }

        public string Name { get; set; } = null!;

        public bool Notification { get; set; }

        public int PhyId { get; set; }

        public string Role { get; set; } = null!;

        public string OnCall { get; set; } = null!;

        public List<Region> Regions { get; set; } = null!;
    }
}
