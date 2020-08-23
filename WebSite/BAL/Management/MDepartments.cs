using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;
using DAL;

namespace BAL
{
    public class MDepartments
    {
        public Department Get(String Title)
        {
            return Get_Data.Get_Department(Title);
        }

        public List<Department> Get_All()
        {
            return Get_Data.Get_Departments().ToList();
        }

        public void Update(Department department)
        {
            Department org = Get(department.Title);
            if (org == null) throw new Exception($"This Department ({department.Title}) is Not Exist");
            Management.Detach(org);
            Management.Update(department);
        }

        public void Add(Department department)
        {
            if (Get(department.Title) != null) throw new Exception("This Department is Aready Exist");
            Management.Add(department);
        }

        public void Remove(String Title)
        {
            Department org = Get(Title);
            if (org == null) throw new Exception($"Department ({Title}) is Not Exist");
            Role role = Get_Data.GetRole(Title);
            var me = new MEmployees();
            foreach (var item in me.Get_All())
            {
                if (item.Department_Title.CompareTo(Title) == 0)
                {
                    item.Department_Title = "Unknown";
                    me.Update(item);
                }
            }
            Management.Remove(role);
            Management.Remove(org);
        }

        public void NewTitle(String Org, Department NewDepartment)
        {
            Department org = Get(Org);
            if (org == null) throw new Exception($"This Department ({org}) is Not Exist");
            Management.Add(NewDepartment);
            foreach (var item in Get_Data.Get_Employees()
                .Where(i=>i.Department_Title.CompareTo(Org) == 0))
            {
                item.Department_Title = NewDepartment.Title;
                new MEmployees().Update(item);
            }
            Role orgrole = Get_Data.GetRole(Org);
            Role role = new Role()
            {
                Department_Title = NewDepartment.Title,
                Producers = orgrole.Producers,
                Products = orgrole.Products,
                Categories = orgrole.Categories,
                Orders = orgrole.Orders,
                Employees = orgrole.Employees,
                ContractTypes = orgrole.ContractTypes,
                Departments = orgrole.Departments
            };
            Management.Add(role);
            Management.Remove(orgrole);
            Management.Remove(org);
         }

        public void AddRole(Role role)
        {
            Management.Add(role);
        }

        public Role GetRole(String Department)
        {
            return Get_Data.GetRole(Department);
        }

        public void UpdateRole(Role role)
        {
            Role org = GetRole(role.Department_Title);
            if (org == null) throw new Exception();
            Management.Detach(org);
            Management.Update(role);
        }
    }
}
