using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class RoleModel
    {
        public string? RoleName { get; set; }
        public List<int> RoleIds { get; set; }
        public List<RoleMenu> rolemenus { get; set; }
        public List<Menu> menu { get; set; }
        public int? SelectedRole { get; set; }
        public int RoleId { get; set; }
        public List<Role> rolesofphy { get; set; }  
    }
}
