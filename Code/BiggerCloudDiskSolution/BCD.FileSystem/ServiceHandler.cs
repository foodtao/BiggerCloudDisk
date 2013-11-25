using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCD.DiskInterface;

namespace BCD.FileSystem
{
    using System.IO;
    using System.Threading;

    using BCD.Model.CloudDisk;
    using BCD.Utility;

    public class ServiceHandler
    {
        /// <summary>
        /// 动态缓存线程
        /// </summary>
        private static Thread DataThreadSingle;
        /// <summary>
        /// 时间
        /// </summary>
        private static DateTime dtStartTime = default(DateTime);

        /// <summary>
        /// 动态数据缓存启动
        /// </summary>
        public static void Start()
        {
            try
            {
                //5秒钟内不可重复启动线程
                if ((DateTime.Now - dtStartTime).TotalSeconds <= 5)
                {
                }
                else
                {
                    if (DataThreadSingle == null
                        ||
                        (DataThreadSingle.ThreadState == ThreadState.Stopped
                         && DataThreadSingle.ThreadState != ThreadState.Background))
                    {
                        DataThreadSingle = new Thread(CheckFile);
                        dtStartTime = DateTime.Now;
                        DataThreadSingle.Start();
                    }
                }
            }
            catch (ThreadStateException threadStateException)
            {
                //过滤Thread错误
            }
            catch
            {

            }
        }

        /// <summary>
        /// 检查文件
        /// </summary>
        public static void CheckFile()
        {
            //while (true)
            //{
                try
                {
                    //Thread.Sleep(10 * 1000);

                    DateTime dataChangeDate1 = DateTime.MinValue;
                    if (MemoryFileManager.GetInstance().GetCacheStatus(out dataChangeDate1)
                        || MemoryFileManager.GetInstance().GetAllFiles().Count == 0) //如果缓存有更新
                    {
                        SysToCloud();
                    }
                    DateTime dataChangeDate2 = DateTime.MinValue;
                    if (dataChangeDate1 == dataChangeDate2)
                    {
                        MemoryFileManager.GetInstance().SetCacheStatus(false);
                    }
                }
                catch
                { }
            //}
        }

        /// <summary>
        /// 同步本地更改到云。
        /// </summary>
        public static void SysToCloud()
        {
            //SyncRemoveFileToCloud();
            //SyncChangeFileToCloud();

            var fileInfo = new CloudFileInfoModel { Path = "/", IsDir = true };

            SyncCloudFileToLocal(fileInfo);
        }

        /// <summary>
        /// 同步删除的内容到云
        /// </summary>
        public static void SyncRemoveFileToCloud()
        {
            var memFiles =
                MemoryFileManager.GetInstance().GetAllFiles().OrderBy(p => p.FilePath).Where(
                    p => p.FileStatus == FileStatusEnum.Remove).ToList();
            var cloudDisk = new CloudDiskManager();
            if (memFiles.Count > 0)
            {
                while (memFiles.Count > 0)
                {
                    try
                    {
                        var memFile = memFiles[0];
                        if (memFile.FileType == FileTypeEnum.Directory)
                        {
                            cloudDisk.DeleteDirectory(memFile.FilePath);
                            memFiles.RemoveAll(p => p.FilePath.Contains(memFile.FilePath));
                            MemoryFileManager.GetInstance().RemoveFile(memFile.FilePath);
                        }
                        else if (memFile.FileType == FileTypeEnum.File)
                        {
                            cloudDisk.DeleteFile(CloudDiskType.NOT_SPECIFIED, memFile.FilePath);
                            memFiles.RemoveAll(p => p.FilePath.Contains(memFile.FilePath));
                            MemoryFileManager.GetInstance().RemoveFile(memFile.FilePath);
                        }
                    }
                    catch
                    {
                        
                    }
                }
            }
        }

        /// <summary>
        /// 同步更改的内容到云
        /// </summary>
        public static void SyncChangeFileToCloud()
        {
            var memFiles =
                MemoryFileManager.GetInstance().GetAllFiles().OrderBy(p => p.FilePath).Where(
                    p => p.FileStatus != FileStatusEnum.Remove).ToList();
            var cloudDisk = new CloudDiskManager();
            var root = LocalDiskPathHelper.GetPath();
            if (memFiles.Count > 0)
            {
                while (memFiles.Count > 0)
                {
                    var memFile = memFiles[0];
                    try
                    {
                        if (memFile.FileType == FileTypeEnum.Directory)
                        {
                            if (memFile.FileStatus == FileStatusEnum.Create)
                            {
                                cloudDisk.CreateDirectory(memFile.FilePath);
                                memFile.FileStatus = FileStatusEnum.Normal;
                                MemoryFileManager.GetInstance().SetFile(memFile);
                                memFiles.Remove(memFile);
                            }
                            else
                            {
                                var cloudFiles = cloudDisk.GetCloudFileInfo(CloudDiskType.KINGSOFT, memFile.FilePath);
                                if (cloudFiles == null)
                                {
                                    cloudDisk.CreateDirectory(memFile.FilePath);
                                    memFiles.Remove(memFile);
                                }
                            }
                        }
                        else if (memFile.FileType == FileTypeEnum.File)
                        {
                            if (memFile.FileStatus == FileStatusEnum.Create)
                            {
                                var fileInfo = new FileInfo(root + memFile.FilePath);
                                var buffer = new byte[fileInfo.Length];
                                fileInfo.OpenRead().Read(buffer, 0, (int)fileInfo.Length);
                                cloudDisk.UploadFile(
                                    CloudFileUploadType.Create, memFile.FilePath, buffer);
                                memFile.FileStatus = FileStatusEnum.Normal;
                                MemoryFileManager.GetInstance().SetFile(memFile);
                                memFiles.Remove(memFile);
                            }
                            else
                            {
                                var cloudFiles = cloudDisk.GetCloudFileInfo(CloudDiskType.KINGSOFT, memFile.FilePath);
                                if (cloudFiles == null)
                                {
                                    var fileInfo = new FileInfo(root + memFile.FilePath);
                                    var buffer = new byte[fileInfo.Length];
                                    fileInfo.OpenRead().Read(buffer, 0, (int)fileInfo.Length);
                                    cloudDisk.UploadFile(
                                        CloudFileUploadType.Create, memFile.FilePath, buffer);
                                    memFiles.Remove(memFile);
                                }
                                else
                                {
                                    var fileInfo = new FileInfo(root + memFile.FilePath);
                                    if (cloudFiles.LastModifiedDate.HasValue)
                                    {
                                        if (fileInfo.LastWriteTime > cloudFiles.LastModifiedDate)
                                        {
                                            var buffer = new byte[fileInfo.Length];
                                            fileInfo.OpenRead().Read(buffer, 0, (int)fileInfo.Length);
                                            cloudDisk.UploadFile(
                                                CloudFileUploadType.Create, memFile.FilePath, buffer);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        memFiles.Remove(memFile);
                    }
                }
            }
        }

        /// <summary>
        /// 同步云端的文件到本地。
        /// </summary>
        public static void SyncCloudFileToLocal(CloudFileInfoModel fileInfo)
        {
            var root = LocalDiskPathHelper.GetPath();

            var cloudManager = new CloudDiskManager();
            var cloudFile = cloudManager.GetCloudFileInfo(CloudDiskType.KINGSOFT, fileInfo.Path);
            if (cloudFile != null)
            {
                var path = root + cloudFile.LocalPath;

                if (fileInfo.Path != "/")
                {
                    if (cloudFile.IsDir)
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                    else
                    {
                        var needCreat = false;
                        if (!File.Exists(path))
                        {
                            needCreat = true;
                        }
                        else
                        {
                            var file = new FileInfo(path);
                            if (cloudFile.LastModifiedDate.HasValue)
                            {
                                if (file.LastWriteTime < cloudFile.LastModifiedDate)
                                {
                                    File.Delete(path);
                                    needCreat = true;
                                }
                            }
                        }
                        if (needCreat)
                        {
                            var fileContent = cloudManager.DownloadFile(CloudDiskType.NOT_SPECIFIED, cloudFile.Path);
                            File.WriteAllBytes(path, fileContent);
                        }
                    }
                }

                if (cloudFile != null && cloudFile.Contents != null && cloudFile.Contents.Count > 0)
                {
                    foreach (var subFile in cloudFile.Contents)
                    {
                        if (!cloudFile.Path.EndsWith("/")) cloudFile.Path = cloudFile.Path + "/";
                        subFile.Path = cloudFile.Path + subFile.name;
                        SyncCloudFileToLocal(subFile);
                    }
                }
            }
        }


        public static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;
            double dblSByte = bytes;
            if (bytes > 1024)
                for (i = 0; (bytes / 1024) > 0; i++, bytes /= 1024)
                    dblSByte = bytes / 1024.0;
            return String.Format("{0:0.##}{1}", dblSByte, Suffix[i]);
        }
    }
}
