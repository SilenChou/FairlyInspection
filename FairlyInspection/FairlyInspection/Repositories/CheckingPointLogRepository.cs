using FairlyInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Repositories
{
    public class CheckingPointLogRepository : GenericRepository<CheckingPointLog>
    {
        public CheckingPointLogRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<CheckingPointLog> Query(bool? isCompleted = null, int pathId = 0, int pointId = 0, int itemId = 0, string title = "", int pathLogId = 0)
        {
            var query = GetAll();

            if (isCompleted.HasValue)
            {
                query = query.Where(q => q.IsCompleted == isCompleted);
            }
            if (pathId != 0)
            {
                query = query.Where(q => q.PathID == pathId);
            }
            if (pointId != 0)
            {
                query = query.Where(q => q.PointID == pointId);
            }
            if (itemId != 0)
            {
                query = query.Where(q => q.PointID == itemId);
            }
            if (pathLogId != 0)
            {
                query = query.Where(q => q.PathLogID == pathLogId);
            }

            return query;
        }

        public CheckingPointLog FindBy(int id)
        {
            return db.CheckingPointLog.Find(id);
        }
    }
}