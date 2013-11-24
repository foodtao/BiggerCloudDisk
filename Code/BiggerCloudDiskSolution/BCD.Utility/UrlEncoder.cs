using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BCD.Utility
{
    public static class UrlEncoder
    {
        /// <summary>
        /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
        /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth
        /// </summary>
        /// <param name="value">The value to Url encode</param>
        /// <returns>Returns a Url encoded string</returns>
        public static string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in value)
            {
                result.Append('%' + String.Format("{0:X2}", (int)symbol));
            }

            return result.ToString();
        }

        public static string UrlDecode(string value) 
        {
            return HttpUtility.UrlDecode(value);
        }
    }
}
