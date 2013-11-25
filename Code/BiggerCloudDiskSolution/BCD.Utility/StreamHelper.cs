using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BCD.Utility
{
    /// <summary>
    /// Stream流转换助手
    /// 2013-11-24 by 丁智渊
    /// </summary>
    public class StreamHelper
    {
        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 无法获取长度的网络流
        /// </summary>
        /// <param name="instream"></param>
        /// <param name="outstream"></param>
        public static void CopyStream(Stream instream, Stream outstream)
        {
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            while ((count = instream.Read(buffer, 0, bufferLen)) > 0)
            {
                outstream.Write(buffer, 0, count);
            }
        }
    }
}
