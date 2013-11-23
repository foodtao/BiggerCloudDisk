using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Xml;

namespace BCD.Utility
{
    public class JsonHelper
    {
        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// 将Json字符串反序列化成.net对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object DeserializeObject(string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// 将Json字符串转换为Xml
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlNode DeserializeToXmlNode(string value)
        {
            return JsonConvert.DeserializeXmlNode(value, "Root");
        }
    }
}
