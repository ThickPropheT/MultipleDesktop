using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    public interface IFile
    {
        bool Exists(string path);

        string[] ReadAllLines(string path);
        void WriteAllLines(string path, string[] contents);
    }
}
