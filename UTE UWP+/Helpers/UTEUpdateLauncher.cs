using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTE_UWP_.Views;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UTE_UWP_.Helpers
{
    class UTEUpdateLauncher
    {
        public async void LaunchUTEUpdate()
        {
            ApplicationView currentAV = ApplicationView.GetForCurrentView();
            CoreApplicationView newAV = CoreApplication.CreateNewView();
            await newAV.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var newWindow = Window.Current;
                var newAppView = ApplicationView.GetForCurrentView();
                newAppView.Title = $"UltraTextEdit Update";

                var frame = new Frame();
                frame.Navigate(typeof(UTEUpdate));
                newWindow.Content = frame;
                newWindow.Activated += NewWindow_Activated;
                newWindow.Activate();

                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newAppView.Id,
                    ViewSizePreference.UseMinimum, currentAV.Id, ViewSizePreference.UseMinimum);
            });
        }

        private void NewWindow_Activated(object sender, WindowActivatedEventArgs e)
        {
            float DPI = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().LogicalDpi;

            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchWindowingMode = Windows.UI.ViewManagement.ApplicationViewWindowingMode.PreferredLaunchViewSize;

            var desiredSize = new Windows.Foundation.Size(((float)640 * 96.0f / DPI), ((float)580 * 96.0f / DPI));

            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchViewSize = desiredSize;

            Window.Current.Activate();

            bool result = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryResizeView(desiredSize);
        }
    }
}
