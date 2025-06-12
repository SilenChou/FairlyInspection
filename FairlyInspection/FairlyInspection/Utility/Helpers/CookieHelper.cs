using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Utility.Helpers
{
    public class CookieHelper
    {
        /// <summary>
        /// 設定Cookie
        /// </summary>
        /// <param name="name">session 名</param>
        /// <param name="val">session 值</param>       
        public static void SetCookie(string name, object val, DateTime? expires = null)
        {
            var cookie = new HttpCookie(name, val.ToString());
            if (expires != null)
            {
                DateTime dt = (DateTime)expires;
                cookie.Expires = dt;
            }

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        public static object GetCookie(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        public static string Get(string name)
        {
            if (HttpContext.Current.Request.Cookies[name] == null)
                return null;
            else
                return HttpContext.Current.Request.Cookies[name].Value;
        }

        public static void Del(string name)
        {
            DateTime dt = DateTime.Now.AddDays(-1);
            SetCookie(name, "", dt);
        }
    }
}