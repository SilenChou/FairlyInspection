using FairlyInspectionNotify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspectionNotify.Repositories
{
    public class SequenceRepository : GenericRepository<SendSequence>
    {
        public SequenceRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<SendSequence> Query(bool? isSent, int nid = 0, int iid = 0)
        {
            var query = GetAll();

            if (isSent != null)
            {
                query = query.Where(p => p.IsSent == isSent);
            }
            if (nid != 0)
            {
                query = query.Where(p => p.NotifyID == nid);
            }
            if (iid != 0)
            {
                query = query.Where(p => p.InterlockID == iid);
            }

            return query.OrderBy(q => q.CreateDate);
        }

        public SendSequence FindBy(int id)
        {
            return db.SendSequence.Find(id);
        }

        public SendSequence GetByGuid(Guid id)
        {
            return db.SendSequence.Where(q => q.ID == id).FirstOrDefault();
        }

        public void DeleteByGuid(Guid id)
        {
            var query = db.SendSequence.Where(q => q.ID == id).FirstOrDefault();
            db.SendSequence.Remove(query);
            db.SaveChanges();
        }
    }
}