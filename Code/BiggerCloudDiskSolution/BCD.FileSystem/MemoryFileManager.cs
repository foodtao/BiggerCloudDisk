using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.FileSystem
{
    using System.IO;
    using System.Threading;

    /// <summary>
    /// 磁盘缓存文件管理类
    /// </summary>
    public class MemoryFileManager
    {
        private static readonly object _lock = new object();

        private static readonly object _fileLock = new object();

        private static volatile MemoryFileManager memoryFileManager = null;

        private static List<MemoryFile> memoryFiles = new List<MemoryFile>();

        /// <summary>
        /// 是否数据发生变化。
        /// </summary>
        private static bool IsDataChange = false;

        /// <summary>
        /// 数据变化的时间。
        /// </summary>
        private static DateTime DataChangeDate = DateTime.MinValue;

        public static MemoryFileManager GetInstance()
        {
            if (memoryFileManager == null)
            {
                lock (_lock)
                {
                    if (memoryFileManager == null)
                    {
                        memoryFileManager = new MemoryFileManager();
                        GetLocalFileData();
                    }
                }
            }
            return memoryFileManager;
        }

        /// <summary>
        /// 添加或修改文件信息到缓存中
        /// </summary>
        /// <param name="memoryFile"></param>
        public void SetFile(MemoryFile memoryFile)
        {
            lock (_fileLock)
            {
                var file = memoryFiles.FirstOrDefault(p => p.FilePath == memoryFile.FilePath);
                if (file == null)
                {
                    memoryFiles.Add(memoryFile);
                    IsDataChange = true;
                    DataChangeDate = DateTime.Now;
                }
                else
                {
                    file.FilePath = memoryFile.FilePath;
                    file.FileStatus = memoryFile.FileStatus;
                    file.FileType = memoryFile.FileType;
                    file.CreateDate = memoryFile.CreateDate;
                    file.LastModifyDate = memoryFile.LastModifyDate;
                    if (file.FileType == FileTypeEnum.Directory && file.FileStatus == FileStatusEnum.Remove)
                    {
                        var files = memoryFiles.Where(p => p.FilePath.Contains(memoryFile.FilePath)).ToList();
                        if (files.Count > 0)
                        {
                            foreach (var f in files)
                            {
                                f.FileStatus = FileStatusEnum.Remove;
                            }
                        }
                    }
                    if (file.FileStatus != FileStatusEnum.Normal)
                    {
                        IsDataChange = true;
                        DataChangeDate = DateTime.Now;
                    }
                }
            }
        }

        /// <summary>
        /// 设置缓存状态
        /// </summary>
        /// <param name="isDataChange"></param>
        public void SetCacheStatus(bool isDataChange)
        {
            lock (memoryFiles)
            {
                IsDataChange = isDataChange;
                DataChangeDate = DateTime.Now;
            }
        }

        /// <summary>
        /// 获取缓存状态
        /// </summary>
        /// <returns></returns>
        public bool GetCacheStatus(out DateTime dateTime)
        {
            lock (memoryFiles)
            {
                dateTime = DataChangeDate;
                return IsDataChange;
            }
        }

        /// <summary>
        /// 从缓存项中移除此文件信息。
        /// </summary>
        /// <param name="filePath"></param>
        public void RemoveFile(string filePath)
        {
            lock (_fileLock)
            {
                var file = memoryFiles.FirstOrDefault(p => p.FilePath == filePath);
                if (file != null)
                {
                    memoryFiles.Remove(file);
                }
            }
        }

        /// <summary>
        /// 获取某个文件的缓存项信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public MemoryFile GetFile(string filePath)
        {
            lock (_fileLock)
            {
                MemoryFile file = memoryFiles.FirstOrDefault(p => p.FilePath == filePath);

                return file;
            }
        }

        /// <summary>
        /// 获取所有文件的缓存项信息。
        /// </summary>
        /// <returns></returns>
        public List<MemoryFile> GetAllFiles()
        {
            lock (_fileLock)
            {
                return memoryFiles;
            }
        }

        /// <summary>
        /// 初始化文件缓存信息。
        /// </summary>
        public static void GetLocalFileData()
        {
            try
            {
                var root = "G:\\Temp";
                var rootFiles = (new DirectoryInfo(root)).GetFiles();
                if (rootFiles.Length > 0)
                {
                    foreach (var rootFile in rootFiles)
                    {
                        var memoryFile = new MemoryFile();
                        memoryFile.CreateDate = rootFile.CreationTime;
                        memoryFile.FilePath = rootFile.FullName.Replace(root, "");
                        memoryFile.FileStatus = FileStatusEnum.Normal;
                        memoryFile.FileType = FileTypeEnum.File;
                        memoryFile.LastModifyDate = rootFile.LastWriteTime;
                        GetInstance().SetFile(memoryFile);
                    }
                }

                var dirs = new List<DirectoryInfo>();

                GetALlDirectoryInfo(root, ref dirs);

                if (dirs.Count > 0)
                {
                    foreach (var dir in dirs)
                    {
                        var memoryFile = new MemoryFile();
                        memoryFile.CreateDate = dir.CreationTime;
                        memoryFile.FilePath = dir.FullName.Replace(root, "");
                        memoryFile.FileStatus = FileStatusEnum.Normal;
                        memoryFile.FileType = FileTypeEnum.Directory;
                        memoryFile.LastModifyDate = dir.LastWriteTime;
                        GetInstance().SetFile(memoryFile);

                        var files = dir.GetFiles();

                        foreach (var file in files)
                        {
                            var memoryFile1 = new MemoryFile();
                            memoryFile1.CreateDate = file.CreationTime;
                            memoryFile1.FilePath = file.FullName.Replace(root, "");
                            memoryFile1.FileStatus = FileStatusEnum.Normal;
                            memoryFile1.FileType = FileTypeEnum.File;
                            memoryFile1.LastModifyDate = file.LastWriteTime;
                            GetInstance().SetFile(memoryFile1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 获取所有文件夹信息。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dirs"></param>
        private static void GetALlDirectoryInfo(string path, ref List<DirectoryInfo> dirs)
        {
            var dir = new DirectoryInfo(path);
            var dir_temps = dir.GetDirectories();
            if (dir_temps.Length > 0)
            {
                foreach (var dirTemp in dir_temps)
                {
                    dirs.Add(dirTemp);
                    GetALlDirectoryInfo(dirTemp.FullName, ref dirs);
                }
            }
        }

    }

    /// <summary>
    /// 文件管理线程类
    /// </summary>
    public class MemoryFileManagerThead
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
                if ((DateTime.Now - dtStartTime).TotalSeconds <= 5) { }
                else
                {
                    if (DataThreadSingle == null || 
                        (DataThreadSingle.ThreadState == ThreadState.Stopped 
                        && DataThreadSingle.ThreadState != ThreadState.Background))
                    {
                        DataThreadSingle = new Thread(GetFileInfo);
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
        /// 发送酒店访问数据到缓存服务器。
        /// </summary>
        public static void GetFileInfo()
        {
            while (true)
            {
                try
                {
                    var root = "G:\\Temp";
                    var rootFiles = (new DirectoryInfo(root)).GetFiles();
                    if (rootFiles.Length > 0)
                    {
                        foreach (var rootFile in rootFiles)
                        {
                            var memoryFile = new MemoryFile();
                            memoryFile.CreateDate = rootFile.CreationTime;
                            memoryFile.FilePath = rootFile.FullName.Replace(root, "");
                            memoryFile.FileStatus = FileStatusEnum.Normal;
                            memoryFile.FileType = FileTypeEnum.File;
                            memoryFile.LastModifyDate = rootFile.LastWriteTime;
                            MemoryFileManager.GetInstance().SetFile(memoryFile);
                        }
                    }

                    var dirs = new List<DirectoryInfo>();

                    GetALlDirectoryInfo(root, ref dirs);

                    if (dirs.Count > 0)
                    {
                        foreach (var dir in dirs)
                        {
                            var memoryFile = new MemoryFile();
                            memoryFile.CreateDate = dir.CreationTime;
                            memoryFile.FilePath = dir.FullName.Replace(root, "");
                            memoryFile.FileStatus = FileStatusEnum.Normal;
                            memoryFile.FileType = FileTypeEnum.Directory;
                            memoryFile.LastModifyDate = dir.LastWriteTime;
                            MemoryFileManager.GetInstance().SetFile(memoryFile);

                            var files = dir.GetFiles();

                            foreach (var file in files)
                            {
                                var memoryFile1 = new MemoryFile();
                                memoryFile1.CreateDate = file.CreationTime;
                                memoryFile1.FilePath = file.FullName.Replace(root, "");
                                memoryFile1.FileStatus = FileStatusEnum.Normal;
                                memoryFile1.FileType = FileTypeEnum.File;
                                memoryFile1.LastModifyDate = file.LastWriteTime;
                                MemoryFileManager.GetInstance().SetFile(memoryFile1);
                            }
                        }
                    }

                    Thread.Sleep(10 * 1000);
                }
                catch (Exception ex)
                {
                   
                }
            }
        }

        public static void GetALlDirectoryInfo(string path, ref List<DirectoryInfo> dirs)
        {
            var dir = new DirectoryInfo(path);
            var dir_temps = dir.GetDirectories();
            if (dir_temps.Length > 0)
            {
                foreach (var dirTemp in dir_temps)
                {
                    dirs.Add(dirTemp);
                    GetALlDirectoryInfo(dirTemp.FullName, ref dirs);
                }
            }
        }
    }
}
