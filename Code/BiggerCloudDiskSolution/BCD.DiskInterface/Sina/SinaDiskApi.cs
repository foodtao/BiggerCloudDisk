using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using BCD.Model.CloudDisk;
using BCD.Utility;
using System.Xml;

namespace BCD.DiskInterface.Sina
{
    /// <summary>
    /// 新浪微盘API接口
    /// 2013-11-23 by 丁智渊
    /// </summary>
    public class SinaDiskApi : ICloudDiskAPI
    {
        ///sina token第一次获取方法:浏览器访问 https://auth.sina.com.cn/oauth2/authorize?client_id=1551059632&redirect_uri=http://localhost/&response_type=token
        ///在返回的页面里手动复制access_token部分值
        private string _appKey = "";

        private string _appSecret = "";

        private string _accessToken = "";

        public CloudDiskType DiskType { get { return CloudDiskType.SINA; } }

        public SinaDiskApi()
        {
            _appKey = GetLocalStoredAppKey();
            _appSecret = GetLocalStoredAppSeceret();
            _accessToken = GetLocalStoredAccessToken();
        }

        /// <summary>
        /// 获取本网盘的类型
        /// </summary>
        /// <returns></returns>
        public CloudDiskType GetDiskType()
        {
            return CloudDiskType.SINA;
        }

        public string GetLocalStoredAppKey()
        {
            var tmp_appKey = ConfigurationManager.AppSettings["SINA_APP_KEY"];
            return tmp_appKey;
        }

        public string GetLocalStoredAppSeceret()
        {
            var tmp = ConfigurationManager.AppSettings["SINA_APP_SECRET"];
            return tmp;
        }

        public string GetLocalStoredAccessToken()
        {
            var tmp = ConfigurationManager.AppSettings["SINA_ACCESS_TOKEN"];
            return tmp;
        }

        public void WriteLocalAccessToken(AccessTokenModel newToken)
        {
            ConfigurationManager.AppSettings["SINA_ACCESS_TOKEN"] = newToken.AccessToken;
        }

        public Model.CloudDisk.AccessTokenModel GetAccessToken()
        {
            AccessTokenModel m = new AccessTokenModel();
            try
            {
                WebRequestHelper helper = new WebRequestHelper("https://auth.sina.com.cn/");
                var result =
                    helper.Post("https://auth.sina.com.cn/oauth2/access_token/client_id=" + _appKey + "&client_secret=" + _appSecret
                    + "&grant_type=refresh_token&access_token=" + _accessToken);
                m.FullText = result;
                SinaResponseAccessTokenJsonEntity json = JsonHelper.DeserializeObject<SinaResponseAccessTokenJsonEntity>(result);
                m.AccessToken = json.access_token;
                m.Expire = json.expires_in;
                m.DiskType = CloudDiskType.SINA;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return m;
        }

        public Model.CloudDisk.SingleCloudDiskCapacityModel GetCloudDiskCapacityInfo()
        {
            try
            {
                string url = "https://api.weipan.cn/2/account/info";
                SingleCloudDiskCapacityModel m = new SingleCloudDiskCapacityModel();
                WebRequestHelper helper = new WebRequestHelper(url);
                var result = helper.Get(url + "?access_token=" + _accessToken);
                XmlNode node = JsonHelper.DeserializeToXmlNode(result);
                m.TotalByte = Convert.ToDouble(node.SelectSingleNode("root").SelectSingleNode("quota_info").SelectSingleNode("quota").ToString());
                var comsumed = Convert.ToDouble(node.SelectSingleNode("root").SelectSingleNode("quota_info").SelectSingleNode("consumed").ToString());
                m.TotalAvailableByte = m.TotalByte - comsumed;
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Model.CloudDisk.CloudFileInfoModel GetCloudFileInfo(string remotePath)
        {
            try
            {
                string url = "https://api.weipan.cn/2/metadata/basic/sandbox";
                WebRequestHelper helper = new WebRequestHelper(url);
                var result = helper.Get(url + "?access_token=" + _accessToken);
                CloudFileInfoModel m = new CloudFileInfoModel();
                return m;
            }
            catch (System.Net.WebException webEx) 
            {
                throw webEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Model.CloudDisk.CloudFileInfoModel UploadFile(byte[] fileContent,string filePath)
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
