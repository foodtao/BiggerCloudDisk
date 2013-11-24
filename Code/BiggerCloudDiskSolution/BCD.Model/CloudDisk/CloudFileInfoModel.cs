using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.Model.CloudDisk
{
    /// <summary>
    /// 网盘文件/文件夹信息
    /// 2013-11-19 by 丁智渊
    /// </summary>
    public class CloudFileInfoModel
    {
        /// <summary>
        /// 远程文件ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 所属网盘
        /// </summary>
        public CloudDiskType DiskType { get; set; }

        /// <summary>
        /// 文件大小的字符串描述
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 文件大小字节数
        /// </summary>
        public double Bytes { get; set; }

        /// <summary>
        /// 最后修改日期,可以为空
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public bool IsDir { get; set; }

        /// <summary>
        /// 远程路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 远程路径对应的本地路径,将所有"\"替换成"/"
        /// </summary>
        public string LocalPath { get; set; }

        /// <summary>
        /// 网盘根路径(有些网盘只允许在某个特定的目录下开放API,此目录即为API的根目录)
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>
        /// 远程网盘的完整路径.形如http://onecloud.com/filepath 
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Mime类型
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// 文件的MD5编码
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        /// 文件夹的Hash编码
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// 文件的SHA1编码
        /// </summary>
        public string SHA1 { get; set; }

        /// <summary>
        /// 如果是文件夹,则这里是目录下的文件信息
        /// </summary>
        public List<CloudFileInfoModel> Contents { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        public string name { get; set; }
    }
}
