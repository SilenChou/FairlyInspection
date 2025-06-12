using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Interface
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();

        TEntity GetById(object id);

        int Insert(TEntity newData) ;

        void Update(TEntity newData);

        void Delete(int id);        
    }
}