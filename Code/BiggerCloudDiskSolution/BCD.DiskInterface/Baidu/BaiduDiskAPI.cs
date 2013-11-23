using BCD.Model.CloudDisk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.DiskInterface.Baidu
{
    public class BaiduDiskAPI : ICloudDiskAPI
    {
        /// <summary>
        /// 获取本网盘的类型
        /// </summary>
        /// <returns></returns>
        public CloudDiskType GetDiskType()
        {
            return CloudDiskType.BAIDU;
        }

        public string GetLocalStoredAppKey()
        {
            throw new NotImplementedException();
        }

        public string GetLocalStoredAppSeceret()
        {
            throw new NotImplementedException();
        }

        public string GetLocalStoredAccessToken()
        {
            throw new NotImplementedException();
        }

        public void WriteLocalAccessToken(string newToken)
        {
            throw new NotImplementedException();
        }

        public Model.CloudDisk.AccessTokenModel GetAccessToken()
        {
            throw new NotImplementedException();
        }

        public SingleCloudDiskCapacityModel GetCloudDiskCapacityInfo()
        {
            throw new NotImplementedException();
        }

        public Model.CloudDisk.CloudFileInfoModel GetCloudFileInfo(string remotePath)
        {
            throw new NotImplementedException();
        }

        public Model.CloudDisk.CloudFileInfoModel UploadFile(byte[] fileContent)
        {
            throw new NotImplementedException();
        }

        public byte[] DownloadFile(string remotePath)
        {
            throw new NotImplementedException();
        }

        public Model.CloudDisk.CloudFileInfoModel CreateDirectory(string dir)
        {
            throw new NotImplementedException();
        }

        public int DeleteFile(string remotePath)
        {
            throw new NotImplementedException();
        }

        public int DeleteDirectory(string remotePath)
        {
            throw new NotImplementedException();
        }

        public Model.CloudDisk.CloudFileInfoModel MoveFile(string originPath, string newPath, bool isCopy)
        {
            throw new NotImplementedException();
        }
    }
}
