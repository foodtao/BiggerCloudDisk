﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BCD.FileSystem
{
    using BCD.DiskInterface;

    using Dokan;
    using System.IO;

    public class MirrorDisk : DokanOperations
    {
        private string root_;
        
        public MirrorDisk(string root)
        {
            root_ = root;
        }
        private string GetPath(string filename)
        {
            string path = root_ + filename;
            return path;
        }

        public int CreateFile(string filename, FileAccess access, 
            FileShare share, FileMode mode, FileOptions options, DokanFileInfo info)
        {
            try
            {
                string path = GetPath(filename);
                if (Directory.Exists(path))
                {
                    info.IsDirectory = true;
                    return DokanNet.DOKAN_SUCCESS;
                }
                switch (mode)
                {
                    case FileMode.Append:
                        return DokanNet.DOKAN_SUCCESS;
                    case FileMode.Create:
                        if (!File.Exists(path))
                        {
                            FileStream fs = File.Create(path);
                            fs.Close();

                            var fileInfo = new FileInfo(path);
                            var memoryFile = new MemoryFile
                                {
                                    CreateDate = fileInfo.CreationTime,
                                    FilePath = path.Replace(this.root_, ""),
                                    FileStatus = FileStatusEnum.Create,
                                    FileType = FileTypeEnum.File,
                                    LastModifyDate = fileInfo.LastWriteTime
                                };
                            MemoryFileManager.GetInstance().SetFile(memoryFile);
                        }
                        return DokanNet.DOKAN_SUCCESS;
                    case FileMode.CreateNew:
                        if (!File.Exists(path))
                        {
                            FileStream fs = File.Create(path);
                            fs.Close();

                            var fileInfo = new FileInfo(path);
                            var memoryFile = new MemoryFile
                            {
                                CreateDate = fileInfo.CreationTime,
                                FilePath = path.Replace(this.root_, ""),
                                FileStatus = FileStatusEnum.Create,
                                FileType = FileTypeEnum.File,
                                LastModifyDate = fileInfo.LastWriteTime
                            };
                            MemoryFileManager.GetInstance().SetFile(memoryFile);
                        }
                        return DokanNet.DOKAN_SUCCESS;
                    case FileMode.Open:
                        return DokanNet.DOKAN_SUCCESS;
                    case FileMode.OpenOrCreate:
                        return DokanNet.DOKAN_SUCCESS;

                }
                return -DokanNet.ERROR_FILE_NOT_FOUND;
            }
            catch 
            {
                return DokanNet.DOKAN_SUCCESS;
            }
        }

        public int FindFiles(string filename, System.Collections.ArrayList files, DokanFileInfo info)
        {
            try
            {
                string path = GetPath(filename);
                if (Directory.Exists(path))
                {
                    var d = new DirectoryInfo(path);
                    FileSystemInfo[] entries = d.GetFileSystemInfos();
                    foreach (FileSystemInfo f in entries)
                    {
                        var fi = new FileInformation();
                        fi.Attributes = f.Attributes;
                        fi.CreationTime = f.CreationTime;
                        fi.LastAccessTime = f.LastAccessTime;
                        fi.LastWriteTime = f.LastWriteTime;
                        fi.Length = (f is DirectoryInfo) ? 0 : ((FileInfo)f).Length;
                        fi.FileName = f.Name;
                        files.Add(fi);
                    }
                    return DokanNet.DOKAN_SUCCESS;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
                return DokanNet.DOKAN_SUCCESS;
            }
        }

        public int GetFileInformation(string filename, FileInformation fileinfo, DokanFileInfo info)
        {
            try
            {
                string path = GetPath(filename);
                if (File.Exists(path))
                {
                    var f = new FileInfo(path);

                    fileinfo.Attributes = f.Attributes;
                    fileinfo.CreationTime = f.CreationTime;
                    fileinfo.LastAccessTime = f.LastAccessTime;
                    fileinfo.LastWriteTime = f.LastWriteTime;
                    fileinfo.Length = f.Length;
                    return DokanNet.DOKAN_SUCCESS;
                }
                else if (Directory.Exists(path))
                {
                    var f = new DirectoryInfo(path);

                    fileinfo.Attributes = f.Attributes;
                    fileinfo.CreationTime = f.CreationTime;
                    fileinfo.LastAccessTime = f.LastAccessTime;
                    fileinfo.LastWriteTime = f.LastWriteTime;
                    fileinfo.Length = 0;
                    return DokanNet.DOKAN_SUCCESS;
                }
                else
                {
                    return DokanNet.ERROR_FILE_NOT_FOUND;
                }
            }
            catch (Exception)
            {
                return DokanNet.DOKAN_SUCCESS;
            }
        }

        public int DeleteFile(string filename, DokanFileInfo info)
        {
            try
            {
                var path = this.GetPath(filename);
                if (File.Exists(path))
                {

                    var fileInfo = new FileInfo(path);
                    var memoryFile = new MemoryFile
                        {
                            CreateDate = fileInfo.CreationTime,
                            FilePath = path.Replace(this.root_, ""),
                            FileStatus = FileStatusEnum.Remove,
                            FileType = FileTypeEnum.File,
                            LastModifyDate = fileInfo.LastWriteTime
                        };

                    File.Delete(path);

                    MemoryFileManager.GetInstance().SetFile(memoryFile);
                }
                return DokanNet.DOKAN_SUCCESS;
            }
            catch (Exception)
            {
                return DokanNet.DOKAN_SUCCESS;
            }
        }

        public int CreateDirectory(string filename, DokanFileInfo info)
        {
            try
            {
                if (filename == "\\") return DokanNet.DOKAN_SUCCESS;
                var path = this.GetPath(filename);
                Directory.CreateDirectory(path);

                var dir = new DirectoryInfo(path);
                var memoryFile = new MemoryFile
                    {
                        CreateDate = dir.CreationTime,
                        FilePath = path.Replace(this.root_, ""),
                        FileStatus = FileStatusEnum.Create,
                        FileType = FileTypeEnum.Directory,
                        LastModifyDate = dir.LastWriteTime
                    };

                MemoryFileManager.GetInstance().SetFile(memoryFile);

                return DokanNet.DOKAN_SUCCESS;
            }
            catch
            {
                return DokanNet.DOKAN_SUCCESS;
            }
        }

        public int OpenDirectory(string filename, DokanFileInfo info)
        {
            try
            {
                if (Directory.Exists(GetPath(filename)))
                    return DokanNet.DOKAN_SUCCESS;
                else
                    return -DokanNet.ERROR_PATH_NOT_FOUND;
            }
            catch (Exception)
            {
                return DokanNet.DOKAN_SUCCESS;
            }
        }

        public int DeleteDirectory(string filename, DokanFileInfo info)
        {
            try
            {
                if (filename == "\\") return DokanNet.DOKAN_SUCCESS;
                var path = this.GetPath(filename);
                if (Directory.Exists(path))
                {
                    var dir = new DirectoryInfo(path);
                    var memoryFile = new MemoryFile
                    {
                        CreateDate = dir.CreationTime,
                        FilePath = path.Replace(this.root_, ""),
                        FileStatus = FileStatusEnum.Remove,
                        FileType = FileTypeEnum.Directory,
                        LastModifyDate = dir.LastWriteTime
                    };

                    Directory.Delete(path, true);

                    MemoryFileManager.GetInstance().SetFile(memoryFile);
                }
                
                return DokanNet.DOKAN_SUCCESS;
            }
            catch (Exception)
            {
                return DokanNet.DOKAN_SUCCESS;
            }
        }

        public int GetDiskFreeSpace(ref ulong freeBytesAvailable, ref ulong totalBytes, ref ulong totalFreeBytes, DokanFileInfo info)
        {
            try
            {
                var space = (Model.CloudDisk.CloudDiskCapacityModel)AppDomain.CurrentDomain.GetData("diskspace");
                if (space != null)
                {
                    totalBytes = (ulong)space.TotalByte;
                    freeBytesAvailable = (ulong)space.TotalAvailableByte;
                    totalFreeBytes = (ulong)space.TotalAvailableByte;
                }
                if (totalBytes == 0)
                {
                    totalBytes = 1024 * 1024 * 1024;
                }
                if (freeBytesAvailable == 0)
                {
                    freeBytesAvailable = 1024 * 1024 * 1024;
                }
                if (totalFreeBytes == 0)
                {
                    totalFreeBytes = 1024 * 1024 * 1024;
                }
            }
            catch
            {
                totalBytes = 1024 * 1024 * 1024;
                freeBytesAvailable = 1024 * 1024 * 1024;
                totalFreeBytes = 1024 * 1024 * 1024;
            }
            return 0;
        }

        public int MoveFile(string filename, string newname, bool replace, DokanFileInfo info)
        {
            try
            {
                if (filename == "\\") return DokanNet.DOKAN_SUCCESS;
                var newPath = this.GetPath(newname);
                var path = this.GetPath(filename);

                if (string.IsNullOrEmpty(Path.GetExtension(newPath)))
                {
                    var dir = new DirectoryInfo(path);

                    var oldFiles =
                        MemoryFileManager.GetInstance().GetAllFiles().Where(p => p.FilePath.Contains(filename)).ToList();

                    Directory.Move(path, newPath);

                    var memoryFile = new MemoryFile
                    {
                        CreateDate = dir.CreationTime,
                        FilePath = dir.FullName.Replace(this.root_, ""),
                        FileStatus = FileStatusEnum.Remove,
                        FileType = FileTypeEnum.Directory,
                        LastModifyDate = dir.LastWriteTime
                    };

                    MemoryFileManager.GetInstance().SetFile(memoryFile);

                    foreach (var oldFile in oldFiles)
                    {
                        var memoryFileNew = new MemoryFile()
                            {
                                CreateDate = oldFile.CreateDate,
                                FilePath = oldFile.FilePath.Replace(filename, newname),
                                FileStatus = FileStatusEnum.Create,
                                FileType = oldFile.FileType,
                                LastModifyDate = oldFile.LastModifyDate
                            };
                        MemoryFileManager.GetInstance().SetFile(memoryFileNew);
                    }

                }
                else
                {
                    var fileOld = new FileInfo(path);
                    var memoryFile = new MemoryFile
                    {
                        CreateDate = fileOld.CreationTime,
                        FilePath = fileOld.FullName.Replace(this.root_, ""),
                        FileStatus = FileStatusEnum.Remove,
                        FileType = FileTypeEnum.File,
                        LastModifyDate = fileOld.LastWriteTime
                    };

                    File.Move(path, newPath);

                    var fileNew = new FileInfo(newPath);

                    var memoryFileNew = new MemoryFile
                    {
                        CreateDate = fileNew.CreationTime,
                        FilePath = fileNew.FullName.Replace(this.root_, ""),
                        FileStatus = FileStatusEnum.Create,
                        FileType = FileTypeEnum.File,
                        LastModifyDate = fileNew.LastWriteTime
                    };

                    MemoryFileManager.GetInstance().SetFile(memoryFile);

                    MemoryFileManager.GetInstance().SetFile(memoryFileNew);
                }

                return DokanNet.DOKAN_SUCCESS;
            }
            catch (Exception)
            {
                return DokanNet.DOKAN_SUCCESS;
            }

        }

        public int ReadFile(string filename, byte[] buffer, ref uint readBytes, long offset, DokanFileInfo info)
        {
            try
            {
                FileStream fs = File.OpenRead(GetPath(filename));
                fs.Seek(offset, SeekOrigin.Begin);
                readBytes = (uint)fs.Read(buffer, 0, buffer.Length);
                fs.Dispose();
                fs.Close();
                return DokanNet.DOKAN_SUCCESS;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int WriteFile(string filename, byte[] buffer, ref uint writtenBytes, long offset, DokanFileInfo info)
        {
            try
            {
                var path = this.GetPath(filename);
                if (!File.Exists(path)) return DokanNet.ERROR_FILE_NOT_FOUND;
                File.WriteAllBytes(path, buffer);
                var file = MemoryFileManager.GetInstance().GetFile(path);
                if (file != null && file.FileStatus != FileStatusEnum.Create && file.FileStatus != FileStatusEnum.Remove)
                {
                    var fileInfo = new FileInfo(path);
                    var memoryFile = new MemoryFile
                    {
                        CreateDate = fileInfo.CreationTime,
                        FilePath = path.Replace(this.root_, ""),
                        FileStatus = FileStatusEnum.Append,
                        FileType = FileTypeEnum.File,
                        LastModifyDate = fileInfo.LastWriteTime
                    };
                    MemoryFileManager.GetInstance().SetFile(memoryFile);
                }
                return DokanNet.DOKAN_SUCCESS;
            }
            catch (Exception)
            {
                return DokanNet.DOKAN_SUCCESS;
            }
        }

        public int SetAllocationSize(string filename, long length, DokanFileInfo info)
        {
            return DokanNet.DOKAN_SUCCESS;
        }

        public int SetEndOfFile(string filename, long length, DokanFileInfo info)
        {
            return DokanNet.DOKAN_SUCCESS;
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

    }
}
