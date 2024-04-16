using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class Order
    {
        public List<HealthProfessional>? HealthProfessionals { get; set; }

        public List<HealthProfessionalType>? HealthProfessionalType { get; set; }

        [Required]
        public int? SelectedVendorId { get; set; }
        public int? SelectedId { get; set; }

        [Required]
        public string Businesscontact { get; set; }
        [Required]
        public string Email { get; set; } 

        [Required]
        public string Fax { get; set; } 


        public string? Detail { get; set; }

        [Required]
        public int? refill { get; set; }
    }
}
