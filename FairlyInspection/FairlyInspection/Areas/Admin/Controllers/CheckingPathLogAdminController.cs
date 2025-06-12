using AutoMapper;
using FairlyInspection.ActionFilters;
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
    public class CheckingPathLogAdminController : BaseAdminController
    {
        private CheckingPathRepository checkingPathRepository = new CheckingPathRepository(new InspectionEntities());
        private CheckingPointRepository checkingPointRepository = new CheckingPointRepository(new InspectionEntities());
        private CheckingItemRepository checkingItemRepository = new CheckingItemRepository(new InspectionEntities());
        private CheckingPathLogRepository checkingPathLogRepository = new CheckingPathLogRepository(new InspectionEntities());
        private CheckingPointLogRepository checkingPointLogRepository = new CheckingPointLogRepository(new InspectionEntities());
        private CheckingItemLogRepository checkingItemLogRepository = new CheckingItemLogRepository(new InspectionEntities());

        // GET: Admin/CheckingPathLogAdmin
        public ActionResult Index(CheckingPathLogIndexView model)
        {
            var query = checkingPathLogRepository.Query(model.IsCompleted, model.PathID, model.Title);
            var pageResult = query.ToPageResult<CheckingPathLog>(model);
            model.PageResult = Mapper.Map<PageResult<CheckingPathLogView>>(pageResult);
            foreach (var item in model.PageResult.Data)
            {
                item.PathInfo = Mapper.Map<CheckingPathView>(checkingPathRepository.GetById(model.PathID));
            }

            return View(model);
        }

        public ActionResult Edit(CheckingPathLogView model, int id = 0)
        {
            if (model.PathID == 0)
            {
                return RedirectToAction("Index", "CheckingPathAdmin");
            }
            if (id == 0)
            {
                var adminInfo = AdminInfoHelper.GetAdminInfo();
                id = checkingPathLogRepository.Insert(new CheckingPathLog() { 
                    PathID = model.PathID,
                    Name = adminInfo.Name,
                    PersonId = adminInfo.Account,
                    IsCompleted = false,
                    CreateDate = DateTime.Now,
                });

                var pointList = checkingPointRepository.Query(true, model.PathID);
                foreach (var point in pointList)
                {
                    int pointLogId = checkingPointLogRepository.Insert(new CheckingPointLog()
                    {
                        PathID = model.PathID,
                        PointID = point.ID,
                        PathLogID = id,
                        IsCompleted = false,
                        CreateDate = DateTime.Now,
                    });

                    var itemList = checkingItemRepository.Query(true, point.PathID, point.ID).ToList();
                    foreach (var item in itemList)
                    {
                        int itemLogId = checkingItemLogRepository.Insert(new CheckingItemLog()
                        {
                            PathID = model.PathID,
                            PointID = point.ID,
                            ItemID = item.ID,
                            PathLogID = id,
                            Remark = "",
                            IsCompleted = false,
                            CreateDate = DateTime.Now,
                        });
                    }
                }

                return RedirectToAction("Edit", "CheckingPathLogAdmin", new { id = id, PathID = model.PathID});
            }
            var query = checkingPathLogRepository.FindBy(id);
            model = Mapper.Map<CheckingPathLogView>(query);
            if (model == null)
            {
                return RedirectToAction("Index", "CheckingPathAdmin");
            }
            model.PointLogList = Mapper.Map<List<CheckingPointLogView>>(checkingPointLogRepository.Query(null, model.PathID));
            foreach (var point in model.PointLogList)
            {
                point.PointInfo = Mapper.Map<CheckingPointView>(checkingPointRepository.GetById(point.PointID));
            }
            
            model.PathInfo = Mapper.Map<CheckingPathView>(checkingPathRepository.GetById(model.PathID));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(CheckingPathLogView model, string StartDate = "", string NextDate = "")
        {
            if (ModelState.IsValid)
            {
                CheckingPath data = Mapper.Map<CheckingPath>(model);
                int adminId = AdminInfoHelper.GetAdminInfo().ID;
                //model.StartDate = StartDate;
                //model.NextDate = NextDate;

                if (model.ID == 0)
                {
                    model.ID = checkingPathRepository.Insert(data);
                    ShowMessage(true, "新增成功");
                }
                else
                {
                    checkingPathRepository.Update(data);
                    ShowMessage(true, "修改成功");
                }

                return RedirectToAction("Edit", new { id = model.ID });
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {//刪除路線
            var pointQuery = checkingPointRepository.Query(null, id);
            if (pointQuery.Count() > 0)
            {//刪除巡查點
                var pointAdminController = new CheckingPointAdminController();
                foreach (var item in pointQuery)
                {
                    pointAdminController.Delete(item.ID);
                }
            }
            checkingPathRepository.Delete(id);

            ShowMessage(true, "刪除成功");
            return RedirectToAction("Index");
        }
    }
}