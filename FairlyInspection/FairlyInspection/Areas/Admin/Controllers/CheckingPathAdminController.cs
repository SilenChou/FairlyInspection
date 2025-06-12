using AutoMapper;
using FairlyInspection.ActionFilters;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPath;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPoint;
using FairlyInspection.Models;
using FairlyInspection.Models.Others;
using FairlyInspection.Repositories;
using FairlyInspection.Repositories.T8Repositories;
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
    public class CheckingPathAdminController : BaseAdminController
    {
        private PersonRepository t8PersonRepository = new PersonRepository(new T8ERPEntities());
        private CheckingPathRepository checkingPathRepository = new CheckingPathRepository(new InspectionEntities());
        private CheckingPointRepository checkingPointRepository = new CheckingPointRepository(new InspectionEntities());
        private CheckingInspectorsRepository checkingInspectorsRepository = new CheckingInspectorsRepository(new InspectionEntities());

        // GET: Admin/CheckingPathAdmin
        public ActionResult Index(CheckingPathIndexView model)
        {
            var query = checkingPathRepository.Query(model.IsOnline, model.Title);
            var tempList = query.ToList();
            var pageResult = query.ToPageResult<CheckingPath>(model);
            model.PageResult = Mapper.Map<PageResult<CheckingPathView>>(pageResult);

            return View(model);
        }

        public ActionResult Edit(CheckingPathView model, int id = 0)
        {
            if (id != 0)
            {
                var query = checkingPathRepository.FindBy(id);
                model = Mapper.Map<CheckingPathView>(query);
                model.PointsList = Mapper.Map<List<CheckingPointView>>(checkingPointRepository.Query(true, id));
            }
            else
            {//預設
                model.PointsList = new List<CheckingPointView>();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(CheckingPathView model)
        {
            if (ModelState.IsValid)
            {
                CheckingPath data = Mapper.Map<CheckingPath>(model);
                var adminInfo = AdminInfoHelper.GetAdminInfo();
                //model.StartDate = Convert.ToDateTime(model.StartDateValue);
                data.NextDate = Convert.ToDateTime(model.NextDateValue);
                data.Dispatcher = string.IsNullOrEmpty(data.Dispatcher) ? t8PersonRepository.FindByJobID(adminInfo.Account).Name : "";

                if (model.ID == 0)
                {
                    model.ID = checkingPathRepository.Insert(data);
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