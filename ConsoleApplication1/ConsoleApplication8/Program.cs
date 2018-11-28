using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedAutomation.UaBase;
using UnifiedAutomation.UaClient;

namespace ConsoleApplication8
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<ApplicationDescription> servers = new List<ApplicationDescription>();
            Discovery discovery = new Discovery();
            //servers = discovery.FindServers("opc.tcp://localhost:48030");

            List<EndpointDescription> endpoints = new List<EndpointDescription>();
            endpoints = discovery.GetEndpoints("opc.tcp://localhost:48041");
            endpoints.ForEach(o => Console.WriteLine(o.EndpointUrl));

            var session = new Session(ApplicationInstance.Default);
            session.ConnectionStatusUpdate += Session_ConnectionStatusUpdate;
            session.Connect(endpoints[0], session.DefaultRequestSettings);

            List<ReadValueId> nodesToRead = new List<ReadValueId>();
            nodesToRead.Add(new ReadValueId() { NodeId = VariableIds.Server_ServerStatus_State, AttributeId = Attributes.Value });
            nodesToRead.Add(new ReadValueId() { NodeId = VariableIds.Server_ServerStatus_CurrentTime, AttributeId = Attributes.Value });
            var results = session.Read(nodesToRead, 0, TimestampsToReturn.Both, new RequestSettings { OperationTimeout = 10000 });

            Console.WriteLine(session.Cache.GetEnumerationText(VariableIds.Server_ServerStatus_State, results[0]));

            UnifiedAutomation.UaClient.Controls.BrowseDlg dialog = new UnifiedAutomation.UaClient.Controls.BrowseDlg();
            var nodeid = dialog.ShowDialog(session, ObjectIds.ObjectsFolder, ReferenceTypeIds.HierarchicalReferences);
            nodeid = null;
            nodeid = NodeId.Parse("ns=2;s=0:Jemima?Jackey");
            nodesToRead.Clear();
            nodesToRead.Add(new ReadValueId() { NodeId = nodeid, AttributeId = Attributes.DisplayName });
            nodesToRead.Add(new ReadValueId() { NodeId = nodeid, AttributeId = Attributes.Value });
            results = session.Read(nodesToRead, 0, TimestampsToReturn.Both, new RequestSettings() { OperationTimeout = 10000 });
            Console.WriteLine(results[1]);


            List<WriteValue> nodesToWrites = new List<WriteValue>();
            nodesToWrites.Add(new WriteValue() { NodeId = nodeid, AttributeId = Attributes.Value, Value = new DataValue() { Value = "Hello World." } });
            var resultss  = session.Write(nodesToWrites, new RequestSettings() { OperationTimeout = 10000 });
            resultss.ForEach(o => Console.WriteLine(o));

            List<StatusCode> inputArgumentErrors = null;
            List<Variant> outputArguments = null;

            NodeId objectId = NodeId.Parse("ns=2;s=0:Jemima");
            NodeId methodId = NodeId.Parse("ns=2;s=0:Jemima?WriteValue");
            List<Variant> inputArguments = new List<Variant>();
            inputArguments.Add(new Variant("Jackey Say: 'Hello World.'"));
            session.Call(objectId, methodId, inputArguments, out inputArgumentErrors, out outputArguments);


            Console.ReadKey();
        }

        private static void Session_ConnectionStatusUpdate(Session sender, ServerConnectionStatusUpdateEventArgs e)
        {
            switch (e.Status)
            {
                case ServerConnectionStatus.Connected:
                case ServerConnectionStatus.Connecting:
                case ServerConnectionStatus.SessionAutomaticallyRecreated:
                case ServerConnectionStatus.ConnectionErrorClientReconnect:

                    break;
                case ServerConnectionStatus.LicenseExpired:
                    break;
                default:
                    break;
            }
            Console.WriteLine(sender.ConnectionStatus);
        }
    }
}
