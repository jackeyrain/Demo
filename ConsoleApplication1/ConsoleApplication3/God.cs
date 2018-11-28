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

        ExclusiveLevelAlarmModel alarm = new ExclusiveLevelAlarmModel();

        public GodNodManager(ServerManager server, params string[] namespaceUris) : base(server, namespaceUris)
        {
            InstanceNamespaceIndex = AddNamespaceUri("http://namespaceuri/");
            TypeNamespaceIndex = AddNamespaceUri("http://namespaceuri/");

            // Load();
            NodeId alarmId = ParsedNodeId.Construct(1, "Jemima", DefaultNamespaceIndex, "Jemima Alarm");
            alarm.NodeId = alarmId;
            alarm.EventType = ObjectTypeIds.ExclusiveLimitAlarmType;
            alarm.SourceNode = new NodeId("Root", DefaultNamespaceIndex);
            alarm.SourceName = "Jemima";
            alarm.Message = "Jemima say's Hello World.";
            alarm.Severity = (ushort)EventSeverity.Low;
            alarm.ConditionName = "Jemima";
            alarm.ConditionClassId = ObjectTypeIds.ProcessConditionClassType;
            alarm.ConditionClassName = BrowseNames.ProcessConditionClassType;
            alarm.Retain = false;
            alarm.EnabledState.Value = ConditionStateNames.Enabled;
            alarm.EnabledState.Id = true;
            alarm.AckedState.Value = ConditionStateNames.Acknowledged;
            alarm.AckedState.Id = true;
            alarm.ConfirmedState = new TwoStateVariableModel();
            alarm.ConfirmedState.Value = ConditionStateNames.Confirmed;
            alarm.ConfirmedState.Id = true;
            alarm.ActiveState.Value = ConditionStateNames.Inactive;
            alarm.ActiveState.Id = false;
            alarm.SuppressedOrShelved = false;
            alarm.HighLimit = 35;
            alarm.LowLimit = 15;
            alarm.InputNode = new NodeId("Jackey", DefaultNamespaceIndex);
            alarm.UserData = new SystemAddress() { Address = 10, Offset = 1000 };
        }

        private object m_lock = new object();
        public override void Startup()
        {

            try
            {
                Console.WriteLine("Starting Lesson3bNodeManager.");

                base.Startup();

                // save the namespaces used by this node manager.
                AddNamespaceUri("http://yourcompany.com/lesson03b/");

                // initialize the underlying system.

                // create root folder.
                NodeId rootId = ParsedNodeId.Construct(0, "Root", DefaultNamespaceIndex);

                CreateObjectSettings settings = new CreateObjectSettings()
                {
                    ParentNodeId = ObjectIds.ObjectsFolder,
                    ReferenceTypeId = ReferenceTypeIds.Organizes,
                    RequestedNodeId = rootId,
                    BrowseName = new QualifiedName("Root", DefaultNamespaceIndex),
                    TypeDefinitionId = ObjectTypeIds.FolderType,

                    NotifierParent = ObjectIds.Server,
                };

                CreateObject(Server.DefaultRequestContext, settings);

                // add controllers.

                NodeId controllerId = ParsedNodeId.Construct(0, "Jemima", DefaultNamespaceIndex);

                // create object.
                settings = new CreateObjectSettings()
                {
                    ParentNodeId = rootId,
                    ReferenceTypeId = ReferenceTypeIds.Organizes,
                    RequestedNodeId = controllerId,
                    BrowseName = new QualifiedName("Jemima", DefaultNamespaceIndex),
                    TypeDefinitionId = ObjectTypeIds.BaseObjectType,

                    NotifierParent = new NodeId("Root", DefaultNamespaceIndex),
                };

                CreateObject(Server.DefaultRequestContext, settings);


                NodeId variableId = ParsedNodeId.Construct(0, "Jemima", DefaultNamespaceIndex, "Jackey");

                // configure the variable so the SDK will automatically poll the system when it is subscribed.
                // this node manager needs to implement the read/write requests.
                CreateVariableSettings settings2 = new CreateVariableSettings()
                {
                    ParentNodeId = controllerId,
                    ReferenceTypeId = ReferenceTypeIds.HasComponent,
                    RequestedNodeId = variableId,
                    BrowseName = new QualifiedName("Jackey", DefaultNamespaceIndex),
                    TypeDefinitionId = (true) ? VariableTypeIds.AnalogItemType : VariableTypeIds.DataItemType,
                    DataType = NodeId.Parse("i=12"),
                    ValueRank = ValueRanks.Scalar,
                    AccessLevel = AccessLevels.CurrentReadOrWrite,
                    ValueType = NodeHandleType.ExternalPolled,
                    ValueData = new SystemAddress() { Address = 10, Offset = 100 },
                    // Value = "Hello World.",
                };

                CreateVariable(Server.DefaultRequestContext, settings2);

                NodeId methodId = ParsedNodeId.Construct(0, "Jemima", DefaultNamespaceIndex, "WriteValue");

                var settings3 = new CreateMethodSettings()
                {
                    ParentNodeId = controllerId,
                    ReferenceTypeId = ReferenceTypeIds.HasComponent,
                    RequestedNodeId = methodId,
                    BrowseName = new QualifiedName("WriteValue", DefaultNamespaceIndex),
                    NodeData = new SystemFunction() { Address = "Address", Function = "WriteValue" },

                    InputArguments = new[]
                    {
                        new Argument() { Name="Value_String", DataType =  DataTypeIds.String, ValueRank =  ValueRanks.Any },
                    }
                };
                CreateMethod(Server.DefaultRequestContext, settings3);

            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to start Lesson3bNodeManager. " + e.Message);
            }

        }

        private string currentData = "Hello";

        protected override void Read(
           RequestContext context,
           TransactionHandle transaction,
           IList<NodeAttributeOperationHandle> operationHandles,
           IList<ReadValueId> settings)
        {
            DataValue dv = new DataValue(new Variant(currentData, null), DateTime.UtcNow);
            foreach (var handle in operationHandles)
            {
                SystemAddress address = handle.NodeHandle.UserData as SystemAddress;

                ((ReadCompleteEventHandler)transaction.Callback)(
                       handle,
                       transaction.CallbackData,
                       dv,
                       false);
            }

        }

        protected override void Write(
           RequestContext context,
           TransactionHandle transaction,
           IList<NodeAttributeOperationHandle> operationHandles,
           IList<WriteValue> settings)
        {

            StatusCode error = StatusCodes.Good;

            // the data passed to CreateVariable is returned as the UserData in the handle.
            SystemAddress address = operationHandles[0].NodeHandle.UserData as SystemAddress;

            currentData = settings[0].Value.Value.ToString();

            // error = StatusCodes.BadNodeIdUnknown;

            // return the data to the caller.
            ((WriteCompleteEventHandler)transaction.Callback)(
                operationHandles[0],
                transaction.CallbackData,
                error,
                false);
        }

        protected override CallMethodEventHandler GetMethodDispatcher(RequestContext context, MethodHandle methodHandle)
        {
            if (methodHandle.MethodData is SystemFunction)
            {
                return DispatchControllerMethod;
            }
            return null;
        }
        private StatusCode DispatchControllerMethod(
            RequestContext context,
            MethodHandle methodHandle,
            IList<Variant> inputArguments,
            List<StatusCode> inputArgumentResults,
            List<Variant> outputArguments)
        {
            SystemFunction data = methodHandle.MethodData as SystemFunction;

            if (data != null)
            {
                this.currentData = "CALL " + data.Function + inputArguments[0].ToString();


                GenericEvent e = new GenericEvent(Server.FilterManager);
                e.Initialize(
                    null,
                    ObjectTypeIds.SystemEventType,
                    ParsedNodeId.Construct(0, "Jemima", DefaultNamespaceIndex),
                    "Jemima",
                    EventSeverity.Medium,
                    "A Jemima is CALL."
                    );
                //e.Set(e.ToPath(new QualifiedName("Jackey", DefaultNamespaceIndex)), currentData);
                ReportEvent(e.SourceNode, e);

                return StatusCodes.Good;
            }

            return StatusCodes.BadNotImplemented;
        }

        public override void ConditionRefresh(RequestContext context, NodeId notifierId, MonitoredItemHandle itemHandle, EventNotificationEventHandler callback)
        {
            base.ConditionRefresh(context, notifierId, itemHandle, callback);

            GenericEvent e = alarm.CreateEvent(Server.FilterManager, true);

            ReportEvent(e.SourceNode, e);
        }

        public class SystemAddress
        {
            public int Address;
            public int Offset;
        }

        public class SystemFunction
        {
            public string Address { get; set; }
            public string Function { get; set; }
        }
    }
}