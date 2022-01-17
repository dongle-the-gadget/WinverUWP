using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Navigation;

namespace WinverUWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static AppWindow AppWindow { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            ApplicationView.PreferredLaunchViewSize = new Size(500, 675);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            StartApp();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            StartApp();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private async void StartApp()
        {
            Frame frame;
            if (!ApiInformation.IsTypePresent("Windows.UI.WindowManagement.AppWindow"))
            {
                frame = Window.Current.Content as Frame;
                if (frame != null)
                    return;
                frame = new Frame();
                frame.NavigationFailed += OnNavigationFailed;
                frame.Navigate(typeof(MainPage));
                Window.Current.Content = frame;

                // Ensure the current window is active
                Window.Current.Activate();
            }
            else
            {
                if (AppWindow != null)
                    return;
                frame = new Frame();
                frame.NavigationFailed += OnNavigationFailed;
                AppWindow = await AppWindow.TryCreateAsync();
                ElementCompositionPreview.SetAppWindowContent(AppWindow, frame);
                frame.Navigate(typeof(MainPage));
                await AppWindow.TryShowAsync();
                await ApplicationView.GetForCurrentView().TryConsolidateAsync();
            }
        }
    }
}
