using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class ViewCase
    {
   
        public string? ConfirmationNO { get; set; }
        public string? PatientNotes { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PatientRegion { get; set; }
        public string? AddressORBusinessName { get; set; }
        public string? Room { get; set; }
        public short? Status { get; set; }
        public int ReqType { get; set; }
        public int? ReqId { get; set; }
    }
}
