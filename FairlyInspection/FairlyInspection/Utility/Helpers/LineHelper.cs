using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Utility.Helpers
{
    public class LineHelper
    {
        public static string GetNotifyToken(string code, string ClientID, string ClientSecret, string CallbackUrl)
        {
            //用code換Token
            var ret = isRock.LineNotify.Utility.GetTokenFromCode(
                 code, ClientID, //ClientID一定要100%對
                 ClientSecret, //ClientSecret 一定要100%對
                 CallbackUrl //Callback url一定要100%對
                 );

            return ret.access_token;
        }
    }
}