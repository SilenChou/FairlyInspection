using FairlyInspection.ActionFilters;
using FairlyInspection.Areas.Admin.ViewModels.Auth;
using FairlyInspection.Models;
using FairlyInspection.Models.Others;
using FairlyInspection.Repositories;
using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FairlyInspection.Controllers
{
    [ErrorHandleActionFilter]
    public class AuthController : BaseController
    {
        private AdminRepository adminRepository;

        public AuthController() : this(null) { }

        public AuthController(AdminRepository repo)
        {
            adminRepository = repo ?? new AdminRepository(new InspectionEntities());
        }

        // GET: Admin/AccountAdmin
        public ActionResult Login()
        {
            var memberInfo = MemberInfoHelper.GetMemberInfo();
            if (memberInfo != null)
            {
                var controller = DependencyResolver.Current.GetService<AuthController>();
                controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
                var loginInfo = adminRepository.GetById(memberInfo.ID);
                if (loginInfo != null && loginInfo.Type <= (int)AdminType.Manager)
                {
                    return controller.Login(new LoginView() { Account = loginInfo.Account, Password = loginInfo.Password });
                }
            }

            var login = new LoginView();
            return View(login);
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginView login)
        {
            if (ModelState.IsValid)
            {
                login.RememberMe = true;
                var admin = adminRepository.Login(login.Account, login.Password);
                if (admin != null && admin.Type <= (int)AdminType.Manager)
                {
                    MemberInfo memberInfo = new MemberInfo();
                    memberInfo.ID = admin.ID;
                    memberInfo.Account = admin.Account;
                    memberInfo.Type = admin.Type;
                    //memberInfo.Name = admin.Name;
                    MemberInfoHelper.Login(memberInfo, login.RememberMe);

                    return RedirectToAction("AddGroup", "Api");
                }
            }
            return View(login);
        }

        public ActionResult Logout()
        {
            MemberInfoHelper.Logout();
            return RedirectToAction("Login");
        }
    }
}