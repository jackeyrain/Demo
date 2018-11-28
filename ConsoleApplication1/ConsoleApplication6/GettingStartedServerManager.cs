using System;
using System.IO;
using System.Reflection;
using System.Text;
using UnifiedAutomation.UaBase;
using UnifiedAutomation.UaServer;

namespace ConsoleApplication3
{
    internal class GettingStartedServerManager : ServerManager
    {
        protected override void OnRootNodeManagerStarted(RootNodeManager nodeManager)
        {
            Lesson1NodeManager lesson1 = new Lesson1NodeManager(this);
            lesson1.Startup();

            base.OnRootNodeManagerStarted(nodeManager);
        }
    }

    internal class Lesson1NodeManager : BaseNodeManager
    {
        public ushort InstanceNamespaceIndex { get; set; }
        public ushort TypeNamespaceIndex { get; set; }
        public Lesson1NodeManager(ServerManager server, params string[] namespaceUris) : base(server, namespaceUris)
        {
        }

        public override void Startup()
        {
            Console.WriteLine("Starting Lesson2NodeManager.");
            // New Code Begin
            // Assign namespaces for type and instance nodes to this NodeManager
            // Get namespace index for the namespaces
            InstanceNamespaceIndex = AddNamespaceUri("http://namespaceuri/");
            TypeNamespaceIndex = AddNamespaceUri("http://namespaceuri/");
            // New Code End
            Console.WriteLine("Loading the Controller Model.");
            var xml = new XmlGeneric().getXml();

            ImportUaNodeset(new FileStream(xml, FileMode.Open));


            // New Code Begin
            // Create a Folder for Controllers
            CreateObjectSettings settings = new CreateObjectSettings()
            {
                ParentNodeId = ObjectIds.ObjectsFolder,
                ReferenceTypeId = ReferenceTypeIds.Organizes,
                RequestedNodeId = new NodeId("Demo", InstanceNamespaceIndex),
                BrowseName = new QualifiedName("Demo", InstanceNamespaceIndex),
                TypeDefinitionId = ObjectTypeIds.FolderType
            };
            var root = CreateObject(Server.DefaultRequestContext, settings);
            // Create an Air Conditioner Controller
            settings = new CreateObjectSettings()
            {
                ParentNodeId = root.NodeId,
                ReferenceTypeId = ReferenceTypeIds.Organizes,
                RequestedNodeId = new NodeId("Jemima", InstanceNamespaceIndex),
                BrowseName = new QualifiedName("Jemima", InstanceNamespaceIndex),
                TypeDefinitionId = new NodeId(1001, TypeNamespaceIndex)
            };
            CreateObject(Server.DefaultRequestContext, settings);

            //var vsettings = new CreateVariableSettings()
            //{
            //    ParentNodeId = new NodeId("Jemima", InstanceNamespaceIndex),
            //    ReferenceTypeId = ReferenceTypeIds.HasComponent,
            //    BrowseName = new QualifiedName("newone", InstanceNamespaceIndex),
            //    DisplayName = new LocalizedText("newone" ),
            //    DataType =  DataTypeIds.Byte,
            //    AccessLevel = 3,
            //};
            //CreateVariable(Server.DefaultRequestContext, vsettings);

            // 这里的node名字一定要是xml加载的节点
            VariableNode variable = SetVariableConfiguration(
                new NodeId("Jemima", InstanceNamespaceIndex),
                new QualifiedName("J_Int32", TypeNamespaceIndex),
                NodeHandleType.ExternalPolled,
                null
                );
            variable.AccessLevel = AccessLevels.CurrentReadOrWrite;

            // SetVariableDefaultValue(variable.NodeId, new QualifiedName(BrowseNames.EURange), new Variant());
        }
    }
}
