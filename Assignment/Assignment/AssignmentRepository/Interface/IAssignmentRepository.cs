using AssignmentRepository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentRepository.Interface
{
    public interface IAssignmentRepository
    {
        void AddCityTbl(City city);
        void AddUser(User user);
        List<User> AllDataUser();
        City Checkciti(string? city);
        User CheckEmail(string email);
        User SelectedUser(int id);
        void UpdateTbl(User user);
    }
}
