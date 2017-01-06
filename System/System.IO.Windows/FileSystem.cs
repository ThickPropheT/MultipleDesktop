using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO.Windows
{
    public class FileSystem : IFileSystem
    {
        private static readonly IFileSystem _fileSystem = new FileSystem();

        public IFile File { get; }

        private FileSystem()
        {
            File = new File();
        }

        public static IFileSystem GetFileSystem()
        {
            return _fileSystem;
        }
    }
}
