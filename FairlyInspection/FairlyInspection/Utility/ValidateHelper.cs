using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Utility
{
    public class ValidateHelper
    {
        public static bool IsValidUrl(string url)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult);
            return result;                
        }
    }
}