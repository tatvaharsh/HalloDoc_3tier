using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentRepository.ViewModel
{
    public class UserModel
    {
   

        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public required string LastName { get; set; }

        public int? Id { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public required string PhoneNumber { get; set; }


        public string? Country { get; set; }
        public string? City { get; set; }
        public int? Age { get;set; }

        public string? Gendermale { get; set; }
        public string? Genderfemale { get; set; }
        public string? Genderother { get; set; }

        public  DateOnly BirthDate { get; set; }

        public int? PgCount { get; set; }
    }
}
