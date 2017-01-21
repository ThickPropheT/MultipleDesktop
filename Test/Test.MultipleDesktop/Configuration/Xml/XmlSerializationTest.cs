using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultipleDesktop.Configuration.Xml;
using System.Xml;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Test.MultipleDesktop.Configuration.Xml
{
    [TestClass]
    public class XmlSerializationTest
    {
        [ClassInitialize]
        public static void Init(TestContext context)
        {
            ShouldNotFail();
        }

        private static void ShouldNotFail()
        {

        }

        [TestMethod]
        public void TestMethod1()
        {
            var settings = new XmlReaderSettings() { ValidationType = ValidationType.Schema };
            using (var fileReader0 = new StreamReader(@"Configuration\schema0.xsd"))
            using (var fileReader1 = new StreamReader(@"Configuration\schema1.xsd"))
            {
                settings.Schemas.Add(XmlSchema.Read(fileReader0,
                    (s, e) => Assert.Fail()));
                settings.Schemas.Add(XmlSchema.Read(fileReader1,
                    (s, e) => Assert.Fail()));
            }

            settings.ValidationEventHandler += (s, e) => Assert.Fail();

            var test = new AppConfiguration();

            var serializer = new XmlSerializer(typeof(AppConfiguration));

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, test);

                stream.Position = 0;

                var v = XmlReader.Create(stream, settings);

                while (v.Read()) { }
            }
        }
    }
}
