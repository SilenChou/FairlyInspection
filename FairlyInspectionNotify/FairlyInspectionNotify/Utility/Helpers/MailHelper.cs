using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FairlyInspectionNotify.Utility.Helpers
{
    public class MailHelper
    {
        public static string POP3Host = "10.10.10.6";
        public static string POP3Acc = "notify-fbm@fairlybike.com";
        public static string POP3Pwd = "Nf22687166";
        public static int POP3Port = 25;
        //public static bool SendEmail(string title, string mailbody, List<string> mailList, string ImgPath, string filename)
        //{
        //    try
        //    {
        //        MailAddress From = new MailAddress(ConfigurationManager.AppSettings["SenderMail"], "Team Fairly");//填寫發信電子郵件地址，和顯示名稱
        //        //設置好發送地址，和接收地址，接收地址可以是多個
        //        MailMessage mail = new MailMessage();
        //        mail.From = From;
        //        foreach (var item in mailList)
        //        {
        //            mail.To.Add(item);
        //        }
        //        mail.Subject = title;

        //        System.Text.StringBuilder strBody = new System.Text.StringBuilder();
        //        strBody.Append(mailbody);

        //        mail.Body = strBody.ToString();
        //        mailBody(mail, title, mailbody, ImgPath, filename);
        //        mail.IsBodyHtml = true;//設置顯示htmls

        //        int Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
        //        string Host = ConfigurationManager.AppSettings["Host"];
        //        string CredentialsAcc = ConfigurationManager.AppSettings["CredentialsAcc"];
        //        string CredentialsPwd = ConfigurationManager.AppSettings["CredentialsPwd"];

        //        //設置好發送郵件服務地址
        //        using (SmtpClient client = new SmtpClient(Host, Port))
        //        {
        //            client.Credentials = new NetworkCredential(CredentialsAcc, CredentialsPwd);
        //            client.EnableSsl = false;
        //            client.Send(mail);
        //        }
        //        //SmtpClient client = new SmtpClient();
        //        ////client.Host = "smtp.163.com";
        //        //client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
        //        //client.Host = ConfigurationManager.AppSettings["Host"];
        //        ////client.Host = "login.live.com";                

        //        ////填寫服務器地址相關的用户名和密碼信息
        //        //client.Credentials = new System.Net.NetworkCredential("a842695137@gmail.com", "A1100102250");
        //        //發送郵件
        //        //client.Send(mail);
        //    }
        //    catch (Exception ex)
        //    {
        //        using (StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("/App_Data/maillog.txt")))   //小寫TXT     
        //        {
        //            // Add some text to the file.                  
        //            sw.WriteLine("-------------------");
        //            // Arbitrary objects can also be written to the file.
        //            sw.Write("Date : ");
        //            sw.WriteLine(DateTime.Now);
        //            sw.WriteLine(ex.Message);
        //        }
        //        return false;
        //    }
        //    return true;
        //}

        public static void mailBody(MailMessage mail, string title, string mailBody, string ImgPath, string filename)
        {
            try
            {
                string palinBody = title;
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                         palinBody, null, "text/plain");

                string htmlBody = mailBody;

                // add the views
                mail.AlternateViews.Add(plainView);
                AlternateView htmlView =
                       AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

                //Add Image
                if (!string.IsNullOrEmpty(ImgPath) && !string.IsNullOrEmpty(filename))
                {
                    int l = filename.IndexOf(".");
                    int typel = filename.Length - l;
                    string mediaType = "image/" + filename.Substring(l + 1, typel - 1);

                    //AlternateView htmlView =
                    //   AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");
                    imgResource(htmlView, "eventimg.jpg", mediaType, ImgPath);

                    //// add the views
                    //mail.AlternateViews.Add(htmlView);
                }
                // add the views
                mail.AlternateViews.Add(htmlView);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static void imgResource(AlternateView htmlView, string imgName, string imgType, string ImgPath)
        {
            try
            {
                // create image resource from image path using LinkedResource class..   
                LinkedResource imageResource = new LinkedResource(ImgPath, imgType);
                string[] imgArr = imgName.Split('.');
                imageResource.ContentId = imgArr[0];
                imageResource.TransferEncoding = TransferEncoding.Base64;
                htmlView.LinkedResources.Add(imageResource);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static void POP3Mail(string to, string subject, string body/*, MemoryStream ms = null, string icsStr = "會議邀請"*/)
        {
            try
            {
                MailMessage message = new MailMessage();

                //設定發件人,發件人需要與設定的郵件傳送伺服器的郵箱一致
                MailAddress fromAddr = new MailAddress(POP3Acc);
                message.From = fromAddr;
                //設定收件人,可新增多個,新增方法與下面的一樣
                message.To.Add(to);
                //設定抄送人
                //message.CC.Add("i1o2i2o3i1@gmail.com");
                //設定郵件標題
                message.Subject = subject;
                //設定郵件內容
                message.Body = body;
                message.IsBodyHtml = true;

                //附件
                //if (ms != null)
                //{
                //    #region iCal Attachments                

                //    System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, icsStr + ".ics", "text/calendar");
                //    message.Attachments.Add(attachment);
                //    #endregion
                //}

                //設定郵件傳送伺服器,伺服器根據你使用的郵箱而不同,可以到相應的 郵箱管理後臺檢視
                SmtpClient client = new SmtpClient(POP3Host, POP3Port);
                //設定傳送人的郵箱賬號和授權碼
                client.Credentials = new NetworkCredential(POP3Acc, POP3Pwd);

                //啟用ssl,也就是安全傳送
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AddressOf RemoteCertificateValidationCallback);
                //client.EnableSsl = true;
                //傳送郵件
                client.Send(message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static bool CertificateCheck(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return !certificate.Issuer.Equals("解決");
            // or
            //return false;
        }
    }

}
