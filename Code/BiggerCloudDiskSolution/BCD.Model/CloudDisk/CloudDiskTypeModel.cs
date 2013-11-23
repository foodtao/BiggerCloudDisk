using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.Model.CloudDisk
{
    /// <summary>
    /// 网盘类型
    /// 2013-11-23 by 丁智渊
    /// </summary>
    public enum CloudDiskType
    {
        /// <summary>
        /// 未指定
        /// </summary>
        NOT_SPECIFIED = 0,
 
        /// <summary>
        /// 新浪微盘
        /// </summary>
        SINA = 1,

        /// <summary>
        /// 金山
        /// </summary>
        KINGSOFT = 2,

        /// <summary>
        /// 百度
        /// </summary>
        BAIDU = 3
    }
}
