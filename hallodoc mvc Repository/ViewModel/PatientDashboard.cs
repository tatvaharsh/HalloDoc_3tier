using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class PatientDashboard
    {
        public List<Request> requests { get; set; }

        public List<RequestWiseFile> rwfiles { get; set; }

        public List<Admin> adminData { get; set; }
    }
}
