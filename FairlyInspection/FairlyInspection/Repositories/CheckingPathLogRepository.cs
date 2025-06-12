using FairlyInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Repositories
{
    public class CheckingPathLogRepository : GenericRepository<CheckingPathLog>
    {
        public CheckingPathLogRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<CheckingPathLog> Query(bool? isCompleted = null, int pathId = 0, string title = "", string name = "", string personId = "")
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
            if (!string.IsNullOrEmpty(title))
            {
                var pathIdList = db.CheckingPath.Where(q => q.Title.Contains(title)).Select(q => q.ID).ToList();
                query = query.Where(q => pathIdList.Contains(q.PathID));
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(q => q.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(personId))
            {
                query = query.Where(q => q.PersonId.Contains(personId));
            }

            return query;
        }

        public CheckingPathLog FindBy(int id)
        {
            return db.CheckingPathLog.Find(id);
        }
    }
}