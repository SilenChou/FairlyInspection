using FairlyInspection.Models.Others;
using FairlyInspection.Repositories;
using FairlyInspection.Utility.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Utility.Helpers
{
    public class MemberInfoHelper
    {
        /// <summary>
        /// 存放資料的Key
        /// </summary>
        private const string FAIRLY_USER = "FairlyUser";

        public static AdminRepository adminRepository = new AdminRepository(new Models.InspectionEntities());

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="isRemeber"></param>
        public static void Login(MemberInfo memberInfo, bool isRemeber)
        {
            string data = JsonConvert.SerializeObject(memberInfo);

            if (isRemeber)
            {
                DateTime expires = DateTime.Now.AddYears(100);
                CookieHelper.SetCookie(FAIRLY_USER, data, expires);
            }
            else
            {
                DateTime expires = DateTime.Now.AddHours(1);
                CookieHelper.SetCookie(FAIRLY_USER, data, expires);
                SessionHelper.SetSession(FAIRLY_USER, data);
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        public static void Logout()
        {
            SessionHelper.Del(FAIRLY_USER);
            CookieHelper.Del(FAIRLY_USER);
        }

        /// <summary>
        /// 取得會員資料
        /// </summary>
        /// <returns></returns>
        public static MemberInfo GetMemberInfo()
        {
            string data = SessionHelper.Get(FAIRLY_USER);
            if (!string.IsNullOrEmpty(data))
            {
                MemberInfo memberInfo = JsonConvert.DeserializeObject<MemberInfo>(data);
                memberInfo.Name = adminRepository.GetById(memberInfo.ID).Name;
                return memberInfo;
            }

            data = CookieHelper.Get(FAIRLY_USER);
            if (!string.IsNullOrEmpty(data))
            {
                MemberInfo memberInfo = JsonConvert.DeserializeObject<MemberInfo>(data);
                var adminInfo = adminRepository.GetById(memberInfo.ID);
                if (adminInfo != null)
                {
                    memberInfo.Name = adminInfo.Name;
                }
                return memberInfo;
            }
            return null;
        }

        /// <summary>
        /// 會員是否為登入狀態
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin()
        {
            MemberInfo memberInfo = GetMemberInfo();
            return memberInfo != null;
        }
    }
}