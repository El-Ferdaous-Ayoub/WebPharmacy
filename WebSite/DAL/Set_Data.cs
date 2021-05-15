using Classes;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace DAL
{
    public static class Management
    {
        

        public static void Add(Object O)
        {
            InitialContext.db.Entry(O).State = EntityState.Added;
            Save();
        }

        public static void Remove(Object O)
        {
            InitialContext.db.Entry(O).State = EntityState.Deleted;
            Save();
        }

        public static void Update(Object O)
        {
            InitialContext.db.Entry(O).State = EntityState.Modified;
            Save();
        }

        public static void Detach(Object O)
        {
            InitialContext.db.Entry(O).State = EntityState.Detached;
        }

        public static void Save()
        {
                InitialContext.db.SaveChanges();
        }

    }
}
