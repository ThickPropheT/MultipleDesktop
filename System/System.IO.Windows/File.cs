using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO.Windows
{
    public class File : IFile
    {
        public bool Exists(string path)
        {
            return IO.File.Exists(path);
        }

        public string[] ReadAllLines(string path)
        {
            return IO.File.ReadAllLines(path);
        }

        public void WriteAllLines(string path, string[] contents)
        {
            IO.File.WriteAllLines(path, contents);
        }
    }
}
