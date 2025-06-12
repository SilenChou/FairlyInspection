using AutoMapper;
using FairlyInspection.ActionFilters;
using FairlyInspection.Areas.Admin.ViewModels.Items;
using FairlyInspection.Areas.Admin.ViewModels.Manager;
using FairlyInspection.Models;
using FairlyInspection.Models.Others;
using FairlyInspection.Repositories;
using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlyInspection.Areas.Admin.Controllers
{
    [Authorize]
    //[ErrorHandleAdminActionFilter]
    public class ManagerAdminController : BaseAdminController
    {
        private AdminRepository adminRepository = new AdminRepository(new InspectionEntities());        
        private ItemsRepository itemsRepository = new ItemsRepository(new InspectionEntities());        
        private ItemAssignRepository itemAssignRepository = new ItemAssignRepository(new InspectionEntities());        
        private Repositories.T8Repositories.PersonRepository personRepository = new Repositories.T8Repositories.PersonRepository(new T8ERPEntities());

        public ManagerAdminController() : this(null, null) { }

        public ManagerAdminController(AdminRepository repo, Repositories.T8Repositories.PersonRepository repo2)
        {
            adminRepository = repo ?? new AdminRepository(new InspectionEntities());
            personRepository = repo2 ?? new Repositories.T8Repositories.PersonRepository(new T8ERPEntities());
        }

        // GET: Admin/ManagerAdmin
        public ActionResult Index(ManagerIndexView model)
        {
            var adminInfo = AdminInfoHelper.GetAdminInfo();
            if (adminInfo.Type > (int)AdminType.Manager)
            {
                return RedirectToAction("Edit", new { id = adminInfo.ID });
            }
            var thisAcc = adminInfo.Account;
            var admintype = adminRepository.GetAll().Where(a => a.Account == thisAcc).FirstOrDefault().Type;
            var query = adminRepository.Query(model.Status, model.Type, model.Account);
            if (admintype == (int)AdminType.Manager)
            {
                query = query.Where(a => a.Type > (int)AdminType.Manager || a.Account == thisAcc);
            }
            var pageResult = query.ToPageResult<Models.Admin>(model);
            model.PageResult = Mapper.Map<PageResult<ManagerView>>(pageResult);
            foreach (var item in model.PageResult.Data)
            {
                var personInfo = personRepository.FindByJobID(item.Account);
                if (personInfo != null)
                {
                    //item.Department = personInfo.DeptID;
                    item.Name = personInfo.Name;
                }
            }
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            var adminInfo = AdminInfoHelper.GetAdminInfo();
            if (adminInfo.Type > 1 && adminInfo.ID != id)
            {
                return RedirectToAction("Edit", new { id = adminInfo.ID});
            }
            ManagerView model;
            if (id == 0)
            {
                model = new ManagerView();
                model.Type = (int)AdminType.Inspector;
            }
            else
            {
                var query = adminRepository.GetById(id);
                model = Mapper.Map<ManagerView>(query);

                var personInfo = personRepository.FindByJobID(model.Account);
                if (personInfo != null)
                {
                    //model.Department = personInfo.DeptID;
                    model.Name = personInfo.Name;
                }

                var assingItems = itemsRepository.Query(true, "", 3, 0, 1).ToList();
                foreach (var item in assingItems.ToList())
                {
                    if (item.ParentID != 0 && !CheckParentOnline(item.ParentID))
                    {
                        assingItems.Remove(item);
                    }                    
                }
                model.AllSystemList = Mapper.Map<List<ItemsView>>(assingItems);

                var selectedIdList = itemAssignRepository.Query(0, model.Account).Select(q => q.ItemID).ToList();
                if (selectedIdList.Count() > 0)
                {
                    foreach (var item in model.AllSystemList)
                    {
                        item.IsSelected = selectedIdList.Contains(item.ID);
                    }
                }

                model.newAssignList = string.Join(",", selectedIdList);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ManagerView model)
        {
            if (ModelState.IsValid)
            {
                Models.Admin admin = Mapper.Map<Models.Admin>(model);
                admin.UpdateDate = DateTime.Now;
                admin.Updater = 1;
                int accountCount = adminRepository.GetAll().Where(a => a.Account == model.Account).Count();
                if (accountCount > 0 && AdminInfoHelper.GetAdminInfo().Account != model.Account && model.ID == 0)
                {
                    ShowMessage(false, "該帳號已被使用，請重新輸入");
                    return View(model);
                }
                if (model.ID == 0)
                {
                    admin.CreateDate = DateTime.Now;
                    admin.Creater = 1;
                    model.ID = adminRepository.Insert(admin);
                }
                else
                {
                    adminRepository.Update(admin);
                }

                var oldData = itemAssignRepository.Query(0, model.Account).ToList();
                foreach (var data in oldData)
                {
                    itemAssignRepository.Delete(data.ID);
                }
                //分派關聯
                if (!string.IsNullOrEmpty(model.newAssignList))
                {
                    foreach (var item in model.newAssignList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            ItemAssign newAssign = new ItemAssign()
                            {
                                ItemID = Convert.ToInt32(item),
                                PersonID = model.Account,
                                CreateDate = DateTime.Now
                            };
                            itemAssignRepository.Insert(newAssign);
                        }
                    }
                }
                ShowMessage(true, "");
                return RedirectToAction(nameof(ManagerAdminController.Edit), new { id = model.ID });
            }

            ShowMessage(false, "輸入資料有誤");
            return View(model);
        }

        public bool CheckParentOnline(int parentId)
        {
            var parentData = itemsRepository.GetById(parentId);
            if (!parentData.IsOnline)
            {
                return false;
            }
            else
            {
                if (parentData.ParentID != 0)
                {
                    return CheckParentOnline(parentData.ParentID);
                }                
                return true;
            }
        }

        public ActionResult Delete(int id)
        {
            Models.Admin admin = adminRepository.GetById(id);
            adminRepository.Delete(admin.ID);
            ShowMessage(true, "Data deleted.");
            return RedirectToAction(nameof(ManagerAdminController.Index));
        }

    }
}