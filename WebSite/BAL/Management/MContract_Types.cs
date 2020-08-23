using Classes;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL
{
    public class MContract_Types
    {
        public Contract_Type Get(String C)
        {
            return Get_Data.Get_Contract_Type(C);
        }

        public List<Contract_Type> Get_All()
        {
            return Get_Data.Get_Contract_Types().ToList();
        }

        public void Update(Contract_Type C)
        {
            Contract_Type org = Get(C.Title);
            if (org == null) throw new Exception($"This Type of Contract {C.Title} is Not Exist");
            Management.Detach(org);
            Management.Update(C);
        }

        public void Add(Contract_Type C)
        {
            if (Get(C.Title) != null) throw new Exception($"This Type of Contract {C.Title} is Aready Exist");
            Management.Add(C);
        }

        public void Delete(String Title)
        {
            Contract_Type org = Get(Title);
            if (org == null) throw new Exception($"This Type of Contract {Title} is Not Exist");
            var mc = new MContrats();
            foreach (var item in mc.Get_All())
            {
                if (item.Type.CompareTo(Title) == 0)
                {
                    item.Type = "Unknown";
                    mc.Update(item);
                }
            }
            Management.Remove(org);
        }

        public void NewTitle(String org, Contract_Type contrat_Type)
        {
            Contract_Type ct_org = Get(org);
            if (ct_org == null) throw new Exception($"This Type of Contract {org} is Not Exist");
            Add(contrat_Type);
            var mc = new MContrats();
            foreach (var item in mc.Get_All())
            {
                if (item.Type.CompareTo(org) == 0)
                {
                    item.Type = contrat_Type.Title;
                    mc.Update(item);
                }
            }
            Management.Remove(ct_org);
        }
    }
}