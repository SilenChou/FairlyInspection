using AutoMapper;
using FairlyInspection.ActionFilters;
using FairlyInspection.Areas.Admin.ViewModels.CheckingItem;
using FairlyInspection.Areas.Admin.ViewModels.CheckingItemLog;
using FairlyInspection.Areas.Admin.ViewModels.CheckingItemLogsImage;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPath;
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
    public class CheckingItemLogAdminController : BaseAdminController
    {
        private CheckingPathRepository checkingPathRepository = new CheckingPathRepository(new InspectionEntities());
        private CheckingPointRepository checkingPointRepository = new CheckingPointRepository(new InspectionEntities());
        private CheckingItemRepository checkingItemRepository = new CheckingItemRepository(new InspectionEntities());
        private CheckingPathLogRepository checkingPathLogRepository = new CheckingPathLogRepository(new InspectionEntities());
        private CheckingPointLogRepository checkingPointLogRepository = new CheckingPointLogRepository(new InspectionEntities());
        private CheckingItemLogRepository checkingItemLogRepository = new CheckingItemLogRepository(new InspectionEntities());
        private CheckingItemLogsImageRepository checkingItemLogsImageRepository = new CheckingItemLogsImageRepository(new InspectionEntities());

        // GET: Admin/CheckingItemLogAdmin
        public ActionResult Index(CheckingItemLogIndexView model)
        {
            if (model.PointID == 0)
            {
                return RedirectToAction("Index", "CheckingPathAdmin");
            }
            var query = checkingItemLogRepository.Query(model.IsCompleted, model.PathID, model.PointID);
            var pageResult = query.ToPageResult<CheckingItemLog>(model);
            model.PageResult = Mapper.Map<PageResult<CheckingItemLogView>>(pageResult);
            foreach (var item in model.PageResult.Data)
            {
                item.ItemInfo = Mapper.Map<CheckingItemView>(checkingItemRepository.GetById(model.ItemID));
            }

            model.PathInfo = Mapper.Map<CheckingPathView>(checkingPathRepository.GetById(model.PathID));
            model.PointInfo = Mapper.Map<CheckingPointView>(checkingPathRepository.GetById(model.PointID));
            model.ItemInfo = Mapper.Map<CheckingItemView>(checkingPathRepository.GetById(model.ItemID));

            return View(model);
        }

        public ActionResult Edit(CheckingItemLogView model, int id = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "CheckingPathAdmin");
            }

            var query = checkingItemLogRepository.FindBy(id);
            model = Mapper.Map<CheckingItemLogView>(query);
            model.ItemInfo = Mapper.Map<CheckingItemView>(checkingItemRepository.GetById(model.ItemID));
            model.PicList = Mapper.Map<List<CheckingItemLogsImageView>>(checkingItemLogsImageRepository.Query(id));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(CheckingItemLogView model)
        {
            if (ModelState.IsValid)
            {
                CheckingItemLog data = Mapper.Map<CheckingItemLog>(model);
                int adminId = AdminInfoHelper.GetAdminInfo().ID;

                #region 檔案處理
                if (model.PicsFiles != null)
                {
                    foreach (HttpPostedFileBase pic in model.PicsFiles)
                    {
                        bool hasFile = ImageHelper.CheckFileExists(pic);
                        if (hasFile)
                        {
                            CheckingItemLogsImage imageData = new CheckingItemLogsImage();
                            //ImageHelper.DeleteFile(PhotoFolder, model.FileOne);
                            imageData.LogID = model.ID;
                            imageData.Pics = ImageHelper.SaveFile(pic, PhotoFolder);
                            checkingItemLogsImageRepository.Insert(imageData);
                        }
                    }
                }
                #endregion

                var imageList = checkingItemLogsImageRepository.Query(data.ID).ToList();
                //完成檢查(圖片紀錄)
                data.IsCompleted = imageList.Count() > 0;
                checkingItemLogRepository.Update(data);

                if (data.IsCompleted)
                {
                    //其他項目是否完成
                    var otherItemsLog = checkingItemLogRepository.Query(null, data.PathID, data.PointID, 0, data.PathLogID, data.PointLogID);
                    if (otherItemsLog.Where(q => !q.IsCompleted).Count() <= 0)
                    {
                        //檢查點完成
                        var pointLog = checkingPointLogRepository.GetById(data.PointLogID);
                        pointLog.IsCompleted = true;
                        checkingPointLogRepository.Update(pointLog);
                        //其他檢查點完成狀態
                        var otherPointsLog = checkingPointLogRepository.Query(null, data.PathID, 0, 0, "", data.PathLogID);
                        if (otherPointsLog.Where(q => !q.IsCompleted).Count() <= 0)
                        {
                            //檢查路線完成
                            var pathLog = checkingPathLogRepository.GetById(data.PathLogID);
                            pathLog.IsCompleted = true;
                            checkingPathLogRepository.Update(pathLog);

                            var pathData = checkingPathRepository.GetById(data.PathID);
                            switch (pathData.IntervalType)
                            {
                                case 1:
                                    pathData.NextDate = Convert.ToDateTime(pathLog.CompleteDate).AddYears(pathData.Interval);
                                    break;
                                case 2:
                                    pathData.NextDate = Convert.ToDateTime(pathLog.CompleteDate).AddMonths(pathData.Interval);
                                    break;
                                case 3:
                                    pathData.NextDate = Convert.ToDateTime(pathLog.CompleteDate).AddDays(pathData.Interval);
                                    break;
                            }                            
                            checkingPathRepository.Update(pathData);
                        }
                    }                                    
                }

                ShowMessage(true, "修改成功");

                return RedirectToAction("Edit", new { id = model.ID });
            }

            return View(model);
        }

        public ActionResult DeleteFile(int id)
        {
            var imageData = checkingItemLogsImageRepository.GetById(id);
            int itemLogId = imageData.LogID;
            ImageHelper.DeleteFile(PhotoFolder, imageData.Pics);
            checkingItemLogsImageRepository.Delete(id);
            ShowMessage(true, "刪除成功");
            return RedirectToAction("Edit", new { id = itemLogId });
        }
    }
}