using RegistryRT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WinverUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Registry registry = new Registry();
        private string OSName = "";
        private ResourceLoader resourceLoader;

        public MainPage()
        {
            this.InitializeComponent();

            resourceLoader = ResourceLoader.GetForCurrentView();

            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            UpdateTitleBarLayout(coreTitleBar);
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;

            Window.Current.SetTitleBar(TitleBar);

            Window.Current.Activated += Current_Activated;
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            SolidColorBrush defaultForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];
            SolidColorBrush inactiveForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorDisabledBrush"];

            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                AppTitle.Foreground = inactiveForegroundBrush;
            }
            else
            {
                AppTitle.Foreground = defaultForegroundBrush;
            }
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            // Update title bar control size as needed to account for system size changes.
            TitleBar.Height = coreTitleBar.Height;

            // Ensure the custom title bar does not overlap window caption controls
            Thickness currMargin = TitleBar.Margin;
            TitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
        }

        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            DataPackage package = new DataPackage();
            string targetText = "";
            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong build = (version & 0x00000000FFFF0000L) >> 16;
            if (build >= 19041)
            {
                string[] labels = new[]
                {
                    $"{resourceLoader.GetString("Edition/Text")}",
                    $"{resourceLoader.GetString("Version/Text")}",
                    $"{resourceLoader.GetString("InstalledOn/Text")}",
                    $"{resourceLoader.GetString("OSBuild/Text")}",
                    $"{resourceLoader.GetString("Experience/Text")}",
                };

                int length = labels.Max(f => f.Length) + 4;

                labels = labels.Select(f => string.Format($"{{0,-{length}}}", f)).ToArray();

                targetText =
                    $@"{labels[0]}{Edition.Text}
{labels[1]}{Version.Text}
{labels[2]}{InstalledOn.Text}
{labels[3]}{Build.Text}
{labels[4]}{Experience.Text}";
            }
            else
            {
                string[] labels = new[]
                {
                    $"{resourceLoader.GetString("Edition/Text")}",
                    $"{resourceLoader.GetString("Version/Text")}",
                    $"{resourceLoader.GetString("InstalledOn/Text")}",
                    $"{resourceLoader.GetString("OSBuild/Text")}"
                };

                int length = labels.Max(f => f.Length) + 4;

                labels = labels.Select(f => string.Format($"{{0,-{length}}}", f)).ToArray();

                targetText =
                    $@"{labels[0]}{Edition.Text}
{labels[1]}{Version.Text}
{labels[2]}{InstalledOn.Text}
{labels[3]}{Build.Text}";
            }

            package.SetText(targetText);

            Clipboard.SetContent(package);
            CopyButton.Content = resourceLoader.GetString("Copied");
            await Task.Delay(1000);
            CopyButton.Content = resourceLoader.GetString("CopyButton/Content");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong build = (version & 0x00000000FFFF0000L) >> 16;
            var revision = version & 0x000000000000FFFF;

            OSName = build >= 21996 ? "Windows11Logo" : "Windows10Logo";
            UpdateWindowsBrand();

            ActualThemeChanged += (a, b) =>
            {
                UpdateWindowsBrand();
            };

            Build.Text = build.ToString();

            if (revision != 0)
                Build.Text += $".{revision}";

            registry.InitNTDLLEntryPoints();
            var productName = ReturnValueFromRegistry(RegistryHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName");

            if (build >= 21996)
                productName = productName.Replace("Windows 10", "Windows 11");

            Edition.Text = productName;
            LicensingText.Text = resourceLoader.GetString("Trademark/Text").Replace("Windows", productName);

            var displayVersion = ReturnValueFromRegistry(RegistryHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "DisplayVersion");
            Version.Text = displayVersion;

            InstalledOn.Text = GetWindowsInstallationDateTime().ToShortDateString();

            var ownerName = ReturnValueFromRegistry(RegistryHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "RegisteredOwner");
            OwnerText.Text = ownerName;
            OwnerText.Visibility = string.IsNullOrEmpty(ownerName) ? Visibility.Collapsed : Visibility.Visible;

            var ownerOrg = ReturnValueFromRegistry(RegistryHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "RegisteredOrganization");
            OrgText.Text = ownerOrg;
            OrgText.Visibility = string.IsNullOrEmpty(ownerOrg) ? Visibility.Collapsed : Visibility.Visible;

            if (build < 19041)
            {
                ExperienceLabel.Visibility = Visibility.Collapsed;
                Experience.Visibility = Visibility.Collapsed;
                return;
            }

            string userId = GetUserSid();
            registry.GetSubKeyList(RegistryHive.HKEY_LOCAL_MACHINE, $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\{userId}", out string[] subKeys);
            string cbs = subKeys.First(f => f.StartsWith("MicrosoftWindows.Client.CBS_", StringComparison.CurrentCultureIgnoreCase) && f.EndsWith("_neutral_neutral_cw5n1h2txyewy", StringComparison.CurrentCultureIgnoreCase));
            cbs = cbs.Replace("MicrosoftWindows.Client.CBS_", "", StringComparison.CurrentCultureIgnoreCase).Replace("_neutral_neutral_cw5n1h2txyewy", "", StringComparison.CurrentCultureIgnoreCase);
            Experience.Text = $"{resourceLoader.GetString("ExperiencePack")} {cbs}";
        }

        private void UpdateWindowsBrand()
        {
            BrandImage.Source = new SvgImageSource(
                new Uri(
                    "ms-appx:///Assets/"
                    + OSName
                    + "-"
                    + (ActualTheme == ElementTheme.Dark ? "light" : "dark")
                    + ".svg"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private string ReturnValueFromRegistry(RegistryHive hive, string key, string value)
        {
            var success = registry.QueryValue(hive, key, value, out RegistryType _, out byte[] rawData);
            if (!success)
                return "";
            return Encoding.Unicode.GetString(rawData).Replace("\0", "");
        }

        private DateTime GetWindowsInstallationDateTime()
        {
            bool success = registry.QueryValue(RegistryHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "InstallDate", out RegistryType _, out byte[] buffer);
            if (!success)
                throw new Exception();

            var seconds = BitConverter.ToInt32(buffer, 0);
            DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime installDate = startDate.AddSeconds(seconds);
            return installDate;
        }

        enum TOKEN_INFORMATION_CLASS
        {
            TokenUser = 1,
            TokenAppContainerSid = 31
        }

        [DllImport("api-ms-win-core-processthreads-l1-1-0.dll", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        static extern int OpenProcessToken(System.IntPtr processHandle, int desiredAccess, ref IntPtr tokenHandle);
        const int OpenProcessTokenFail = 0;

        [DllImport("api-ms-win-security-base-l1-1-0.dll", SetLastError = true)]
        static extern bool GetTokenInformation(IntPtr tokenHandle, TOKEN_INFORMATION_CLASS tokenInformationClass, byte[] tokenInformation, int tokenInformationLength, out int returnLength);
        const int GetTokenInformationFail = 0;

        [DllImport("api-ms-win-security-sddl-l1-1-0.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool ConvertSidToStringSidW(IntPtr securityIdentifier, out IntPtr securityIdentifierName);

        [DllImport("api-ms-win-core-heap-l2-1-0.dll")]
        static extern IntPtr LocalFree(IntPtr hMem);

        [StructLayout(LayoutKind.Sequential)]
        struct TokenAppContainerInfo
        {
            public IntPtr psid;
        }

        static int GetTokenInformationLength(IntPtr token, TOKEN_INFORMATION_CLASS tic)
        {
            int lengthNeeded;
            bool success = GetTokenInformation(token, tic, null, 0, out lengthNeeded);
            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                if (error != 122)
                {
                    throw new Win32Exception(error);
                }
            }

            return lengthNeeded;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SidAndAttributes
        {
            public IntPtr Sid;
            public int Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct TokenUser
        {
            public SidAndAttributes User;
        }

        static unsafe string GetUserSid()
        {
            var processHandle = Process.GetCurrentProcess().Handle;
            IntPtr tokenHandle = IntPtr.Zero;
            const int TOKEN_READ = 0x20008;

            if (OpenProcessToken(processHandle, TOKEN_READ, ref tokenHandle) != OpenProcessTokenFail)
            {

                // Get length of buffer needed for sid.
                int returnLength = GetTokenInformationLength(tokenHandle, TOKEN_INFORMATION_CLASS.TokenAppContainerSid);

                byte[] tokenInformation = new byte[returnLength];
                fixed (byte* pTokenInformation = tokenInformation)
                {
                    if (!GetTokenInformation(
                                                    tokenHandle,
                                                    TOKEN_INFORMATION_CLASS.TokenUser,
                                                    tokenInformation,
                                                    returnLength,
                                                    out returnLength))
                    {
                        int errorCode = Marshal.GetLastWin32Error();
                        throw new Win32Exception(errorCode);
                    }

                    TokenUser* ptg = (TokenUser*)pTokenInformation;

                    IntPtr pstr = IntPtr.Zero;
                    ConvertSidToStringSidW(ptg->User.Sid, out pstr);
                    string retVal = Marshal.PtrToStringAuto(pstr);
                    LocalFree(pstr);

                    return retVal;
                }
            }

            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }
}
