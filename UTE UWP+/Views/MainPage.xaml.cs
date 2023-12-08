using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Graphics.Printing;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SplitButton = Microsoft.UI.Xaml.Controls.SplitButton;
namespace UTE_UWP_.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private bool saved;
        private bool _wasOpen;
        private string fileNameWithPath;
        private bool _openDialog;
        private string originalDocText;

        public MainPage()
        {
            InitializeComponent();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            var appViewTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            appViewTitleBar.ButtonBackgroundColor = Colors.Transparent;
            appViewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            appViewTitleBar.ButtonForegroundColor = (Color)Resources["SystemAccentColor"];

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            UpdateTitleBarLayout(coreTitleBar);

            Window.Current.SetTitleBar(AppTitleBar);

            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
            Window.Current.Activated += Current_Activated;

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += OnCloseRequest;

            NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (sender.IsVisible)
            {
                AppTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                AppTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        // Update the TitleBar based on the inactive/active state of the app
        private void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            SolidColorBrush defaultForegroundBrush = new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]);
            SolidColorBrush inactiveForegroundBrush = new SolidColorBrush(Color.FromArgb(255, 155, 155, 155));

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
        }

        private void OnCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
           // if (!saved) { e.Handled = true; ShowUnsavedDialog(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void showinsiderinfo(object sender, RoutedEventArgs e)
        {
            //ToggleThemeTeachingTip1.IsOpen = true;
        }
    }
}
