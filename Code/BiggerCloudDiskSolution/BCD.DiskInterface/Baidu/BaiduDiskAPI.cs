using BCD.Model.CloudDisk;
using BCD.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace BCD.DiskInterface.Baidu
{
    public class BaiduDiskAPI : ICloudDiskAPI
    {
        private string _appKey = "";

        private string _appSecret = "";

        private string _accessToken = "";

        private string _bucketName = "";

        public BaiduDiskAPI()
        {
            _appKey = GetLocalStoredAppKey();
            _appSecret = GetLocalStoredAppSeceret();
            _accessToken = GetLocalStoredAccessToken();
            _bucketName = GetBucketName();
        }

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
            var tmp_appKey = ConfigurationManager.AppSettings["BAIDU_APP_KEY"];
            return tmp_appKey;
        }

        public string GetLocalStoredAppSeceret()
        {
            var tmp = ConfigurationManager.AppSettings["BAIDU_APP_SECRET"];
            return tmp;
        }

        public string GetLocalStoredAccessToken()
        {
            var tmp = ConfigurationManager.AppSettings["BAIDU_ACCESS_TOKEN"];
            return tmp;
        }

        public string GetBucketName()
        {
            var tmp = ConfigurationManager.AppSettings["BAIDU_BUCKET_NAME"];
            return tmp;
        }

        public void WriteLocalAccessToken(AccessTokenModel newToken)
        {
            //ConfigurationManager.AppSettings.Set("BAIDU_ACCESS_TOKEN", newToken.AccessToken);
            //ConfigurationManager.AppSettings["BAIDU_ACCESS_TOKEN"] = newToken.AccessToken;
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("BAIDU_ACCESS_TOKEN");
            config.AppSettings.Settings.Add("BAIDU_ACCESS_TOKEN", newToken.AccessToken);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public Model.CloudDisk.AccessTokenModel GetAccessToken()
        {
            AccessTokenModel result = new AccessTokenModel();
            result.AccessToken = GetLocalStoredAccessToken();
            return result;
        }

        public SingleCloudDiskCapacityModel GetCloudDiskCapacityInfo()
        {
            /*
            string url = "https://pcs.baidu.com/rest/2.0/pcs/quota";
            string para = "?method=info&access_token=" + _accessToken;
            SingleCloudDiskCapacityModel m = new SingleCloudDiskCapacityModel();
            WebRequestHelper helper = new WebRequestHelper(url);
            var result = helper.Get(url + para);
            XmlNode node = JsonHelper.DeserializeToXmlNode(result);
            m.TotalByte = Convert.ToDouble(node.ChildNodes[0].SelectSingleNode("quota").InnerText);
            var comsumed = Convert.ToDouble(node.ChildNodes[0].SelectSingleNode("used").InnerText);
            m.TotalAvailableByte = m.TotalByte - comsumed;
            return m;*/
            string url = "http://bcs.duapp.com/?";
            string flag = "MBO";
            string para = "sign=" + flag + ":" + _appKey + ":";
            string content = flag + "\n"
                    + "Method=GET" + "\n"
                    + "Bucket=" + "\n"
                    + "Object=/" + "\n";
            string sign = BaiduSignatureHelper.GetSignature(content, _appSecret);
            para = para + sign;
            WebRequestHelper helper = new WebRequestHelper(url);
            var result = helper.Get(url + para);
            List<BaiduBucketInfo> info = JsonHelper.DeserializeObject<List<BaiduBucketInfo>>(result);
            SingleCloudDiskCapacityModel m = new SingleCloudDiskCapacityModel();
            if (info != null)
            {
                foreach (BaiduBucketInfo one in info)
                {
                    if (one.bucket_name == _bucketName)
                    {
                        m.TotalByte = Convert.ToDouble(info[0].total_capacity);
                        var comsumed = Convert.ToDouble(info[0].used_capacity);
                        m.TotalAvailableByte = m.TotalByte - comsumed;
                    }
                }
            }

            return m;
        }

        public Model.CloudDisk.CloudFileInfoModel GetCloudFileInfo(string remotePath)
        {
            string url = "http://bcs.duapp.com/" + _bucketName;
            //if (remotePath == "/" || string.IsNullOrEmpty(remotePath)) 
            //{

            //}
            //else 
            //{
            //    url += remotePath;
            //}
            string flag = "MBO";
            string para = "?sign=" + flag + ":" + _appKey + ":";
            string content = flag + "\n"
                    + "Method=GET" + "\n"
                    + "Bucket=" + _bucketName + "\n"
                    + "Object=/" + "\n";
            //+ "Object=" + remotePath + "\n";
            string sign = BaiduSignatureHelper.GetSignature(content, _appSecret);
            para = para + sign;
            WebRequestHelper helper = new WebRequestHelper(url);
            url = url + para;
            var result = helper.Get(url);
            BaiduFileInfo info = JsonHelper.DeserializeObject<BaiduFileInfo>(result);

            CloudFileInfoModel m = null;
            if (info != null)
            {
                foreach (BaiduObjectList obj in info.object_list)
                {
                    if (obj.Object == remotePath)
                    {
                        m = new CloudFileInfoModel();
                        m.Bytes = Convert.ToDouble(obj.size);
                        m.IsDir = obj.is_dir == "1";
                        m.Path = obj.Object;
                        m.MD5 = obj.content_md5;

                        break;
                    }

                }
            }
            return m;
        }

        public Model.CloudDisk.CloudFileInfoModel UploadFile(byte[] fileContent, string filePath)
        {
            string url = "http://bcs.duapp.com/" + _bucketName + filePath;
            string flag = "MBO";
            string para = "?sign=" + flag + ":" + _appKey + ":";
            string content = flag + "\n"
                    + "Method=PUT" + "\n"
                    + "Bucket=" + _bucketName + "\n"
                    + "Object=" + filePath + "\n";
            //+ "Object=" + remotePath + "\n";
            string sign = BaiduSignatureHelper.GetSignature(content, _appSecret);
            para = para + sign;
            url = url + para; 
            CloudFileInfoModel result = new CloudFileInfoModel();
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "PUT";
                request.ContentLength = fileContent.Length;
                //request.GetRequestStream
                Stream newStream = request.GetRequestStream();
                newStream.Write(fileContent, 0, fileContent.Length);

                // Close the Stream object.
                newStream.Close();

                Stream stream=request.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string responseString = reader.ReadToEnd();

                result.Path = filePath;
            }
            catch (Exception ex) 
            {
                //throw ex;
                result = null;
            }
            return result;
        }

        public byte[] DownloadFile(string remotePath)
        {
            string url = "http://bcs.duapp.com/" + _bucketName;
            if (remotePath == "/" || string.IsNullOrEmpty(remotePath))
            {

            }
            else
            {
                url += remotePath;
            }
            string flag = "MBO";
            string para = "?sign=" + flag + ":" + _appKey + ":";
            string content = flag + "\n"
                    + "Method=GET" + "\n"
                    + "Bucket=" + _bucketName + "\n"
                    + "Object=" + remotePath + "\n";
            string sign = BaiduSignatureHelper.GetSignature(content, _appSecret);
            para = para + sign;
            //WebRequestHelper helper = new WebRequestHelper(url);
            url = url + para;
            //var result = helper.Get(url);
            byte[] returnByte;
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();

                Stream stream = response.GetResponseStream();
                /*
                StreamHelper helper = new StreamHelper();
                Stream newStream = new Stream();
                helper.CopyStream(stream, newStream);
                returnByte = helper.StreamToBytes(newStream);
                */
                long length = response.ContentLength;
                returnByte = new byte[length];
                stream.Read(returnByte, 0, (int)length);
                /*
                MemoryStream stream = (MemoryStream)response.GetResponseStream();
                returnByte = stream.ToArray();
                */
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnByte;
        }

        public Model.CloudDisk.CloudFileInfoModel CreateDirectory(string dir)
        {
            CloudFileInfoModel m = new CloudFileInfoModel();
            m.Path = dir;
            return m;
        }

        public int DeleteFile(string remotePath)
        {
            string url = "http://bcs.duapp.com/" + _bucketName;
            if (remotePath == "/" || string.IsNullOrEmpty(remotePath))
            {

            }
            else
            {
                url += remotePath;
            }
            string flag = "MBO";
            string para = "?sign=" + flag + ":" + _appKey + ":";
            string content = flag + "\n"
                    + "Method=DELETE" + "\n"
                    + "Bucket=" + _bucketName + "\n"
                    + "Object=" + remotePath + "\n";
            string sign = BaiduSignatureHelper.GetSignature(content, _appSecret);
            para = para + sign;
            //WebRequestHelper helper = new WebRequestHelper(url);
            url = url + para;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "DELETE";
                //request.GetRequestStream

                Stream stream = request.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string responseString = reader.ReadToEnd();
                
            }
            catch (Exception ex)
            {
                //throw ex;
                return 0;
            }
            return 1;
        }

        public int DeleteDirectory(string remotePath)
        {
            return 1;
        }

        public Model.CloudDisk.CloudFileInfoModel MoveFile(string originPath, string newPath, bool isCopy)
        {
            throw new NotImplementedException();
        }
    }
}
