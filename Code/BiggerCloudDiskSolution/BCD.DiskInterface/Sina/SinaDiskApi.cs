﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using BCD.Model.CloudDisk;
using BCD.Utility;
using System.Xml;
using System.Net;
using System.Web;
using System.IO;

namespace BCD.DiskInterface.Sina
{
    /// <summary>
    /// 新浪微盘API接口
    /// 2013-11-23 by 丁智渊
    /// </summary>
    public class SinaDiskAPI : ICloudDiskAPI
    {
        ///sina token第一次获取方法:浏览器访问 https://auth.sina.com.cn/oauth2/authorize?client_id=1551059632&redirect_uri=http://localhost/&response_type=token
        ///在返回的页面里手动复制access_token部分值
        private string _appKey = "";

        private string _appSecret = "";

        private string _accessToken = "";

        public CloudDiskType DiskType { get { return CloudDiskType.SINA; } }

        public SinaDiskAPI()
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
            //ConfigurationManager.AppSettings["SINA_ACCESS_TOKEN"] = newToken.AccessToken;
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("SINA_ACCESS_TOKEN");
            config.AppSettings.Settings.Add("SINA_ACCESS_TOKEN", newToken.AccessToken);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
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
                m.TotalByte = Convert.ToDouble(node.ChildNodes[0].SelectSingleNode("quota_info").SelectSingleNode("quota").InnerText);
                var comsumed = Convert.ToDouble(node.ChildNodes[0].SelectSingleNode("quota_info").SelectSingleNode("consumed").InnerText);
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
                string url = "https://api.weipan.cn/2/metadata/sandbox";
                WebRequestHelper helper = new WebRequestHelper(url);
                var result = helper.Get(url + remotePath + "?access_token=" + _accessToken);
                CloudFileInfoModel m = new CloudFileInfoModel();
                var fileInfo = JsonHelper.DeserializeObject<SinaResponseFileInfoJsonEntity>(result);

                m.Bytes = Convert.ToDouble(fileInfo.bytes);
                m.DiskType = CloudDiskType.SINA;
                m.Path = fileInfo.path;
                m.IsDir = Convert.ToBoolean(fileInfo.is_dir);
                m.LastModifiedDate = Convert.ToDateTime(fileInfo.modified);
                m.MD5 = fileInfo.md5;
                m.SHA1 = fileInfo.sha1;
                if (fileInfo.contents != null)
                {
                    m.Contents = new List<CloudFileInfoModel>();
                    foreach (var oneDir in fileInfo.contents)
                    {
                        CloudFileInfoModel subDir = new CloudFileInfoModel();
                        subDir.Bytes = Convert.ToDouble(oneDir.bytes);
                        subDir.Path = oneDir.path;
                        subDir.IsDir = Convert.ToBoolean(oneDir.is_dir);
                        subDir.LastModifiedDate = Convert.ToDateTime(oneDir.modified);
                        subDir.MD5 = oneDir.md5;
                        subDir.SHA1 = oneDir.sha1;

                        m.Contents.Add(subDir);
                    }
                }

                return m;
            }
            catch (System.Net.WebException webEx)
            {
                HttpWebResponse errorResponse = webEx.Response as HttpWebResponse;
                if (errorResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    //
                    return null;
                }
                //webEx.Status == System.Net.WebExceptionStatus.CacheEntryNotFound
                //throw webEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public Model.CloudDisk.CloudFileInfoModel UploadFile(byte[] fileContent, string filePath)
        {
            string url = "https://upload-vdisk.sina.com.cn/2/files/sandbox";
            var contentEncoding = "UTF-8";//"iso-8859-1";
            var request = new WebRequestHelper(url);
            string fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
            url = url + filePath + "?access_token=" + _accessToken;
            var boundary = Guid.NewGuid().ToString();

            var header = string.Format("--{0}", boundary);
            var footer = string.Format("--{0}--", boundary);

            var contents = new StringBuilder();

            //contents.AppendLine(header);
            //contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "status"));
            //contents.AppendLine("Content-Type: text/plain; charset=US-ASCII");
            //contents.AppendLine("Content-Transfer-Encoding: 8bit");
            //contents.AppendLine();
            //contents.AppendLine(HttpUtility.UrlEncode(status));


            //contents.AppendLine(header);
            //contents.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", "source"));
            //contents.AppendLine("Content-Type: text/plain; charset=US-ASCII");
            //contents.AppendLine("Content-Transfer-Encoding: 8bit");
            //contents.AppendLine();
            //contents.AppendLine(this.appKey);

            string fileHeader = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", "file", fileName);
            string fileData = System.Text.Encoding.GetEncoding(contentEncoding).GetString(fileContent);

            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);

            contents.AppendLine(header);
            contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "FileName"));
            contents.AppendLine();
            contents.AppendLine(fileName);

            contents.AppendLine(header);
            contents.AppendLine(fileHeader);
            contents.AppendLine("Content-Type: application/octet-stream;");
            //contents.AppendLine("Content-Length:" + fileContent.LongLength.ToString());
            //contents.AppendLine("Content-Transfer-Encoding: binary");
            contents.AppendLine();
            contents.AppendLine(fileData);
            contents.AppendLine(footer);
            //contents.AppendLine();

            try
            {
                var result = request.Post(url, null, null, null, contents.ToString());

                var entity = JsonHelper.DeserializeObject<SinaResponseFileInfoJsonEntity>(result);

                CloudFileInfoModel fileInfo = new CloudFileInfoModel();

                fileInfo.Bytes = Convert.ToDouble(entity.bytes);
                fileInfo.Path = entity.path;
                fileInfo.LastModifiedDate = Convert.ToDateTime(entity.modified);
                fileInfo.MD5 = entity.md5;
                fileInfo.SHA1 = entity.sha1;
                fileInfo.RootPath = entity.root;
                fileInfo.DiskType = CloudDiskType.SINA;

                return fileInfo;
            }
            catch (WebException webEx)
            {
                HttpWebResponse errorResponse = webEx.Response as HttpWebResponse;
                string ex_txt = "";
                string strContentEncoding = "utf-8";

                System.IO.StreamReader sr = new StreamReader(webEx.Response.GetResponseStream(), System.Text.Encoding.GetEncoding(strContentEncoding));

                ex_txt = sr.ReadToEnd();

                if (errorResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    //
                    //throw new Exception("上传文件时出错!可能是权限不足!" + ex_txt);
                    return null;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
            return null;
        }

        public byte[] DownloadFile(string remotePath)
        {
            string url = "https://api.weipan.cn/2/files/sandbox";
            //var request = new WebRequestHelper(url);
            string fileName = remotePath.Substring(remotePath.LastIndexOf("/") + 1);
            url = url + remotePath + "?access_token=" + _accessToken;
            //var result = request.Get(url);
            //return System.Text.Encoding.ASCII.GetBytes(result);
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
            string url = "https://api.weipan.cn/2/fileops/create_folder";
            //url += "?access_token=" + _accessToken + "&root=sandbox&path=" +  UrlEncoder.UrlEncode(dir);
            IDictionary<string, string> postParameters = new Dictionary<string, string>();
            postParameters.Add("access_token", _accessToken);
            postParameters.Add("root", "sandbox");
            postParameters.Add("path", dir);
            CloudFileInfoModel m = new CloudFileInfoModel();
            try
            {
                var request = new WebRequestHelper(url);
                var result = request.Post(url, postParameters);
                var entity = JsonHelper.DeserializeObject<SinaResponseFileInfoJsonEntity>(result);

                m.Bytes = 0;
                m.Path = UrlEncoder.UrlDecode(entity.path);
                m.RootPath = entity.root;
                m.LastModifiedDate = Convert.ToDateTime(entity.modified);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return m;
        }

        public int DeleteFile(string remotePath)
        {
            string url = "https://api.weipan.cn/2/fileops/delete";
            //url += "?root=sandbox&path=" + HttpUtility.UrlEncode(remotePath) + "&access_token=" + _accessToken;
            IDictionary<string, string> postParameters = new Dictionary<string, string>();
            postParameters.Add("access_token", _accessToken);
            postParameters.Add("root", "sandbox");
            postParameters.Add("path", remotePath);
            CloudFileInfoModel m = new CloudFileInfoModel();
            try
            {
                var request = new WebRequestHelper(url);
                var result = request.Post(url, postParameters);
                var entity = JsonHelper.DeserializeObject<SinaResponseFileInfoJsonEntity>(result);

                m.Bytes = 0;
                m.Path = entity.path;
                m.RootPath = entity.root;
                m.LastModifiedDate = Convert.ToDateTime(entity.modified);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 1;
        }

        public int DeleteDirectory(string remotePath)
        {
            return DeleteFile(remotePath);
        }

        public Model.CloudDisk.CloudFileInfoModel MoveFile(string originPath, string newPath, bool isCopy)
        {
            throw new NotImplementedException();
        }
    }
}
