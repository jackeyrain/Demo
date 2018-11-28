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
using UnifiedAutomation.UaBase;
using UnifiedAutomation.UaSchema;
using UnifiedAutomation.UaServer;

namespace YourCompany.GettingStarted
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ApplicationLicenseManager.AddProcessLicenses(System.Reflection.Assembly.GetExecutingAssembly(), "License.lic");
                GettingStartedServerManager server = new GettingStartedServerManager();

                ConfigureOpcUaApplicatiopnFromCode();
                ApplicationInstance.Default.Start(server, null, server);

                Console.WriteLine("Press <Enter> to exit the program.");
                Console.ReadLine();

                server.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static void ConfigureOpcUaApplicatiopnFromCode()
        {
            SecuredApplication application = new SecuredApplication();

            application.ApplicationName = "UnifiedAutomation GettingStartedServer";
            application.ApplicationUri = "urn:localhost:UnifiedAutomation:GettingStartedServer";
            application.ApplicationType = UnifiedAutomation.UaSchema.ApplicationType.Server_0;
            application.ProductName = "UnifiedAutomation GettingStartedServer";

            application.ApplicationCertificate = new CertificateIdentifier();
            application.ApplicationCertificate.StoreType = "Directory";
            application.ApplicationCertificate.StorePath = @"Config\own";
            application.ApplicationCertificate.SubjectName = "CN=GettingStartedServer/O=UnifiedAutomation/DC=localhost";

            application.TrustedCertificateStore = new CertificateStoreIdentifier();
            application.TrustedCertificateStore.StoreType = "Directory";
            application.TrustedCertificateStore.StorePath = @"Config\trusted";

            application.IssuerCertificateStore = new UnifiedAutomation.UaSchema.CertificateStoreIdentifier();
            application.IssuerCertificateStore.StoreType = "Directory";
            application.IssuerCertificateStore.StorePath = @"Config\issuers";

            application.RejectedCertificatesStore = new UnifiedAutomation.UaSchema.CertificateStoreIdentifier();
            application.RejectedCertificatesStore.StoreType = "Directory";
            application.RejectedCertificatesStore.StorePath = @"Config\rejected";

            application.BaseAddresses = new ListOfBaseAddresses();
            application.BaseAddresses.Add("opc.tcp://localhost:48041");

            application.SecurityProfiles = new ListOfSecurityProfiles();
            application.SecurityProfiles.Add(new SecurityProfile() { ProfileUri = SecurityProfiles.Basic256, Enabled = true });
            application.SecurityProfiles.Add(new SecurityProfile() { ProfileUri = SecurityProfiles.Basic128Rsa15, Enabled = true });
            application.SecurityProfiles.Add(new SecurityProfile() { ProfileUri = SecurityProfiles.None, Enabled = true });

            TraceSettings trace = new TraceSettings();

            trace.MasterTraceEnabled = true;
            trace.DefaultTraceLevel = UnifiedAutomation.UaSchema.TraceLevel.Info;
            trace.TraceFile = "log.txt";
            trace.MaxLogFileBackups = 3;

            trace.ModuleSettings = new ModuleTraceSettings[]
            {
                new ModuleTraceSettings() {ModuleName = "UnifiedAutomation.Stack", TraceEnabled = true },
                new ModuleTraceSettings() {ModuleName = "UnifiedAutomation.Server", TraceEnabled =true },
            };
            application.Set<TraceSettings>(trace);

            InstallationSettings installation = new InstallationSettings();

            installation.GenerateCertificateIfNone = true;
            installation.DeleteCertificateOnUninstall = true;

            application.Set<InstallationSettings>(installation);

            ApplicationInstance.Default.SetApplicationSettings(application);
        }
    }
}
