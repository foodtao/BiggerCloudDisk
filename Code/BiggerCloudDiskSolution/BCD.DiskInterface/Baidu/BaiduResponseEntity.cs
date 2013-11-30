using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.DiskInterface.Baidu
{
    class BaiduResponseEntity
    {
    }

    public class BaiduBucketInfo
    {
        public string bucket_name;
        public string status;
        public string cdatetime;
        public string used_capacity;
        public string total_capacity;
        public string region;
    }

    public class BaiduFileInfo
    {
        public string object_total;
        public string bucket;
        public string start;
        public string limit;
        public List<BaiduObjectList> object_list;
    }

    public class BaiduObjectList
    {
        public string version_key;
        public string Object;
        public string superfile;
        public string size;
        public string parent_dir;
        public string is_dir;
        public string mdatetime;
        public string ref_key;
        public string content_md5;
    }
}
