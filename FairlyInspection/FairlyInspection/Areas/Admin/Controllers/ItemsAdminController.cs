using AutoMapper;
using FairlyInspection.ActionFilters;
using FairlyInspection.Areas.Admin.ViewModels.Items;
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
    public class ItemsAdminController : BaseAdminController
    {
        private ItemsRepository itemsRepository = new ItemsRepository(new InspectionEntities());
        private ItemLogsRepository itemLogsRepository = new ItemLogsRepository(new InspectionEntities());
        private ItemAssignRepository itemAssignRepository = new ItemAssignRepository(new InspectionEntities());

        // GET: Admin/ItemsAdmin
        public ActionResult Index(ItemsIndexView model)
        {
            var query = itemsRepository.Query(model.IsOnline, model.Title, model.Level, model.ParentID);
            var pageResult = query.ToPageResult<Items>(model);
            model.PageResult = Mapper.Map<PageResult<ItemsView>>(pageResult);
            model.ParentBreadList = ComposeBreadInfo(model.ParentID, new List<BreadItem>());

            return View(model);
        }

        public ActionResult Edit(ItemsView model, int id = 0)
        {            
            if (id != 0)
            {
                var query = itemsRepository.FindBy(id);
                model = Mapper.Map<ItemsView>(query);
                model.ParentBreadList = ComposeBreadInfo(model.ID, new List<BreadItem>());
            }
            else
            {//預設
                model.ParentBreadList = ComposeBreadInfo(model.ParentID, new List<BreadItem>());
            }           

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ItemsView model)
        {
            if (ModelState.IsValid)
            {
                Items data = Mapper.Map<Items>(model);
                int adminId = AdminInfoHelper.GetAdminInfo().ID;

                if (model.ID == 0)
                {
                    model.ID = itemsRepository.Insert(data);
                    if (model.Level >= 3)
                    {
                        itemLogsRepository.Insert(new ItemLogs()
                        {
                            ItemID = model.ID,
                            ItemSystem = GetSystem(model.ID, 1),
                            ItemTitle = model.Title,
                            Level = model.Level,
                            CheckResult = (int)LogCheckResult.Normal,
                            MainPic = "",
                            Description = "",
                            IsLatest = true,
                            NextCheckDate = DateTime.Now.AddDays(model.Interval),
                            CreateDate = DateTime.Now,
                        });
                    }
                    ShowMessage(true, "新增成功");
                }
                else
                {
                    itemsRepository.Update(data);
                    ShowMessage(true, "修改成功");
                }

                try
                {
                    var parentData = itemsRepository.GetById(model.ParentID);
                    parentData.MinLevel = false;
                    itemsRepository.Update(parentData);
                }
                catch (Exception e)
                {

                    throw;
                }

                #region 取消上層巡檢紀錄
                if (model.ParentID != 0)
                {
                    var deleteList = itemLogsRepository.Query(model.ParentID).ToList();
                    foreach (var item in deleteList)
                    {
                        itemLogsRepository.Delete(item.ID);
                    }
                }
                #endregion

                return RedirectToAction("Edit", new { id = model.ID });
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var childQuery = itemsRepository.Query(null, "", 0, id);
            if (childQuery.Count() > 0)
            {
                foreach (var item in childQuery.ToList())
                {
                    Delete(item.ID);
                }
            }
            itemsRepository.Delete(id);

            ShowMessage(true, "刪除成功");
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 取得樹狀清單選項
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public JsonResult GetItems(int parentId, int level)
        {            
            var plidList = itemsRepository.ItemsJoinLogsQuery(0, 0, true).Select(q => q.ItemID).ToList();
            var query = itemsRepository.Query(true, "", level, parentId);

            var adminInfo = AdminInfoHelper.GetAdminInfo();
            if (level == (int)LevelClass.Factory)
            {
                var assignList = itemAssignRepository.Query(0, adminInfo.Account, adminInfo.Type);
                if (assignList == null || assignList.Count() == 0)
                {
                    return Json(new List<ItemsTreeView>());
                }
                var IdList = assignList.Select(q => q.ItemID).ToList();
                var factoryIdList = itemsRepository.Query(true).Where(q => IdList.Contains(q.ID)).Select(q => q.ParentID).ToList();
                query = query.Where(q => factoryIdList.Contains(q.ID));
            }
            else if (level == (int)LevelClass.System)
            {
                var assignList = itemAssignRepository.Query(0, adminInfo.Account, adminInfo.Type);
                if (assignList == null || assignList.Count() == 0)
                {
                    return Json(new List<ItemsTreeView>());
                }
                var IdList = assignList.Select(q => q.ItemID).ToList();
                query = query.Where(q => IdList.Contains(q.ID));
            }

            var result = Mapper.Map<List<ItemsTreeView>>(query);
            foreach (var item in result)
            {
                if (plidList.Contains(item.ID))
                {
                    item.isLight = true;
                }
                item.isLatest = itemsRepository.Query(true, "", 0, item.ID).Count() <= 0;
            }

            return Json(result);
        }

        /// <summary>
        /// 取得全部提醒清單
        /// </summary>
        /// <returns></returns>
        public JsonResult GetNearlyItems()
        {
            var nearlyList = itemsRepository.ItemsJoinLogsQuery(0, 0, true);
            return Json(nearlyList);
        }      
    }
}