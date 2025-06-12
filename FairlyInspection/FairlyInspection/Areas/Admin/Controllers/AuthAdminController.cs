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

namespace FairlyInspection.Areas.Admin.Controllers
{
    [ErrorHandleAdminActionFilter]
    public class AuthAdminController : BaseAdminController
    {
        private AdminRepository adminRepository;

        public AuthAdminController() : this(null) { }

        public AuthAdminController(AdminRepository repo)
        {
            adminRepository = repo ?? new AdminRepository(new InspectionEntities());
        }

        // GET: Admin/AccountAdmin
        public ActionResult Login()
        {
            //#if DEBUG
            //            if (System.Diagnostics.Debugger.IsAttached)
            //            {
            //                var controller = DependencyResolver.Current.GetService<AuthAdminController>();
            //                controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
            //                return controller.Login(new LoginView() { Account = "fairlyadmin", Password = "123456" });
            //            }
            //#endif
            var adminInfo = AdminInfoHelper.GetAdminInfo();
            if (adminInfo != null)
            {
                var controller = DependencyResolver.Current.GetService<AuthAdminController>();
                controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);

                var loginInfo = adminRepository.GetById(adminInfo.ID);
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
                var admin = adminRepository.Login(login.Account, login.Password);
                if (admin != null)
                {
                    AdminInfo adminInfo = new AdminInfo();
                    adminInfo.ID = admin.ID;
                    adminInfo.Account = admin.Account;
                    adminInfo.Type = admin.Type;
                    adminInfo.Name = admin.Name;
                    AdminInfoHelper.Login(adminInfo, login.RememberMe);

                    ShowMessage(true, "登入成功");
                    if (FormsAuthentication.GetRedirectUrl(login.Account,false).IndexOf("AuthAdmin") < 0)
                    {
                        return Redirect(FormsAuthentication.GetRedirectUrl(login.Account, false));
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            ShowMessage(false, "登入失敗");
            return View(login);
        }

        [Authorize]
        public ActionResult Logout()
        {
            AdminInfoHelper.Logout();
            return RedirectToAction("Login");
        }
    }
}