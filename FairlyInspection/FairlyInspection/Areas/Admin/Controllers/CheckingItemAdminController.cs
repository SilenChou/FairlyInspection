using AutoMapper;
using FairlyInspection.ActionFilters;
using FairlyInspection.Areas.Admin.ViewModels.CheckingItem;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPoint;
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
    public class CheckingItemAdminController : BaseAdminController
    {
        private CheckingPointRepository checkingPointRepository = new CheckingPointRepository(new InspectionEntities());
        private CheckingItemRepository checkingItemRepository = new CheckingItemRepository(new InspectionEntities());
        private CheckingItemLogRepository checkingItemLogRepository = new CheckingItemLogRepository(new InspectionEntities());
        private CheckingItemLogsImageRepository checkingItemLogsImageRepository = new CheckingItemLogsImageRepository(new InspectionEntities());
        private CheckingInspectorsRepository checkingPersonBindRepository = new CheckingInspectorsRepository(new InspectionEntities());

        // GET: Admin/CheckingItemAdmin
        public ActionResult Index(CheckingPointIndexView model)
        {
            var query = checkingPointRepository.Query(model.IsOnline, model.PathID, model.Title);
            var pageResult = query.ToPageResult<CheckingPoint>(model);
            model.PageResult = Mapper.Map<PageResult<CheckingPointView>>(pageResult);

            return View(model);
        }

        public ActionResult Edit(CheckingPointView model, int id = 0)
        {
            if (id != 0)
            {
                var query = checkingPointRepository.FindBy(id);
                model = Mapper.Map<CheckingPointView>(query);
            }
            else
            {//預設

            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(CheckingPointView model)
        {
            if (ModelState.IsValid)
            {
                CheckingPoint data = Mapper.Map<CheckingPoint>(model);
                int adminId = AdminInfoHelper.GetAdminInfo().ID;

                if (model.ID == 0)
                {
                    model.ID = checkingPointRepository.Insert(data);
                    //if (model.Level >= 3)
                    //{
                    //    itemLogsRepository.Insert(new ItemLogs()
                    //    {
                    //        ItemID = model.ID,
                    //        ItemSystem = GetSystem(model.ID, 1),
                    //        ItemTitle = model.Title,
                    //        Level = model.Level,
                    //        CheckResult = (int)LogCheckResult.Normal,
                    //        MainPic = "",
                    //        Description = "",
                    //        IsLatest = true,
                    //        NextCheckDate = DateTime.Now.AddDays(model.Interval),
                    //        CreateDate = DateTime.Now,
                    //    });
                    //}
                    ShowMessage(true, "新增成功");
                }
                else
                {
                    checkingPointRepository.Update(data);
                    ShowMessage(true, "修改成功");
                }

                return RedirectToAction("Edit", new { id = model.ID });
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {//刪除細項
            var query = checkingItemRepository.GetById(id);
            var itemChildQuery = checkingItemLogRepository.Query(true, 0, query.ID);
            if (itemChildQuery.Count() > 0)
            {//刪除巡查紀錄
                foreach (var log in itemChildQuery.ToList())
                {
                    var logImageQuery = checkingItemLogsImageRepository.Query(log.ID);
                    if (logImageQuery.Count() > 0)
                    {//刪除巡查圖片
                        foreach (var image in logImageQuery.ToList())
                        {
                            checkingItemLogsImageRepository.Delete(image.ID);
                        }
                    }
                    checkingItemLogRepository.Delete(log.ID);
                }
            }
            checkingItemRepository.Delete(query.ID);

            ShowMessage(true, "刪除成功");
            return RedirectToAction("Index");
        }

        public JsonResult GetItem(int id)
        {
            try
            {
                var query = checkingItemRepository.GetById(id);

                return Json(query, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool SaveItem(CheckingItemView data)
        {
            try
            {
                var itemData = Mapper.Map<CheckingItem>(data);
                if (data.ID != 0)
                {
                    checkingItemRepository.Update(itemData);
                }
                else
                {
                    checkingItemRepository.Insert(itemData);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}