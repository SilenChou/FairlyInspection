using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FairlyInspection.Utility.Helpers
{
    public class LineCardHelper
    {
        //public string ClassCardsJsonText(ViewModels.Repair.RepairView model, string urlHost, string liffID, string link)
        //{
        //    var result = "";
        //    string jsonText = "";
        //    string icontext = "";
        //    string linkjson = "";
        //    string sharelinkjson = "";
        //    string tagjson = "";
        //    string BackgroundColor = "#bfa478";
        //    string TextColor = "#ffffff";

        //    icontext = "{\"type\": \"box\"," +
        //                "\"layout\": \"baseline\"," +
        //                "\"contents\": [" + "{\"type\": \"icon\"," +
        //                //"\"url\": \"" + Request.Url.Scheme + "://" + Request.Url.Host + "/Content/images/" + sharepng + "\"," +
        //                "\"url\": \"" + urlHost + "/Content/img/share-black.png\"," +
        //                "\"size\": \"xl\"}]," +
        //                "\"position\": \"absolute\"," +
        //                "\"action\": {" +
        //                "\"type\": \"uri\"," +
        //                "\"label\": \"action\"," +
        //                //"\"uri\":\"" + "https://liff.line.me/" + liffID + "/?id=" + model.ID + "\"}," +
        //                "\"uri\":\"" + urlHost + link + model.ID + "\"}," +
        //                "\"paddingTop\": \"20px\"," +
        //                "\"offsetStart\": \"260px\"},";


        //    linkjson = "{\"type\": \"box\"," +
        //                "\"cornerRadius\": \"10px\"," +
        //                "\"layout\": \"vertical\"," +
        //                "\"paddingAll\": \"5px\"," +
        //                "\"action\": {" +
        //                "\"type\": \"uri\"," +
        //                "\"label\":\"action\"," +
        //                "\"uri\": \"" + urlHost + link + model.ID + "\"}," +
        //                "\"contents\":[{" +
        //                "\"align\": \"center\"," +
        //                "\"type\": \"text\"," +
        //                "\"text\": \"課程資訊\"," +
        //                "\"weight\": \"bold\"," +
        //                "\"gravity\": \"center\"," +
        //                "\"color\":\"" + TextColor + "\"}]," +
        //                "\"backgroundColor\": \"" + BackgroundColor + "\"},";

        //    sharelinkjson = "{\"type\": \"box\"," +
        //                "\"cornerRadius\": \"10px\"," +
        //                "\"layout\": \"vertical\"," +
        //                "\"paddingAll\": \"5px\"," +
        //                "\"action\": {" +
        //                "\"type\": \"uri\"," +
        //                "\"label\":\"action\"," +
        //                "\"uri\": \"https://liff.line.me/1656164182-BOEOa24b/" + model.ID + "\"}," +
        //                //"\"uri\": \"https://liff.line.me/1655842947-YdWLJ9J8/" + model.ID + "\"}," +
        //                "\"contents\":[{" +
        //                "\"align\": \"center\"," +
        //                "\"type\": \"text\"," +
        //                "\"text\": \"分享\"," +
        //                "\"weight\": \"bold\"," +
        //                "\"gravity\": \"center\"," +
        //                "\"color\":\"" + TextColor + "\"}]," +
        //                "\"backgroundColor\": \"" + BackgroundColor + "\"}";

        //    string Content = Regex.Replace(model.Subject, @"<[^>]*>", String.Empty);
        //    string content = !string.IsNullOrEmpty(Content.Replace("\r\n", "")) ? ",{\"type\": \"text\",\"wrap\": true,\"size\": \"sm\",\"text\":\"" + Content.Replace("\r\n", "") + "\"}" : null;
        //    string Pic = string.IsNullOrEmpty(model.MainPic) ? "/Content/img/buddahahand.png" : "/FileUploads/CoursePhoto/" + model.MainPic;

        //    //回傳JSON字串
        //    jsonText += "{\"type\": \"bubble\"," +
        //                    "\"hero\": {" +
        //                        "\"type\": \"image\"," +
        //                        "\"url\": \"" + urlHost + Pic + "\"," +
        //                        "\"size\": \"full\"," +
        //                        "\"aspectRatio\": \"15:7\"," +
        //                        "\"aspectMode\": \"cover\"," +
        //                        "\"action\": {" +
        //                          "\"type\": \"uri\"," +
        //                          "\"uri\": \"" + urlHost + link + model.ID + "\"" +
        //                        "}" +
        //                      "}," +
        //                    //"\"header\": {" +
        //                    //    "\"backgroundColor\": \"" + BackgroundColor + "\"," +
        //                    //    "\"layout\": \"horizontal\"," +
        //                    //    "\"spacing\": \"md\"," +
        //                    //    "\"type\": \"box\"," +
        //                    //    "\"contents\": [{" +
        //                    //        "\"borderColor\": \"#ffffff\"," +
        //                    //        "\"borderWidth\": \"3px\"," +
        //                    //        "\"cornerRadius\": \"50px\"," +
        //                    //        "\"width\": \"100px\"," +
        //                    //        "\"height\": \"100px\"," +
        //                    //        "\"layout\": \"vertical\"," +
        //                    //        "\"type\": \"box\"," +
        //                    //        "\"contents\": [{" +
        //                    //            "\"type\": \"image\"," +
        //                    //            "\"align\": \"center\"," +
        //                    //            "\"aspectMode\": \"cover\"," +
        //                    //            "\"aspectRatio\": \"1:1\"," +
        //                    //            "\"gravity\": \"center\"," +
        //                    //            "\"size\": \"full\"," +
        //                    //            "\"url\": \"" + urlHost + "/Content/img/course_default.jpg\"}]}," +
        //                    ////icontext +
        //                    //        "{\"type\": \"box\"," +
        //                    //        "\"layout\": \"vertical\"," +
        //                    //        "\"spacing\": \"sm\"," +
        //                    //        "\"contents\": [{" +
        //                    //                "\"type\": \"text\"," +
        //                    //                "\"text\": \"" + model.Title + "\"," +
        //                    //                "\"color\": \"" + TextColor + "\"," +
        //                    //                "\"size\": \"md\"," +
        //                    //                "\"wrap\": true," +
        //                    //                "\"weight\": \"bold\"}," +
        //                    //            "{\"type\": \"text\"," +
        //                    //                "\"text\": \"善性導師\"," +
        //                    //                "\"color\": \"" + TextColor + "\"," +
        //                    //                "\"size\": \"xs\"," +
        //                    //                "\"wrap\": true," +
        //                    //                "\"weight\": \"bold\"}]," +
        //                    //            "\"justifyContent\": \"center\"," +
        //                    //        "\"offsetTop\": \"10px\"}]}," +
        //                    "\"body\": {" +
        //                        "\"layout\": \"vertical\"," +
        //                        "\"spacing\": \"sm\"," +
        //                        "\"type\": \"box\"," +
        //                        "\"contents\": [" +
        //                            //icontext +
        //                            "{\"type\": \"text\"," +
        //                                "\"text\": \"課程 : " + model.Title + "\"," +
        //                                "\"size\": \"lg\"}," +
        //                            "{\"type\": \"text\"," +
        //                                "\"text\": \"善性導師\"," +
        //                                "\"size\": \"sm\"}," +
        //                            "{\"type\": \"text\"," +
        //                                "\"text\": \"時間 : " + model.ClassDate.ToString("yyyy/M/d") + "\"," +
        //                                "\"size\": \"sm\"}," +
        //                            "{\"type\": \"text\"," +
        //                                "\"text\": \"地址 : " + model.Address + "\"," +
        //                                "\"size\": \"sm\"}," +
        //                            "{\"type\": \"text\"," +
        //                                "\"text\": \"講堂 : " + model.ClassRoomStr + "\"," +
        //                                "\"size\": \"sm\"}," +
        //                            "{\"type\": \"box\"," +
        //                                "\"layout\": \"horizontal\"," +
        //                                "\"spacing\": \"sm\"," +
        //                                "\"contents\": [" +
        //                                    tagjson + "]}" +
        //                            HttpUtility.HtmlDecode(content) + "]}," +
        //                    "\"footer\": {" +
        //                        "\"type\": \"box\"," +
        //                        "\"layout\": \"horizontal\"," +
        //                        "\"spacing\": \"md\"," +
        //                        "\"contents\": [" +
        //                linkjson + sharelinkjson + "]}}";

        //    result = "[{\"type\":\"flex\",\"altText\":\"課程名稱" + model.ClassRoomStr + " - " + model.Title + "，時間是" + model.ClassDate + "地點在" + model.ClassRoomStr + " (" + model.Address + ") " + "，請大家踴躍參加!\",\"contents\":" + jsonText + "}]";
        //    return result;
        //}
    }
}