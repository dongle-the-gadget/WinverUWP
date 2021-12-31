using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinverUWP.InterCommunication;

namespace WinverUWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static AppServiceConnection Connection { get; private set; }

        private BackgroundTaskDeferral Deferral;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            ApplicationView.PreferredLaunchViewSize = new Size(500, 700);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        SplashScreen extendedSplash;

        protected override void OnActivated(IActivatedEventArgs args)
        {
            StartApp(args.SplashScreen);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            StartApp(e.SplashScreen);
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            var taskInstance = args.TaskInstance;
            if (taskInstance.TriggerDetails is AppServiceTriggerDetails triggerDetails)
            {
                Deferral = taskInstance.GetDeferral();
                taskInstance.Canceled += (s, e) =>
                {
                    Deferral?.Complete();
                };
                Connection = triggerDetails.AppServiceConnection;
                Connection.RequestReceived += OnRequest;
            }
        }

        public static OSInfoData OSInfo { get; set; }

        private void OnRequest(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var deferral = args.GetDeferral();
            if (args.Request.Message.ContainsKey(InterCommunicationConstants.MessageKey))
            {
                var msg = JsonSerializer.Deserialize<InterCommunicationMessage>(args.Request.Message[InterCommunicationConstants.MessageKey] as string);
                if (msg != null && msg.Type == InterCommunicationType.OSInfo)
                {
                    OSInfo = msg.OSInfo;
                    extendedSplash.DismissExtendedSplash();
                }
            }
            deferral.Complete();
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

        private async void StartApp(Windows.ApplicationModel.Activation.SplashScreen splash)
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            TitleBar = coreTitleBar;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;

            extendedSplash = new SplashScreen(splash);
            Window.Current.Content = extendedSplash;

            await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();

            // Register close
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += App_CloseRequested;
            
            // Ensure the current window is active
            Window.Current.Activate();
        }

        public static event EventHandler TitleBarChanged;

        public static CoreApplicationViewTitleBar TitleBar { get; set; }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar = sender;
            TitleBarChanged?.Invoke(null, EventArgs.Empty);
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar = sender;
            TitleBarChanged?.Invoke(null, EventArgs.Empty);
        }

        private void App_CloseRequested(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            var deferral = e.GetDeferral();
            if (Connection != null)
            {
                InterCommunicationMessage msg = new InterCommunicationMessage { Type = InterCommunicationType.Exit };
                string json = JsonSerializer.Serialize(msg);
                ValueSet valueSet = new ValueSet
                {
                    { InterCommunicationConstants.MessageKey, json }
                };
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Connection.SendMessageAsync(valueSet);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            deferral.Complete();
        }
    }
}
