using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Boardgames.Utilities
{
    public class XmlHelper
    {
        public static T Deserialize<T>(string inputXml, string rootName)
        {
            if (string.IsNullOrEmpty(inputXml))
                throw new ArgumentException("Input XML cannot be null or empty.", nameof(inputXml));

            if (string.IsNullOrEmpty(rootName))
                throw new ArgumentException("Root name cannot be null or empty.", nameof(rootName));

            try
            {
                XmlRootAttribute xmlRoot = new(rootName);
                XmlSerializer xmlSerializer = new(typeof(T), xmlRoot);

                using var reader = new StringReader(inputXml);
                return (T)xmlSerializer.Deserialize(reader);
            }
            catch (XmlException ex)
            {
                Debug.WriteLine(ex);
                throw new InvalidOperationException("XML deserialization failed.", ex);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex);
                throw new InvalidOperationException($"{typeof(T)} deserialization failed.", ex);
            }
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