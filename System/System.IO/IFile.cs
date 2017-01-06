namespace System.IO
{
    public interface IFile
    {
        bool Exists(string path);

        string[] ReadAllLines(string path);
        void WriteAllLines(string path, string[] contents);
    }
}
