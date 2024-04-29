using AssignmentRepository.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentService.Interface
{
    public interface IAssignmentService
    {
        void AddUser(UserModel model);
        void Delete(int id);
        List<UserModel> GetData(string search,int page);
        UserModel GetSelectedData(int id);

        List<string> CityNames();
    }
}
