using AutoMapper;
using FairlyInspection.ActionFilters;
using FairlyInspection.Areas.Admin.ViewModels.ItemAssign;
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
    [ErrorHandleAdminActionFilter]
    public class ItemAssignAdminController : BaseAdminController
    {
        private ItemAssignRepository itemAssignRepository = new ItemAssignRepository(new InspectionEntities());
      
        // GET: Admin/ItemAssignAdmin
        public ActionResult Index(ItemAssignIndexView model)
        {
            var query = itemAssignRepository.Query();          
            var pageResult = query.ToPageResult<ItemAssign>(model);
            model.PageResult = Mapper.Map<PageResult<ItemAssignView>>(pageResult);
            return View();
        }

        public ActionResult Edit(int id = 0)
        {
            var model = new ItemAssignView();
            if (id != 0)
            {
                var query = itemAssignRepository.FindBy(id);
                model = Mapper.Map<ItemAssignView>(query);
            }
            else
            {

            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ItemAssignView model)
        {
            if (ModelState.IsValid)
            {
                ItemAssign data = Mapper.Map<ItemAssign>(model);
                int adminId = AdminInfoHelper.GetAdminInfo().ID;

                if (model.ID == 0)
                {
                    model.ID = itemAssignRepository.Insert(data);
                    ShowMessage(true, "新增成功");
                }
                else
                {
                    itemAssignRepository.Update(data);
                    ShowMessage(true, "修改成功");
                }

                return RedirectToAction("Edit", new { id = model.ID });
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            itemAssignRepository.Delete(id);
            ShowMessage(true, "刪除成功");
            return RedirectToAction("Index");
        }
    }
}