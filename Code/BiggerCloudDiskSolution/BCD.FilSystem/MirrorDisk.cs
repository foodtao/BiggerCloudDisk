using System;
using System.Collections.Generic;
using System.Linq;

namespace BCD.FilSystem
{
    using Dokan;
    using System.IO;

    public class MirrorDisk : DokanOperations
    {
        /// <summary>
        /// 实际磁盘存储目录
        /// </summary>
        readonly string _root;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="root">实际磁盘存储目录</param>
        public MirrorDisk(string root)
        {
            _root = root;
        }

        public int CreateFile(string filename, FileAccess access, 
            FileShare share, FileMode mode, FileOptions options, DokanFileInfo info)
        {
            if (filename == "\\") return DokanNet.DOKAN_SUCCESS;
            var path = _root + filename;

            if (string.IsNullOrEmpty(Path.GetExtension(path)))
            {
                Directory.CreateDirectory(path);
                return DokanNet.DOKAN_SUCCESS;
            }

            switch (mode)
            {
                case  FileMode.Append:
                    if (!File.Exists(path))
                        File.Create(path);
                    return DokanNet.DOKAN_SUCCESS;
                case FileMode.Create:
                    if (!File.Exists(path))
                        File.Create(path);
                    return DokanNet.DOKAN_SUCCESS;
                case FileMode.CreateNew:
                    if (File.Exists(path))
                        return DokanNet.ERROR_FILE_EXISTS;
                    File.Create(path);
                    return DokanNet.DOKAN_SUCCESS;
                case FileMode.Open:
                    if (!File.Exists(path))
                        File.Create(path);
                    return DokanNet.DOKAN_SUCCESS;
                case FileMode.OpenOrCreate:
                    if (!File.Exists(path))
                        File.Create(path);
                    return DokanNet.DOKAN_SUCCESS;
                case FileMode.Truncate:
                    if (!File.Exists(path))
                    {
                        using (var fs = File.Create(path))
                        {
                            fs.SetLength(0);
                        }
                    }
                    return DokanNet.DOKAN_SUCCESS;
            }

            return DokanNet.DOKAN_SUCCESS;
        }

        public int FindFiles(string filename, System.Collections.ArrayList files, DokanFileInfo info)
        {
            var path = _root + filename;
            if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);
                var fileInfomation = new FileInformation();
                fileInfomation.FileName = fileInfo.Name;
                fileInfomation.Attributes = fileInfo.Attributes;
                fileInfomation.LastAccessTime = fileInfo.LastAccessTime;
                fileInfomation.LastWriteTime = fileInfo.LastWriteTime;
                fileInfomation.CreationTime = fileInfo.CreationTime;
                fileInfomation.Length = fileInfo.Length;
                files.Add(fileInfomation);
            }
            else
            {
                var dirs = new List<DirectoryInfo>();
                this.GetALlDirectoryInfo(path, ref dirs);
                if (dirs.Count > 0)
                {
                    foreach (var directoryInfo in dirs)
                    {
                        var fileInfos = directoryInfo.GetFiles();
                        foreach (var fileInfo in fileInfos)
                        {
                            var fileInfomation = new FileInformation();
                            fileInfomation.FileName = fileInfo.Name;
                            fileInfomation.Attributes = fileInfo.Attributes;
                            fileInfomation.LastAccessTime = fileInfo.LastAccessTime;
                            fileInfomation.LastWriteTime = fileInfo.LastWriteTime;
                            fileInfomation.CreationTime = fileInfo.CreationTime;
                            fileInfomation.Length = fileInfo.Length;
                            files.Add(fileInfomation);
                        }
                    }
                }
            }
            return DokanNet.DOKAN_SUCCESS;
        }

        public int GetFileInformation(string filename, FileInformation fileinfo, DokanFileInfo info)
        {
            if (filename == _root || info.IsDirectory)
            {
                var path = (filename == _root) ? _root : _root + filename;
                if (!Directory.Exists(path)) return DokanNet.ERROR_PATH_NOT_FOUND;
                var dir = new DirectoryInfo(path);
                fileinfo.FileName = dir.Name;
                fileinfo.Attributes = dir.Attributes;
                fileinfo.LastAccessTime = dir.LastAccessTime;
                fileinfo.LastWriteTime = dir.LastWriteTime;
                fileinfo.CreationTime = dir.CreationTime;
                return DokanNet.DOKAN_SUCCESS;
            }
            else
            {

                var path = _root + filename;
                if (!File.Exists(path)) return DokanNet.ERROR_FILE_NOT_FOUND;
                var file = new FileInfo(path);
                fileinfo.FileName = file.Name;
                fileinfo.Attributes = file.Attributes;
                fileinfo.LastAccessTime = file.LastAccessTime;
                fileinfo.LastWriteTime = file.LastWriteTime;
                fileinfo.CreationTime = file.CreationTime;
                fileinfo.Length = file.Length;
                return DokanNet.DOKAN_SUCCESS;
            }
        }

        public int DeleteFile(string filename, DokanFileInfo info)
        {
            var path = _root + filename;
            if (!File.Exists(path)) return DokanNet.ERROR_FILE_NOT_FOUND;
            File.Delete(path);
            return DokanNet.DOKAN_SUCCESS;
        }

        public int CreateDirectory(string filename, DokanFileInfo info)
        {
            var path = _root + filename;
            if (Directory.Exists(path)) return DokanNet.ERROR_ALREADY_EXISTS;
            Directory.CreateDirectory(path);
            return DokanNet.DOKAN_SUCCESS;
        }

        public int OpenDirectory(string filename, DokanFileInfo info)
        {
            if (filename == _root)
                return DokanNet.DOKAN_SUCCESS;

            if (Directory.Exists(filename))
            {
                return DokanNet.DOKAN_SUCCESS;
            }
            return DokanNet.ERROR_FILE_NOT_FOUND;
        }

        public int DeleteDirectory(string filename, DokanFileInfo info)
        {
            var path = _root + filename;
            if (!Directory.Exists(path))
            {
                return -DokanNet.ERROR_PATH_NOT_FOUND;
            }
            Directory.Delete(path, true);
            return DokanNet.DOKAN_SUCCESS;
        }

        public int GetDiskFreeSpace(ref ulong freeBytesAvailable, ref ulong totalBytes, ref ulong totalFreeBytes, DokanFileInfo info)
        {
            totalBytes = (ulong)Environment.WorkingSet;
            freeBytesAvailable = totalBytes - GetDirectoryLength(_root);

            totalFreeBytes = int.MaxValue;

            return DokanNet.DOKAN_SUCCESS;
        }

        public int MoveFile(string filename, string newname, bool replace, DokanFileInfo info)
        {
            var newPath = _root + newname;
            var path = _root + filename;

            if (string.IsNullOrEmpty(Path.GetExtension(newPath)))
            {
                if (File.Exists(path))
                {
                    var name = Path.GetFileName(path);
                    var dirInfo = new DirectoryInfo(newPath);
                    var file = dirInfo.GetFiles().FirstOrDefault(p => p.Name == name);
                    if (file != null)
                    {
                        if (replace)
                        {
                            File.Delete(file.FullName);
                            File.Move(path, newPath);
                        }
                    }
                    else
                    {
                        File.Move(path, newPath);
                    }
                }
                else if (Directory.Exists(filename))
                {
                    var parentFolderName = path.Substring(path.LastIndexOf("\\") + 1, path.Length);
                    var dirInfo = new DirectoryInfo(newPath);
                    var folder = dirInfo.GetDirectories().FirstOrDefault(p => p.Name == parentFolderName);
                    if (folder != null)
                    {
                        if (replace)
                        {
                            Directory.Delete(folder.FullName);
                            Directory.Move(path, newPath);
                        }
                        else
                        {
                            Directory.Move(path, newPath);
                        }
                    }
                }
            }
            else
            {
                File.Move(path, newPath);
            }

            return DokanNet.DOKAN_SUCCESS;

        }

        public int ReadFile(string filename, byte[] buffer, ref uint readBytes, long offset, DokanFileInfo info)
        {
            var path = _root + filename;
            if (File.Exists(path)) return DokanNet.ERROR_FILE_NOT_FOUND;
            using (var stream = File.OpenRead(path))
            {
                stream.Seek(offset, SeekOrigin.Begin);
                readBytes = (uint)stream.Read(buffer, 0, buffer.Length);
                return DokanNet.DOKAN_SUCCESS;
            }
        }

        public int SetAllocationSize(string filename, long length, DokanFileInfo info)
        {
            var path = _root + filename;
            if (!File.Exists(path)) return DokanNet.ERROR_FILE_NOT_FOUND;
            using (var stream = new FileStream(path, FileMode.Append))
            {
                stream.SetLength(length);
                return DokanNet.DOKAN_SUCCESS;
            }
        }

        public int SetEndOfFile(string filename, long length, DokanFileInfo info)
        {
            var path = _root + filename;
            if (!File.Exists(path)) return DokanNet.ERROR_FILE_NOT_FOUND;
            using (var stream = new FileStream(path, FileMode.Append))
            {
                stream.SetLength(length);
                return DokanNet.DOKAN_SUCCESS;
            }
        }

        public int WriteFile(string filename, byte[] buffer, ref uint writtenBytes, long offset, DokanFileInfo info)
        {
            var path = _root + filename;
            if (!File.Exists(path)) return DokanNet.ERROR_FILE_NOT_FOUND;
            using (var stream = new FileStream(path, FileMode.Append))
            {
                if (offset + buffer.Length > stream.Length) stream.SetLength(offset + buffer.Length);
                stream.Seek(offset, SeekOrigin.Begin);
                stream.Write(buffer, 0, buffer.Length);
                writtenBytes = (uint)buffer.Length;
                return DokanNet.DOKAN_SUCCESS;
            }
        }

        public int Cleanup(string filename, DokanFileInfo info)
        {
            return DokanNet.DOKAN_SUCCESS;
        }

        public int CloseFile(string filename, DokanFileInfo info)
        {
            return DokanNet.DOKAN_SUCCESS;
        }

        public int UnlockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            return DokanNet.DOKAN_SUCCESS;
        }

        public int Unmount(DokanFileInfo info)
        {
            return DokanNet.DOKAN_SUCCESS;
        }

        public int LockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            return DokanNet.DOKAN_SUCCESS;
        }

        public int FlushFileBuffers(string filename, DokanFileInfo info)
        {
            return DokanNet.DOKAN_SUCCESS;
        }

        public int SetFileAttributes(string filename, FileAttributes attr, DokanFileInfo info)
        {
            return -DokanNet.DOKAN_ERROR;
        }

        public int SetFileTime(string filename, DateTime ctime, DateTime atime, DateTime mtime, DokanFileInfo info)
        {
            return -DokanNet.DOKAN_ERROR;
        }


        private ulong GetDirectoryLength(string dirPath)
        {

            if (!Directory.Exists(dirPath))

                return 0;

            ulong len = 0;

            var di = new DirectoryInfo(dirPath);

            foreach (FileInfo fi in di.GetFiles())
            {
                len += (ulong)fi.Length;
            }

            DirectoryInfo[] dis = di.GetDirectories();

            if (dis.Length > 0)
            {

                for (int i = 0; i < dis.Length; i++)
                {

                    len += GetDirectoryLength(dis[i].FullName);

                }

            }

            return len;

        }

        private void GetALlDirectoryInfo(string path, ref List<DirectoryInfo> dirs)
        {
            var dir = new DirectoryInfo(path);
            var dirTemps = dir.GetDirectories();
            if (dirTemps.Length > 0)
            {
                foreach (var dirTemp in dirTemps)
                {
                    dirs.Add(dirTemp);
                    this.GetALlDirectoryInfo(dirTemp.FullName, ref dirs);
                }
            }
        }

    }
}
