using FairlyInspectionNotify.Interface;
using FairlyInspectionNotify.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FairlyInspectionNotify.Repositories.T8Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public T8ERPEntities Context { get; set; }

        public GenericRepository(T8ERPEntities context)
        {
            this.Context = context;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> FindBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>().Where(predicate);
            return query;
        }

        /// <summary>
        /// 新增一筆資料到資料庫。
        /// </summary>
        /// <param name="entity">要新增到資料的庫的Entity</param>
        public int Insert(TEntity entity, bool rtnId = true)
        {
            entity = setInsertData(entity);
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
            if (rtnId)
                return getEntityId(entity);
            else
                return 0;
        }

        public virtual TEntity GetById(object id)
        {
            var item = Context.Set<TEntity>().Find(id);
            return item;
        }

        public virtual void Update(TEntity entity, bool rtnId = true)
        {
            if (rtnId)
            {
                entity = setUpdateData(entity);
            }
            else
            {
                entity = setUpdateData(entity, true);
            }
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            TEntity entity = GetById(id);
            Context.Entry<TEntity>(entity).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        //private int getUpdaterId()
        //{
        //    AdminInfo adminInfo = AdminInfoHelper.GetAdminInfo();
        //    return adminInfo.ID;
        //}

        private int getEntityId(TEntity entity)
        {
            var idProperty = entity.GetType().GetProperty("ID").GetValue(entity, null);
            int id = (int)idProperty;
            return id;
        }
        private Guid getEntityGuidId(TEntity entity)
        {
            var idProperty = entity.GetType().GetProperty("ID").GetValue(entity, null);
            Guid id = (Guid)idProperty;
            return id;
        }


        private TEntity setInsertData(TEntity entity)
        {
            Type t = entity.GetType();
            var props = t.GetProperties();
            foreach (var p in props)
            {
                if (p.Name == nameof(EntityBase.UpdateTime) || p.Name == nameof(EntityBase.CreateTime) || p.Name == nameof(EntityBase.UpdateDate) || p.Name == nameof(EntityBase.CreateDate))
                {
                    p.SetValue(entity, DateTime.Now);
                }
                else if (p.Name == nameof(EntityBase.Updater) || p.Name == nameof(EntityBase.Creater))
                {
                    int adminId = 1;
                    if (p.PropertyType != props[0].PropertyType)
                    {
                        p.SetValue(entity, Convert.ToInt64(adminId));
                    }
                    else
                    {
                        p.SetValue(entity, adminId);
                    }
                }
                else
                {
                    //not handle
                }
            }
            return entity;
        }

        private TEntity setUpdateData(TEntity entity, bool isGuid = false)
        {
            TEntity newData;
            if (isGuid)
            {
                Guid id = getEntityGuidId(entity);

                newData = GetById(id);
            }
            else
            {
                int id = getEntityId(entity);

                newData = GetById(id);
            }

            Type t = newData.GetType();
            var props = t.GetProperties();
            foreach (var p in props)
            {
                PropertyInfo prop = t.GetProperty(p.Name);
                if (p.Name == nameof(EntityBase.UpdateTime) || p.Name == nameof(EntityBase.UpdateDate))
                {
                    prop.SetValue(newData, DateTime.Now);
                }
                else if (p.Name == nameof(EntityBase.Updater))
                {
                    int adminId = 1;

                    if (p.PropertyType != props[0].PropertyType)
                    {
                        prop.SetValue(newData, Convert.ToInt64(adminId));
                    }
                    else
                    {
                        prop.SetValue(newData, adminId);
                    }

                }
                else if (p.Name == nameof(EntityBase.Creater) || p.Name == nameof(EntityBase.CreateTime))
                {
                    //not handle
                }
                else
                {
                    prop.SetValue(newData, p.GetValue(entity));
                }
            }
            return newData;
        }
    }
}
