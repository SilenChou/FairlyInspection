using FairlyInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Repositories
{
    public class CheckingPointRepository : GenericRepository<CheckingPoint>
    {
        public CheckingPointRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<CheckingPoint> Query(bool? status = null, int pathId = 0, string title = "")
        {
            var query = GetAll();

            if (status.HasValue)
            {
                query = query.Where(q => q.IsOnline == status);
            }
            if (pathId != 0)
            {
                query = query.Where(q => q.PathID == pathId);
            }
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.Title.Contains(title));
            }

            return query;
        }

        public CheckingPoint FindBy(int id)
        {
            return db.CheckingPoint.Find(id);
        }
    }
}