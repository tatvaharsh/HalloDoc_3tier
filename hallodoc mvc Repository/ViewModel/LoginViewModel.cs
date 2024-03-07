using System.ComponentModel.DataAnnotations;


namespace hallodoc_mvc_Repository.ViewModel
{
    public class LoginViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "UserName is incorrect")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is incorrect")]
        public string Passwordhash { get; set; }
    }
}
