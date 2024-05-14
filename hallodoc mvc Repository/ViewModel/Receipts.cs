using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class Receipts
    {
        public int ReimburseId { get; set; }

        public DateTime ReceiptDate { get; set; }

        public int PhysicianId { get; set; }

        public string Item { get; set; } = String.Empty;

        public int Amount { get; set; }

        public IFormFile File { get; set; } = null!;

        public string FileName { get; set; } = String.Empty;
    }
}
