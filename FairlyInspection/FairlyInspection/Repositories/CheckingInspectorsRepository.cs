using FairlyInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Repositories
{
    public class CheckingInspectorsRepository : GenericRepository<CheckingInspectors>
    {
        public CheckingInspectorsRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<CheckingInspectors> Query(int pathId = 0, int accountId = 0, string name = "", string account = "", int type = 0)
        {
            var query = GetAll();

            if (pathId != 0)
            {
                query = query.Where(q => q.PathID == pathId);
            }
            if (accountId != 0)
            {
                query = query.Where(q => q.AccountID == accountId);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(q => q.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(account))
            {
                query = query.Where(q => q.Account.Contains(account));
            }
            if (type != 0)
            {
                query = query.Where(q => q.Type == type);
            }

            return query;
        }

        public CheckingInspectors FindBy(int id)
        {
            return db.CheckingInspectors.Find(id);
        }
    }
}