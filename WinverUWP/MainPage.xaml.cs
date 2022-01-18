using RegistryRT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

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
        private UISettings _uiSettings;
        private BackdropBrushXaml Backdrop;

        public MainPage()
        {
            this.InitializeComponent();

            _uiSettings = new UISettings();

            resourceLoader = ResourceLoader.GetForCurrentView();

            if (!ApiInformation.IsTypePresent("Windows.UI.WindowManagement.AppWindow"))
            {
                CloseButton.Visibility = Visibility.Collapsed;
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
            else
            {
                Backdrop = new BackdropBrushXaml();
                Backdrop.SetAppWindow(App.AppWindow);
                Background = Backdrop;
                var token = FeatureTokenGenerator.GenerateTokenFromFeatureId("com.microsoft.windows.windowdecorations");
                var attestation = FeatureTokenGenerator.GenerateAttestation("com.microsoft.windows.windowdecorations");
                LimitedAccessFeatures.TryUnlockFeature("com.microsoft.windows.windowdecorations", token, attestation);
                App.AppWindow.TitleBar.SetPreferredVisibility(Windows.UI.WindowManagement.AppWindowTitleBarVisibility.AlwaysHidden);
                App.AppWindow.Frame.DragRegionVisuals.Add(TitleBar);
            }
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
                    resourceLoader.GetString("Edition/Text"),
                    resourceLoader.GetString("Version/Text"),
                    resourceLoader.GetString("InstalledOn/Text"),
                    resourceLoader.GetString("OSBuild/Text")
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

            _uiSettings.ColorValuesChanged += async (a, b) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => UpdateWindowsBrand());
            };

            Build.Text = build.ToString();

            if (revision != 0)
                Build.Text += $".{revision}";

            registry.InitNTDLLEntryPoints();
            string productName = "";

            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop")
                productName = WinverNative.Winbrand.BrandingFormatString("%WINDOWS_LONG%");
            else
                productName = ReturnValueFromRegistry(RegistryHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName");

            Edition.Text = productName;
            LicensingText.Text = resourceLoader.GetString("Trademark/Text").Replace("Windows", productName);

            var displayVersion = ReturnValueFromRegistry(RegistryHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "DisplayVersion");
            if (string.IsNullOrEmpty(displayVersion))
                displayVersion = ReturnValueFromRegistry(RegistryHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ReleaseId");
            Version.Text = displayVersion;

            var date = GetWindowsInstallationDateTime().ToLocalTime();
            var userCulture = CultureInfoHelper.GetCurrentCulture();
            InstalledOn.Text = date.ToString("d", userCulture);

            var ownerName = ReturnValueFromRegistry(RegistryHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "RegisteredOwner");
            OwnerText.Text = ownerName;
            OwnerText.Visibility = string.IsNullOrEmpty(ownerName) ? Visibility.Collapsed : Visibility.Visible;

            var ownerOrg = ReturnValueFromRegistry(RegistryHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "RegisteredOrganization");
            OrgText.Text = ownerOrg;
            OrgText.Visibility = string.IsNullOrEmpty(ownerOrg) ? Visibility.Collapsed : Visibility.Visible;

            // TODO: Add Experience for non-Desktop.
            if (build < 19041 || AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Desktop")
            {
                ExperienceLabel.Visibility = Visibility.Collapsed;
                Experience.Visibility = Visibility.Collapsed;
                return;
            }

            registry.GetSubKeyList(RegistryHive.HKEY_LOCAL_MACHINE, $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\InboxApplications", out string[] subKeys);
            string cbs = subKeys.First(f => f.StartsWith("MicrosoftWindows.Client.CBS_", StringComparison.CurrentCultureIgnoreCase));
            cbs = cbs.Split('_')[1];
            Experience.Text = $"{resourceLoader.GetString("ExperiencePack")} {cbs}";
        }

        private void UpdateWindowsBrand()
        {
            BrandImage.Source = new SvgImageSource(
                new Uri(
                    "ms-appx:///Assets/"
                    + OSName
                    + "-"
                    + (Application.Current.RequestedTheme == ApplicationTheme.Dark ? "light" : "dark")
                    + ".svg"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (App.AppWindow == null)
                Application.Current.Exit();
            else
#pragma warning disable CS4014
                App.AppWindow.CloseAsync();
#pragma warning restore CS4014
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
    }
}
