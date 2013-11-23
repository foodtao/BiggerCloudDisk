﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCD.Model.CloudDisk;
using BCD.DiskInterface.Sina;
using BCD.DiskInterface.Baidu;
using BCD.DiskInterface.Kingsoft;
using System.Configuration;

namespace BCD.DiskInterface
{
    /// <summary>
    /// 云盘通用的接口,内部封装实现多个网盘
    /// 2013-11-19 by 丁智渊
    /// </summary>
    public class CloudDiskManager
    {
        //private SinaDiskApi _sinaApi = new SinaDiskApi();
        //private BaiduDiskAPI _baiduApi = new BaiduDiskAPI();
        //private KingsoftDiskAPI _kingsoftApi = new KingsoftDiskAPI();

        private string _loadModulesStr = "";

        private CloudDiskFactory _factory = new CloudDiskFactory();

        private List<ICloudDiskAPI> _loadedCloudDiskApi = new List<ICloudDiskAPI>();

        public List<ICloudDiskAPI> LoadedCloudDiskApi { get { return _loadedCloudDiskApi; } }

        public CloudDiskManager()
        {
            try
            {
                _loadModulesStr = ConfigurationManager.AppSettings["LoadCloudDiskModules"];
                string[] moduels = _loadModulesStr.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string m in moduels)
                {
                    ICloudDiskAPI d = _factory.CreateDiskAPI(m);
                    if (d != null)
                    {
                        _loadedCloudDiskApi.Add(d);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("在创建网盘API对象时出错!" + ex.Message);
            }
        }

        /// <summary>
        /// 获取网盘空间信息
        /// </summary>
        /// <returns>包括总空间,可用空间在内的信息</returns>
        public CloudDiskCapacityModel GetCloudDiskCapacityInfo()
        {
            CloudDiskCapacityModel m = new CloudDiskCapacityModel();
            //var sinaC = _sinaApi.GetCloudDiskCapacityInfo();
            //var baiduC = _baiduApi.GetCloudDiskCapacityInfo();
            //var kingsoftC = _kingsoftApi.GetCloudDiskCapacityInfo();
            double max = 0;
            double total = 0;
            double totalAvailable = 0;
            double min = 9999999999999999999;
            foreach (ICloudDiskAPI api in _loadedCloudDiskApi)
            {
                var tmpM = api.GetCloudDiskCapacityInfo();
                if (tmpM.TotalAvailableByte > max)
                {
                    max = tmpM.TotalAvailableByte;
                }
                if (tmpM.TotalAvailableByte < min)
                {
                    min = tmpM.TotalAvailableByte;
                }
                total += tmpM.TotalByte;
                totalAvailable += tmpM.TotalAvailableByte;
            }
            m.MaxAvailableByte = max;
            m.MinAvailableByte = min;
            m.TotalAvailableByte = totalAvailable;
            m.TotalByte = total;
            return m;
        }

        /// <summary>
        /// 获取一个远程文件的信息
        /// </summary>
        /// <param name="type">网盘类型</param>
        /// <param name="remotePath">远程文件的相对路径</param>
        /// <returns></returns>
        public CloudFileInfoModel GetCloudFileInfo(CloudDiskType type, string remotePath)
        {
            CloudFileInfoModel m = null;
            try
            {
                //CloudDiskType type = GetDiskTypeByURL(remotePath);
                ICloudDiskAPI oneApi;
                if (IsDiskTypeLoaded(type, out oneApi))
                {
                    m = oneApi.GetCloudFileInfo(remotePath);
                }
                else
                {
                    throw new Exception("文件地址不正确或者网盘接口模块没有被正确加载!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取远程文件信息出错!" + ex.Message);
            }
            return m;
        }

        /// <summary>
        /// 上传一个文件,具体上传至哪个网盘由内部决定,有可能会抛出异常,异常在外面处理
        /// </summary>
        /// <param name="type">上传类型,新增或者更新</param>
        /// <param name="filePath">文件相对路径,含文件名</param>
        /// <param name="fileContent">文件内容</param>
        /// <returns></returns>
        public CloudFileInfoModel UploadFile(CloudFileUploadType type, string filePath, byte[] fileContent)
        {
            CloudFileInfoModel uploaded = new CloudFileInfoModel();
            if (type == CloudFileUploadType.Create)
            {
                ICloudDiskAPI api = GetOptimizedDisk(CloudDiskOptimizationTypeModel.AVAILABLE_BIGGEST);
                if (api != null)
                {
                    uploaded = api.UploadFile(fileContent, filePath);
                }
                else
                {
                    throw new Exception("没有可用的网盘可供上传!");
                }
            }
            //修改
            if (type == CloudFileUploadType.Update)
            {
                //遍历所有网盘
                foreach (ICloudDiskAPI api in _loadedCloudDiskApi)
                {
                    if (api.GetCloudFileInfo(filePath) != null)
                    {
                        uploaded = api.UploadFile(fileContent, filePath);
                    }
                }
            }
            return uploaded;
        }

        /// <summary>
        /// 下载一个文件.有可能会抛出异常,异常在外面处理
        /// </summary>
        /// <param name="remotePath">远程文件地址</param>
        /// <returns>文件字节内容</returns>
        public byte[] DownloadFile(CloudDiskType type, string remotePath)
        {
            byte[] fileContent = null;
            try
            {
                //CloudDiskType type = GetDiskTypeByURL(remotePath);
                if (type != CloudDiskType.NOT_SPECIFIED)
                {
                    ICloudDiskAPI oneApi;
                    if (IsDiskTypeLoaded(type, out oneApi))
                    {
                        fileContent = oneApi.DownloadFile(remotePath);
                    }
                    else
                    {
                        throw new Exception("文件地址不正确或者网盘接口模块没有被正确加载!");
                    }
                }
                else
                {
                    foreach (ICloudDiskAPI api in _loadedCloudDiskApi)
                    {
                        if (api.GetCloudFileInfo(remotePath) != null)
                        {
                            fileContent = api.DownloadFile(remotePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取远程文件信息出错!" + ex.Message);
            }
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
            try
            {
                foreach (ICloudDiskAPI api in _loadedCloudDiskApi)
                {
                    var oneDir = api.CreateDirectory(dir);
                    list.Add(oneDir);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("同步文件夹时出错!" + ex.Message);
            }
            return list;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="remotePath"></param>
        /// <returns></returns>
        public int DeleteFile(CloudDiskType type, string remotePath)
        {
            try
            {
                //CloudDiskType type = GetDiskTypeByURL(remotePath);
                ICloudDiskAPI oneApi;
                if (type != CloudDiskType.NOT_SPECIFIED)
                {
                    if (IsDiskTypeLoaded(type, out oneApi))
                    {
                        oneApi.DeleteFile(remotePath);
                    }
                    else
                    {
                        throw new Exception("文件地址不正确或者网盘接口模块没有被正确加载!");
                    }
                }
                else
                {
                    foreach (ICloudDiskAPI api in _loadedCloudDiskApi)
                    {
                        if (api.GetCloudFileInfo(remotePath) != null)
                        {
                            api.DeleteFile(remotePath);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("删除远程文件信息出错!" + ex.Message);
            }
            return 1;
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="remotePath"></param>
        /// <returns></returns>
        public int DeleteDirectory(string remotePath)
        {
            foreach (ICloudDiskAPI api in _loadedCloudDiskApi)
            {
                api.DeleteDirectory(remotePath);
            }
            return 0;
        }

        /// <summary>
        /// 移动远程文件
        /// </summary>
        /// <param name="originPath">原始路径</param>
        /// <param name="newPath">新路径</param>
        /// <param name="isCopy">是否需要复制</param>
        /// <returns>新文件的信息</returns>
        public CloudFileInfoModel MoveFile(CloudDiskType type, string originPath, string newPath, bool isCopy)
        {
            CloudFileInfoModel m = new CloudFileInfoModel();
            try
            {
                //CloudDiskType type = GetDiskTypeByURL(originPath);
                ICloudDiskAPI oneApi;
                if (IsDiskTypeLoaded(type, out oneApi))
                {
                    oneApi.MoveFile(originPath, newPath, isCopy);
                }
                else
                {
                    throw new Exception("文件地址不正确或者网盘接口模块没有被正确加载!");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("移动远程文件信息出错!" + ex.Message);
            }
            return m;
        }

        /// <summary>
        /// 定期刷新令牌
        /// </summary>
        /// <returns>所有网盘的新令牌</returns>
        public IList<AccessTokenModel> RefreshAccessToken()
        {
            IList<AccessTokenModel> result = new List<AccessTokenModel>();
            foreach (ICloudDiskAPI api in _loadedCloudDiskApi)
            {
                var oneToken = api.GetAccessToken();
                api.WriteLocalAccessToken(oneToken);
                result.Add(oneToken);
            }
            return result;
        }

        /// <summary>
        /// 根据访问或者文件的URL获得所属网盘类型
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private CloudDiskType GetDiskTypeByURL(string url)
        {
            CloudDiskType type = CloudDiskType.NOT_SPECIFIED;
            //if (url.Contains("sina"))
            //{
            //    type = CloudDiskType.SINA;
            //}
            return type;
        }

        /// <summary>
        /// 判断一个网盘是否被正常加载
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsDiskTypeLoaded(CloudDiskType type, out ICloudDiskAPI api)
        {
            api = null;
            foreach (ICloudDiskAPI one in _loadedCloudDiskApi)
            {
                if (one.GetDiskType() == type)
                {
                    api = one;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据特定优化的要求返回符合条件的网盘
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ICloudDiskAPI GetOptimizedDisk(CloudDiskOptimizationTypeModel model)
        {
            ICloudDiskAPI result = null;
            switch (model)
            {
                case CloudDiskOptimizationTypeModel.AVAILABLE_BIGGEST:
                    GetAvailableBiggestCloudDisk(); //可用空间最大的网盘.
                    break;
                case CloudDiskOptimizationTypeModel.BIGGEST:
                    GetAvailableBiggestCloudDisk();
                    break;
                case CloudDiskOptimizationTypeModel.FASTEST:
                    GetAvailableBiggestCloudDisk();
                    break;
                case CloudDiskOptimizationTypeModel.OTHER:
                    GetAvailableBiggestCloudDisk();
                    break;
                default:
                    GetAvailableBiggestCloudDisk();
                    break;
            }
            return result;
        }

        private ICloudDiskAPI GetAvailableBiggestCloudDisk()
        {
            double max = 0;
            ICloudDiskAPI result = null;
            foreach (ICloudDiskAPI api in _loadedCloudDiskApi)
            {
                var tmp = api.GetCloudDiskCapacityInfo();
                if (tmp.TotalAvailableByte > max)
                {
                    max = tmp.TotalAvailableByte;
                    result = api;
                }
            }
            return result;
        }
    }
}
