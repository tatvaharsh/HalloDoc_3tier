using hallodoc_mvc_Repository.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class CreatePhy
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
        public string? Businessname { get; set; }
        public string? Adminnote { get; set; }
        public string? Businesswebsite { get; set; }
        public string? syncemail { get; set; }
        public string? city { get; set; }
     
        public string? zipcode { get; set; }
        [Required(ErrorMessage = "Select any Role")]
        public int? SelectedRoleId { get; set; }    
        public int? SelectedStateId { get; set; }
        public List<int>? SelectedRegions { get; set; }

        public IFormFile? HIPAA { get; set; }
        public IFormFile? NonDisclosureDoc { get; set; }
        public IFormFile? BackgroundDoc { get; set; }
        public IFormFile? AgreementDoc { get; set; }
        public IFormFile? Photo { get; set; }

        public IFormFile? Signature { get; set; }
        public IFormFile? LicenseDoc { get; set; }
        public bool islisence { get; set; }

        public string? pic { get; set; }
        public string? SignatureCheck { get; set; }
        public bool ishippa { get; set; }
        public bool isnonclosure { get; set; }
        public bool isagreement { get; set; }
        public bool isbackground { get; set; }


        public decimal? lat { get; set; }
        public decimal? log { get; set; }
    }
}
