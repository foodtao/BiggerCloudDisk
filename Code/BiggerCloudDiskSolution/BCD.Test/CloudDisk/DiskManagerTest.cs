using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCD.DiskInterface;
using BCD.Model;

namespace BCD.Test.CloudDisk
{
    [TestClass]
    public class DiskManagerTest
    {
        [TestMethod]
        public void GetDiskCapacityInfo()
        {
            ///新浪测试通过
            CloudDiskManager m = new CloudDiskManager();
            var s = m.GetCloudDiskCapacityInfo();
        }
    }
}
