using Classes;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class MEmployees
    {
        public Employee Get(int id)
        {
            return Get_Data.Get_Employee(id);
        }

        public Employee GetByNIC(String nic)
        {
            return Get_Data.GetEmployeeByNIC(nic);
        }

        public List<Employee> Get_All()
        {
            return Get_Data.Get_Employees().ToList();
        }

        public void Update(Employee employee)
        {
            Employee org = Get(employee.ID);
            if (org == null) throw new Exception($"The Employee is Not Exist");
            var emp = GetByNIC(employee.NIC);
            if(emp != null && emp.ID != org.ID) throw new Exception($"Employee ({employee.NIC}) is Aready Exist");
            Management.Detach(org);
            Management.Update(employee);
        }

        public void Add(Employee employee)
        {
            if (Get(employee.ID) != null) throw new Exception($"Employee ({employee.ID}) is Aready Exist");
            Management.Add(employee);
        }

        public void Remove(int ID)
        {
            Employee org = Get(ID);
            if (org == null) throw new Exception($"Employee is Not Exist");
            var contract = new MContrats().GetEmpContract(ID);
            if(contract != null)
            new MContrats().Remove(contract.ID);
            Management.Remove(org);
        } 
    }
}
