using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BCD.Utility
{
    public class WebRequestHelper
    {
        private string _userAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN; rv:1.9.0.7) Gecko/2009021910 Firefox/3.0.7";

        public string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        private string _accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

        public string Accept
        {
            get { return _accept; }
            set { _accept = value; }
        }

        private string _acceptCharset = "utf-8;q=0.7";

        public string AcceptCharset
        {
            get { return _acceptCharset; }
            set { _acceptCharset = value; }
        }

        private string _contentType = "application/x-www-form-urlencoded"; //application/atom+xml
        public string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }

        public string ContentEncoding { get; set; }

        private string _siteUrl;

        public string SiteUrl
        {
            get
            {
                return _siteUrl;
            }

            set
            {
                if (value.EndsWith("/"))
                {
                    _siteUrl = value;
                }
                else
                {
                    _siteUrl = value + "/";
                }
            }
        }

        private WebProxy _proxy;

        public WebProxy Proxy
        {
            get { return _proxy; }
            set { _proxy = value; }
        }

        private Uri _websiteUri;

        public Uri WebSiteUri
        {
            get
            {
                if (this._websiteUri == null)
                {
                    this._websiteUri = new Uri(this.SiteUrl);
                }

                return this._websiteUri;
            }
        }

        private CookieContainer _cookieContainer = new CookieContainer();

        public CookieContainer CookieContainer
        {
            get { return _cookieContainer; }
            set { _cookieContainer = value; }
        }

        private CredentialCache _credentialCache = new CredentialCache();

        public CredentialCache CredentialCache
        {
            get { return _credentialCache; }
            set { _credentialCache = value; }
        }

        public WebRequestHelper(string siteUrl)
        {
            this.SiteUrl = siteUrl;
        }

        public WebRequestHelper(string siteUrl, string cookies)
        {
            this.SiteUrl = siteUrl;
            this.SetCookies(cookies);
        }

        public bool PreConnect(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(this.ResolveRequestUrl(url));
            request.Timeout = 60000;
            request.UserAgent = this.UserAgent;
            request.Accept = this.Accept;
            request.Headers["Accept-Charset"] = this.AcceptCharset;
            request.Method = "GET";

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    if (this.SiteUrl != "/")
                    {
                        _cookieContainer.SetCookies(this.WebSiteUri, response.GetResponseHeader("Set-Cookie"));
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }

        /// <summary>
        /// »ñÈ¡ÇëÇó£¬ÓÃÄ¬ÈÏurl
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            return Get(this.SiteUrl);
        }

        public string Get(string url)
        {
            /*
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(this.ResolveRequestUrl(url));
            request.Timeout = 60000;
            request.CookieContainer = _cookieContainer;
            request.UserAgent = this.UserAgent;
            request.Accept = this.Accept;
            request.Headers["Accept-Charset"] = this.AcceptCharset;
            request.Referer = this.SiteUrl;
            request.Method = "GET";
            request.Credentials = this._credentialCache;

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    if (this.SiteUrl != "/")
                    {
                        _cookieContainer.SetCookies(this.WebSiteUri, response.GetResponseHeader("Set-Cookie"));
                    }

                    return this.GetResponseText(response, true);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            */
            return this.Get(url, null, null);

        }

        public string Get(string url, IDictionary<string, string> urlParameters)
        {
            return this.Get(this.ResolveUrlParameters(url, urlParameters));
        }

        public string Get(string url, IDictionary<string, string> urlParameters, IDictionary<string, string> headers)
        {
            url = this.ResolveRequestUrl(this.ResolveUrlParameters(url, urlParameters));


            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                if (this._proxy != null)
                {
                    request.Proxy = this._proxy;
                }

                request.Timeout = 60000;
                request.CookieContainer = _cookieContainer;
                request.UserAgent = this.UserAgent;
                request.Accept = this.Accept;
                request.Headers["Accept-Charset"] = this.AcceptCharset;
                request.Referer = this.SiteUrl;
                request.Method = "GET";
                request.ContentType = this.ContentType;//"application/x-www-form-urlencoded";
                //request.Credentials = this._credentialCache;

                if (headers != null)
                {
                    foreach (string eachKey in headers.Keys)
                    {
                        if (eachKey == "Content-Type")
                        {
                            request.ContentType = headers[eachKey];
                        }
                        else
                        {
                            request.Headers[eachKey] = headers[eachKey];
                            //_log.Error("header:" + headers[eachKey].ToString());
                        }
                    }
                }


                HttpWebResponse response = null;

                response = (HttpWebResponse)request.GetResponse();



                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    if (this.SiteUrl != "/")
                    {
                        _cookieContainer.SetCookies(this.WebSiteUri, response.GetResponseHeader("Set-Cookie"));
                    }
                    return this.GetResponseText(response, true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return null;
        }

        //public T GetXml<T>(string url)
        //{
        //    string strResult = this.Get(url);

        //    T result = XmlSerializeHelper.XmlDeserialize<T>(strResult);
        //    return result;
        //}

        //public T GetXml<T>(string url, IDictionary<string, string> urlParameters)
        //{
        //    string strResult = this.Get(url, urlParameters);

        //    T result = XmlSerializeHelper.XmlDeserialize<T>(strResult);
        //    return result;
        //}

        public string Post(string url)
        {
            return this.Post(url, null, null);
        }

        public string Post(string url, IDictionary<string, string> postParameters)
        {
            return this.Post(url, postParameters, null);
        }

        public string Post(string url, IDictionary<string, string> postParameters, IDictionary<string, string> urlParameters)
        {
            return this.Post(url, postParameters, null, null, null);
        }

        public string Post(string url, IDictionary<string, string> postParameters, IDictionary<string, string> urlParameters, IDictionary<string, string> headers, string body)
        {
            #region nouse
            /*
            url = this.ResolveRequestUrl(this.ResolveUrlParameters(url, urlParameters));

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Timeout = 60000;
                request.CookieContainer = _cookieContainer;
                request.UserAgent = this.UserAgent;
                request.Accept = this.Accept;
                request.Headers["Accept-Charset"] = this.AcceptCharset;
                request.Referer = this.SiteUrl;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Credentials = this._credentialCache;

                if (headers != null)
                {
                    foreach (string eachKey in headers.Keys)
                    {
                        if (eachKey == "Content-Type")
                        {
                            request.ContentType = headers[eachKey];
                        }
                        else
                        {
                            request.Headers[eachKey] = headers[eachKey];
                        }
                    }
                }

                HttpWebResponse response = null;

                if (postParameters != null)
                {
                    StringBuilder sb = new StringBuilder();
                    if (postParameters != null)
                    {
                        foreach (string eachKey in postParameters.Keys)
                        {
                            string eachValue = postParameters[eachKey];
                            if (sb.Length != 0)
                            {
                                sb.Append("&");
                            }
                            sb.Append(eachKey + "=" + System.Web.HttpUtility.UrlEncode(eachValue));
                        }
                    }

                    ASCIIEncoding encoding = new ASCIIEncoding();
                    byte[] data = encoding.GetBytes(body != null ? body : sb.ToString());
                    request.ContentLength = data.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Close();
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    if (this.SiteUrl != "/")
                    {
                        _cookieContainer.SetCookies(this.WebSiteUri, response.GetResponseHeader("Set-Cookie"));
                    }
                    return this.GetResponseText(response, true);
                }
            }
            catch (Exception ex)
            {
            }
            */
            #endregion

            HttpWebResponse response = PostGetResponse(url, postParameters, urlParameters, headers, body);
            if (response != null)
            {
                return this.GetResponseText(response, true);
            }
            return null;
        }

        public HttpWebResponse PostGetResponse(string url)
        {
            return PostGetResponse(url, null);
        }

        public HttpWebResponse PostGetResponse(string url, IDictionary<string, string> postParameters)
        {
            return this.PostGetResponse(url, postParameters, null);
        }

        public HttpWebResponse PostGetResponse(string url, IDictionary<string, string> postParameters, IDictionary<string, string> urlParameters)
        {
            return this.PostGetResponse(url, postParameters, urlParameters, null, null);
        }

        public HttpWebResponse PostGetResponse(string url, IDictionary<string, string> postParameters, IDictionary<string, string> urlParameters, IDictionary<string, string> headers, string body)
        {
            url = this.ResolveRequestUrl(this.ResolveUrlParameters(url, urlParameters));

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                if (this._proxy != null)
                {
                    request.Proxy = this._proxy;
                }
                request.Timeout = 60000;
                request.CookieContainer = _cookieContainer;
                request.UserAgent = this.UserAgent;
                request.Accept = this.Accept;
                request.Headers["Accept-Charset"] = this.AcceptCharset;
                request.Referer = this.SiteUrl;
                request.Method = "POST";
                request.ContentType = this.ContentType;//"application/x-www-form-urlencoded";
                //request.Credentials = this._credentialCache;

                if (headers != null)
                {
                    foreach (string eachKey in headers.Keys)
                    {
                        if (eachKey == "Content-Type")
                        {
                            request.ContentType = headers[eachKey];
                        }
                        else
                        {
                            request.Headers[eachKey] = headers[eachKey];
                        }
                    }
                }

                HttpWebResponse response = null;

                StringBuilder sb = new StringBuilder();
                byte[] data;
                if (postParameters != null)
                {

                    if (postParameters != null)
                    {
                        foreach (string eachKey in postParameters.Keys)
                        {
                            string eachValue = postParameters[eachKey];
                            if (sb.Length != 0)
                            {
                                sb.Append("&");
                            }
                            sb.Append(eachKey + "=" + System.Web.HttpUtility.UrlEncode(eachValue));
                        }
                    }
                }
                if (!string.IsNullOrEmpty(body) || !string.IsNullOrEmpty(sb.ToString()))
                {
                    if (string.IsNullOrEmpty(ContentEncoding))
                    {
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        data = encoding.GetBytes(body != null ? body : sb.ToString());
                    }
                    else
                    {
                        data = Encoding.GetEncoding(ContentEncoding).GetBytes(body != null ? body : sb.ToString());
                    }


                    request.ContentLength = data.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Close();
                }





                response = (HttpWebResponse)request.GetResponse();

                if (response != null)//&& response.StatusCode == HttpStatusCode.OK)
                {
                    if (this.SiteUrl != "/")
                    {
                        _cookieContainer.SetCookies(this.WebSiteUri, response.GetResponseHeader("Set-Cookie"));
                    }
                    //return this.GetResponseText(response, true);
                    return response;
                }
            }
            catch (WebException wex)
            {
                string ex_txt = "";
                string strContentEncoding = "utf-8";

                System.IO.StreamReader sr = new StreamReader(wex.Response.GetResponseStream(), System.Text.Encoding.GetEncoding(strContentEncoding));

                ex_txt = sr.ReadToEnd();
                throw wex;
                //_log.Error("Post:'" + url + "' Òì³£:" + wex.Message + "." + ex_txt); //+ "|InnerEx:" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                //_log.Error("Post:'" + url + "' Òì³£:" + ex.Message);
                throw ex;
            }

            return null;
        }

        public string ResolveRequestUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return this.SiteUrl;
            }

            if (url.StartsWith("~/"))
            {
                return url.Replace("~/", this.SiteUrl);
            }

            return url;
        }

        public string ResolveUrlParameters(string url, IDictionary<string, string> urlParameters)
        {
            if (urlParameters != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string eachKey in urlParameters.Keys)
                {
                    string eachValue = urlParameters[eachKey];
                    if (sb.Length != 0)
                    {
                        sb.Append("&");
                    }
                    sb.Append(eachKey + "=" + System.Web.HttpUtility.UrlEncode(eachValue));
                }

                string str1;
                if (url.Contains("?"))
                {
                    str1 = "&";
                }
                else
                {
                    str1 = "?";
                }
                url = url + str1 + sb.ToString();
            }
            return url;
        }

        public string GetResponseText(HttpWebResponse response, bool autoClose)
        {
            string strText = "";
            try
            {
                string strContentEncoding = response.ContentEncoding;
                if (string.IsNullOrEmpty(strContentEncoding))
                {
                    strContentEncoding = "utf-8";
                }
                System.IO.StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(strContentEncoding));

                strText = sr.ReadToEnd();

                if (strText == "")
                {
                    //_log.Error("GetResponseText·µ»ØÖµÎª¿Õ.StatusCode:" + response.StatusCode + ".StatusDescription:" + response.StatusDescription);
                }

                sr.Close();
                if (autoClose)
                {
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                strText = "";
                //_log.Error("GetResponseTextÒì³£:" + ex.Message);
                throw ex;
            }
            return strText;
        }

        /// <summary>
        /// »ñÈ¡CookieÍ·ÐÅÏ¢
        /// </summary>
        /// <returns></returns>
        public string GetCookieHeader()
        {
            return this.CookieContainer.GetCookieHeader(this.WebSiteUri);
        }

        /// <summary>
        /// ÉèÖÃCookieÖµ
        /// </summary>
        /// <param name="cookies"></param>
        public void SetCookies(string cookies)
        {
            if (cookies != null)
            {
                cookies = cookies.Trim();
            }

            if (string.IsNullOrEmpty(cookies))
            {
                return;
            }

            string[] nameValueList = cookies.Split(';');

            CookieCollection cc = new CookieCollection();
            foreach (string eachNameValue in nameValueList)
            {
                string[] nameAndValue = eachNameValue.Trim().Split('=');
                if (nameAndValue.Length == 1)
                {
                    cc.Add(new Cookie(nameAndValue[0], ""));
                }
                else
                {
                    cc.Add(new Cookie(nameAndValue[0], nameAndValue[1]));
                }
            }

            this.CookieContainer.Add(this.WebSiteUri, cc);
        }
    }
}
