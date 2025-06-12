using FairlyInspectionNotify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlyInspectionNotify.Repositories
{
    class CheckingPathRepository : GenericRepository<CheckingPath>
    {
        public CheckingPathRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<CheckingPath> Query(bool? status = null, string title = "", bool? isPending = null)
        {
            var query = GetAll();

            if (status.HasValue)
            {
                query = query.Where(q => q.IsOnline == status);
            }
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.Title.Contains(title));
            }
            if (isPending.HasValue)
            {
                DateTime today = DateTime.Now;
                query = query.Where(q => q.NextDate <= today);
            }

            return query;
        }

        public CheckingPath FindBy(int id)
        {
            return db.CheckingPath.Find(id);
        }
    }
}
