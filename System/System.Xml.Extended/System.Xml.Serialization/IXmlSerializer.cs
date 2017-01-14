using System.IO;

namespace System.Xml.Serialization.Extended
{
    public interface IXmlSerializer
    {
        void Serialize(Stream stream, object o);
    }
}
