using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BCD.Utility
{
    /// <summary>
    /// 本地与虚拟磁盘绑定路径。
    /// </summary>
    public class LocalDiskPathHelper
    {
        public static string LocalPath = "";

        private static string savedpathFileName = Directory.GetCurrentDirectory() + "\\" + "LocalDiskPath.txt";

        /// <summary>
        /// 设置与网盘同步文件夹路径。
        /// </summary>
        /// <param name="path"></param>
        public static void SetPath(string path)
        {
            File.WriteAllText(savedpathFileName, path);
        }

        /// <summary>
        /// 获取与网盘同步文件夹路径。
        /// </summary>
        public static string GetPath()
        {
            var path = "";
            if (string.IsNullOrEmpty(LocalPath))
            {
                if (!File.Exists(savedpathFileName))
                {
                    var fs = File.Create(savedpathFileName);
                    fs.Dispose();
                    fs.Close();
                }
                path = File.ReadAllText(savedpathFileName);
            }
            if (!string.IsNullOrEmpty(path))
            {
                return path;
            }
            return "G:\\Temp";
        }

    }
}
