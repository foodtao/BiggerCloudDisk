using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCD.Model.CloudDisk
{
    /// <summary>
    /// 所有网盘容量数据模型
    /// 2013-11-19 by 丁智渊
    /// </summary>
    public class CloudDiskCapacityModel : SingleCloudDiskCapacityModel
    {
        /// <summary>
        /// 所有网盘中单个网盘最大可用空间
        /// </summary>
        public double MaxAvailableByte { get; set; }

        /// <summary>
        /// 所有网盘中单个网盘最小可用空间
        /// </summary>
        public double MinAvailableByte { get; set; }
    }

    /// <summary>
    /// 单个网盘的容量信息
    /// </summary>
    public class SingleCloudDiskCapacityModel
    {
        /// <summary>
        /// 网盘总空间字节数
        /// </summary>
        public double TotalByte { get; set; }

        /// <summary>
        /// 所有网盘的可用空间总和
        /// </summary>
        public double TotalAvailableByte { get; set; }
    }
}
