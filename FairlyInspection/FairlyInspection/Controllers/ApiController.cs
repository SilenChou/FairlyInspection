using FairlyInspection.Models;
using FairlyInspection.Repositories;
using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlyInspection.Controllers
{
    public class ApiController : BaseController
    {
        public string notifyUrl = ConfigurationManager.AppSettings["notifyUrl"];
        public string ClientID = ConfigurationManager.AppSettings["ClientID"];
        public string ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];
        private InterlockRepository interlockRepository = new InterlockRepository(new InspectionEntities());

        // GET: Api
        public ActionResult AddGroup()
        {
            var acc = MemberInfoHelper.GetMemberInfo();
            if (acc == null)
            {
                return RedirectToAction("LogIn", "Auth");
            }
            MessageView model = new MessageView()
            {
                ClientID = ClientID,
                ClientSecret = ClientSecret,
                CallbackUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/Api/Callback"
            };

            return View(model);
        }
        public ActionResult Callback()
        {
            var queryCode = Request.QueryString["code"];
            if (!string.IsNullOrEmpty(queryCode))
            {
                string code = queryCode.ToString();
                //string tokenkey = LineHelper.GetNotifyToken(code, ClientID, ClientSecret, Request.Url.Scheme + "://" + Request.Url.Authority + "/LastInfo/Callback?s=" + s);
                string tokenkey = LineHelper.GetNotifyToken(code, ClientID, ClientSecret, Request.Url.Scheme + "://" + Request.Url.Authority + "/Api/Callback");
                var memberInfo = MemberInfoHelper.GetMemberInfo();
                if (memberInfo == null)
                {
                    ShowMessage(true, "與Line Notify連動失敗，請先行登入再執行連動。");
                    return RedirectToAction("AddGroup", "Api");
                }
                Interlock token = new Interlock
                {
                    JobID = memberInfo.Account,
                    TokenKey = tokenkey,
                    LineID = "",
                    CallbackCode = code,
                    CreateDate = DateTime.Now,
                };
                interlockRepository.Insert(token);
                ShowMessage(true, "與Line Notify連動完成。");
            }

            //return RedirectToAction("FinishRegister", new { id = sid });
            return RedirectToAction("AddGroup", "Api");
        }
    }
}