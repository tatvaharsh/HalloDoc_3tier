using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class Create
    {
        [Key]
        public int? Id { get; set; }

        [StringLength(256)]
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; } = null!;

        public string? PasswordHash { get; set; }


    }
}
