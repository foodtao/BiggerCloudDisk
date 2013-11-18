using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.FilSystem
{
    using Dokan;
    using System.IO;

    public class MirrorDisk : DokanOperations
    {
        /// <summary>
        /// 实际磁盘存储目录
        /// </summary>
        string _root;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="root">实际磁盘存储目录</param>
        public MirrorDisk(string root)
        {
            _root = root;
        }

        public int CreateFile(string filename, System.IO.FileAccess access, System.IO.FileShare share, System.IO.FileMode mode, System.IO.FileOptions options, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int FindFiles(string filename, System.Collections.ArrayList files, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int GetFileInformation(string filename, FileInformation fileinfo, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int DeleteFile(string filename, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int CreateDirectory(string filename, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int OpenDirectory(string filename, DokanFileInfo info)
        {
            if (filename == _root)
                return DokanNet.DOKAN_SUCCESS;

            if (Directory.Exists(filename))
            {
                return DokanNet.DOKAN_SUCCESS;
            }
            else
            {
                return DokanNet.ERROR_FILE_NOT_FOUND;
            }
        }

        public int DeleteDirectory(string filename, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int FlushFileBuffers(string filename, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int GetDiskFreeSpace(ref ulong freeBytesAvailable, ref ulong totalBytes, ref ulong totalFreeBytes, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int MoveFile(string filename, string newname, bool replace, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int ReadFile(string filename, byte[] buffer, ref uint readBytes, long offset, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int SetAllocationSize(string filename, long length, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int SetEndOfFile(string filename, long length, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int SetFileAttributes(string filename, System.IO.FileAttributes attr, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int SetFileTime(string filename, DateTime ctime, DateTime atime, DateTime mtime, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int WriteFile(string filename, byte[] buffer, ref uint writtenBytes, long offset, DokanFileInfo info)
        {
            throw new NotImplementedException();
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

    }
}
