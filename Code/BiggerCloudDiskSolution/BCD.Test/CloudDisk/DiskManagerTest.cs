using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCD.DiskInterface;
using BCD.Model;
using BCD.Model.CloudDisk;
using System.IO;

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

        [TestMethod]
        public void Upload()
        {
            CloudDiskManager m = new CloudDiskManager();
            var bytes = File.ReadAllBytes("c:\\1.jpg");
            var result = m.UploadFile(CloudFileUploadType.Create, "/1.txt", bytes);
        }

        [TestMethod]
        public void GetDirInfo()
        {
            CloudDiskManager m = new CloudDiskManager();
            var result = m.GetCloudFileInfo(CloudDiskType.SINA, "/");
        }
    }
}
