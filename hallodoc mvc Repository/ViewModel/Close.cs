using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class Close
    {
        public int RequestId { get; set; }
        public string? Fname { get; set; }
        public string? Lname { get; set; }
        public string? Phone { get; set; } = "ABDHSGb";
        public string? Email { get; set; }
        public List<IFormFile>? File { get; set; }
        public  DateOnly? BirthDate { get; set; }
        public DateTime Uploaddate { get; set; }

        public int RequestWiseId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
     
        public string? FileName { get; set; }
        public int? FileId { get; set; }
        public string Confirmationnumber { get; set; }

        public List<ViewDocument> viewDocuments { get; set; }
    }
}
