namespace System.IO.Extended
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
