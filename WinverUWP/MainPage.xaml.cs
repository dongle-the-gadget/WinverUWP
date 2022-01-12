using RegistryRT;
using System;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WinverUWP.Helpers;
using WinverUWP.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WinverUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string OSName = "";

        public string[] test =
        {
            "Test",
            "test 2"
        };

        public MainPage()
        {
            this.InitializeComponent();

            // Initialize everything.
            Singleton.GetInstance(ResourceLoader.GetForCurrentView());

            Registry registry = new Registry();
            registry.InitNTDLLEntryPoints();
            Singleton.GetInstance(registry);

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

            frame.Navigate(typeof(WindowsPage));
        }
    }
}
