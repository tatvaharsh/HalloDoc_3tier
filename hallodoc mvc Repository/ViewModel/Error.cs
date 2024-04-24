using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class Error
    {
        public string? Message { get; set; }

        public string? Path { get; set; }

        public string? Stack { get; set; }
    }
}
