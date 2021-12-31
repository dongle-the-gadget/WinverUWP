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
using WinverUWP.InterCommunication;

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

        public MainPage()
        {
            this.InitializeComponent();

            resourceLoader = ResourceLoader.GetForCurrentView();

            UpdateTitleBarLayout(App.TitleBar);

            App.TitleBarChanged += (s, e) => UpdateTitleBarLayout(App.TitleBar);

            Window.Current.SetTitleBar(TitleBar);

            Window.Current.Activated += Current_Activated;
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

            string text =
                $@"{labels[0]}{Edition.Text}
{labels[1]}{Version.Text}
{labels[2]}{App.OSInfo.InstalledOn}
{labels[3]}{App.OSInfo.Build}
{labels[4]}{Experience.Text}";

            package.SetText(text);

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
            var release = version & 0x000000000000FFFF;

            OSName = build >= 21996 ? "Windows11Logo" : "Windows10Logo";
            UpdateWindowsBrand();

            ActualThemeChanged += (a, b) =>
            {
                UpdateWindowsBrand();
            };

            OwnerText.Text = App.OSInfo.Owner;
            OwnerText.Visibility = string.IsNullOrWhiteSpace(App.OSInfo.Owner) ? Visibility.Collapsed : Visibility.Visible;
            OrgText.Text = App.OSInfo.Corporation;
            OrgText.Visibility = string.IsNullOrWhiteSpace(App.OSInfo.Corporation) ? Visibility.Collapsed : Visibility.Visible;
            InstalledOn.Text = App.OSInfo.InstalledOn;
            Version.Text = App.OSInfo.Version;
            Edition.Text = App.OSInfo.Edition;
            LicensingText.Text = resourceLoader.GetString("Trademark/Text").Replace("Windows", App.OSInfo.Edition);
            Experience.Text = $"{resourceLoader.GetString("ExperiencePack")} {App.OSInfo.Experience}";
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
            if (App.Connection == null)
                return;
            InterCommunicationMessage msg = new InterCommunicationMessage { Type = InterCommunicationType.Exit };
            string json = JsonSerializer.Serialize(msg);
            ValueSet valueSet = new ValueSet
            {
                { InterCommunicationConstants.MessageKey, json }
            };
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            App.Connection.SendMessageAsync(valueSet);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Application.Current.Exit();
        }
    }
}
