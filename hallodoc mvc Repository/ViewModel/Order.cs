using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class Order
    {
        public List<HealthProfessional>? HealthProfessionals { get; set; }

        public List<HealthProfessionalType>? HealthProfessionalType { get; set; }

        public int? SelectedVendorId { get; set; }
        public String? SelectedVendorname { get; set; }
        public String? Businesscontact { get; set; }
        public String? Email { get; set; }
        public String? Fax { get; set; }
        public String? Detail { get; set; }
        public int? refill { get; set; }
    }
}
