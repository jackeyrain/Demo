/******************************************************************************
** Copyright (c) 2006-2018 Unified Automation GmbH All rights reserved.
**
** Software License Agreement ("SLA") Version 2.7
**
** Unless explicitly acquired and licensed from Licensor under another
** license, the contents of this file are subject to the Software License
** Agreement ("SLA") Version 2.7, or subsequent versions
** as allowed by the SLA, and You may not copy or use this file in either
** source code or executable form, except in compliance with the terms and
** conditions of the SLA.
**
** All software distributed under the SLA is provided strictly on an
** "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED,
** AND LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
** LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
** PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the SLA for specific
** language governing rights and limitations under the SLA.
**
** Project: .NET based OPC UA Client Server SDK
**
** Description: OPC Unified Architecture Software Development Kit.
**
** The complete license agreement can be found here:
** http://unifiedautomation.com/License/SLA/2.7/
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;
using UnifiedAutomation.UaBase;
using UnifiedAutomation.UaServer;

namespace YourCompany.GettingStarted
{
    internal class Lesson3bNodeManager : BaseNodeManager
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Lesson3bNodeManager(ServerManager server) : base(server)
        {
            m_system = new UnderlyingSystem();
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TBD
            }
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Called when the node manager is started.
        /// </summary>
        public override void Startup()
        {
            try
            {
                Console.WriteLine("Starting Lesson3bNodeManager.");

                base.Startup();

                 // save the namespaces used by this node manager.
                AddNamespaceUri("http://yourcompany.com/lesson03b/");

                // initialize the underlying system.
                m_system.Initialize();

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
                foreach (BlockConfiguration block in m_system.GetBlocks())
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

        /// <summary>
        /// Called when the node manager is stopped.
        /// </summary>
        public override void Shutdown()
        {
            try
            {
                Console.WriteLine("Stopping Lesson3bNodeManager.");

                // TBD 

                base.Shutdown();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to stop Lesson3bNodeManager. " + e.Message);
            }
        }

        /// <summary>
        /// Reads the attributes.
        /// </summary>
        protected override void Read(
            RequestContext context,
            TransactionHandle transaction,
            IList<NodeAttributeOperationHandle> operationHandles,
            IList<ReadValueId> settings)
        {
            for (int ii = 0; ii < operationHandles.Count; ii++)
            {
                DataValue dv = null;

                // the data passed to CreateVariable is returned as the UserData in the handle.
                SystemAddress address = operationHandles[ii].NodeHandle.UserData as SystemAddress;

                if (address != null)
                {
                    // read the data from the underlying system.
                    object value = m_system.Read(address.Address, address.Offset);

                    if (value != null)
                    {
                        dv = new DataValue(new Variant(value, null), DateTime.UtcNow);

                        // apply any index range or encoding.
                        if (!String.IsNullOrEmpty(settings[ii].IndexRange) || !QualifiedName.IsNull(settings[ii].DataEncoding))
                        {
                            dv = ApplyIndexRangeAndEncoding(
                                operationHandles[ii].NodeHandle,
                                dv,
                                settings[ii].IndexRange,
                                settings[ii].DataEncoding);
                        }
                    }
                }

                // set an error if not found.
                if (dv == null)
                {
                    dv = new DataValue(new StatusCode(StatusCodes.BadNodeIdUnknown));
                }

                // return the data to the caller.
                ((ReadCompleteEventHandler)transaction.Callback)(
                    operationHandles[ii],
                    transaction.CallbackData,
                    dv,
                    false);
            }
        }

        /// <summary>
        /// Write the attributes
        /// </summary>
        protected override void Write(
            RequestContext context,
            TransactionHandle transaction,
            IList<NodeAttributeOperationHandle> operationHandles,
            IList<WriteValue> settings)
        {
            for (int ii = 0; ii < operationHandles.Count; ii++)
            {
                StatusCode error = StatusCodes.Good;

                // the data passed to CreateVariable is returned as the UserData in the handle.
                SystemAddress address = operationHandles[ii].NodeHandle.UserData as SystemAddress;

                if (address != null)
                {
                    if (!String.IsNullOrEmpty(settings[ii].IndexRange))
                    {
                        error = StatusCodes.BadIndexRangeInvalid;
                    }
                    else if (!m_system.Write(address.Address, address.Offset, settings[ii].Value.Value))
                    {
                        error = StatusCodes.BadUserAccessDenied;
                    }
                }
                else
                {
                    error = StatusCodes.BadNodeIdUnknown;
                }

                // return the data to the caller.
                ((WriteCompleteEventHandler)transaction.Callback)(
                    operationHandles[ii],
                    transaction.CallbackData,
                    error,
                    false);
            }
        }
        #endregion

        #region SystemAddress Class
        private class SystemAddress
        {
            public int Address;
            public int Offset;
        }
        #endregion

        #region Private Methods
        #endregion

        #region Private Fields
        private UnderlyingSystem m_system;
        #endregion
    }
}
