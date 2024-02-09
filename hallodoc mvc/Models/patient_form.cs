using System.ComponentModel.DataAnnotations;

namespace HalloDocMvc.ViewModel
{
    public class patient_form
    {
      

        [Required(ErrorMessage = "UserName is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public required string PhoneNumber { get; set; }

        public required string ZipCode { get; set; }

        public required string State { get; set; }

        public required string City { get; set; }

        public required string Street { get; set; }

        public required DateOnly BirthDate { get; set; }
    }
}