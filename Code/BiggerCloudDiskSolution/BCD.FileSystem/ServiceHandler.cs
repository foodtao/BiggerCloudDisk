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
            while (true)
            {
                try
                {
                    Thread.Sleep(10 * 1000);


                    //var root = "G:\\temp";
                    //var files = MemoryFileManager.GetInstance().GetAllFiles();
                    //CloudDiskManager cloudDiskManager = new CloudDiskManager();
                    //if (files != null && files.Count > 0)
                    //{
                    //    files = files.OrderBy(p => p.FilePath).ToList();
                    //    foreach (var file in files)
                    //    {
                    //        try
                    //        {
                    //            var cloudFile = cloudDiskManager.GetCloudFileInfo(
                    //                CloudDiskType.NOT_SPECIFIED, file.FilePath);
                    //            if (cloudFile == null)
                    //            {
                    //                if (file.FileType == FileTypeEnum.Directory
                    //                    && file.FileStatus != FileStatusEnum.Remove)
                    //                {
                    //                    cloudDiskManager.CreateDirectory(file.FilePath);
                    //                    file.FileStatus = FileStatusEnum.Normal;
                    //                    MemoryFileManager.GetInstance().SetFile(file);
                    //                }
                    //                else if (file.FileType == FileTypeEnum.File
                    //                         && file.FileStatus != FileStatusEnum.Remove)
                    //                {
                    //                    var fileInfo = new FileInfo(root + file.FilePath);
                    //                    var buffer = new byte[fileInfo.Length];
                    //                    fileInfo.OpenRead().Read(buffer, 0, (int)fileInfo.Length);
                    //                    cloudDiskManager.UploadFile(
                    //                        CloudFileUploadType.Create, file.FilePath, buffer);
                    //                    file.FileStatus = FileStatusEnum.Normal;
                    //                    MemoryFileManager.GetInstance().SetFile(file);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if (file.FileStatus == FileStatusEnum.Remove)
                    //                {
                    //                    if (file.FileType == FileTypeEnum.Directory)
                    //                    {
                    //                        cloudDiskManager.DeleteDirectory(file.FilePath);
                    //                        MemoryFileManager.GetInstance().RemoveFile(file.FilePath);
                    //                    }
                    //                    else if (file.FileType == FileTypeEnum.File)
                    //                    {
                    //                        cloudDiskManager.DeleteFile(CloudDiskType.NOT_SPECIFIED, file.FilePath);
                    //                        MemoryFileManager.GetInstance().RemoveFile(file.FilePath);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    if (file.LastModifyDate > cloudFile.LastModifiedDate
                    //                        && file.FileStatus == FileStatusEnum.Append)
                    //                    {
                    //                        var fileInfo = new FileInfo(root + file.FilePath);
                    //                        var buffer = new byte[fileInfo.Length];
                    //                        fileInfo.OpenRead().Read(buffer, 0, (int)fileInfo.Length);
                    //                        cloudDiskManager.UploadFile(
                    //                            CloudFileUploadType.Create, file.FilePath, buffer);
                    //                        file.FileStatus = FileStatusEnum.Normal;
                    //                        MemoryFileManager.GetInstance().SetFile(file);
                    //                    }
                    //                    else if (file.LastModifyDate > cloudFile.LastModifiedDate)
                    //                    {
                    //                        var cloudbyte = cloudDiskManager.DownloadFile(CloudDiskType.NOT_SPECIFIED, file.FilePath);

                    //                        File.Delete();
                    //                    }
                    //                }
                    //            }
                    //        }
                    //        catch
                    //        {
                                
                    //        }
                    //    }
                    //}
                }
                catch
                { }
                Thread.Sleep(60 * 1000);
            }
        }

        public static int CreateDir(string dirName)
        {
            var client = new CloudDiskManager();
            client.CreateDirectory(dirName);
            return 1;
        }

        public static int RemoveDir(string dirName)
        {
            var client = new CloudDiskManager();
            var result = client.DeleteDirectory(dirName);
            return 1;
        }

        public static int CreateFile(byte[] fileContent)
        {
            var client = new CloudDiskManager();
            //client.UploadFile(fileContent);
            return 1;
        }

        public int PullFile()
        {
            return 1;
        }

    }
}
