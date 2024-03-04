using System.ComponentModel.DataAnnotations;


namespace hallocdoc_mvc_Service.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "UserName is incorrect")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is incorrect")]
        public string Passwordhash { get; set; }
    }
}
