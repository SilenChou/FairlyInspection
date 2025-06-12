using AutoMapper;
using FairlyInspection.ActionFilters;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPath;
using FairlyInspection.Areas.Admin.ViewModels.Home;
using FairlyInspection.Areas.Admin.ViewModels.Items;
using FairlyInspection.Models;
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
    public class HomeAdminController : BaseAdminController
    {
        private AdminRepository adminRepository = new AdminRepository(new InspectionEntities());
        private ItemsRepository itemsRepository = new ItemsRepository(new InspectionEntities());
        private ItemLogsRepository itemLogsRepository = new ItemLogsRepository(new InspectionEntities());
        private ItemAssignRepository itemAssignRepository = new ItemAssignRepository(new InspectionEntities());

        private CheckingPathRepository checkingPathRepository = new CheckingPathRepository(new InspectionEntities());
        private CheckingPointRepository checkingPointRepository = new CheckingPointRepository(new InspectionEntities());
        private CheckingItemRepository checkingItemRepository = new CheckingItemRepository(new InspectionEntities());
        private CheckingPathLogRepository checkingPathLogRepository = new CheckingPathLogRepository(new InspectionEntities());
        private CheckingPointLogRepository checkingPointLogRepository = new CheckingPointLogRepository(new InspectionEntities());
        private CheckingItemLogRepository checkingItemLogRepository = new CheckingItemLogRepository(new InspectionEntities());


        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            return RedirectToAction("OverView");
            //return View();
        }

        public ActionResult OverView()
        {
            OverViewView model = new OverViewView();

            var adminInfo = AdminInfoHelper.GetAdminInfo();
            #region 舊版
            //var assignList = itemAssignRepository.Query(0, adminInfo.Account, adminInfo.Type);
            //if (assignList == null || assignList.Count() == 0)
            //{
            //    model.PendingList = new List<Models.Join.ItemsJoinLogs>();
            //    model.CheckingPendingList = new List<CheckingPathView>();
            //    return View(model);
            //}
            //var IdList = assignList.Select(q => q.ItemID).ToList();
            //var allPendingList = itemsRepository.ItemsJoinLogsQuery(0, 0, true);
            //model.PendingList = allPendingList.Where(q => IdList.Contains(q.ItemSystem)).ToList();
            //foreach (var item in model.PendingList.ToList())
            //{
            //    item.ParentBreadList = ComposeBreadInfo(item.ItemID, new List<BreadItem>(), false);
            //    if (item.ParentBreadList == null)
            //    {
            //        //item.Message = "已停用";
            //        model.PendingList.Remove(item);
            //    }

            //    item.Inspectors = "";
            //    item.InspectorsList = itemAssignRepository.Query(GetSystem(item.ItemID, 1)).ToList();
            //    if (item.InspectorsList.Count() > 0)
            //    {
            //        foreach (var insp in item.InspectorsList)
            //        {
            //            var inspAdminInfo = adminRepository.Query(true, 0, insp.PersonID).FirstOrDefault();
            //            if (inspAdminInfo != null)
            //            {
            //                item.Inspectors += inspAdminInfo.Name + ",";
            //            }
            //        }
            //    }
            //}
            #endregion

            #region 新版
            //取得待檢清單
            var pendingQuery = checkingPathRepository.Query(true, "", true).Where(q => q.Inspectors.Contains(adminInfo.Name) || q.Dispatcher == adminInfo.Name);
            model.CheckingPendingList = Mapper.Map<List<CheckingPathView>>(pendingQuery);
            foreach (var item in model.CheckingPendingList)
            {
                var lastPathLog = checkingPathLogRepository.Query(true, item.ID).OrderByDescending(q => q.CreateDate).FirstOrDefault();
                item.CompleteDate = lastPathLog == null ? null : lastPathLog.CompleteDate;
            }
            #endregion

            return View(model);
        }
    }
}