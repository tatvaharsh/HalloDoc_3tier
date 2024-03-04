using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallocdoc_mvc_Service.ViewModel
{
    public class Create
    {
        [Key]
        public int Id { get; set; }

        [StringLength(256)]
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; } = null!;

        public string? PasswordHash { get; set; }


    }
}
