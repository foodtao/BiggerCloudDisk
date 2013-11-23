using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.DiskInterface.Sina
{
    /// <summary>
    /// 新浪微盘API接口
    /// 2013-11-23 by 丁智渊
    /// </summary>
    public class SinaDiskApi : ICloudDiskAPI
    {
        private string _appKey = "";

        private string _appSecret = "";

        private string _accessToken = "";

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

        public void WriteLocalAccessToken()
        {
            throw new NotImplementedException();
        }

        public Model.CloudDisk.AccessTokenModel GetAccessToken()
        {
            throw new NotImplementedException();
        }

        public Model.CloudDisk.CloudDiskCapacityModel GetCloudDiskCapacityInfo()
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

        public Model.CloudDisk.CloudFileInfoModel MoveFile(string originPath, string newPath, bool isCopy)
        {
            throw new NotImplementedException();
        }
    }
}
