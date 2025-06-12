using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Utility.Helpers
{
    public class SessionHelper
    {
        /// <summary>
        /// 設定session
        /// </summary>
        /// <param name="name">session 名</param>
        /// <param name="val">session 值</param>
        public static void SetSession(string name, object val)
        {
            HttpContext.Current.Session.Remove(name);
            HttpContext.Current.Session.Add(name, val);
        }

        /// <summary>
        /// 取得Session (回傳object)
        /// </summary>
        /// <param name="name">Session名稱</param>
        /// <returns></returns>
        public static object GetSession(string name)
        {
            return HttpContext.Current.Session[name];
        }

        /// <summary>
        /// 取得Session (回傳字串)
        /// </summary>
        /// <param name="name">Session名稱</param>
        /// <returns>Session</returns>
        public static string Get(string name)
        {
            if (HttpContext.Current.Session[name] == null)
                return null;
            else
                return HttpContext.Current.Session[name].ToString();
        }

        /// <summary>
        /// 刪除Session
        /// </summary>
        /// <param name="strSessionName">Session名稱</param>
        public static void Del(string name)
        {
            HttpContext.Current.Session[name] = null;
        }
    }
}