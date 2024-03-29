using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class UserAccess
    {
        public List<AspNetUser> Aspnetuser { get; set; }
        public List<Admin> admins { get; set; }
        public List<Physician> physicsian { get; set; }
        public short? adminstatus { get; set; }
        public short? physicianstatus { get; set; }
        public string Username { get; set; }
        public string? Phonenumber { get; set; }
        public string? role { get; set; }
        public string? accountType { get; set; }
        public int? PhyStatus { get; set; }
        public int? AdminStatus { get; set; }
        public int? Status { get; set; }
    }
}
