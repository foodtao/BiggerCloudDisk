using BCD.Model.CloudDisk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCD.Utility;
using System.Configuration;
using System.Web;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;

namespace BCD.DiskInterface.Kingsoft
{

    public class KingsoftDiskAPI : ICloudDiskAPI
    {

        private string metadataUrl = "http://openapi.kuaipan.cn/1/metadata/app_folder{0}";     
        private string createFolderUrl = "http://openapi.kuaipan.cn/1/fileops/create_folder";
        private string deleteFolderUrl = "http://openapi.kuaipan.cn/1/fileops/delete";
        private string fileUploadLocationUrl = "http://api-content.dfs.kuaipan.cn/1/fileops/upload_locate";
        private string fileUploadUrl = "{0}1/fileops/upload_file";
        private string fileDownloadUrl = "http://api-content.dfs.kuaipan.cn/1/fileops/download_file";
        private string account_infoUrl = "http://openapi.kuaipan.cn/1/account_info";
     

        private string _http_method = "GET";
        public string http_method
        {
            get
            {
                return _http_method;
            }
            set
            {
                _http_method = value;
            }
        }


        private string _oauth_version = "1.0";
        public string oauth_version
        {
            get
            {
                return _oauth_version;
            }
            set
            {
                _oauth_version = value;
            }
        }


        private string _oauth_signature_method = "HMAC-SHA1";
        public string oauth_signature_method
        {
            get
            {
                return _oauth_signature_method;
            }
            set
            {
                _oauth_signature_method = value;
            }
        }

        public string BaseUri
        {
            get;
            set;
        }


        public string _consumerKey { set; get; }

        public string _consumerSecret { set; get; }

        public string _accessToken { set; get; }

        public string _accessTokenSecret { set; get; }


        public KingsoftDiskAPI()
        {
            _consumerKey = GetLocalStoredAppKey();
            _consumerSecret = GetLocalStoredAppSeceret();
            _accessToken = GetLocalStoredAccessToken();
            _accessTokenSecret = GetLocalStoredAccessTokenSecret();
        }

        /// <summary>
        /// 获取本网盘的类型
        /// </summary>
        /// <returns></returns>
        public CloudDiskType GetDiskType()
        {
            return CloudDiskType.KINGSOFT;
        }

        public string GetLocalStoredAppKey()
        {
            return ConfigurationManager.AppSettings["KINGSOFT_APP_KEY"];
        }

        public string GetLocalStoredAppSeceret()
        {
            return ConfigurationManager.AppSettings["KINGSOFT_APP_SECRET"];
        }

        public string GetLocalStoredAccessToken()
        {
            return ConfigurationManager.AppSettings["KINGSOFT_ACCESS_TOKEN"];
        }

        public string GetLocalStoredAccessTokenSecret()
        {
            return ConfigurationManager.AppSettings["KINGSOFT_ACCESS_TOKEN_SECRET"];
        }

        public void WriteLocalAccessToken(AccessTokenModel newToken)
        {

            ConfigurationManager.AppSettings["KINGSOFT_ACCESS_TOKEN"] = newToken.AccessToken;
        }

        public AccessTokenModel GetAccessToken()
        {
            AccessTokenModel accessToken = new AccessTokenModel();
            accessToken.AccessToken = _accessToken;
            return accessToken;
        }

        public SingleCloudDiskCapacityModel GetCloudDiskCapacityInfo()
        {
            SortedDictionary<string, string> ParamList = getParamList();
            string SourceString =GetApiSourceString(this.account_infoUrl,ParamList);
            string SecretKey = GetSecretKey();
            string Sign = GetSignature(SourceString, SecretKey);
            ParamList.Add("oauth_signature", Sign);
            string URL = this.account_infoUrl + "?" + ParamToUrl(ParamList, false);
            object jsonAccess = GetGeneralContent(URL);
            XmlNode node = JsonHelper.DeserializeToXmlNode(jsonAccess.ToString());
            SingleCloudDiskCapacityModel fileInfo = new SingleCloudDiskCapacityModel();
            fileInfo.TotalByte = Convert.ToDouble(node.ChildNodes[0].SelectSingleNode("quota_total").InnerText);
            var used=Convert.ToDouble(node.ChildNodes[0].SelectSingleNode("quota_used").InnerText);
            fileInfo.TotalAvailableByte = fileInfo.TotalByte - used;
            return fileInfo;
        }

        public CloudFileInfoModel GetCloudFileInfo(string remotePath)
        {
            String metaUrl=String.Format(this.metadataUrl,remotePath);
            SortedDictionary<string, string> ParamList = getParamList();
            ParamList.Add("list", "false");
            string SourceString = GetApiSourceString(metaUrl, ParamList);
            string SecretKey = GetSecretKey();
            string Sign = GetSignature(SourceString, SecretKey);
            ParamList.Add("oauth_signature", Sign);
            string URL = metaUrl + "?" + ParamToUrl(ParamList, false);
            object jsonAccess=GetGeneralContent(URL);
            if (jsonAccess != "")
            {
                XmlNode node = JsonHelper.DeserializeToXmlNode(jsonAccess.ToString());
                CloudFileInfoModel fileInfo = new CloudFileInfoModel();
                fileInfo.ID = Convert.ToString(node.ChildNodes[0].SelectSingleNode("file_id").InnerText);
                fileInfo.name = Convert.ToString(node.ChildNodes[0].SelectSingleNode("name").InnerText);
                var type = Convert.ToString(node.ChildNodes[0].SelectSingleNode("type").InnerText);
                if (type == "folder")
                {
                    fileInfo.IsDir = true;
                }
                else
                {
                    fileInfo.IsDir = false;
                }
                return fileInfo;
            }
            else
            {
                return null;
            }
           
        }

        public CloudFileInfoModel UploadFile(byte[] fileContent, string filePath)
        {
            NameValueCollection stringDict = new NameValueCollection();
            stringDict.Add("file", "fileKeyValue");
            object jsonAccess = GetGeneralContent(this.fileUploadLocationUrl);
            XmlNode node = JsonHelper.DeserializeToXmlNode(jsonAccess.ToString());
            String updateUrl = Convert.ToString(node.ChildNodes[0].SelectSingleNode("url").InnerText);
            String status = Convert.ToString(node.ChildNodes[0].SelectSingleNode("stat").InnerText);
            if (status.Equals("OK"))
            {
                String Url = string.Format(this.fileUploadUrl, updateUrl);
                SortedDictionary<string, string> ParamList = getParamList();
                ParamList.Add("root", "app_folder");
                ParamList.Add("path", filePath);
                ParamList.Add("overwrite", "true");
                StringBuilder sb = new StringBuilder();
                sb.Append("POST&");
                sb.Append(UrlEncode(Url) + "&");
                string InnerParam = ParamToUrl(ParamList, true);

                sb.Append(UrlEncode(InnerParam));
                string SourceString = sb.ToString();

                string SecretKey = GetSecretKey();
                string Sign = GetSignature(SourceString, SecretKey);
                ParamList.Add("oauth_signature", Sign);
                String finalUrl = Url + "?" + ParamToUrl(ParamList, false);

                string responseContent;
                var memStream = new MemoryStream();
                var webRequest = (HttpWebRequest)WebRequest.Create(finalUrl);
                // 边界符
                var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                // 边界符
                var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
                // 最后的结束符
                var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

                // 设置属性
                webRequest.Method = "POST";
                webRequest.Timeout = 10000;
                webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

                // 写入文件
                const string filePartHeader =
                    "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                     "Content-Type: application/octet-stream\r\n\r\n";
                var header = string.Format(filePartHeader, "file", filePath);
                var headerbytes = Encoding.UTF8.GetBytes(header);

                memStream.Write(beginBoundary, 0, beginBoundary.Length);
                memStream.Write(headerbytes, 0, headerbytes.Length);


                memStream.Write(fileContent, 0, fileContent.Length);


                // 写入字符串的Key
                var stringKeyHeader = "\r\n--" + boundary +
                                "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                "\r\n\r\n{1}\r\n";

                foreach (byte[] formitembytes in from string key in stringDict.Keys
                                                 select string.Format(stringKeyHeader, key, stringDict[key])
                                                     into formitem
                                                     select Encoding.UTF8.GetBytes(formitem))
                {
                    memStream.Write(formitembytes, 0, formitembytes.Length);
                }




                // 写入最后的结束边界符
                memStream.Write(endBoundary, 0, endBoundary.Length);

                webRequest.ContentLength = memStream.Length;

                var requestStream = webRequest.GetRequestStream();

                memStream.Position = 0;
                var tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();

                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();

                var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),
                                                                Encoding.GetEncoding("utf-8")))
                {
                    responseContent = httpStreamReader.ReadToEnd();
                }

                httpWebResponse.Close();
                webRequest.Abort();
                 node = JsonHelper.DeserializeToXmlNode(responseContent.ToString());          
                CloudFileInfoModel fileInfo = new CloudFileInfoModel();
                fileInfo.Size = Convert.ToString(node.ChildNodes[0].SelectSingleNode("size").InnerText);
                fileInfo.Path = filePath;
                return fileInfo;
            }
            return null;
        }

        public byte[] DownloadFile(string remotePath)
        {
            SortedDictionary<string, string> ParamList = getParamList();
            ParamList.Add("root", "app_folder");
            ParamList.Add("path", remotePath);
            string SourceString = GetApiSourceString(this.fileDownloadUrl, ParamList);
            string SecretKey = GetSecretKey();
            string Sign = GetSignature(SourceString, SecretKey);
            ParamList.Add("oauth_signature", Sign);
            string url = this.fileDownloadUrl + "?" + ParamToUrl(ParamList, false);
            byte[] returnByte = null;
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
             
                MemoryStream stream = (MemoryStream)response.GetResponseStream();
               returnByte =stream.ToArray();
              

                response.Close();
            }
            catch
            { }
            return returnByte;
        }

        public CloudFileInfoModel CreateDirectory(string dir)
        {
            SortedDictionary<string, string> ParamList = getParamList();
            ParamList.Add("root", "app_folder");
            ParamList.Add("path", dir);
            string SourceString = GetApiSourceString(this.createFolderUrl, ParamList);
            string SecretKey = GetSecretKey();
            string Sign = GetSignature(SourceString, SecretKey);
            ParamList.Add("oauth_signature", Sign);
            string url = this.createFolderUrl + "?" + ParamToUrl(ParamList, false);
            object jsonAccess = GetGeneralContent(url);
            object json = JsonHelper.DeserializeObject(jsonAccess.ToString());
            Dictionary<string, object> dict = (Dictionary<string, object>)json;
            CloudFileInfoModel fileInfo = new CloudFileInfoModel();
            fileInfo.Path = dict["path"].ToString();
            return fileInfo;
        }

        public int DeleteFile(string remotePath)
        {
            SortedDictionary<string, string> ParamList = getParamList();
            ParamList.Add("root", "app_folder");
            ParamList.Add("path", remotePath);
            string SourceString = GetApiSourceString(this.deleteFolderUrl, ParamList);
            string SecretKey = GetSecretKey();
            string Sign = GetSignature(SourceString, SecretKey);
            ParamList.Add("oauth_signature", Sign);
            string url = this.deleteFolderUrl + "?" + ParamToUrl(ParamList, false);
            GetGeneralContent(url);
            return 1;
        }

        public int DeleteDirectory(string remotePath)
        {
            SortedDictionary<string, string> ParamList = getParamList();
            ParamList.Add("root", "app_folder");
            ParamList.Add("path", remotePath);
            string SourceString = GetApiSourceString(this.deleteFolderUrl, ParamList);
            string SecretKey = GetSecretKey();
            string Sign = GetSignature(SourceString, SecretKey);
            ParamList.Add("oauth_signature", Sign);
            string url = this.deleteFolderUrl + "?" + ParamToUrl(ParamList, false);
            GetGeneralContent(url);
            return 1;
        }

        public Model.CloudDisk.CloudFileInfoModel MoveFile(string originPath, string newPath, bool isCopy)
        {
            throw new NotImplementedException();
        }

        #region 辅助函数
        /// <summary>
        /// 根据API地址和授权类型生成URL
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="oauth_token"></param>
        /// <param name="oauth_token_secret"></param>
        /// <returns></returns>
        private string BuildUrl(string Url, string oauth_token, string oauth_token_secret)
        {
            SortedDictionary<string, string> ParamList = new SortedDictionary<string, string>();
            ParamList.Add("oauth_consumer_key", this._consumerKey);
            ParamList.Add("oauth_nonce", GetRandomString(8));
            ParamList.Add("oauth_timestamp", GetTimeStamp());
            ParamList.Add("oauth_version", this.oauth_version);
            ParamList.Add("oauth_signature_method", this.oauth_signature_method);
            ParamList.Add("oauth_token", oauth_token);
            string SourceString = GetApiSourceString(Url, ParamList);
            string SecretKey = GetSecretKey();
            string Sign = GetSignature(SourceString, SecretKey);
            ParamList.Add("oauth_signature", Sign);
            string url = Url + "?" + ParamToUrl(ParamList, false);
            return url;
        }
        /// <summary>
        /// 获取源串.
        /// </summary>
        /// <returns></returns>
        private string GetApiSourceString(string baseUrl, SortedDictionary<string, string> ParamList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(http_method + "&");
            sb.Append(UrlEncode(baseUrl) + "&");
            string InnerParam = ParamToUrl(ParamList, true);

            sb.Append(UrlEncode(InnerParam));
            return sb.ToString();
        }

        /// <summary>
        /// 获取签名值.
        /// </summary>
        /// <param name="SourceString"></param>
        /// <param name="SecretKey"></param>
        /// <returns></returns>
        private string GetSignature(string SourceString, string SecretKey)
        {
            return UrlEncode(Hmac_Sha1AndBase64(SourceString, SecretKey));
        }
        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <param name="consumer_secret"></param>
        /// <param name="oauth_token_secret"></param>
        /// <returns></returns>
        private string GetSecretKey()
        {
            return this._consumerSecret + "&" + this._accessTokenSecret;
        }

        /// <summary>
        /// 将参数字典转换为URL 参数.用&符号拼接
        /// </summary>
        /// <param name="dictParam"></param>
        /// <param name="isEncode">是否编码</param>
        /// <returns></returns>
        private string ParamToUrl(SortedDictionary<string, string> dictParam, bool isEncode)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> key in dictParam)
            {
                if (isEncode)
                {
                    sb.AppendFormat("&{0}={1}", UrlEncode(key.Key), UrlEncode(key.Value));
                }
                else
                    sb.AppendFormat("&{0}={1}", key.Key, key.Value);
            }
            return sb.ToString().TrimStart('&');
        }

        private string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                if ((byStr[i] >= 48 && byStr[i] <= 57)
                    || (byStr[i] >= 97 && byStr[i] <= 122)
                    || (byStr[i] >= 65 && byStr[i] <= 90)
                    || (byStr[i] == 46)
                || byStr[i] == 45
                    || byStr[i] == 95
                    || byStr[i] == 126
                   )
                {
                    sb.Append((char)byStr[i]);
                }
                else
                {
                    string t = Convert.ToString(byStr[i], 16);
                    sb.AppendFormat(@"%{0}", t.ToUpper());
                }
            }
            return sb.ToString();
        }
        private string Hmac_Sha1AndBase64(string Source, string SecretKey)
        {
            System.Security.Cryptography.HMACSHA1 hmacsha1 = new System.Security.Cryptography.HMACSHA1();
            hmacsha1.Key = System.Text.Encoding.ASCII.GetBytes(SecretKey);
            byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(Source); //signatureBase要进行签名的基础字符串
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private string GetTimeStamp()
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
            return timeStamp;
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string GetRandomString(int length)
        {
            string result = System.Guid.NewGuid().ToString();
            result = result.Replace("-", "");
            return result.Substring(0, length);
        }

        private string GetGeneralContent(string strUrl)
        {
            string strMsg = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(strUrl);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));

                strMsg = reader.ReadToEnd();

                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch
            { }
            return strMsg;
        }
        private SortedDictionary<string, string> getParamList()
        {
            SortedDictionary<string, string> ParamList = new SortedDictionary<string, string>();
            ParamList.Add("oauth_consumer_key", this._consumerKey);
            ParamList.Add("oauth_nonce", GetRandomString(8));
            ParamList.Add("oauth_timestamp", GetTimeStamp());
            ParamList.Add("oauth_version", this.oauth_version);
            ParamList.Add("oauth_signature_method", this.oauth_signature_method);
            ParamList.Add("oauth_token", this._accessToken);
            return ParamList;
        }
        #endregion
    }
}
