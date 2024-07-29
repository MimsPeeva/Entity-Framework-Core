using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Footballers.Utilities
{
    public class xmlHelper
    {
        public T Deserialize<T>(string inputXml, string rootName)
            where T : class
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), xmlRoot);

            using StringReader reader = new StringReader(inputXml);
            object? deserializedObjects = xmlSerializer.Deserialize(reader);
            if (deserializedObjects == null ||
                deserializedObjects is not T deserializedObjectsTypes)
            {
                throw new InvalidOperationException();
            }
            return deserializedObjectsTypes;
        }

        public string Serialize<T>(T obj, string rootName)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot =
                new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), xmlRoot);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };
            using (StringWriter writer = new StringWriter(sb))
            using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
            {
                xmlSerializer.Serialize(xmlWriter, obj, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
