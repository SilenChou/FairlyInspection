using FairlyInspection.Models.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace FairlyInspection.Utility.Helpers
{
    public static class PageHelper
    {
        public static PageResult<T> ToPageResult<T>(this IQueryable<T> source, PageQuery param) where T : class
        {
            var result = new PageResult<T>();

            result.PageSize = param.PageSize;
            result.CurrentPage = param.CurrentPage;
            result.Total = source.Count(); //總筆數

            //計算總頁數
            double total = result.Total;
            int totalPages = 0;
            totalPages = Convert.ToInt16(Math.Ceiling(total / param.PageSize));
            result.TotalPages = totalPages;

            source = source.OrderBy(param.Sorting, param.IsDescending);

            var data = source.Skip((param.CurrentPage - 1) * param.PageSize).Take(param.PageSize);
            result.Data = data;
            result.Count = data.Count();

            return result;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, bool isDescending)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            MethodCallExpression resultExp;
            if (isDescending)
            {
                resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            }
            else
            {
                resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            }
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}