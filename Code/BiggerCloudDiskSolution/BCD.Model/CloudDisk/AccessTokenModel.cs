using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.Model.CloudDisk
{
    /// <summary>
    /// 网盘oAuth认证后返回的读写权限令牌
    /// 2013-11-23 by 丁智渊
    /// </summary>
    public class AccessTokenModel
    {
        /// <summary>
        /// 网盘类型
        /// </summary>
        public CloudDiskType DiskType { get; set; }

        /// <summary>
        /// 返回的完整的字符串
        /// </summary>
        public string FullText { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 过期时间(单位秒)
        /// </summary>
        public string Expire { get; set; }
    }
}
