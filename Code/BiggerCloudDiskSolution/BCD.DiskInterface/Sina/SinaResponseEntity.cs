using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.DiskInterface.Sina
{
    class SinaResponseEntity
    {
    }

    /// <summary>
    /// 对应获取token返回的json对象
    /// </summary>
    public class SinaResponseAccessTokenJsonEntity
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string time_left { get; set; }
        public string uid { get; set; }
    }

    public class SinaResponseFileInfoJsonEntity
    {
        public string size { get; set; }
        public string hash { get; set; }
        public string rev { get; set; }
        public string thumb_exists { get; set; }
        public string bytes { get; set; }
        public string modified { get; set; }
        public string path { get; set; }
        public string is_dir { get; set; }
        public string root { get; set; }
        public string icon { get; set; }
        public string revision { get; set; }
        public string is_deleted { get; set; }
        public string md5 { get; set; }
        public string sha1 { get; set; }
        public List<SinaResponseFileInfoJsonEntity> contents { get; set; }
    }
}
