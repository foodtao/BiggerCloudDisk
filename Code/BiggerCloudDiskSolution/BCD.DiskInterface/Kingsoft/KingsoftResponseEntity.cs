using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.DiskInterface.Kingsoft
{
    class KingsoftResponseEntity
    {
    }


    public class KingsoftResponseFileInfoJsonEntity
    {
        public string size { get; set; }
        public string name { get; set; }
        public string hash { get; set; }
        public string rev { get; set; }
        public string modify_time { get; set; }
        public string path { get; set; }
        public string type { get; set; }
        public string root { get; set; }
        public string is_deleted { get; set; }
        public List<KingsoftResponseFileInfoJsonEntity> files { get; set; }
    }
}
