//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FairlyInspection.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SendSequence
    {
        public System.Guid ID { get; set; }
        public int NotifyID { get; set; }
        public int InterlockID { get; set; }
        public string TokenKey { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool IsSent { get; set; }
        public int MessageType { get; set; }
    }
}
