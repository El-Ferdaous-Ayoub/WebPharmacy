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
        public Employee Get(String NIC)
        {
            return Get_Data.Get_Employee(NIC);
        }

        public List<Employee> Get_All()
        {
            return Get_Data.Get_Employees().ToList();
        }

        public void Update(Employee employee)
        {
            Employee org = Get(employee.ID);
            if (org == null) throw new Exception($"Employee ({employee.ID}) is Not Exist");
            Management.Detach(org);
            Management.Update(employee);
        }

        public void Add(Employee employee)
        {
            if (Get(employee.ID) != null) throw new Exception($"Employee ({employee.ID}) is Aready Exist");
            Management.Add(employee);
        }

        public void Remove(String ID)
        {
            Employee org = Get(ID);
            if (org == null) throw new Exception($"Employee ({ID}) is Not Exist");
            new MUsers().Remove(org.UserName);
            new MContrats().Remove(ID);
            Management.Remove(org);
        }

        public void NewNIC(String orgNIC,Employee employee)
        {
            Employee org = Get(orgNIC);
            if (org == null) throw new Exception($"Employee ({orgNIC}) is Not Exist");
            Add(employee);
            Contract orgContract = new MContrats().Get(orgNIC);
            if (orgContract != null)
            {
                Contract contract = new Contract()
                {
                    Employee_NIC = employee.ID,
                    Start = orgContract.Start,
                    End = orgContract.End,
                    Document = orgContract.Document
                };
                new MContrats().NewNIC(orgNIC, contract);
            }
            Management.Remove(org);
        }
    }
}
