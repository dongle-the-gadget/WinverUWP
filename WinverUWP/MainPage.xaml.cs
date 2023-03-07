using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WinverUWP.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WinverUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string OSName = "";
        private ResourceLoader resourceLoader;
        private UISettings _uiSettings;
        private bool isCopying;

        public MainPage()
        {
            InitializeComponent();
                
            _uiSettings = new UISettings();

            resourceLoader = ResourceLoader.GetForCurrentView();

            var appdata = ApplicationData.Current.LocalSettings.Values.Where(f => f.Key.EndsWith("Expander"));

            if (appdata.Count() == 0)
                SpecExpander.IsExpanded = LegalExpander.IsExpanded = true;
            else
                foreach(var value in appdata)
                    ((Microsoft.UI.Xaml.Controls.Expander)FindName(value.Key)).IsExpanded = (bool)value.Value;

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
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
                AppTitle.Style = (Style)Application.Current.Resources["InactivatedAppTitle"];
            else
                AppTitle.Style = (Style)Application.Current.Resources["ActivatedAppTitle"];
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            // Update title bar control size as needed to account for system size changes.
            TitleBar.Height = coreTitleBar.Height;

            // Ensure the custom title bar does not overlap window caption controls
            Thickness currMargin = TitleBar.Margin;
            TitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (isCopying)
                return;

            isCopying = true;
            DataPackage package = new DataPackage();

            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { resourceLoader.GetString("Edition/Text"), Edition.Text },
                { resourceLoader.GetString("Version/Text"), Version.Text },
                { resourceLoader.GetString("InstalledOn/Text"), InstalledOn.Text },
                { resourceLoader.GetString("OSBuild/Text"), Build.Text },
            };

            if (Expiration.Visibility == Visibility.Visible)
                data.Add(resourceLoader.GetString("Expiration/Text"), Expiration.Text);

            int maxLength = data.Keys.Max(f => f.Length + 5);

            var lines = data.Select(f => string.Format($"{{0,-{maxLength}}}", f.Key) + f.Value);

            string targetText = string.Join(Environment.NewLine, lines);

            package.SetText(targetText);
            Clipboard.SetContent(package);
            CopyToClipboardSuccessAnimation.Begin();
            CopyToClipboardSuccessAnimation.Completed += CopyToClipboardSuccessAnimation_Completed;
        }

        private void CopyToClipboardSuccessAnimation_Completed(object sender, object e)
        {
            isCopying = false;
            CopyToClipboardSuccessAnimation.Completed -= CopyToClipboardSuccessAnimation_Completed;
        }

        private void SetTitleBarBackground()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonHoverBackgroundColor = (Color)Application.Current.Resources["SubtleFillColorSecondary"];
            titleBar.ButtonPressedBackgroundColor = (Color)Application.Current.Resources["SubtleFillColorTertiary"];
        }

#if DEBUG
        unsafe void Test()
        {
            const string activatableClassId = "Windows.Internal.StateRepository.Package";
            fixed (char* pActivatableClassId = activatableClassId)
            {
                Interop.HSTRING hStringActivatableClass;
                Interop.WindowsCreateString((ushort*)pActivatableClassId, (uint)activatableClassId.Length, &hStringActivatableClass);

                using Interop.ComPtr<Interop.IUnknown> packageStaticsUnknown = default;
                Interop.RoGetActivationFactory(hStringActivatableClass, Interop.__uuidof<Interop.IUnknown>(), packageStaticsUnknown.GetVoidAddressOf());

                using Interop.ComPtr<IPackageStatics_StateRepository> packageStatics = default;

                if (packageStaticsUnknown.Get()->QueryInterface(Interop.__uuidof<IPackageStatics_StateRepository>(), packageStatics.GetVoidAddressOf()) == 0)
                {
                    // QueryInterface was successful, meaning that the IPackageStatics is actually Windows 11.
                    const string packageName = "MicrosoftWindows.Client.CBS_cw5n1h2txyewy";
                    fixed (char* pPackageName = packageName)
                    {
                        Interop.HSTRING hStringPackageName;
                        Interop.WindowsCreateString((ushort*)pPackageName, (uint)packageName.Length, &hStringPackageName);

                        bool exists = false;
                        Interop.HRESULT success = packageStatics.Get()->ExistsByPackageFamilyName(hStringPackageName, &exists);
                    }
                }
            }
        }
#endif

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
#if DEBUG
            try
            {
                Test();
            }
            catch { }
#endif
            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong build = (version & 0x00000000FFFF0000L) >> 16;
            var revision = version & 0x000000000000FFFF;

            OSName = build >= 21996 ? "Windows11Logo" : "Windows10Logo";
            UpdateWindowsBrand();
            SetTitleBarBackground();

            _uiSettings.ColorValuesChanged += async (a, b) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    UpdateWindowsBrand();
                    SetTitleBarBackground();
                });
            };

            Build.Text = build.ToString();

            if (revision != 0)
                Build.Text += $".{revision}";

            string productName = "";

            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop")
                productName = WinbrandHelper.GetWinbrand();
            else
                productName = RegistryHelper.GetInfoString("ProductName");

            Edition.Text = productName;
            LicensingText.Text = resourceLoader.GetString("Trademark/Text").Replace("Windows", productName);

            var displayVersion = RegistryHelper.GetInfoString("DisplayVersion");
            if (string.IsNullOrEmpty(displayVersion))
                displayVersion = RegistryHelper.GetInfoString("ReleaseId");

            Version.Text = displayVersion;

            var date = GetWindowsInstallationDateTime().ToLocalTime();
            var userCulture = CultureInfoHelper.GetCurrentCulture();
            InstalledOn.Text = date.ToString("d", userCulture);


            using (X509Certificate2 cert = new X509Certificate2("C:\\Windows\\System32\\ntdll.dll"))
            {
                if (cert.Issuer.Contains("Development"))
                    Expiration.Text = cert.NotAfter.ToString("g", userCulture);
                else
                {
                    Expiration.Visibility = Visibility.Collapsed;
                    ExpirationLabel.Visibility = Visibility.Collapsed;
                }
            }

            var ownerName = RegistryHelper.GetInfoString("RegisteredOwner");
            if (ownerName != null)
                OwnerText.Text = ownerName;
            OwnerText.Visibility = string.IsNullOrEmpty(ownerName) ? Visibility.Collapsed : Visibility.Visible;

            var ownerOrg = RegistryHelper.GetInfoString("RegisteredOrganization");
            if (ownerOrg != null)
                OrgText.Text = ownerOrg;
            OrgText.Visibility = string.IsNullOrEmpty(ownerOrg) ? Visibility.Collapsed : Visibility.Visible;

            if (string.IsNullOrEmpty(ownerName) && string.IsNullOrEmpty(ownerOrg))
                LicenseTo.Visibility = Visibility.Collapsed;
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
            _ = ApplicationView.GetForCurrentView().TryConsolidateAsync();
        }

        private DateTime GetWindowsInstallationDateTime()
        {
            var seconds = RegistryHelper.GetInfoDWord("InstallDate");
            DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime installDate = startDate.AddSeconds(seconds.Value);
            return installDate;
        }

        private void Expander_Collapsed(Microsoft.UI.Xaml.Controls.Expander sender, Microsoft.UI.Xaml.Controls.ExpanderCollapsedEventArgs args)
            => ApplicationData.Current.LocalSettings.Values[sender.Name] = false;

        private void Expander_Expanding(Microsoft.UI.Xaml.Controls.Expander sender, Microsoft.UI.Xaml.Controls.ExpanderExpandingEventArgs args)
            => ApplicationData.Current.LocalSettings.Values[sender.Name] = true;
    }
}
