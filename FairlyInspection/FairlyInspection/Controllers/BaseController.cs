using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace FairlyInspection.Controllers
{
    public class BaseController : Controller
    {
        public bool IsNumeric(String strNumber)
        {
            Regex NumberPattern = new Regex("[^0-9.-]");
            //Regex NumberPattern = new Regex("^\\d+$");  //含0正整數
            return !NumberPattern.IsMatch(strNumber);
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
    }
}