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
        /// 本地更改同步线程
        /// </summary>
        private static Thread DataThreadLocalSync;

        /// <summary>
        /// 云端更改同步线程
        /// </summary>
        private static Thread DataThreadCloudSync;

        /// <summary>
        /// 本地更改同步线程同步时间
        /// </summary>
        private static DateTime dtLocalStartTime = default(DateTime);

        /// <summary>
        /// 云端更改同步线程同步时间
        /// </summary>
        private static DateTime dtCloudStartTime = default(DateTime);

        /// <summary>
        /// 动态数据缓存启动
        /// </summary>
        public static void Start()
        {
            try
            {
                //5秒钟内不可重复启动线程
                if ((DateTime.Now - dtLocalStartTime).TotalSeconds <= 5)
                {
                }
                else
                {
                    if (DataThreadLocalSync == null
                        ||
                        (DataThreadLocalSync.ThreadState == ThreadState.Stopped
                         && DataThreadLocalSync.ThreadState != ThreadState.Background))
                    {
                        DataThreadLocalSync = new Thread(CheckFile);
                        dtLocalStartTime = DateTime.Now;
                        DataThreadLocalSync.Start();
                    }
                }

                //if((DateTime.Now - dtCloudStartTime).TotalSeconds <= 5)
                //{
                //}
                //else
                //{
                //    if (DataThreadCloudSync == null
                //        || (DataThreadCloudSync.ThreadState == ThreadState.Stopped
                //        && DataThreadCloudSync.ThreadState != ThreadState.Background))
                //    {
                //        DataThreadCloudSync = new Thread(SysCloudToLocal);
                //        dtCloudStartTime = DateTime.Now;
                //        DataThreadCloudSync.Start();
                //    }
                //}
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
        /// 同步本地更改到云端
        /// </summary>
        public static void CheckFile()
        {
            while (true)
            {
                try
                {
                    if (MemoryFileManager.GetInstance().IsNeedSync()
                        || MemoryFileManager.GetInstance().GetAllFiles().Count == 0) //如果缓存有更新
                    {
                        SysToCloud();
                    }

                    Thread.Sleep(10 * 1000);
                }
                catch
                {
                }
            }
        }

        public static List<string> CloudFiles = null; 
        
        /// <summary>
        /// 同步远端更改到本地
        /// </summary>
        public static void SysCloudToLocal()
        {
            while (true)
            {
                try
                {
                    CloudFiles = new List<string>();

                    var fileInfo = new CloudFileInfoModel { Path = "/", IsDir = true };

                    SyncCloudFileToLocal(fileInfo);

                    SyncCloudRemoveFileToLocal();

                    Thread.Sleep(30 * 1000);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 同步本地更改到云。
        /// </summary>
        public static void SysToCloud()
        {
            SyncRemoveFileToCloud();
            SyncChangeFileToCloud();

            CloudFiles = new List<string>();

            var fileInfo = new CloudFileInfoModel { Path = "/", IsDir = true };

            SyncCloudFileToLocal(fileInfo);

            SyncCloudRemoveFileToLocal();
        }

        /// <summary>
        /// 同步删除的内容到云
        /// </summary>
        public static void SyncRemoveFileToCloud()
        {
            var memFiles = new List<MemoryFile>();
            memFiles =
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
            var memFiles = new List<MemoryFile>();
            memFiles =
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
                            }
                            else
                            {
                                var cloudFiles = cloudDisk.GetCloudFileInfo(CloudDiskType.NOT_SPECIFIED, true, memFile.FilePath);
                                if (cloudFiles == null)
                                {
                                    cloudDisk.CreateDirectory(memFile.FilePath);
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
                            }
                            else
                            {
                                var cloudFiles = cloudDisk.GetCloudFileInfo(CloudDiskType.NOT_SPECIFIED, false, memFile.FilePath);
                                if (cloudFiles == null)
                                {
                                    var fileInfo = new FileInfo(root + memFile.FilePath);
                                    var buffer = new byte[fileInfo.Length];
                                    fileInfo.OpenRead().Read(buffer, 0, (int)fileInfo.Length);
                                    cloudDisk.UploadFile(
                                        CloudFileUploadType.Create, memFile.FilePath, buffer);
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
                        memFiles.Remove(memFile);
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
            var cloudFile = cloudManager.GetCloudFileInfo(CloudDiskType.NOT_SPECIFIED, fileInfo.IsDir, fileInfo.Path);
            if (cloudFile != null)
            {
                CloudFiles.Add(cloudFile.LocalPath);

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

        /// <summary>
        /// 同步云端已删除的文件到本地。
        /// </summary>
        public static void SyncCloudRemoveFileToLocal()
        {
            var path = LocalDiskPathHelper.GetPath();
            var localFiles = GetLocalFiles();

            if (localFiles != null && localFiles.Count > 0)
            {
                if (CloudFiles != null && CloudFiles.Count > 0)
                {
                    foreach (var localFile in localFiles)
                    {
                        if (!CloudFiles.Contains(localFile))
                        {
                            if (Directory.Exists(path + localFile))
                            {
                                Directory.Delete(path + localFile);
                            }
                            else if(File.Exists(path + localFile))
                            {
                                File.Delete(path + localFile);
                            }
                        }
                    }  
                }
                else
                {
                    Directory.Delete(path);
                    Directory.CreateDirectory(path);
                }
            }
        }

        /// <summary>
        /// 获取本地同步文件夹下所有文件
        /// </summary>
        /// <returns></returns>
        public static List<string> GetLocalFiles()
        {
            var path = LocalDiskPathHelper.GetPath();
            var contents = new List<string>();
            var dirs = new List<DirectoryInfo>();
            MemoryFileManager.GetALlDirectoryInfo(path,ref dirs);
            if (dirs.Count > 0)
            {
                foreach (var dir in dirs)
                {
                    contents.Add(dir.FullName.Replace(path, ""));
                    var files = dir.GetFiles();

                    foreach (var file in files)
                    {
                        contents.Add(file.FullName.Replace(path, ""));
                    }
                }
            }
            return contents;
        }

        /// <summary>
        /// 转换文件大小单位
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
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
