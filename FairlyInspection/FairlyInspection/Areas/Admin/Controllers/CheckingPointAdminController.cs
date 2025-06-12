using AutoMapper;
using FairlyInspection.ActionFilters;
using FairlyInspection.Areas.Admin.ViewModels.CheckingItem;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPathLog;
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
    public class CheckingPointAdminController : BaseAdminController
    {       
        private CheckingPathRepository checkingPathRepository = new CheckingPathRepository(new InspectionEntities());
        private CheckingPointRepository checkingPointRepository = new CheckingPointRepository(new InspectionEntities());
        private CheckingItemRepository checkingItemRepository = new CheckingItemRepository(new InspectionEntities());

        private CheckingPathLogRepository checkingPathLogRepository = new CheckingPathLogRepository(new InspectionEntities());
        private CheckingPointLogRepository checkingPointLogRepository = new CheckingPointLogRepository(new InspectionEntities());
        private CheckingItemLogRepository checkingItemLogRepository = new CheckingItemLogRepository(new InspectionEntities());
        private CheckingItemLogsImageRepository checkingItemLogsImageRepository = new CheckingItemLogsImageRepository(new InspectionEntities());

        private CheckingInspectorsRepository checkingPersonBindRepository = new CheckingInspectorsRepository(new InspectionEntities());

        // GET: Admin/CheckingPointAdmin
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

                model.ItemList = checkingItemRepository.Query(null, model.PathID, id).ToList();

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
        {//刪除巡查點
            var childQuery = checkingItemRepository.Query(null, 0, id);
            if (childQuery.Count() > 0)
            {//刪除細項
                foreach (var item in childQuery.ToList())
                {                    
                    var itemChildQuery = checkingItemLogRepository.Query(true, 0, item.ID);
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
                    checkingItemRepository.Delete(item.ID);
                }
            }
            checkingPointRepository.Delete(id);

            ShowMessage(true, "刪除成功");
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 取得全部提醒清單
        /// </summary>
        /// <returns></returns>
        //public JsonResult GetNearlyCheckingPoint()
        //{
        //    var nearlyList = checkingPointRepository.CheckingPointJoinLogsQuery(0, 0, true);
        //    return Json(nearlyList);
        //}

        public JsonResult GetPoint(int id)
        {
            try
            {
                var query = checkingPointRepository.GetById(id);
                CheckingPointView data = Mapper.Map<CheckingPointView>(query);

                //var itemQuery = checkingItemLogRepository.Query(null, 0, data.ID);
                //data.ItemLogList = Mapper.Map<List<CheckingItemLogView>>(itemQuery);

                return Json(query, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public JsonResult GetPointLog(int id)
        {
            try
            {
                var query = checkingPointLogRepository.GetById(id);
                CheckingPointLogView data = Mapper.Map<CheckingPointLogView>(query);

                var pointQuery = checkingPointRepository.GetById(data.PointID);
                data.PointInfo = Mapper.Map<CheckingPointView>(pointQuery);

                var itemQuery = checkingItemLogRepository.Query(null, 0, data.PointID);
                data.ItemLogList = Mapper.Map<List<CheckingItemLogView>>(itemQuery);
                foreach (var item in data.ItemLogList)
                {
                    item.ItemInfo = Mapper.Map<CheckingItemView>(checkingItemRepository.GetById(item.ItemID));
                }

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public bool SavePoint(CheckingPointView data)
        {
            try
            {
                var pointData = Mapper.Map<CheckingPoint>(data);
                if (data.ID != 0)
                {
                    checkingPointRepository.Update(pointData);
                }
                else
                {
                    checkingPointRepository.Insert(pointData);
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