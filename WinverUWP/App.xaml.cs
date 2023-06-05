using System;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WinverUWP;

sealed partial class App : Application
{
    public App()
    {
        // Commenting this out since it is too buggy right mow.
        // if (ApiInformation.IsTypePresent("Windows.UI.Composition.CompositionShape"))
        //     Native.VectorDetours.EnableVectorRendering();
        
        this.InitializeComponent();
    }

    protected override void OnActivated(IActivatedEventArgs args)
    {
        StartApp();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
        StartApp();
    }

    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    private void StartApp()
    {
        if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("IsNotFirstRun"))
        {
            ApplicationView appView = ApplicationView.GetForCurrentView();
            appView.TryResizeView(new Size(500, 675));
            ApplicationData.Current.LocalSettings.Values["IsNotFirstRun"] = true;
        }
        else
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;

        Frame frame;
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
}
