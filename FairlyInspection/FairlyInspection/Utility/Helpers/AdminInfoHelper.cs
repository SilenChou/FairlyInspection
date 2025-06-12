using FairlyInspection.Models.Others;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FairlyInspection.Utility.Helpers
{
    public class AdminInfoHelper
    {
        /// <summary>
        /// 儲存登入資訊
        /// </summary>
        /// <param name="member"></param>
        /// <param name="isRemeber"></param>
        /// <returns></returns>
        public static void Login(AdminInfo adminInfo, bool isRemeber)
        {
            var now = DateTime.Now;
            string userData = JsonConvert.SerializeObject(adminInfo);

            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: adminInfo.ID.ToString(),
                issueDate: now,
                expiration: now.AddYears(30),
                isPersistent: isRemeber,
                userData: userData,
                cookiePath: FormsAuthentication.FormsCookiePath);

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            CookieHelper.SetCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        }

        /// <summary>
        /// 登出
        /// </summary>
        public static void Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            CookieHelper.Del(FormsAuthentication.FormsCookieName);
        }

        /// <summary>
        /// 取得AdminInfo
        /// </summary>
        /// <returns></returns>
        public static AdminInfo GetAdminInfo()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                // 先取得該使用者的 FormsIdentity
                FormsIdentity id = HttpContext.Current.User.Identity as FormsIdentity;
                string userData = id.Ticket.UserData;
                AdminInfo user = JsonConvert.DeserializeObject<AdminInfo>(userData);

                return user;
            }
            return null;
        }
    }
}