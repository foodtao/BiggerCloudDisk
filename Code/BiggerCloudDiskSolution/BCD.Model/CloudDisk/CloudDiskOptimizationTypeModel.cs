using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.Model.CloudDisk
{
    /// <summary>
    /// 网盘的不同优化结果类型
    /// </summary>
    public enum CloudDiskOptimizationTypeModel
    {
        /// <summary>
        /// 速度最快
        /// </summary>
        FASTEST = 1,

        /// <summary>
        /// 容量最大
        /// </summary>
        BIGGEST = 2,

        /// <summary>
        /// 可用容量最大
        /// </summary>
        AVAILABLE_BIGGEST = 3,

        /// <summary>
        /// 所有可用的网盘
        /// </summary>
        ALL=4,

        /// <summary>
        /// 随机选择
        /// </summary>
        RANDOM=5,

        /// <summary>
        /// 其他情况
        /// </summary>
        OTHER=10,
    }
}
