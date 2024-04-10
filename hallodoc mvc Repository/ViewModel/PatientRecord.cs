using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class PatientRecord
    {
        public int rid { get; set; }
        public string Name { get; set; }
        public string createdDate { get; set; }
        public string conNo { get; set; }
        public string phyName { get; set; }
        public string concludeDate { get; set; }
        public string status { get; set; }
        public int docNo { get; set; }
    }
}
