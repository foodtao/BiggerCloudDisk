using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.FileSystem
{
    using System.IO;
    using System.Threading;

    using BCD.Utility;

    /// <summary>
    /// 磁盘缓存文件管理类
    /// </summary>
    public class MemoryFileManager
    {
        private static readonly object _lock = new object();

        private static readonly object _fileLock = new object();

        private static volatile MemoryFileManager memoryFileManager = null;

        private static List<MemoryFile> memoryFiles = new List<MemoryFile>();

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
                }
            }
        }

        /// <summary>
        /// 是否需要同步
        /// </summary>
        /// <returns></returns>
        public bool IsNeedSync()
        {
            lock (memoryFiles)
            {
                var needSyncFiles = memoryFiles.Where(p => p.FileStatus != FileStatusEnum.Normal).ToList();
                if (needSyncFiles != null && needSyncFiles.Count > 0)
                {
                    return true;
                }
                return false;
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
                var root = LocalDiskPathHelper.GetPath();
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

}
