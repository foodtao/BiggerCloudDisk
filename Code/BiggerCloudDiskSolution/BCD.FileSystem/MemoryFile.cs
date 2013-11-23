using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCD.FileSystem
{
    public class MemoryFile
    {
        public string FilePath { get; set; }

        public FileTypeEnum FileType { get; set; }

        public FileStatusEnum FileStatus { get; set; }
    }

    public enum FileTypeEnum
    {
        File,

        Directory
    }

    public enum FileStatusEnum
    {
        Create,

        Append,

        Remove,

        Normal,
    }
}
