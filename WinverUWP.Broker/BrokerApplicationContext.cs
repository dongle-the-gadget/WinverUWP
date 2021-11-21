using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using WinverUWP.InterCommunication;
using System.Text.Json;
using System.Management;
using Microsoft.Win32;
using System.Diagnostics;
using Windows.Foundation.Collections;

namespace WinverUWP.Broker
{
    internal class BrokerApplicationContext : ApplicationContext
    {
        AppServiceConnection connection = new();
        public BrokerApplicationContext()
        {
            Run();
        }

        public async void Run()
        {
            connection.AppServiceName = "WinverUWPAppService";
            connection.PackageFamilyName = Package.Current.Id.FamilyName;
            connection.RequestReceived += Connection_RequestReceived;
            var status = await connection.OpenAsync();

            if (status != AppServiceConnectionStatus.Success)
            {
                Application.Exit();
                return;
            }

            while (true)
            {
                await SendOSInfo();
                await Task.Delay(1000);
            }
        }

        private async Task SendOSInfo()
        {
            string edition = "", version, build, install = "", experience, owner = "", org = "";
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT Caption, InstallDate, RegisteredUser, Organization FROM Win32_OperatingSystem");
            foreach (var mgrObj in managementObjectSearcher.Get())
            {
                edition = ((string)mgrObj["Caption"]).Replace("Microsoft ", "");
                owner = (string)mgrObj["RegisteredUser"];
                org = (string)mgrObj["Organization"];
                install = ManagementDateTimeConverter.ToDateTime((string)mgrObj["InstallDate"]).ToShortDateString();
            }

            build = Environment.OSVersion.Version.Build.ToString();

#pragma warning disable CS8605 // Unboxing a possibly null value.
            int revision = (int)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "UBR", 0);
#pragma warning restore CS8605 // Unboxing a possibly null value.
            if (revision > 0)
                build += $".{revision}";

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            version = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "DisplayVersion", "Unknown");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            Process process = new Process();
            process.StartInfo.FileName = "powershell.exe";
            process.StartInfo.Arguments = "-Command \"(Get-AppxPackage MicrosoftWindows.Client.CBS).Version\"";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.WaitForExit();
            experience = process.StandardOutput.ReadToEnd().Replace(Environment.NewLine, "");

            OSInfoData OSInfo = new(
                edition, 
                version, 
                install, 
                build, 
                experience,
                owner,
                org);
            InterCommunicationMessage msg = new()
            {
                Type = InterCommunicationType.OSInfo,
                OSInfo = OSInfo
            };
            ValueSet valueSet = new()
            {
                { InterCommunicationConstants.MessageKey, JsonSerializer.Serialize(msg) }
            };

            await connection.SendMessageAsync(valueSet);
        }

        private void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var deferral = args.GetDeferral();
            if (args.Request.Message.ContainsKey(InterCommunicationConstants.MessageKey))
            {
#pragma warning disable CS8604 // Possible null reference argument.
                var msg = JsonSerializer.Deserialize<InterCommunicationMessage>(args.Request.Message[InterCommunicationConstants.MessageKey] as string);
#pragma warning restore CS8604 // Possible null reference argument.
                if (msg != null)
                {
                    switch (msg.Type)
                    {
                        case InterCommunicationType.Exit:
                            Application.Exit();
                            return;
                    }
                }
            }
            deferral.Complete();
        }
    }
}
