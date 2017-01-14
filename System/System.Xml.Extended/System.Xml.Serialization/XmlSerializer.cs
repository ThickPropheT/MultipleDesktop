namespace System.Xml.Serialization.Extended
{
    public sealed class XmlSerializer : Serialization.XmlSerializer, IXmlSerializer
    {
        public XmlSerializer(Type type)
            : base(type)
        {

        }
    }
}
