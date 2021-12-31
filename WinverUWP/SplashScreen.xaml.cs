using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinverUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SplashScreen : Page
    {
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        private Windows.ApplicationModel.Activation.SplashScreen splash; // Variable to hold the splash screen object.
        internal bool dismissed = false; // Variable to track splash screen dismissal status.
        internal Frame rootFrame;

        public SplashScreen(Windows.ApplicationModel.Activation.SplashScreen splashScreen)
        {
            this.InitializeComponent();

            // Listen for window resize events to reposition the extended splash screen image accordingly.
            // This ensures that the extended splash screen formats properly in response to window resizing.
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(OnResize);

            splash = splashScreen;
            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                splash.Dismissed += new TypedEventHandler<Windows.ApplicationModel.Activation.SplashScreen, object>(DismissedEventHandler);

                // Retrieve the window coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();

                // If applicable, include a method for positioning a progress control.
                PositionRing();
            }

            // Create a Frame to act as the navigation context
            rootFrame = new Frame();
        }

        void PositionImage()
        {
            SplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
            SplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
            SplashImage.Height = splashImageRect.Height;
            SplashImage.Width = splashImageRect.Width;
        }

        void PositionRing()
        {
            ProgressRing.SetValue(Canvas.LeftProperty, splashImageRect.X + (splashImageRect.Width * 0.5) - (ProgressRing.Width * 0.5));
            ProgressRing.SetValue(Canvas.TopProperty, (splashImageRect.Y + splashImageRect.Height + splashImageRect.Height * 0.1));
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        void DismissedEventHandler(Windows.ApplicationModel.Activation.SplashScreen sender, object e)
        {
            dismissed = true;
        }

        void OnResize(object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be executed when a user resizes the window.
            if (splash != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();

                // If applicable, include a method for positioning a progress control.
                PositionRing();
            }
        }

        public async void DismissExtendedSplash()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                rootFrame = new Frame();
                rootFrame.Content = new MainPage(); 
                Window.Current.Content = rootFrame;
            });
        }
    }
}
