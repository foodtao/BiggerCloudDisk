using BCD.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BCD.DiskInterface.Baidu
{
    public static class BaiduSignatureHelper
    {
        public static string GetSignature(string content, string secretKey)
        {
            string result = "";
            result = HashHelper.HashStr(content, secretKey);
            result = HttpUtility.UrlEncode(result);
            return result;
        }
    }
}
