using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlyInspection.ActionFilters
{
    /// <summary>
    /// 前台通用錯誤處裡（僅記錄例外訊息）
    /// </summary>
    public class ErrorHandleActionFilter : FilterAttribute, IExceptionFilter
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                Exception ex = filterContext.Exception;
                string msg = string.Format("\r\n【StackTrace】:{0}\r\n【Message】:{1}", ex.StackTrace, ex.Message);
                logger.Error(msg);
            }
        }
    }
}