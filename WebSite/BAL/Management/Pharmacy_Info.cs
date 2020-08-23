using Classes;
using DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL
{
    public class Pharmacy_Info
    {
        public Pharmacy Get()
        {
            return Get_Data.GetPharmacy();
        }

        public void Add(Pharmacy pharmacy)
        {
            Management.Add(pharmacy);
        }

        public void Remove(Pharmacy pharmacy)
        {
            Pharmacy org = Get_Data.GetPharmacy();
            if(org == null) throw new Exception($"There is No Pharmacy is Added Yet");
            Management.Remove(pharmacy);
        }

        public void Update(Pharmacy pharmacy)
        {
            Pharmacy org = Get();
            if (org == null) throw new Exception($"There is No Pharmacy is Added Yet");
            Management.Detach(org);
            Management.Update(pharmacy);
        }

        public void NewName(Pharmacy pharmacy)
        {
            Pharmacy org = Get();
            Add(pharmacy);
            Remove(org);
        }
    }
}
