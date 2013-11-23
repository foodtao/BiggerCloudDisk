using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCD.Model.CloudDisk;

namespace BCD.DiskInterface
{
    /// <summary>
    /// 网盘API的通用抽象接口
    /// 2013-11-19 by 丁智渊
    /// </summary>
    public interface ICloudDiskAPI
    {
        /// <summary>
        /// 获取保存在本地app.config里的app key
        /// </summary>
        /// <returns>app key</returns>
        string GetLocalStoredAppKey();

        /// <summary>
        /// 获取保存在本地app.config里的app secret
        /// </summary>
        /// <returns>app secret</returns>
        string GetLocalStoredAppSeceret();

        /// <summary>
        /// 获取保存在本地app.config里的access token
        /// </summary>
        /// <returns></returns>
        string GetLocalStoredAccessToken();

        /// <summary>
        /// 将获取或者刷新获得的access token写入本地文件
        /// </summary>
        void WriteLocalAccessToken();

        /// <summary>
        /// 从远程api或者或者刷新access token
        /// </summary>
        /// <returns>获得的包含access token和其他验证信息的数据模型</returns>
        AccessTokenModel GetAccessToken();

        /// <summary>
        /// 获取网盘容量信息
        /// </summary>
        /// <returns></returns>
        CloudDiskCapacityModel GetCloudDiskCapacityInfo();

        /// <summary>
        /// 获取一个远程文件的信息
        /// </summary>
        /// <param name="remotePath">远程文件的完整路径</param>
        /// <returns></returns>
        CloudFileInfoModel GetCloudFileInfo(string remotePath);

        /// <summary>
        /// 上传一个文件
        /// </summary>
        /// <param name="fileContent">文件内容</param>
        /// <returns></returns>
        CloudFileInfoModel UploadFile(byte[] fileContent);


        /// <summary>
        /// 下载一个文件.有可能会抛出异常,异常在外面处理
        /// </summary>
        /// <param name="remotePath">远程文件地址</param>
        /// <returns>文件字节内容</returns>
        byte[] DownloadFile(string remotePath);

        /// <summary>
        /// 创建一个远程文件夹
        /// </summary>
        /// <param name="dir">文件夹相对路径</param>
        /// <returns></returns>
        CloudFileInfoModel CreateDirectory(string dir);

        /// <summary>
        /// 删除文件,文件夹
        /// </summary>
        /// <param name="remotePath"></param>
        /// <returns></returns>
        int DeleteFile(string remotePath);

        /// <summary>
        /// 移动远程文件
        /// </summary>
        /// <param name="originPath">原始路径</param>
        /// <param name="newPath">新路径</param>
        /// <param name="isCopy">是否需要复制</param>
        /// <returns>新文件的信息</returns>
        CloudFileInfoModel MoveFile(string originPath, string newPath, bool isCopy);
    }
}
