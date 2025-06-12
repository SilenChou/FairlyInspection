using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Models
{
    public class MessageView
    {
        public string MID { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackUrl { get; set; }
        public string Message { get; set; }
        public int STKID { get; set; }
        public int STKPKGID { get; set; }
        public string ImageFile { get; set; }
        public string ImageUrl { get; set; }
        public string ImageThumbnailUrl { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
    }
}