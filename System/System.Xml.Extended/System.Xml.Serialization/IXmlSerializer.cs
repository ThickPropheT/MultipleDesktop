using System.IO;

namespace System.Xml.Serialization.Extended
{
    public interface IXmlSerializer
    {
        void Serialize(TextWriter writer, object o);
        object Deserialize(TextReader textReader);
    }
}
