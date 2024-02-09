using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Models
{
    //checks whether we have provided input to the username and password fields
    public class LoginViewModel
    {
        [Required(ErrorMessage = "UserName is incorrect")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is incorrect")]
        public string Passwordhash { get; set; }
    }
}
