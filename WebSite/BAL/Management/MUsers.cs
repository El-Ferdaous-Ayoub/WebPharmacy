using Classes;
using Classes.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace BAL
{
    public class MUsers
    {
        public User Login(String UserName,String Password)
        {
            return Get_Data.Login(UserName,Password);
        }

        public User Get(String UserName)
        {
            return Get_Data.Get_User(UserName);
        }

        public User GetByNIC(String NIC)
        {
            return Get_Data.GetUserByNIC(NIC);
        }

        public List<UserModel> GetUEmployees()
        {
            var ems = new MEmployees().Get_All();
            var list = from i in Get_Data.Get_Users()
                       where !String.IsNullOrEmpty(i.Rank) && i.Rank.CompareTo("Employee") == 0
                       join e in ems
                       on i.NIC equals e.ID
                       select new UserModel
                       {
                           UserName = i.UserName,
                           Photo = i.Photo,
                           NIC = i.NIC,
                           FirstName = i.FirstName,
                           LastName = i.LastName,
                           Gender = i.Gender,
                           Phone = i.Phone,
                           Email = i.Email,
                           Address = i.Address,
                           City = i.City,
                           District = i.District,
                           Country = i.Country,
                           Departement_Title = e.Department_Title,
                           Health_Coverage = e.Health_Coverage
                       };
            return list.ToList();
        }

        public List<User> Get_All()
        {
            var list = from i in Get_Data.Get_Users()
                    select new User
                    {
                      NIC =  i.NIC,
                      FirstName =  i.FirstName,
                      LastName=  i.LastName,
                      Gender = i.Gender,
                      Phone = i.Phone,
                      Email = i.Email,
                      Rank = i.Rank,
                      Country = i.Country
                    };
            return list.ToList();
        }

        public void Update(User user)
        {
            User org = Get(user.UserName);
            if (org == null) throw new Exception($"User ({user.UserName}) is Not Exist");
            Management.Detach(org);
            Management.Update(user);
        }

        public void Add(User user)
        {
            if (Get(user.UserName) != null) throw new Exception($"User ({user.UserName}) is Not Exist");
            Management.Add(user);
        }

        public void Remove(String UserName)
        {
            User org = Get(UserName);
            if (org == null) throw new Exception($"User ({UserName}) is Not Exist");
            Management.Remove(org);
        }

        public void NewUserName(String Orgusername,String newusername)
        {
            User org = Get(Orgusername);
            if (org == null) throw new Exception($"User ({Orgusername}) is Not Exist");

            Employee employee = new MEmployees().Get(org.NIC);
            if (employee != null)
            {
                employee.UserName = newusername;
                new MEmployees().Update(employee);
            }
            Management.Remove(org);
        }
    }
}
