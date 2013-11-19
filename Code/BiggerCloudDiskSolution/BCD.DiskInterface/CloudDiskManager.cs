using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCD.Model.CloudDisk;

namespace BCD.DiskInterface
{
    /// <summary>
    /// 云盘通用的接口,内部封装实现多个网盘
    /// 2013-11-19 by 丁智渊
    /// </summary>
    public class CloudDiskManager
    {
        /// <summary>
        /// 获取网盘空间信息
        /// </summary>
        /// <returns>包括总空间,可用空间在内的信息</returns>
        public CloudDiskCapacityModel GetCloudDiskCapacityInfo() 
        {
            CloudDiskCapacityModel m = new CloudDiskCapacityModel();
            return m;
        }

        /// <summary>
        /// 获取一个远程文件的信息
        /// </summary>
        /// <param name="remotePath">远程文件的完整路径</param>
        /// <returns></returns>
        public CloudFileInfoModel GetCloudFileInfo(string remotePath) 
        {
            CloudFileInfoModel m = new CloudFileInfoModel();
            return m;
        }

        /// <summary>
        /// 上传一个文件,具体上传至哪个网盘由内部决定,有可能会抛出异常,异常在外面处理
        /// </summary>
        /// <param name="fileContent">文件内容</param>
        /// <returns></returns>
        public CloudFileInfoModel UploadFile(byte[] fileContent) 
        {
            CloudFileInfoModel uploaded = new CloudFileInfoModel();
            return uploaded;
        }

        /// <summary>
        /// 下载一个文件.有可能会抛出异常,异常在外面处理
        /// </summary>
        /// <param name="remotePath">远程文件地址</param>
        /// <returns>文件字节内容</returns>
        public byte[] DownloadFile(string remotePath) 
        {
            byte[] fileContent = null;
            return fileContent;
        }

        /// <summary>
        /// 创建一个远程文件夹,在所有网盘的所有相同的相对路径下面创建一个一模一样的文件夹
        /// </summary>
        /// <param name="dir">文件夹相对路径</param>
        /// <returns></returns>
        public List<CloudFileInfoModel> CreateDirectory(string dir) 
        {
            List<CloudFileInfoModel> list = new List<CloudFileInfoModel>();
            return list;
        }

        /// <summary>
        /// 删除文件,文件夹
        /// </summary>
        /// <param name="remotePath"></param>
        /// <returns></returns>
        public int DeleteFile(string remotePath) 
        {
            return 0;
        }

        /// <summary>
        /// 移动远程文件
        /// </summary>
        /// <param name="originPath">原始路径</param>
        /// <param name="newPath">新路径</param>
        /// <param name="isCopy">是否需要复制</param>
        /// <returns>新文件的信息</returns>
        public CloudFileInfoModel MoveFile(string originPath, string newPath, bool isCopy)
        {
            CloudFileInfoModel m = new CloudFileInfoModel();
            return m;
        }
    }
}
