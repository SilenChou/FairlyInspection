using AutoMapper;
using FairlyInspection.Areas.Admin.ViewModels.ItemAssign;
using FairlyInspection.Areas.Admin.ViewModels.ItemLogs;
using FairlyInspection.Areas.Admin.ViewModels.Items;
using FairlyInspection.Areas.Admin.ViewModels.Manager;
using FairlyInspection.Models;
using FairlyInspection.Repositories;
using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlyInspection.Areas.Admin.Controllers
{
    public class BaseAdminController : Controller
    {
        public readonly string FileUploads = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploads"];

        // 建立 Mapper
        //public static MapperConfiguration config = new MapperConfiguration(cfg => {
        //    cfg.CreateMap<ManagerView, Models.Admin>();
        //    cfg.CreateMap<Models.Admin, ManagerView>();
        //    cfg.CreateMap<Items, ItemsView>();
        //    cfg.CreateMap<ItemsView, Items>();
        //    cfg.CreateMap<ItemLogs, ItemLogsView>();
        //    cfg.CreateMap<ItemLogsView, ItemLogs>();
        //    cfg.CreateMap<ItemAssign, ItemAssignView>();
        //    cfg.CreateMap<ItemAssignView, ItemAssign>();
        //});

        public string ControllerName
        {
            get
            {
                return this.ControllerContext.RouteData.Values["controller"].ToString();
            }
        }

        /// <summary>
        /// 取得圖片儲存路徑，取得方式為controller名稱，然後將Admin替換成Photo        
        /// </summary>
        public string PhotoFolder
        {
            get
            {
                string controllerName = ControllerName;
                string fileFolder = controllerName.Replace("Admin", "Photo");
                string fileUploads = Server.MapPath(FileUploads);
                string savePath = Path.Combine(fileUploads, fileFolder);
                return savePath;
            }
        }

        /// <summary>
        /// 顯示訊息
        /// 成功時為綠色訊息
        /// 失敗時為紅色訊息
        /// </summary>        
        /// <param name="success">
        /// 是否成功 成功:true 失敗:false
        /// </param>
        /// <param name="message">
        /// 要顯示的訊息，若帶入空字串則預設成[success==true]為"成功"，[success==false]為"失敗"
        /// </param>
        public void ShowMessage(bool success, string message)
        {
            string tempDataKey = success ? "Result" : "Error";

            if (string.IsNullOrEmpty(message))
                message = success ? "Success" : "Failed";

            this.TempData[tempDataKey] = message;
        }

        /// <summary>
        /// 找不到時的頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFound()
        {
            return View("~/Areas/Admin/Views/Shared/NotFound.cshtml");
        }

        /// <summary>
        /// 錯誤頁面
        /// </summary>
        /// <param name="errorMessage">
        /// 錯誤訊息
        /// </param>
        /// <returns></returns>
        public ActionResult Error(string errorMessage = "")
        {
            TempData["ErrorMessage"] = errorMessage;
            return View("~/Areas/Admin/Views/Shared/Error.cshtml");
        }

        /// <summary>
        /// CKEditor上傳圖片
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase upload)
        {
            string newFileName = ImageHelper.SaveFile(upload, PhotoFolder);

            string photoFolder = ControllerName.Replace("Admin", "Photo");
            string savePath = string.Format("{0}/{1}/",
                FileUploads.Replace("~", ""),
                photoFolder
                );

            return Json(new { uploaded = 1, fileName = newFileName, url = savePath + newFileName });
        }

        private ItemsRepository itemsRepository = new ItemsRepository(new InspectionEntities());
        public List<BreadItem> ComposeBreadInfo(int itemId, List<BreadItem> list, bool isModify = true)
        {
            if (itemId != 0)
            {
                var thisInfo = itemsRepository.GetById(itemId);
                if (!isModify &&!thisInfo.IsOnline)
                {
                    return null;
                }
                list.Add(new BreadItem()
                {
                    ID = thisInfo.ID,
                    Title = thisInfo.Title,
                    ParentID = thisInfo.ParentID,
                    Level = thisInfo.Level,
                    LevelStr = EnumHelper.GetDescription((LevelClass)thisInfo.Level),
                });
                if (thisInfo.ParentID != 0)
                {
                    list = ComposeBreadInfo(thisInfo.ParentID, list, isModify);
                }
            }
            if (list == null)
            {
                return null;
            }
            return list.OrderBy(q => q.Level).ToList();
        }

        public int GetSystem(int itemId, int factoryId)
        {
            var data = itemsRepository.GetById(itemId);
            if (data != null)
            {
                if (data.Level > 2)
                {
                    factoryId = GetSystem(data.ParentID, factoryId);
                }
                else if (data.Level == 2)
                {
                    factoryId = data.ID;
                }
            }

            return factoryId;
        }
    }
}