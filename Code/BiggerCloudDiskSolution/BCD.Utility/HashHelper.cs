using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace BCD.Utility
{
    public static class HashHelper
    {
        public static string HashStr(string message, string secretKey)
        {
            byte[] msgBytes = System.Text.Encoding.UTF8.GetBytes(message);
            byte[] sceretKeyBytes = System.Text.Encoding.UTF8.GetBytes(secretKey);
            byte[] hashBytes;
            using (HMACSHA1 hmac = new HMACSHA1(sceretKeyBytes))
            {
                hashBytes = hmac.ComputeHash(msgBytes);
            }
            /*
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("x2"));
            string hexString = sb.ToString();
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(hexString);
            return System.Convert.ToBase64String(toEncodeAsBytes);*/
            return System.Convert.ToBase64String(hashBytes);
        }

        public static byte[] Hash(string message, string secretKey)
        {
            byte[] msgBytes = System.Text.Encoding.UTF8.GetBytes(message);
            byte[] sceretKeyBytes = System.Text.Encoding.UTF8.GetBytes(secretKey);
            byte[] hashBytes;
            using (HMACSHA1 hmac = new HMACSHA1(sceretKeyBytes))
            {
                hashBytes = hmac.ComputeHash(msgBytes);
            }
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("x2"));
            string hexString = sb.ToString();
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(hexString);
            return toEncodeAsBytes;
            //return System.Convert.ToBase64String(toEncodeAsBytes);
        }
    }
}
