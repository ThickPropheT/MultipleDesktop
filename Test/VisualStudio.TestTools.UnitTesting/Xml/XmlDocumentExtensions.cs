using System.IO;
using System.Xml;

namespace VisualStudio.TestTools.UnitTesting.Xml
{
    public static class XmlDocumentExtensions
    {
        public static string GetXmlString(this XmlDocument document)
        {
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                document.WriteTo(xmlTextWriter);

                xmlTextWriter.Flush();

                return stringWriter.GetStringBuilder().ToString();
            }
        }
    }
}