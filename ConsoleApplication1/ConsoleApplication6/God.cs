using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifiedAutomation.UaBase;
using UnifiedAutomation.UaServer;

namespace ConsoleApplication3
{
    public class God : ServerManager
    {
        protected override void OnRootNodeManagerStarted(RootNodeManager nodeManager)
        {
            GodNodManager god = new GodNodManager(this);
            god.Startup();

            base.OnRootNodeManagerStarted(nodeManager);
        }
    }
    public class GodNodManager : BaseNodeManager
    {
        public ushort InstanceNamespaceIndex { get; set; }
        public ushort TypeNamespaceIndex { get; set; }
        public GodNodManager(ServerManager server, params string[] namespaceUris) : base(server, namespaceUris)
        {
            InstanceNamespaceIndex = AddNamespaceUri("http://namespaceuri/");
            TypeNamespaceIndex = AddNamespaceUri("http://namespaceuri/");

            Load();
        }

        private object m_lock = new object();
        private byte[] m_registers;
        private int m_position;
        private Dictionary<int, BlockConfiguration> m_blocks = new Dictionary<int, BlockConfiguration>();
        private Configuration m_configuration;

        private void Load()
        {
            foreach (string resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (resourceName.EndsWith(".SystemConfiguration.xml"))
                {
                    using (Stream istrm = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                        m_configuration = (Configuration)serializer.Deserialize(istrm);
                    }
                }
            }

            if (m_configuration.Controllers != null)
            {
                for (int ii = 0; ii < m_configuration.Controllers.Length; ii++)
                {
                    ControllerConfiguration controller = m_configuration.Controllers[ii];

                    int blockAddress = m_position;
                    int offset = m_position - blockAddress;

                    BlockConfiguration data = new BlockConfiguration()
                    {
                        Address = blockAddress,
                        Name = controller.Name,
                        Type = controller.Type,
                        Properties = new List<BlockProperty>()
                    };

                    if (controller.Properties != null)
                    {
                        for (int jj = 0; jj < controller.Properties.Length; jj++)
                        {
                            ControllerProperty property = controller.Properties[jj];
                            NodeId dataTypeId = NodeId.Parse(property.DataType);
                            string value = property.Value;
                            Range range = null;

                            if (!String.IsNullOrEmpty(property.Range))
                            {
                                try
                                {
                                    NumericRange nr = NumericRange.Parse(property.Range);
                                    range = new Range() { High = nr.End, Low = nr.Begin };
                                }
                                catch (Exception)
                                {
                                    range = null;
                                }
                            }

                            data.Properties.Add(new BlockProperty()
                            {
                                Offset = offset,
                                Name = controller.Properties[jj].Name,
                                DataType = dataTypeId,
                                Writeable = controller.Properties[jj].Writeable,
                                Range = range
                            });

                            switch ((uint)dataTypeId.Identifier)
                            {
                                case DataTypes.Int32:
                                    {
                                        //Write(blockAddress, offset, (int)TypeUtils.Cast(value, BuiltInType.Int32));
                                        offset += 4;
                                        break;
                                    }

                                case DataTypes.Double:
                                    {
                                        // Write(blockAddress, offset, (double)TypeUtils.Cast(value, BuiltInType.Double));
                                        offset += 4;
                                        break;
                                    }
                            }
                        }
                    }

                    m_position += offset;
                    m_blocks[blockAddress] = data;
                }
            }
        }
        public override void Startup()
        {

            try
            {
                Console.WriteLine("Starting Lesson3bNodeManager.");

                base.Startup();

                // save the namespaces used by this node manager.
                AddNamespaceUri("http://yourcompany.com/lesson03b/");

                // initialize the underlying system.

                var nodeLiset = new[] { m_blocks[0] };

                // create root folder.
                NodeId rootId = ParsedNodeId.Construct(0, "3b", DefaultNamespaceIndex);

                CreateObjectSettings settings = new CreateObjectSettings()
                {
                    ParentNodeId = ObjectIds.ObjectsFolder,
                    ReferenceTypeId = ReferenceTypeIds.Organizes,
                    RequestedNodeId = rootId,
                    BrowseName = new QualifiedName("3b", DefaultNamespaceIndex),
                    TypeDefinitionId = ObjectTypeIds.FolderType
                };

                CreateObject(Server.DefaultRequestContext, settings);

                // add controllers.
                foreach (BlockConfiguration block in nodeLiset)
                {
                    NodeId controllerId = ParsedNodeId.Construct(0, block.Name, DefaultNamespaceIndex);

                    // create object.
                    settings = new CreateObjectSettings()
                    {
                        ParentNodeId = rootId,
                        ReferenceTypeId = ReferenceTypeIds.Organizes,
                        RequestedNodeId = controllerId,
                        BrowseName = new QualifiedName(block.Name, DefaultNamespaceIndex),
                        TypeDefinitionId = ObjectTypeIds.BaseObjectType
                    };

                    CreateObject(Server.DefaultRequestContext, settings);

                    foreach (BlockProperty property in block.Properties)
                    {
                        NodeId variableId = ParsedNodeId.Construct(0, block.Name, DefaultNamespaceIndex, property.Name);

                        // configure the variable so the SDK will automatically poll the system when it is subscribed.
                        // this node manager needs to implement the read/write requests.
                        CreateVariableSettings settings2 = new CreateVariableSettings()
                        {
                            ParentNodeId = controllerId,
                            ReferenceTypeId = ReferenceTypeIds.HasComponent,
                            RequestedNodeId = variableId,
                            BrowseName = new QualifiedName(property.Name, DefaultNamespaceIndex),
                            TypeDefinitionId = (property.Range != null) ? VariableTypeIds.AnalogItemType : VariableTypeIds.DataItemType,
                            DataType = property.DataType,
                            ValueRank = ValueRanks.Scalar,
                            AccessLevel = (property.Writeable) ? AccessLevels.CurrentReadOrWrite : AccessLevels.CurrentRead,
                            ValueType = NodeHandleType.ExternalPolled,
                            ValueData = new SystemAddress() { Address = block.Address, Offset = property.Offset }
                        };

                        CreateVariable(Server.DefaultRequestContext, settings2);

                        // check if an EURange property needs to be created.
                        // the value is stored internally by the SDK so no additional work to support read is required.
                        //if (property.Range != null)
                        //{
                        //    NodeId propertyId = ParsedNodeId.Construct(0, block.Name, DefaultNamespaceIndex, property.Name, BrowseNames.EURange);

                        //    settings2 = new CreateVariableSettings()
                        //    {
                        //        ParentNodeId = variableId,
                        //        ReferenceTypeId = ReferenceTypeIds.HasProperty,
                        //        RequestedNodeId = propertyId,
                        //        BrowseName = new QualifiedName(BrowseNames.EURange),
                        //        TypeDefinitionId = VariableTypeIds.PropertyType,
                        //        DataType = DataTypeIds.Range,
                        //        ValueRank = ValueRanks.Scalar,
                        //        AccessLevel = AccessLevels.CurrentRead,
                        //        Value = new Variant(property.Range)
                        //    };

                        //    CreateVariable(Server.DefaultRequestContext, settings2);
                        //}
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to start Lesson3bNodeManager. " + e.Message);
            }

        }
    }

    [XmlRoot(ElementName = "UnderlyingSystem.Configuration", Namespace = "http://yourcompany.com/underlyingsystem")]
    public class Configuration
    {
        [XmlElement(Order = 1)]
        public ControllerConfiguration[] Controllers;
    }
    [XmlType(TypeName = "UnderlyingSystem.ControllerConfiguration", Namespace = "http://yourcompany.com/underlyingsystem")]
    public class ControllerConfiguration
    {
        [XmlElement(Order = 1)]
        public string Name { get; set; }

        [XmlElement(Order = 2)]
        public int Type { get; set; }

        [XmlElement(Order = 3)]
        public ControllerProperty[] Properties;
    }
    [XmlType(TypeName = "UnderlyingSystem.ControllerProperty", Namespace = "http://yourcompany.com/underlyingsystem")]
    public class ControllerProperty
    {
        [XmlElement(Order = 1)]
        public string Name { get; set; }

        [XmlElement(Order = 2)]
        public string DataType { get; set; }

        [XmlElement(Order = 3)]
        public string Value { get; set; }

        [XmlElement(Order = 4)]
        public bool Writeable { get; set; }

        [XmlElement(Order = 5)]
        public string Range { get; set; }
    }
    public class SystemAddress
    {
        public int Address;
        public int Offset;
    }

    public class BlockConfiguration
    {
        public int Address;
        public string Name;
        public int Type;
        public List<BlockProperty> Properties;
    }
    public class BlockProperty
    {
        public int Offset;
        public string Name;
        public NodeId DataType;
        public bool Writeable;
        public Range Range;
    }

}