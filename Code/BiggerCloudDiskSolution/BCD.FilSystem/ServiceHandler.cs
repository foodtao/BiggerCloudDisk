using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCD.DiskInterface;

namespace BCD.FilSystem
{
    using System.IO;
    class ServiceHandler
    {
        public int CreateDir(string dirName)
        {
            var client = new CloudDiskManager();
            client.CreateDirectory(dirName);
            return 1;
        }

        public int RemoveDir(string dirName)
        {
            var client = new CloudDiskManager();
            var result = client.DeleteFile(dirName);
            return 1;
        }

        public int CreateFile(byte[] fileContent)
        {
            var client = new CloudDiskManager();
            client.UploadFile(fileContent);
            return 1;
        }

        public int PullFile()
        {
            return 1;
        }

    }
}
