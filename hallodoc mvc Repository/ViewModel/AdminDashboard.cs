using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections;
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
        public int PhyId { get; set; }
        public int? calltype { get; set; }
        public BitArray? isfinal { get; set; }

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

        public int? ChatWith { get; set; } //-- nahi aave

        public string Physician { get; set; } //phy

        public DateTime DateOfService { get; set; } //nathi aapi

        public string? Region { get; set; } //region

        public int Status { get; set; } //req

        public int PgCount { get; set; }

        public List<Admin> adminData { get; set; }
    }

    public class ChatViewModel    {        public int ChatId { get; set; }        public int AdminId { get; set; }        public int ProviderId { get; set; }        public int RequestId { get; set; }        public int RoleId { get; set; }        public string? Message { get; set; }        public string? ChatDate { get; set; }        public int SentBy { get; set; }        public string? ChatBoxClass { get; set; }        public string? RecieverName { get; set; }        public string? flag { get; set; }        public int GroupFlag { get; set; }        public int Reciever1 { get; set; }        public int Receiver2 { get; set; }        public List<ChatViewModel> Chats { get; set; }    }
}
