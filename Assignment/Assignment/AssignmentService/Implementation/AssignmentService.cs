using AssignmentRepository.DataModels;
using AssignmentRepository.Interface;
using AssignmentRepository.ViewModel;
using AssignmentService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AssignmentService.Implementation
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _Repo;

        public AssignmentService(IAssignmentRepository repo)
        {
            _Repo = repo;
        }



        public void AddUser(UserModel model)
        {
            User user1 = _Repo.CheckEmail(model.Email);
            if (user1 == null)
            {
                var citi = _Repo.Checkciti(model.City);
                if (citi == null)
                {
                    City city = new()
                    {
                        Name = model.City,
                    };
                    _Repo.AddCityTbl(city);

                    User user = new()
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        City = model.City,
                        CityId = city.Id,
                        PhoneNo = model.PhoneNumber,
                        Gender = model.Gendermale,
                        Country = model.Country,
                        Age = DateTime.Now.Year - model.BirthDate.Year,

                };
                    _Repo.AddUser(user);
                }
                else
                {
                    User user = new()
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        City = model.City,
                        CityId = citi.Id,
                        PhoneNo = model.PhoneNumber,
                        Gender = model.Gendermale,
                        Country = model.Country,
                        Age = DateTime.Now.Year - model.BirthDate.Year,
                    };
                    _Repo.AddUser(user);
                }

            }
            else
            {

                var citi = _Repo.Checkciti(model.City);
                if (citi == null)
                {
                    City city = new()
                    {
                        Name = model.City,
                    };
                    _Repo.AddCityTbl(city);

                    user1.Email = model.Email;
                    user1.FirstName = model.FirstName;  
                    user1.LastName = model.LastName;
                    user1.Country = model.Country;
                    user1.City = model.City;
                    user1.PhoneNo = model.PhoneNumber;
                    user1.Gender = model.Gendermale;
                    user1.CityId = city.Id;
                    user1.Age = DateTime.Now.Year-model.BirthDate.Year;
                    _Repo.UpdateTbl(user1);
                }
                else
                {
                    user1.Email = model.Email;
                    user1.FirstName = model.FirstName;
                    user1.LastName = model.LastName;
                    user1.Country = model.Country;
                    user1.City = model.City;
                    user1.PhoneNo = model.PhoneNumber;
                    user1.Gender = model.Gendermale;
                    user1.CityId = citi.Id;
                    user1.Age = DateTime.Now.Year - model.BirthDate.Year;
                    _Repo.UpdateTbl(user1);
                  
                }

            }

        }

        public void Delete(int id)
        {
            User user = _Repo.SelectedUser(id);
            user.IsDeleted = true;
            _Repo.UpdateTbl(user);
        }

        public List<UserModel> GetData(string search, int pageid)
        {
            var data = _Repo.AllDataUser();
            int size = 3;
            var a = data.Skip(pageid * size - size).Take(size).ToList();
            int c = data.Count();
            var user = new List<UserModel>();
            foreach (var item in a)
            {
                user.Add(new UserModel
                {
                    FirstName = item.FirstName ?? "",
                    LastName = item.LastName ?? "",
                    City = item.City,
                    Email = item.Email ?? "",
                    PhoneNumber = item.PhoneNo ?? "",
                    Gendermale = item.Gender ?? "",
                    Id = item.Id,
                    PgCount = c,
                  
            });
            }
         
            if (search != null)
            { user = user.Where(x => x.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList(); pageid = 1; }

            return user;
        }

        public UserModel GetSelectedData(int id)
        {
            User user = _Repo.SelectedUser(id);
            UserModel model = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                Country = user.Country,
                Email = user.Email,
                Gendermale = user.Gender,
                PhoneNumber = user.PhoneNo,
            };
            return model;
        }
    }
}
