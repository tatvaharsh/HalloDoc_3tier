using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class Emaillogs
    {
        public int EmailLogId { get; set; }
        public string Recipient { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string Mobile { get; set; } = null!;

        public string RoleName { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public DateTime? SentDate { get; set; }

        public bool? IsSent { get; set; }

        public int? SentTries { get; set; }

        public string? ConfirmationNumber { get; set; }
        public int? PgCount { get; set; }
    }
}
