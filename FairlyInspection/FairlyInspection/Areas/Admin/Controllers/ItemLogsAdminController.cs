using AutoMapper;
using FairlyInspection.ActionFilters;
using FairlyInspection.Areas.Admin.ViewModels.ItemLogs;
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
    [ErrorHandleAdminActionFilter]
    public class ItemLogsAdminController : BaseAdminController
    {
        private ItemLogsRepository itemLogsRepository = new ItemLogsRepository(new InspectionEntities());
        private ItemsLogsImageRepository itemsLogsImageRepository = new ItemsLogsImageRepository(new InspectionEntities());
        private ItemsRepository itemsRepository = new ItemsRepository(new InspectionEntities());

        // GET: Admin/ItemLogsAdmin
        public ActionResult Index(ItemLogsIndexView model)
        {
            if (model.ItemID == 0)
            {
                return RedirectToAction("OverView", "HomeAdmin");
            }
            var itemData = itemsRepository.GetById(model.ItemID);
            if (itemData != null)
            {
                model.ItemTitle = itemData.Title;
                model.Interval = itemData.Interval;
            }
            
            var query = itemLogsRepository.Query(model.ItemID);          
            var pageResult = query.ToPageResult<ItemLogs>(model);
            model.PageResult = Mapper.Map<PageResult<ItemLogsView>>(pageResult);
            model.ParentBreadList = ComposeBreadInfo(model.ItemID, new List<BreadItem>());
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(ItemLogsView model, int id = 0)
        {
            if (id == 0)
            {
                if (model.ItemID == 0)
                {
                    return RedirectToAction("OverView", "HomeAdmin");
                }
            }
            else
            {
                var query = itemLogsRepository.FindBy(id);
                model = Mapper.Map<ItemLogsView>(query);
                model.PicList = itemsLogsImageRepository.Query(id).ToList();
            }

            var itemInfo = itemsRepository.GetById(model.ItemID);
            model.ItemTitle = itemInfo.Title;
            model.ItemDescription = itemInfo.Description;
            model.ItemSystem = GetSystem(model.ItemID, 1);
            model.ParentBreadList = ComposeBreadInfo(model.ItemID, new List<BreadItem>());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ItemLogsView model, HttpPostedFileBase MainPic)
        {
            //bool hasFile = ImageHelper.CheckFileExists(MainPic);
            //if (model.ID == 0 && !hasFile)
            //{
            //    ModelState.AddModelError(nameof(model.MainPic), "請上傳圖片");
            //}

            if (ModelState.IsValid)
            {
                //if (hasFile)
                //{
                //    ImageHelper.DeleteFile(PhotoFolder, model.MainPic);
                //    model.MainPic = ImageHelper.SaveFile(MainPic, PhotoFolder);
                //}

                ItemLogs data = Mapper.Map<ItemLogs>(model);
                int adminId = AdminInfoHelper.GetAdminInfo().ID;
                var lastLog = itemLogsRepository.Query(model.ItemID).OrderByDescending(q => q.CreateDate).FirstOrDefault();
                if (lastLog != null)
                {
                    lastLog.IsLatest = false;
                    itemLogsRepository.Update(lastLog);
                }
                data.IsLatest = true;
                var interval = itemsRepository.GetById(data.ItemID).Interval;
                data.NextCheckDate = DateTime.Now.AddDays(interval);

                if (model.ID == 0)
                {
                    model.ID = itemLogsRepository.Insert(data);
                    ShowMessage(true, "新增成功");
                }
                else
                {
                    itemLogsRepository.Update(data);
                    ShowMessage(true, "修改成功");
                }

                #region 檔案處理
                if (model.PicsFiles != null)
                {
                    foreach (HttpPostedFileBase pic in model.PicsFiles)
                    {
                        bool hasFile = ImageHelper.CheckFileExists(pic);
                        if (hasFile)
                        {
                            ItemLogsImage repairData = new ItemLogsImage();
                            //ImageHelper.DeleteFile(PhotoFolder, model.FileOne);
                            repairData.LogID = model.ID;
                            repairData.Pics = ImageHelper.SaveFile(pic, PhotoFolder);
                            itemsLogsImageRepository.Insert(repairData);
                        }
                    }
                }
                #endregion
                return RedirectToAction("Edit", new { id = model.ID, ItemID = model.ItemID });
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            itemLogsRepository.Delete(id);
            ShowMessage(true, "刪除成功");
            return RedirectToAction("Index");
        }

        public ActionResult DeleteFile(int id)
        {
            itemsLogsImageRepository.Delete(id);
            ShowMessage(true, "刪除成功");
            return RedirectToAction("Index");
        }
    }
}