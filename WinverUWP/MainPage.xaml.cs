using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
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
using WinverUWP.InterCommunication;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WinverUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private static MainPage _current;
        private string OSName = "";

        public static MainPage Current => _current;

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            UpdateTitleBarLayout(coreTitleBar);
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;

            Window.Current.SetTitleBar(TitleBar);

            Window.Current.Activated += Current_Activated;

            _current = this;
        }

        private OSInfoData osInfo;

        public OSInfoData OSInfo
        {
            get => osInfo;
            set
            {
                osInfo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OSInfo)));
            }
        }

        string baseText = "The Windows operating system and its user interface are protected by trademark and other pending or existing intellecutal property rights in the United States and other countries or regions.";

        public event PropertyChangedEventHandler PropertyChanged;


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

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            // Update title bar control size as needed to account for system size changes.
            TitleBar.Height = coreTitleBar.Height;

            // Ensure the custom title bar does not overlap window caption controls
            Thickness currMargin = TitleBar.Margin;
            TitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
        }

        public async void Connection_RequestReceived(Windows.ApplicationModel.AppService.AppServiceConnection sender, Windows.ApplicationModel.AppService.AppServiceRequestReceivedEventArgs args)
        {
            var deferral = args.GetDeferral();
            if (args.Request.Message.ContainsKey(InterCommunicationConstants.MessageKey))
            {
                var msg = JsonSerializer.Deserialize<InterCommunicationMessage>(args.Request.Message[InterCommunicationConstants.MessageKey] as string);
                if (msg != null && msg.Type == InterCommunicationType.OSInfo)
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        OSInfo = msg.OSInfo;
                        LicensingText.Text = baseText.Replace("Windows", msg.OSInfo.Edition);
                        Experience.Text = $"Windows Feature Experience Pack {msg.OSInfo.Experience}";
                    });
                }
            }
            deferral.Complete();
        }

        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            DataPackage package = new DataPackage();
            package.SetText(
                $"Edition         {Edition.Text}" +
                Environment.NewLine +
                $"Version         {Version.Text}" +
                Environment.NewLine +
                $"Installed on    {InstalledOn.Text}" +
                Environment.NewLine +
                $"OS build        {Build.Text}" +
                Environment.NewLine +
                $"Experience      {Experience.Text}");
            Clipboard.SetContent(package);
            CopyButton.Content = "Copied!";
            await Task.Delay(1000);
            CopyButton.Content = "Copy";
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong build = (version & 0x00000000FFFF0000L) >> 16;

            OSName = build >= 21996 ? "Windows11Logo" : "Windows10Logo";
            UpdateWindowsBrand();

            ActualThemeChanged += (a, b) =>
            {
                UpdateWindowsBrand();
            };

            await Windows.ApplicationModel.FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
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
            InterCommunicationMessage msg = new InterCommunicationMessage { Type = InterCommunicationType.Exit };
            string json = JsonSerializer.Serialize(msg);
            ValueSet valueSet = new ValueSet
            {
                { InterCommunicationConstants.MessageKey, json }
            };
            App.Connection.SendMessageAsync(valueSet);
            Application.Current.Exit();
        }
    }
}
