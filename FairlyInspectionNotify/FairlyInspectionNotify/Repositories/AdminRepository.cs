using FairlyInspectionNotify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspectionNotify.Repositories
{
    public class AdminRepository : GenericRepository<Admin>
    {
        //private FairlyInspectionDBEntity db;

        //public AdminRepository() : this(null) { }

        //public AdminRepository(FairlyInspectionDBEntity context)
        //{
        //    db = context ?? new FairlyInspectionDBEntity();
        //}

        public AdminRepository(InspectionEntities context) : base(context) { }

        public IQueryable<Admin> Query(bool? status, int type = 0, string account = "")
        {
            var query = GetAll();
            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status);
            }
            if (type != 0)
            {
                query = query.Where(q => q.Type == type);
            }
            if (!string.IsNullOrEmpty(account))
            {
                query = query.Where(q => q.Account.Contains(account));
            }
            return query;
        }
      
    }
}