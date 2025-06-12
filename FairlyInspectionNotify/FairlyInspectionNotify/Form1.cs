using FairlyInspectionNotify.Models;
using FairlyInspectionNotify.Repositories;
using FairlyInspectionNotify.Repositories.T8Repositories;
using FairlyInspectionNotify.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FairlyInspectionNotify
{
    public partial class Form1 : Form
    {
        public bool Active = true;
        public string notifyUrl = "https://notify-api.line.me/api/notify";
        string ClientID = "63Zqpardt29Uzt5EiU5rpS";
        string ClientSecret = "LLZaWh3paSVgjfe104Su7CGWzlxnu5Ik6GSspsuiZOg";
        private AdminRepository adminRepository = new AdminRepository(new InspectionEntities());
        private CheckingPathRepository checkingPathRepository = new CheckingPathRepository(new InspectionEntities());
        private InterlockRepository interlockRepository = new InterlockRepository(new InspectionEntities());
        private SequenceRepository sequenceRepository = new SequenceRepository(new InspectionEntities());
        private ItemsRepository itemsRepository = new ItemsRepository(new InspectionEntities());
        private ItemLogsRepository itemLogsRepository = new ItemLogsRepository(new InspectionEntities());
        private ItemAssignRepository itemAssignRepository = new ItemAssignRepository(new InspectionEntities());
        private PersonRepository t8personRepository = new PersonRepository(new T8ERPEntities());
        public int NotifyID = Convert.ToInt32(DateTime.Now.Date.ToString("yyyyMMdd"));

        public Form1()
        {
            InitializeComponent();
            TrayMenuContext();
        }
        private void TrayMenuContext()
        {
            this.notifyIcon1.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.notifyIcon1.ContextMenuStrip.Items.Add("開啟", null, this.button1_Click);
            this.notifyIcon1.ContextMenuStrip.Items.Add("停止", null, this.button2_Click);
            this.notifyIcon1.ContextMenuStrip.Items.Add("關閉", null, this.button3_Click);
        }

        /// <summary>
        /// App 發送訊息
        /// </summary>
        public async Task StartSendNotify()
        {
            try
            {
                await Task.Delay(3000);

                DateTime nowTime = DateTime.Now;

                string link = "http://10.10.10.43:18155/Admin/HomeAdmin/OverView";
                string checkingMsg = "\r\n";
                //string msg1 = "\r\n";
                //string msg2 = "\r\n";
                //var logList = itemsRepository.ItemsJoinLogsQuery(0, 0, true).ToList();
                var allCheckingPendingList = checkingPathRepository.Query(true, "", true).ToList();
                //通知巡檢
                //if (allCheckingPendingList != null && allCheckingPendingList.Count() > 0)
                //{
                //    msg1 += " \r\n !! 巡檢未執行通知 !!  \r\n";
                //    msg1 += " \r\n 有部分設備已超過巡檢時間但尚未檢查，請點擊連結前往察看 \r\n << " + link + " >> \r\n";

                //    msg2 += " \r\n !! 巡檢提醒 !!  \r\n";
                //    msg2 += " \r\n 您負責的項目已到檢查時間，請點擊連結前往查看 \r\n << " + link + " >> \r\n";
                //}

                #region 新增Line發送序列
                //string hour_test = DateTime.Now.ToString("HHmm");
                //int test = hour_test == "1800" ? 1 : 0;
                if ((allCheckingPendingList != null && allCheckingPendingList.Count() > 0))
                {
                    checkingMsg += " \r\n !! 巡檢提醒 !!  \r\n";
                    checkingMsg += " \r\n 您負責的項目已到檢查時間，請點擊連結前往查看 \r\n << " + link + " >> \r\n";
                    if (NotifyID != Convert.ToInt32(DateTime.Now.Date.ToString("yyyyMMdd")))
                    {
                        NotifyID = Convert.ToInt32(DateTime.Now.Date.ToString("yyyyMMdd"));
                    }
                    #region 發送訊息
                    //var allPendingList = itemsRepository.ItemsJoinLogsQuery(0, 0, true);
                    //var allCheckingPendingList = checkingPathRepository.Query(true, "", true);
                    var interlockList = interlockRepository.Query().ToList();

                    //新增Line通知
                    #region 通知負責人員                    
                    //foreach (var personId in interlockList.Select(q => q.JobID))
                    //{
                    //    var assignList = itemAssignRepository.Query(0, personId);
                    //    var IdList = assignList.Select(q => q.ItemID).ToList();
                    //    var PendingList = allPendingList.Where(q => IdList.Contains(q.ItemSystem)).ToList();
                    //    if (PendingList.Count() > 0)
                    //    {
                    //        var ilock = interlockRepository.FindByJobID(personId);
                    //        Guid Id = Guid.NewGuid();
                    //        //檢查是否已加入佇列
                    //        var chk = sequenceRepository.Query(null, NotifyID, ilock.ID).Count() > 0;
                    //        if (ilock.ID == 0 || chk)
                    //        {//缺少必要參數，跳出當次連動迴圈
                    //            continue;
                    //        }
                    //        SendSequence sq = new SendSequence
                    //        {
                    //            ID = Id,
                    //            NotifyID = NotifyID,
                    //            InterlockID = ilock.ID,
                    //            TokenKey = ilock.TokenKey,
                    //            CreateDate = DateTime.Now,
                    //            IsSent = false,
                    //            MessageType = 2
                    //        };
                    //        sequenceRepository.Insert(sq, false);
                    //    }
                    //}
                    #endregion

                    #region 通知巡檢人員
                    var inspectorPersonIdList = adminRepository.Query(true).Where(q => q.Type == (int)AdminType.Inspector).Select(q => q.Account).ToList();
                    DateTime now = DateTime.Now;
                    if (allCheckingPendingList.Where(q => q.NextDate <= now).Count() > 0)
                    {
                        var inspectorILockList = interlockList.Where(q => inspectorPersonIdList.Contains(q.JobID));
                        foreach (var ilock in inspectorILockList)
                        {
                            //1.建立Line訊息序列
                            Guid Id = Guid.NewGuid();
                            SendSequence sq = new SendSequence
                            {
                                ID = Id,
                                NotifyID = NotifyID,
                                InterlockID = ilock.ID,
                                TokenKey = ilock.TokenKey,
                                CreateDate = DateTime.Now,
                                IsSent = false,
                                MessageType = 1
                            };
                            sequenceRepository.Insert(sq, false);

                            #region 寄送POP3郵件
                            if (sq.MessageType == 1)
                            {
                                var reciver = t8personRepository.FindByJobID(ilock.JobID);
                                if (reciver != null)
                                {
                                    MailHelper.POP3Mail(reciver.Email, "巡檢逾時未檢查通知", checkingMsg);
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #endregion
                }

                #endregion

                if (1 > 0)
                {
                    #region 發送訊息
                    //2. 寄送未超過限制的訊息
                    int acceptCount = 1000;
                    var allSq = sequenceRepository.GetAll();
                    var sequenceList = allSq.OrderBy(q => q.CreateDate).ToList();
                    if (sequenceList.Count() > 0)
                    {
                        //計算1小時內已發送/占用額度的訊息數量
                        var occupyTime = nowTime.AddHours(-1);
                        var occupy = sequenceList.Where(q => q.CreateDate > occupyTime && q.IsSent == true).Count();
                        int sendable = acceptCount - occupy;

                        if (sendable > 0)
                        {                            
                            string msg = "";                          

                            var unSentList = sequenceList.Where(q => q.IsSent == false).OrderBy(q => q.CreateDate).Take(sendable);

                            if (allCheckingPendingList.Count() > 0)
                            {
                                foreach (var sq in unSentList)
                                {
                                    //msg = sq.MessageType == 1 ? msg1 : msg2;
                                    msg = checkingMsg;

                                    var result = SetNotify(msg, "", "", sq.InterlockID, sq.TokenKey);                                 

                                    if (result == "success")
                                    {//更新已發送的連動序列
                                        sq.IsSent = true;
                                        sequenceRepository.Update(sq, false);
                                    }
                                    if (result == "delete")
                                    {//刪除已解除連動的序列
                                        sequenceRepository.DeleteByGuid(sq.ID);
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region 刪除已發送完畢的訊息序列
                    //3. 刪除已發送完畢的訊息序列
                    var sentList = sequenceRepository.Query(true).OrderBy(q => q.NotifyID).ToList();
                    if (sentList.Count() > 0)
                    {
                        foreach (var item in sentList)
                        {
                            if (sequenceRepository.Query(false, item.NotifyID).Count() == 0)
                            {
                                var occupyTime = nowTime.AddHours(-1);
                                var occupy = sequenceList.Where(q => q.CreateDate < occupyTime && q.IsSent == true);
                                foreach (var sq in occupy)
                                {
                                    sequenceRepository.DeleteByGuid(sq.ID);
                                }

                                #region 刪除上傳圖檔
                                //if (thisnotify.ImageUrl.IndexOf("MessagePhoto") > 0)
                                //{
                                //    try
                                //    {
                                //        string Url = thisnotify.ImageUrl;
                                //        string fileName = Url.Substring(Url.IndexOf("MessagePhoto") + 13, (Url.Length - Url.IndexOf("MessagePhoto")) - 13);
                                //        string FilePath = $"C:\\inetpub\\wwwroot\\notify.cmindline.com\\MessagePhoto\\" + fileName;
                                //        File.Delete(FilePath);
                                //    }
                                //    catch (Exception e)
                                //    {
                                //        textBox1.Text += "\r\n" + e.Message;
                                //    }
                                //}
                                #endregion
                            }
                        }
                    }
                    #endregion

                }

                //關閉程式
                await Task.Delay(3000);
                System.Environment.Exit(System.Environment.ExitCode);
                this.Dispose();
                this.Close();
            }
            catch (Exception e)
            {
                textBox1.Text = "\r\n" + e.Message;
                throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {//開啟
            Active = true;
            textBox1.Text = "\r\n開始發送訊息 。。。";
            Task.Run(StartSendNotify);
        }

        private void button2_Click(object sender, EventArgs e)
        {//停止
            Active = false;
            textBox1.Text += "\r\n已停止執行";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
            this.Dispose();
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button3.Visible = false;
            Active = true;
            Task.Run(StartSendNotify);
        }

        #region Send Notify Message
        /// <summary>
        /// 發送Notify (multipart)
        /// </summary>
        /// <param name="MemberID"></param>
        /// <param name="ClientID"></param>
        /// <param name="CallbackUrl"></param>
        /// <param name="Token"></param>
        /// <param name="ClientSecret"></param>
        /// <param name="Message"></param>
        /// <param name="ImageFile"></param>
        /// <param name="ImageUrl"></param>
        /// <param name="ImageThumbnailUrl"></param>
        /// <param name="STKID"></param>
        /// <param name="STKPKGID"></param>
        /// <returns></returns>
        public string SetNotify(string Message, string ImageUrl, string Sticker, int Iid, string TokenKey)
        {
            //var serviceInfo = serviceRepository.GetById(sid);
            var stk = Sticker.Split(',');
            MessageView imgmodel = new MessageView
            {
                ClientID = ClientID,
                CallbackUrl = "http://10.10.10.43:18155/Api/Callback",
                ClientSecret = ClientSecret,
                Message = "",
                ImageUrl = ImageUrl,
            };
            MessageView model = new MessageView
            {
                ClientID = ClientID,
                CallbackUrl = "http://10.10.10.43:18155/Api/Callback",
                ClientSecret = ClientSecret,
                Message = Message,
                ImageUrl = "",
            };
            if (stk.Count() == 2)
            {
                model.STKID = Convert.ToInt32(stk[0]);
                model.STKPKGID = Convert.ToInt32(stk[1]);
            }

            List<int> delList = new List<int>();

            int res = multipartTest(imgmodel, TokenKey);
            res = multipartTest(model, TokenKey);
            if (res == 0)
            {
                interlockRepository.Delete(Iid);
                return "delete";
            }

            return "success";
        }

        /// <summary>
        /// multipart 發送訊息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        private int multipartTest(MessageView model, string tokenKey)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Message))
                    model.Message = "發送訊息~";
                Dictionary<string, object> d = new Dictionary<string, object>()
                {
                    // message , imageFile ... name is provided by LINE API
                    { "message", model.Message },
                    //{ "imageFile", new FormFile()
                    //    {
                    //        Name = "notify.jpg", ContentType = "image/jpeg", FilePath="notify.jpg"
                    //    }
                    //}
                };
                if (model.STKID != 0 && model.STKPKGID != 0)
                {
                    d.Add("stickerPackageId", model.STKPKGID.ToString());
                    d.Add("stickerId", model.STKID.ToString());
                    //postData += "&stickerPackageId=" + model.STKPKGID.ToString();
                    //postData += "&stickerId=" + model.STKID.ToString();
                }

                if (!string.IsNullOrEmpty(model.ImageFile))
                {
                    //d.Add("imageFile", new FormFile()
                    //{ Name = Path.GetFileName(model.ImageFile), ContentType = "image/jpeg", FilePath = model.ImageFile }
                    //);
                }

                if (!string.IsNullOrEmpty(model.ImageUrl))
                {
                    d.Add("imageFullsize", model.ImageUrl);
                    if (!string.IsNullOrEmpty(model.ImageThumbnailUrl))
                        d.Add("imageThumbnail", model.ImageThumbnailUrl);
                    else
                        d.Add("imageThumbnail", model.ImageUrl);
                }

                string boundary = "Boundary";
                List<byte[]> output = genMultPart(d, boundary);
                int res = lineNotifyMultipart(tokenKey, boundary, output);
                return res;
            }
            catch (Exception ex)
            {
                return 7;
            }
        }

        private int lineNotifyMultipart(string access_token, string boundary, List<byte[]> output)
        {
            try
            {
                #region POST multipart/form-data
                StringBuilder sb = new StringBuilder();
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(notifyUrl);
                request.Method = "POST";
                request.ContentType = "multipart/form-data; boundary=Boundary";
                request.Timeout = 30000;

                // header
                sb.Clear();
                sb.Append("Bearer ");
                sb.Append(access_token);
                request.Headers.Add("Authorization", sb.ToString());
                // note: multipart/form-data boundary must exist in headers ContentType
                sb.Clear();
                sb.Append("multipart/form-data; boundary=");
                sb.Append(boundary);
                request.ContentType = sb.ToString();

                // write Post Body Message
                BinaryWriter bw = new BinaryWriter(request.GetRequestStream());
                foreach (byte[] bytes in output)
                    bw.Write(bytes);

                #endregion

                int res = getResponse(request, access_token);

                return res;

            }
            catch (Exception ex)
            {
                #region Exception
                StringBuilder sbEx = new StringBuilder();
                sbEx.Append(ex.GetType());
                sbEx.AppendLine();
                sbEx.AppendLine(ex.Message);
                sbEx.AppendLine(ex.StackTrace);
                if (ex.InnerException != null)
                    sbEx.AppendLine(ex.InnerException.Message);
                //myException ex2 = new myException(sbEx.ToString());
                //message(ex2.Message);
                return 6;
                #endregion
            }
        }

        private int getResponse(HttpWebRequest request, string access_token)
        {
            StringBuilder sb = new StringBuilder();
            string result = string.Empty;
            StreamReader sr = null;
            try
            {
                #region Get Response
                if (request == null)
                    return 99;
                // HttpWebRequest GetResponse() if error happened will trigger WebException
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    sb.AppendLine();
                    foreach (var x in response.Headers)
                    {
                        sb.Append(x);
                        sb.Append(" : ");
                        sb.Append(response.Headers[x.ToString()]);
                        if (x.ToString() == "X-RateLimit-Reset")
                        {
                            sb.Append(" ( ");
                            sb.Append(DateTimeOffset.FromFileTime(long.Parse(response.Headers[x.ToString()])));
                            sb.Append(" )");
                        }
                        sb.AppendLine();
                    }
                    using (sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        sb.Append(result);
                    }
                }
                return 1;
                //message(sb.ToString());

                #endregion
            }
            catch (WebException ex)
            {
                if (ex.Message == "遠端伺服器傳回一個錯誤: (401) 未經授權。")
                {
                    return 0;
                }
                else
                {
                    #region WebException handle
                    //var wrong_token = tokenRepository.GetAll().Where(q => q.TokenKey == access_token).FirstOrDefault();
                    //tokenRepository.Delete(wrong_token.ID);

                    // WebException Response
                    using (HttpWebResponse response = (HttpWebResponse)ex.Response)
                    {
                        sb.AppendLine("Error");
                        foreach (var x in response.Headers)
                        {
                            sb.Append(x);
                            sb.Append(" : ");
                            sb.Append(response.Headers[x.ToString()]);
                            sb.AppendLine();
                        }
                        using (sr = new StreamReader(response.GetResponseStream()))
                        {
                            result = sr.ReadToEnd();
                            sb.Append(result);
                        }

                        //message(sb.ToString());
                    }
                    return 5;
                    #endregion
                }
            }
        }

        public List<byte[]> genMultPart(Dictionary<string, object> parameters, string boundary)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("\r\n--");
            sb.Append(boundary);
            sb.Append("\r\n");
            string beginBoundary = sb.ToString();
            sb.Clear();
            sb.Append("\r\n--");
            sb.Append(boundary);
            sb.Append("--\r\n");
            string endBoundary = sb.ToString();
            sb.Clear();
            sb.Append("Content-Type: multipart/form-data; boundary=");
            sb.Append(boundary);
            sb.Append("\r\n");
            List<byte[]> byteList = new List<byte[]>();
            byteList.Add(System.Text.Encoding.UTF8.GetBytes(sb.ToString()));

            foreach (KeyValuePair<string, object> pair in parameters)
            {
                byteList.Add(System.Text.Encoding.ASCII.GetBytes(beginBoundary));
                sb.Clear();
                sb.Append("Content-Disposition: form-data; name=\"");
                sb.Append(pair.Key);
                sb.Append("\"");
                sb.Append("\r\n\r\n");
                sb.Append(pair.Value);
                string data = sb.ToString();
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
                byteList.Add(bytes);
                //}
            }

            byteList.Add(System.Text.Encoding.ASCII.GetBytes(endBoundary));
            return byteList;
        }

        #endregion

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.ShowInTaskbar == false)
                notifyIcon1.Visible = true;

            this.ShowInTaskbar = true;
            this.Show();
            this.Activate();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
            this.Dispose();
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                notifyIcon1.Visible = true;
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }
    }
}
