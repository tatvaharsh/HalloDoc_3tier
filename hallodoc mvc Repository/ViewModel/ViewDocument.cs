//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class ViewDocument
    {
        public int RequestId { get; set; }
        public int RequestWiseId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<IFormFile>? File { get; set; }
        public DateTime Uploaddate { get; set; }
        public string? FileName { get; set; }
        public int? FileId { get; set; }
        public string Confirmationnumber { get; set; }

        public string? Notes { get; set; }
        public List<UploadedFiles> AllFiles { get; set; }

    }

    public class UploadedFiles
    {
        public DateTime Uploaddate { get; set; }
        public string? FileName { get; set; }
        public int? FileId { get; set; }
    }
}
