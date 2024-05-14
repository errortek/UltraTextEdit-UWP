using System;

using UTE_UWP_.Services;

using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace UTE_UWP_
{
    public sealed partial class App : Application
    {
        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            InitializeComponent();
            UnhandledException += OnAppUnhandledException;


            // Deferred execution until used. Check https://docs.microsoft.com/dotnet/api/system.lazy-1 for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            var LocalSettings = ApplicationData.Current.LocalSettings;
            if (LocalSettings.Values["NewRibbon"] == null)
            {
                LocalSettings.Values["NewRibbon"] = "Off";
            }
            if (LocalSettings.Values["AccentTheme"] == null) {
                LocalSettings.Values["AccentTheme"] = "Default";
            }
            if ((string)LocalSettings.Values["AccentTheme"] == "Slate Green")
            {
                var brush = new SolidColorBrush(Color.FromArgb(255, 92, 255, 138));
                Application.Current.Resources["SystemAccentColor"] = Color.FromArgb(255, 92, 255, 138);
                Application.Current.Resources["SystemAccentColorDark1"] = Color.FromArgb(255, 92, 255, 138);
                Application.Current.Resources["SystemAccentColorDark2"] = Color.FromArgb(255, 92, 255, 138);
                Application.Current.Resources["SystemAccentColorDark3"] = Color.FromArgb(255, 92, 255, 138);
                Application.Current.Resources["SystemAccentColorLight1"] = Color.FromArgb(255, 92, 255, 138);
                Application.Current.Resources["SystemAccentColorLight2"] = Color.FromArgb(255, 92, 255, 138);
                Application.Current.Resources["SystemAccentColorLight3"] = Color.FromArgb(255, 92, 255, 138);
            }
            if ((string)LocalSettings.Values["AccentTheme"] == "Lilac")
            {
                var brush = new SolidColorBrush(Color.FromArgb(255, 0x89, 0x61, 0xCC));
                Application.Current.Resources["SystemAccentColor"] = Color.FromArgb(255, 0x89, 0x81, 0xCC);
                Application.Current.Resources["SystemAccentColorDark1"] = Color.FromArgb(255, 0x98, 0x75, 0xD4);
                Application.Current.Resources["SystemAccentColorDark2"] = Color.FromArgb(255, 0xA7, 0x88, 0xDD);
                Application.Current.Resources["SystemAccentColorDark3"] = Color.FromArgb(255, 0xB7, 0x9C, 0xE5);
                Application.Current.Resources["SystemAccentColorLight1"] = Color.FromArgb(255, 0x77, 0x52, 0xBA);
                Application.Current.Resources["SystemAccentColorLight2"] = Color.FromArgb(255, 0x65, 0x43, 0xA9);
                Application.Current.Resources["SystemAccentColorLight3"] = Color.FromArgb(255, 0xA7, 0x88, 0xDD);
            }
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
                
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private void OnAppUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // TODO: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/uwp/api/windows.ui.xaml.application.unhandledexception
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.MainPage));
        }
    }
}
