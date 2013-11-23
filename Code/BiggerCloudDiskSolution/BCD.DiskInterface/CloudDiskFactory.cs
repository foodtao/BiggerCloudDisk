using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BCD.DiskInterface
{
    public class CloudDiskFactory
    {
        public ICloudDiskAPI CreateDiskAPI(string diskName)
        {
            ICloudDiskAPI result;
            Assembly ass = Assembly.GetExecutingAssembly();
            object obj = ass.CreateInstance("BCD.DiskInterface." + diskName, false, BindingFlags.Default, null, null, null, null);
            result = (ICloudDiskAPI)obj;
            return result;
        }
    }
}
