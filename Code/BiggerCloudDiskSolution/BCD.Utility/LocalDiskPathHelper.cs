using System.IO;

namespace BCD.Utility
{
    /// <summary>
    /// 本地与虚拟磁盘绑定路径。
    /// </summary>
    public class LocalDiskPathHelper
    {
        public static string LocalPath = "";

        private static readonly string SavedpathFileName = Directory.GetCurrentDirectory() + "\\" + "LocalDiskPath.txt";

        /// <summary>
        /// 设置与网盘同步文件夹路径。
        /// </summary>
        /// <param name="path"></param>
        public static void SetPath(string path)
        {
            File.WriteAllText(SavedpathFileName, path);
        }

        /// <summary>
        /// 获取与网盘同步文件夹路径。
        /// </summary>
        public static string GetPath()
        {
            var path = "";
            if (string.IsNullOrEmpty(LocalPath))
            {
                if (!File.Exists(SavedpathFileName))
                {
                    var fs = File.Create(SavedpathFileName);
                    fs.Dispose();
                    fs.Close();
                }
                path = File.ReadAllText(SavedpathFileName);
            }
            if (string.IsNullOrEmpty(path))
            {
                path = "C:\\Temp";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            return path;
        }

    }
}
