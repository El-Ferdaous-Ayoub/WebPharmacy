using Classes;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class MContrats
    {
        public Contract Get(int ID)
        {
            return Get_Data.Get_Contract(ID);
        }

        public List<Contract> Get_All()
        {
            return Get_Data.Get_Contracts().ToList();
        }

        public void Update(Contract contrat)
        {
            Contract org = Get(contrat.ID);
            if (org == null) throw new Exception($"Contract Not Exist");
            Management.Detach(org);
            Management.Update(contrat);
        }

        public void Add(Contract contrat)
        {
            if (Get(contrat.Employee_ID) != null) throw new Exception($"Contract Aready Exist");
            Management.Add(contrat);
        }

        public void Remove(int ID)
        {
            Contract org = Get(ID);
            if (org != null) 
            Management.Remove(org);
        }

        public Contract GetEmpContract(int empid)
        {
            return Get_Data.Get_Contracts().Where(item => item.Employee_ID == empid).FirstOrDefault();
        }
    }
}
