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
}
