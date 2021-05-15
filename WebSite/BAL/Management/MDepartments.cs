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
        public Department Get(int id)
        {
            return Get_Data.Get_Department(id);
        }

        public List<Department> Get_All()
        {
            return Get_Data.Get_Departments().ToList();
        }

        public void Update(Department department)
        {
            Department org = Get(department.ID);
            if (org == null) throw new Exception($"This Department ({department.Title}) is Not Exist");
            var dep = Get(department.Title);
            if (dep != null && dep.ID != org.ID)
                throw new Exception($"This Department ({department.Title}) is Aready Exist");
            Management.Detach(org);
            Management.Update(department);
        }

        public Department Get(string title)
        {
            return Get_Data.Get_Departments().Where(item => item.Title.CompareTo(title) == 0).FirstOrDefault();
        }

        public void Add(Department department)
        {
            if (Get(department.Title) != null) throw new Exception("This Department is Aready Exist");
            Management.Add(department);
        }

        public void Remove(int id)
        {
            Department org = Get(id);
            if (org == null) throw new Exception($"Department Not Exist");

            var me = new MEmployees();
            var ud = GetUNDepartment().ID;
            var lst = new List<Employee> (me.Get_All());
            foreach (var item in lst)
            {
                if (item.Department_ID == id)
                {
                    item.Department_ID = ud;
                    me.Update(item);
                }
            }
            Management.Remove(org);
        }

        public Department GetUNDepartment()
        {
            return Get_All().Where(i => i.Title.CompareTo("Unknown") == 0).FirstOrDefault();
        }
    }
}
