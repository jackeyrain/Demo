using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnifiedAutomation.UaBase;
using UnifiedAutomation.UaServer;

namespace ConsoleApplication1
{
    internal class GettingStartedServerManager : ServerManager
    {
        protected override void OnRootNodeManagerStarted(RootNodeManager nodeManager)
        {
            Lesson1NodeManager lesson1 = new Lesson1NodeManager(this);
            lesson1.Startup();

            //Lesson2NodeManager lesson2 = new Lesson2NodeManager(this);
            //lesson2.Startup();

            base.OnRootNodeManagerStarted(nodeManager);
        }
    }

    internal class Lesson1NodeManager : BaseNodeManager
    {
        public ushort InstanceNamespaceIndex { get; set; }
        public ushort TypeNamespaceIndex { get; set; }
        private UnderlyingSystem m_system;
        public Lesson1NodeManager(ServerManager server, params string[] namespaceUris) : base(server, namespaceUris)
        {
            m_system = new UnderlyingSystem();
        }

        public override void Startup()
        {

            // m_system.Initialize();

            Console.WriteLine("Starting Lesson1NodeManager.");

            AddNamespaceUri("http://yourorganisation.com/lesson01/");

            InstanceNamespaceIndex = AddNamespaceUri("http://yourorganisation.com/lesson01/");
            TypeNamespaceIndex = AddNamespaceUri("http://yourorganisation.org/DemoUA/");

            Console.WriteLine("Loading the Controller Model.");
            // ImportUaNodeset(Assembly.GetEntryAssembly(), "buildingautomation.xml");
            //ImportUaNodeset(Assembly.GetEntryAssembly(), "demoua.xml");

            //NodeId _noid;
            //AddNode(null, new AddNodeSettings()
            //{
            //    ParentNodeId = new NodeId("Controllers", InstanceNamespaceIndex),
            //    ReferenceTypeId = ReferenceTypeIds.Organizes,
            //    RequestedNodeId = new NodeId("AirConditioner1", InstanceNamespaceIndex),
            //    BrowseName = new QualifiedName("AirConditioner1", InstanceNamespaceIndex),
            //    // TypeDefinitionId = new NodeId(yourorganisation.BA.ObjectTypes.AirConditionerControllerType, TypeNamespaceIndex)
            //    TypeDefinitionId = new NodeId(DemoOrganization.BA.ObjectTypes.DemoType, TypeNamespaceIndex)
            //}, out _noid);

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
            CreateObject(Server.DefaultRequestContext, settings);
            // Create an Air Conditioner Controller
            settings = new CreateObjectSettings()
            {
                ParentNodeId = new NodeId("Demo", InstanceNamespaceIndex),
                ReferenceTypeId = ReferenceTypeIds.Organizes,
                RequestedNodeId = new NodeId("Jemima", InstanceNamespaceIndex),
                BrowseName = new QualifiedName("Jemima", InstanceNamespaceIndex),
                TypeDefinitionId = ObjectTypeIds.FolderType,

                // TypeDefinitionId = new NodeId(yourorganisation.BA.ObjectTypes.AirConditionerControllerType, TypeNamespaceIndex)
                // TypeDefinitionId = new NodeId(DemoOrganization.BA.ObjectTypes.DemoType, TypeNamespaceIndex)
                //TypeDefinitionId = new NodeId(1002, TypeNamespaceIndex),
            };
            var r = CreateObject(Server.DefaultRequestContext, settings);

            var variableSettings = new CreateVariableSettings()
            {
                BrowseName = new QualifiedName("Jay", InstanceNamespaceIndex),
                DisplayName = new LocalizedText("Jay"),
                RequestedNodeId = new NodeId("Jay", InstanceNamespaceIndex),
                ParentNodeId = r.NodeId,
                ParentAsOwner = true,
                ReferenceTypeId = ReferenceTypeIds.HasComponent,
                TypeDefinitionId = UnifiedAutomation.UaBase.VariableTypeIds.DataItemType,
                DataType = DataTypeIds.String,
                AccessLevel = AccessLevels.CurrentReadOrWrite,
                ModellingRuleId = UnifiedAutomation.UaBase.ObjectIds.ModellingRule_Mandatory,
                Value = "Hello World.",
            };
            CreateVariable(Server.DefaultRequestContext, variableSettings);
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        protected override void Read(RequestContext context, TransactionHandle transaction, IList<NodeAttributeOperationHandle> operationHandles, IList<ReadValueId> settings)
        {
            base.Read(context, transaction, operationHandles, settings);
        }

        protected override void Write(RequestContext context, TransactionHandle transaction, IList<NodeAttributeOperationHandle> operationHandles, IList<WriteValue> settings)
        {
            base.Write(context, transaction, operationHandles, settings);
        }

        private void CreateGenericDataTypeVariable(string name, GenericEncodeableObject eo, NodeId parenterID)
        {
            CreateVariableSettings settings = new CreateVariableSettings()
            {
                RequestedNodeId = new NodeId(name, DefaultNamespaceIndex),
                BrowseName = new QualifiedName(name, DefaultNamespaceIndex),
                Description = new LocalizedText("Variable with a structured DataType that is created using the SchameBuilder"),
                DataType = eo.TypeId.ToNodeId(Server.NamespaceUris),
                ReferenceTypeId = ReferenceTypeIds.Organizes,
                ParentNodeId = parenterID,
                ValueRank = -1,
                Value = eo
            };
            CreateVariable(Server.DefaultRequestContext, settings);
        }
    }

    internal class Lesson2NodeManager : BaseNodeManager
    {
        public Lesson2NodeManager(ServerManager server, params string[] namespaceUris) : base(server, namespaceUris)
        {
        }

        public override void Startup()
        {
            Console.WriteLine("Starting Lesson2NodeManager.");
            AddNamespaceUri("http://yourorganisation.com/lesson02/");
            // New Code Begin
            Console.WriteLine("Loading the Controller Model.");
            ImportUaNodeset(Assembly.GetEntryAssembly(), "buildingautomation.xml");
        }
    }
}
