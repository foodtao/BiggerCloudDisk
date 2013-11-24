using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.Utility
{
    /// <summary>
    /// 本地路径和远程路径互相转换类
    /// </summary>
    public class PathConverter
    {
        /// <summary>
        /// 本地相对路径变成远程相对路径形如"\Dir1\Dir2\1.txt"->"/Dir1/Dir2/1.txt"
        /// </summary>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public string LocalPathToRemotePath(string localPath)
        {
            string remotePath = "";
            remotePath = localPath.Replace("\\", "/");
            return remotePath;
        }

        /// <summary>
        /// 本地相对路径变成远程相对路径形如"\Dir1\Dir2\1.txt"->"/Dir1/Dir2/1.txt"
        /// </summary>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public string RemotePathToLocalPath(string remotePath)
        {
            string localPath = "";
            localPath = remotePath.Replace("/", "\\");
            return localPath;
        }
    }
}
