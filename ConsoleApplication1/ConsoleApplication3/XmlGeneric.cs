using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class XmlGeneric
    {
        public string getXml()
        {
            var type = new List<UAObjectType>();
            type.Add(new UAObjectType()
            {
                BrowseName = "1:Jemima",
                DisplayName = new DisplayName() { DisplayNameValue = "Jemima" },
                NodeId = "ns=1;i=1001"
            });

            var variable = new List<UAVariable>();
            variable.Add(new UAVariable()
            {
                DataType = "String",
                ParentNodeId = "ns=1;i=1001",
                NodeId = "ns=1;i=6001",
                BrowseName = "1:J_String",
                AccessLevel = "3",
                DisplayName = "J_String",
                References = new[] {
                    new Reference {  ReferenceType = "HasTypeDefinition", ReferenceTypeValue = "i=63"  },
                    new Reference { ReferenceType="HasModellingRule", ReferenceTypeValue="i=78" },
                    new Reference {  ReferenceType = "HasComponent" , IsForward = "false", ReferenceTypeValue = "ns=1;i=1001"}
                },
            });
            variable.Add(new UAVariable()
            {
                DataType = "Int32",
                ParentNodeId = "ns=1;i=1001",
                NodeId = "ns=1;i=6002",
                BrowseName = "1:J_Int32",
                AccessLevel = "3",
                DisplayName = "J_Int32",
                References = new[] {
                    new Reference {  ReferenceType = "HasTypeDefinition", ReferenceTypeValue = "i=63"  },
                    new Reference { ReferenceType="HasModellingRule", ReferenceTypeValue="i=78" },
                    new Reference {  ReferenceType = "HasComponent" , IsForward = "false", ReferenceTypeValue = "ns=1;i=1001"}
                },
            });

            variable.Add(new UAVariable()
            {
                DataType = "Double",
                ParentNodeId = "ns=1;i=1001",
                NodeId = "ns=1;i=6003",
                BrowseName = "1:J_Double",
                AccessLevel = "3",
                DisplayName = "J_Double",
                References = new[] {
                    new Reference {  ReferenceType = "HasTypeDefinition", ReferenceTypeValue = "i=63"  },
                    new Reference { ReferenceType="HasModellingRule", ReferenceTypeValue="i=78" },
                    new Reference {  ReferenceType = "HasComponent" , IsForward = "false", ReferenceTypeValue = "ns=1;i=1001"}
                },
            });

            variable.Add(new UAVariable()
            {
                DataType = "Boolean",
                ParentNodeId = "ns=1;i=1001",
                NodeId = "ns=1;i=6004",
                BrowseName = "1:J_Boolean",
                AccessLevel = "3",
                DisplayName = "J_Boolean",
                References = new[] {
                    new Reference {  ReferenceType = "HasTypeDefinition", ReferenceTypeValue = "i=63"  },
                    new Reference { ReferenceType="HasModellingRule", ReferenceTypeValue="i=78" },
                    new Reference {  ReferenceType = "HasComponent" , IsForward = "false", ReferenceTypeValue = "ns=1;i=1001"}
                },
            });

            variable.Add(new UAVariable()
            {
                DataType = "Byte",
                ParentNodeId = "ns=1;i=1001",
                NodeId = "ns=1;i=6005",
                BrowseName = "1:J_Byte",
                AccessLevel = "3",
                DisplayName = "J_Byte",
                References = new[] {
                    new Reference {  ReferenceType = "HasTypeDefinition", ReferenceTypeValue = "i=63"  },
                    new Reference { ReferenceType="HasModellingRule", ReferenceTypeValue="i=78" },
                    new Reference {  ReferenceType = "HasComponent" , IsForward = "false", ReferenceTypeValue = "ns=1;i=1001"}
                },
            });

            variable.Add(new UAVariable()
            {
                DataType = "Int16",
                ParentNodeId = "ns=1;i=1001",
                NodeId = "ns=1;i=6006",
                BrowseName = "1:J_Int16",
                AccessLevel = "3",
                DisplayName = "J_Int16",
                References = new[] {
                    new Reference {  ReferenceType = "HasTypeDefinition", ReferenceTypeValue = "i=63"  },
                    new Reference { ReferenceType="HasModellingRule", ReferenceTypeValue="i=78" },
                    new Reference {  ReferenceType = "HasComponent" , IsForward = "false", ReferenceTypeValue = "ns=1;i=1001"}
                },
            });


            variable.Add(new UAVariable()
            {
                DataType = "DateTime",
                ParentNodeId = "ns=1;i=1001",
                NodeId = "ns=1;i=6007",
                BrowseName = "1:J_DateTime",
                AccessLevel = "3",
                DisplayName = "J_DateTime",
                References = new[] {
                    new Reference {  ReferenceType = "HasTypeDefinition", ReferenceTypeValue = "i=63"  },
                    new Reference { ReferenceType="HasModellingRule", ReferenceTypeValue="i=78" },
                    new Reference {  ReferenceType = "HasComponent" , IsForward = "false", ReferenceTypeValue = "ns=1;i=1001"}
                },
            });


            UANodeSet node = new UANodeSet()
            {
                Uri = new[] { new Uri { UriValue = "http://namespaceuri/" } },
                Aliases = Helper.getAlias(),
                UAObjectType = type,
                UAVariable = variable,
            };
            var r = SerializeHelper<UANodeSet>.Serialize1(node);
            return r;
        }
    }
}
