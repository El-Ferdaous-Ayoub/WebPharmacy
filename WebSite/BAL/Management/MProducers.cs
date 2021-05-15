using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;
using DAL;

namespace BAL
{
   public class MProducers
    {
        public Producer Get(int id)
        {
            return Get_Data.Get_Producer(id);
        }

        public Producer Get(String Name)
        {
            return Get_Data.Get_Producers().Where(item => item.Name.CompareTo(Name) == 0) .FirstOrDefault();
        }

        public List<Producer> Get_All()
        {
            return Get_Data.Get_Producers().ToList();
        }

        public void Add(Producer producer)
        {
            if (Get(producer.Name) != null) throw new Exception($"This Producer ({producer.Name}) is Aready Exist");
            Management.Add(producer);
        }

        public void Update(Producer producer)
        {
            Producer org = Get(producer.ID);
            if (org == null) throw new Exception($"Producer Not Exist");
            var pr = Get(producer.Name);
            if (pr != null && pr.ID != org.ID)
                throw new Exception($"This Producer ({producer.Name}) is Aready Exist");

            Management.Detach(org);
            Management.Update(producer);
        }

        public void Delete(int id)
        {
            Producer org = Get(id);
            if (org == null) throw new Exception($"Producer Not Exist");
            var mp = new MProducts();
            var up = Get_All().Where(i => i.Name.CompareTo("Unknown") == 0).First().ID;
            foreach (var item in mp.Get_All())
            {
                if (item.Producer_ID == id)
                {
                    item.Producer_ID = up;
                    mp.Update(item);
                }
            }
            Management.Remove(org);
        }

        public Producer GetUNProducer()
        {
            return Get_All().Where(i => i.Name.CompareTo("Unknown") == 0).FirstOrDefault();
        }
    }
}
