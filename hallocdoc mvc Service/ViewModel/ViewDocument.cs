//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace hallocdoc_mvc_Service.ViewModel
{
    public class ViewDocument
    {
        public int RequestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<IFormFile>? File { get; set; }
    }
}
