using System.ComponentModel.DataAnnotations;


namespace hallodoc_mvc_Repository.ViewModel
{
    public class LoginViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "UserName is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Passwordhash { get; set; }
    }
}
