using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.FileSystem
{
    public class MemoryFileManager
    {
        private static readonly object _lock = new object();

        private static readonly object _fileLock = new object();

        private static volatile MemoryFileManager memoryFileManager = null;

        private List<MemoryFile> memoryFiles = new List<MemoryFile>(); 

        public static MemoryFileManager GetInstance()
        {
            if (memoryFileManager == null)
            {
                lock (_lock)
                {
                    if (memoryFileManager == null) 
                        return memoryFileManager = new MemoryFileManager();
                }
            }
            return memoryFileManager;
        }

        public void SetFile(string filePath, FileTypeEnum fileType, FileStatusEnum fileStatus)
        {
            lock (_fileLock)
            {
                var file = memoryFiles.FirstOrDefault(p => p.FilePath == filePath);
                if (file == null)
                {
                    file = new MemoryFile { FilePath = filePath, FileStatus = fileStatus, FileType = fileType };
                    memoryFiles.Add(file);
                }
                else
                {
                    file.FilePath = filePath;
                    file.FileStatus = fileStatus;
                    file.FileType = fileType;
                    if (file.FileType == FileTypeEnum.Directory && file.FileStatus == FileStatusEnum.Remove)
                    {
                        var files = memoryFiles.Where(p => p.FilePath.Contains(filePath)).ToList();
                        if (files.Count > 0)
                        {
                            foreach (var memoryFile in files)
                            {
                                memoryFile.FileStatus = FileStatusEnum.Remove;
                            }
                        }
                    }
                }
            }
        }

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

        public MemoryFile GetFile(string filePath)
        {
            lock (_fileLock)
            {
                MemoryFile file = memoryFiles.FirstOrDefault(p => p.FilePath == filePath);

                return file;
            }
        }

        public List<MemoryFile> GetAllFiles()
        {
            lock (_fileLock)
            {
                return memoryFiles;
            }
        }



    }
}
