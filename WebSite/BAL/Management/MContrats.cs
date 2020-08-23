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
        public Contract Get(String NIC)
        {
            return Get_Data.Get_Contract(NIC);
        }

        public List<Contract> Get_All()
        {
            return Get_Data.Get_Contracts().ToList();
        }

        public void Update(Contract contrat)
        {
            Contract org = Get(contrat.Employee_NIC);
            if (org == null) throw new Exception($"Employee's ({contrat.Employee_NIC}) Contract is Not Exist");
            Management.Detach(org);
            Management.Update(contrat);
        }

        public void Add(Contract contrat)
        {
            if (Get(contrat.Employee_NIC) != null) throw new Exception($"Employee's ({contrat.Employee_NIC}) Contract is Aready Exist");
            Management.Add(contrat);
        }

        public void Remove(String NIC)
        {
            Contract org = Get(NIC);
            if (org != null) 
            Management.Remove(org);
        }

        public void NewNIC(String orgNIC, Contract contract)
        {
            Contract org = Get(orgNIC);
            if (org == null) throw new Exception($"Employee's ({orgNIC}) Contract is Not Exist");
            Add(contract);
            Management.Remove(org);
        }
    }
}
