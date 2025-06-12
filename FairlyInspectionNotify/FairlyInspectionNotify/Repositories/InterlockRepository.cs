using FairlyInspectionNotify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspectionNotify.Repositories
{
    public class InterlockRepository : GenericRepository<Interlock>
    {
        public InterlockRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<Interlock> Query()
        {
            var query = GetAll();          

            return query;
        }

        public Interlock FindBy(int id)
        {
            return db.Interlock.Find(id);
        }

        public Interlock FindByJobID(string jobId)
        {
            return db.Interlock.Where(q => q.JobID == jobId).FirstOrDefault();
        }

    }
}