using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedAutomation.UaBase;
using UnifiedAutomation.UaSchema;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ApplicationLicenseManager.AddProcessLicenses(System.Reflection.Assembly.GetExecutingAssembly(), "License.lic");

                //GettingStartedServerManager server = new GettingStartedServerManager();
                
                God server = new God();

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
