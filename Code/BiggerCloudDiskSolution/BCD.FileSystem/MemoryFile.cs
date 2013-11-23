using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.FileSystem
{
    /// <summary>
    /// 缓存文件信息
    /// </summary>
    public class MemoryFile
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件类型。
        /// </summary>
        public FileTypeEnum FileType { get; set; }

        /// <summary>
        /// 文件状态
        /// </summary>
        public FileStatusEnum FileStatus { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 最后修改时间。
        /// </summary>
        public DateTime? LastModifyDate { get; set; }
    }

    /// <summary>
    /// 文件类型。
    /// </summary>
    public enum FileTypeEnum
    {
        /// <summary>
        /// 文件
        /// </summary>
        File,

        /// <summary>
        /// 目录
        /// </summary>
        Directory
    }

    /// <summary>
    /// 文件状态
    /// </summary>
    public enum FileStatusEnum
    {
        /// <summary>
        /// 新建
        /// </summary>
        Create,

        /// <summary>
        /// 修改
        /// </summary>
        Append,

        /// <summary>
        /// 删除
        /// </summary>
        Remove,

        /// <summary>
        /// 正常
        /// </summary>
        Normal,
    }
}
