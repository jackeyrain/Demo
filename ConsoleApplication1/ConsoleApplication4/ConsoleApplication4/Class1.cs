using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ConsoleApplication4
{
    public static class SerializeHelper<T>
           where T : class, new()
    {
        public static Stream Serialize(T obj)
        {
            XmlWriterSettings settings = new XmlWriterSettings();

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            ns.Add("uax", "http://opcfoundation.org/UA/2008/02/Types.xsd");
            ns.Add("", "http://opcfoundation.org/UA/2011/03/UANodeSet.xsd");

            MemoryStream ms = new MemoryStream();
            using (XmlWriter writer = XmlWriter.Create(ms, settings))
            {
                serializer.Serialize(writer, obj, ns);
                writer.Close();
            }
            return ms;
        }
        public static T Deserial(string xmlStr)
        {
            T r = default(T);
            MemoryStream memStream = new MemoryStream();
            byte[] data = Encoding.UTF8.GetBytes(xmlStr);
            memStream.Write(data, 0, data.Length);
            memStream.Position = 0;
            using (XmlReader reader = XmlReader.Create(memStream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                r = serializer.Deserialize(reader) as T;
            }
            return r;
        }
        public static string Serialize1(T obj)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = false;
            settings.Encoding = Encoding.UTF8;

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            ns.Add("", "http://opcfoundation.org/UA/2011/03/UANodeSet.xsd");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            ns.Add("uax", "http://opcfoundation.org/UA/2008/02/Types.xsd");
            ns.Add("xsd", "http://www.w3.org/2001/XMLSchema");

            StringBuilder strBuilder = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(strBuilder, settings);
            using (writer)
            {
                serializer.Serialize(writer, obj, ns);
                writer.Close();
            }
            return strBuilder.ToString();
        }
    }
}
