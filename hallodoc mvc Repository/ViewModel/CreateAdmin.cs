using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class CreateAdmin
    {
        public int id { get; set; }
        public List<Region>? reg { get; set; }
        public List<Role>? roles { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        public string Firstname { get; set; }
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        public string Lastname { get; set; }
        public string email { get; set; }
        public string alterphone { get; set; }
        public string phone { get; set; }
        public string medicallicence { get; set; }
        public string npi { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? city { get; set; }
        public string? zipcode { get; set; }
   
        public int? SelectedRoleId { get; set; }
        public List<int>? SelectedRegions { get; set; }
        public int? SelectedStateId { get; set; }
    }
}
