using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspectionNotify.Interface
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();

        TEntity GetById(object id);

        int Insert(TEntity newData, bool rtnId = true);

        void Update(TEntity newData, bool rtnId = true);

        void Delete(int id);        
    }
}