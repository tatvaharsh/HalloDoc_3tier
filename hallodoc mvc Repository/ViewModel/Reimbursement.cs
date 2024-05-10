using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class Reimbursement
    {
        public int PhysicianId { get; set; }

        public int? ReId { get; set; }

        public bool IsFinalized { get; set; } = false;

        public DateTime Date { get; set; }

        public string? Item { get; set; }
        public string? Filename { get; set; }
        public int Amount { get; set; }
        public IFormFile FormFile { get; set; } 
      
    }
}
