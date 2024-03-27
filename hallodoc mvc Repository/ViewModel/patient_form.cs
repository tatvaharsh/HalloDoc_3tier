using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;


namespace hallodoc_mvc_Repository.ViewModel
{
    public class patient_form
    {
        [Required(ErrorMessage = "UserName is required")]
        public required int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public required string LastName { get; set; }

        public string? Password { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public required string PhoneNumber { get; set; }

        public  string? ZipCode { get; set; }

        public  string? State { get; set; }

        public  string? City { get; set; }

        public  string? Street { get; set; }
        [Required(ErrorMessage = "DOB is required")]
        public required DateOnly BirthDate { get; set; } 

        public List<IFormFile?>? File { get; set; }

        public int? UserId { get; set; }
        public int? Room { get; set; }
        public String? Symtom { get; set; }
        public String? adminnote { get; set; }
    }
}
















