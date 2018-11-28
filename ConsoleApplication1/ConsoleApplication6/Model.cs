using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ConsoleApplication3
{
    public class Helper
    {
        public static Alias[] getAlias()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("XMLFile1.xml");
            var nodeSet = doc.SelectNodes("//Alias");
            List<Alias> result = new List<Alias>();
            foreach (XmlNode node in nodeSet)
            {
                result.Add(new Alias() { AliasAttribute = node.Attributes["Alias"].InnerText, AliasValue = node.InnerText });
            }
            return result.ToArray();
        }
    }

    [XmlRoot(ElementName = "UANodeSet", Namespace = "http://opcfoundation.org/UA/2011/03/UANodeSet.xsd")]
    public class UANodeSet
    {
        [XmlArray("NamespaceUris")]
        public Uri[] Uri { get; set; }

        [XmlArray("Aliases")]
        public Alias[] Aliases { get; set; }

        [XmlElement("UAObjectType")]
        public List<UAObjectType> UAObjectType { get; set; }

        [XmlElement("UAVariable")]
        public List<UAVariable> UAVariable { get; set; }
    }

    public class Uri
    {
        [XmlText]
        public string UriValue { get; set; }
    }

    public class Alias
    {
        [XmlAttribute("Alias")]
        public string AliasAttribute { get; set; }
        [XmlText]
        public string AliasValue { get; set; }

    }

    public class UAObjectType
    {
        [XmlAttribute("NodeId")]
        public string NodeId { get; set; }

        [XmlAttribute("BrowseName")]
        public string BrowseName { get; set; }

        [XmlElement("DisplayName")]
        public DisplayName DisplayName { get; set; }
    }

    public class DisplayName
    {
        [XmlText]
        public string DisplayNameValue { get; set; }
    }

    public class UAVariable
    {
        [XmlAttribute("DataType")]
        public string DataType { get; set; }
        [XmlAttribute("ParentNodeId")]
        public string ParentNodeId { get; set; }
        [XmlAttribute("NodeId")]
        public string NodeId { get; set; }
        [XmlAttribute("BrowseName")]
        public string BrowseName { get; set; }
        [XmlAttribute("AccessLevel")]
        public string AccessLevel { get; set; }

        [XmlElement("DisplayName")]
        public string DisplayName { get; set; }

        [XmlArray("References")]
        public Reference[] References { get; set; }
    }

    public class Reference
    {
        [XmlAttribute("ReferenceType")]
        public string ReferenceType { get; set; }
        [XmlAttribute("IsForward")]
        public string IsForward { get; set; }
        [XmlText]
        public string ReferenceTypeValue { get; set; }
    }
}
