using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    //public class AdminDashboard
    //{
    //    public List<RequestListAdminDash> RequestListAdminDash { get; set; }
    //}

    public class AdminDashboard
    {
        public DateTime? AcceptedDate { get; set; }
        public int Id { get; set; }
        public string? RPhone { get; set; }

        public string? FName { get; set; }
        public string? LName { get; set; }
        public string?  Email { get; set; }

        public string? Name { get; set; } //rc

        public int RequestTypeId { get; set;  } //req

        public DateOnly DateOfBirth { get; set; } //rc

        public string Requestor { get; set; } //req

        public DateTime RequestDate { get; set; } //req

        public string Phone { get; set; }  //rc

        public List<string>? Notes { get; set; }

        public string? Address { get; set; } //rc

        public string ChatWith { get; set; } //-- nahi aave

        public string Physician { get; set; } //phy

        public DateTime DateOfService { get; set; } //nathi aapi

        public string? Region { get; set; } //region

        public int Status { get; set; } //req

        public int PgCount { get; set; }
    }
}
