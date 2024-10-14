using MicaForUWP.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UTE_UWP_.Helpers;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UTE_UWP_.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPageContainer : Page
    {
        public SettingsPageContainer()
        {
            this.InitializeComponent();
            SettingsFrame.Navigate(typeof(SettingsPage));

            if (BuildInfo.BeforeWin11)
            {
                if (App.Current.RequestedTheme == ApplicationTheme.Light)
                {
                    Application.Current.Resources["AppTitleBarBrush"] = new BackdropMicaBrush()
                    {
                        LuminosityOpacity = 0.8F,
                        TintOpacity = 0F,
                        BackgroundSource = BackgroundSource.WallpaperBackdrop,
                        Opacity = 1,
                        TintColor = Windows.UI.Color.FromArgb(255, 230, 230, 230),
                        FallbackColor = Windows.UI.Color.FromArgb(255, 230, 230, 230)
                    };
                    this.Background = (Brush)Application.Current.Resources["AppTitleBarBrush"];
                }
                else
                {
                    Application.Current.Resources["AppTitleBarBrush"] = new BackdropMicaBrush()
                    {
                        LuminosityOpacity = 0.8F,
                        TintOpacity = 0F,
                        BackgroundSource = BackgroundSource.WallpaperBackdrop,
                        Opacity = 1,
                        TintColor = Windows.UI.Color.FromArgb(255, 25, 25, 25),
                        FallbackColor = Windows.UI.Color.FromArgb(25, 25, 25, 25)
                    };
                    this.Background = (Brush)Application.Current.Resources["AppTitleBarBrush"];
                }

                var appViewTitleBar = ApplicationView.GetForCurrentView().TitleBar;

                appViewTitleBar.ButtonBackgroundColor = Colors.Transparent;
                appViewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = true;
                UpdateTitleBarLayout(coreTitleBar);

                Window.Current.SetTitleBar(AppTitleBar);

                coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
                coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
            }
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            AppTitleBar.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        // Update the TitleBar based on the inactive/active state of the app
        private void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            SolidColorBrush defaultForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];
            SolidColorBrush inactiveForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorDisabledBrush"];

            if (e.WindowActivationState == CoreWindowActivationState.Deactivated)
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
            AppTitleBar.Height = coreTitleBar.Height;

            // Ensure the custom title bar does not overlap window caption controls
            Thickness currMargin = AppTitleBar.Margin;
            AppTitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window.Current.Content is Frame rootFrame && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }
    }
}
