using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FairlyInspection.Utility.Helpers
{
    public class ImageHelper
    {
        /// <summary>
        /// 檔案大小限制
        /// </summary>
        private const int imageSizeLimit = 3;

        /// <summary>
        /// 允許上傳圖片的副檔名
        /// </summary>
        private const string imageExtensionLimit = ".jpg.png";

        /// <summary>
        /// 儲存圖片
        /// </summary>
        /// <param name="file">
        /// 要儲存的檔案
        /// </param>
        /// <param name="savePath">
        /// 儲存的路徑
        /// </param>
        /// /// <param name="fileName">
        /// 檔名，若輸入空字串則用GenerateFileName()方法產生
        /// </param>
        /// <returns>
        /// 儲存結果，
        /// 成功時Message為檔案名稱
        /// </returns>
        public static string SaveFile(HttpPostedFileBase file, string savePath, string fileName = "")
        {            
            string extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = GenerateFileName();
            }
            string newFileName = string.Format("{0}{1}", fileName, extension);
            Directory.CreateDirectory(savePath);
            string path = Path.Combine(savePath, newFileName);
            file.SaveAs(path);
            return newFileName;
        }

        /// <summary>
        /// 檢查圖片是否存在
        /// </summary>
        /// <param name="photoFile"></param>
        /// <returns></returns>
        public static bool CheckFileExists(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return false;
            }
            if (file.ContentLength == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 檢查圖片是否符合格式
        /// 1.副檔名
        /// 2.檔案大小
        /// </summary>
        /// <param name="photoFile"></param>
        /// <returns></returns>
        public static string CheckIfFileSpec(HttpPostedFileBase photoFile)
        {
            string extension = Path.GetExtension(photoFile.FileName);
            if (imageExtensionLimit.IndexOf(extension) == -1)
            {
                return "圖片檔案格式不符";
            }
            int limit = Convert.ToInt32(imageSizeLimit) * 1024 * 1024;
            if (photoFile.ContentLength > limit)
            {
                string message = string.Format("圖片檔案大小不得超過{0}MB", imageSizeLimit);
                return message;
            }
            return string.Empty;
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="filename">檔名</param>
        /// <param name="path">路徑</param>
        public static void DeleteFile(string path, string filename)
        {
            filename = filename ?? "";
            string ComBinePath = Path.Combine(path, filename);

            if (File.Exists(ComBinePath))
            {
                File.Delete(ComBinePath);
            }
        }

        public static string GenerateFileName()
        {
            return DateTime.Now.ToString("yyyyMMddhhmmssfff");
        }
    }
}