using AssignmentRepository.DataContext;
using AssignmentRepository.DataModels;
using AssignmentRepository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentRepository.Implementation
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly IAssignmentRepository _Repo;

        private readonly ApplicationDbContext _DbContext;

        public AssignmentRepository(ApplicationDbContext context)
        {
            _DbContext = context;
        }

        public void AddCityTbl(City city)
        {
           _DbContext.Cities.Add(city);
            _DbContext.SaveChanges();   
        }

        public void AddUser(User user)
        {
            _DbContext.Users.Add(user);
            _DbContext.SaveChanges();
        }

        public List<User> AllDataUser()
        {
           return _DbContext.Users.Where(x=>x.IsDeleted!=true).ToList();
        }

        public City Checkciti(string? city)
        {
            return _DbContext.Cities.FirstOrDefault(x => x.Name == city);
        }

        public User CheckEmail(string email)
        {
            return _DbContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public User SelectedUser(int id)
        {
            return _DbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateTbl(User user)
        {
            _DbContext.Users.Update(user);
            _DbContext.SaveChanges();

        }
    }
}
