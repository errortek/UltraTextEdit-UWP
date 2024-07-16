using System;

using UTE_UWP_.Services;
using UTE_UWP_.Helpers;

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
            ConfigureUIResources();
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
            if ((string)LocalSettings.Values["AccentTheme"] == "Crimson")
            {
                var brush = new SolidColorBrush(Color.FromArgb(255, 0x89, 0x61, 0xCC));
                Application.Current.Resources["SystemAccentColor"] = Color.FromArgb(255, 0xD1, 0x34, 0x38);
                Application.Current.Resources["SystemAccentColorDark1"] = Color.FromArgb(255, 0xD9, 0x4D, 0x50);
                Application.Current.Resources["SystemAccentColorDark2"] = Color.FromArgb(255, 0xE1, 0x65, 0x68);
                Application.Current.Resources["SystemAccentColorDark3"] = Color.FromArgb(255, 0xE9, 0x7E, 0x81);
                Application.Current.Resources["SystemAccentColorLight1"] = Color.FromArgb(255, 0xBE, 0x2A, 0x2D);
                Application.Current.Resources["SystemAccentColorLight2"] = Color.FromArgb(255, 0xAB, 0x1F, 0x22);
                Application.Current.Resources["SystemAccentColorLight3"] = Color.FromArgb(255, 0x99, 0x15, 0x16);
            }
            if ((string)LocalSettings.Values["AccentTheme"] == "Blue")
            {
                var brush = new SolidColorBrush(Color.FromArgb(255, 0x89, 0x61, 0xCC));
                Application.Current.Resources["SystemAccentColor"] = Color.FromArgb(255, 0x00, 0x73, 0xCF);
                Application.Current.Resources["SystemAccentColorDark1"] = Color.FromArgb(255, 0x1D, 0x85, 0xD7);
                Application.Current.Resources["SystemAccentColorDark2"] = Color.FromArgb(255, 0x3B, 0x97, 0xDF);
                Application.Current.Resources["SystemAccentColorDark3"] = Color.FromArgb(255, 0x58, 0xA8, 0xE8);
                Application.Current.Resources["SystemAccentColorLight1"] = Color.FromArgb(255, 0x00, 0x64, 0xBE);
                Application.Current.Resources["SystemAccentColorLight2"] = Color.FromArgb(255, 0x00, 0x55, 0xAD);
                Application.Current.Resources["SystemAccentColorLight3"] = Color.FromArgb(255, 0x00, 0x45, 0x9D);
            }
            if ((string)LocalSettings.Values["AccentTheme"] == "Seafoam")
            {
                var brush = new SolidColorBrush(Color.FromArgb(255, 0x89, 0x61, 0xCC));
                Application.Current.Resources["SystemAccentColor"] = Color.FromArgb(255, 0x00, 0xB7, 0xC3);
                Application.Current.Resources["SystemAccentColorDark1"] = Color.FromArgb(255, 0x1C, 0xC1, 0xCC);
                Application.Current.Resources["SystemAccentColorDark2"] = Color.FromArgb(255, 0x38, 0xCA, 0xD4);
                Application.Current.Resources["SystemAccentColorDark3"] = Color.FromArgb(255, 0x54, 0xD4, 0xDD);
                Application.Current.Resources["SystemAccentColorLight1"] = Color.FromArgb(255, 0x00, 0xA4, 0xB0);
                Application.Current.Resources["SystemAccentColorLight2"] = Color.FromArgb(255, 0x00, 0x90, 0x9D);
                Application.Current.Resources["SystemAccentColorLight3"] = Color.FromArgb(255, 0x00, 0x7D, 0x89);
            }
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
                
            }
        }

        private void ConfigureUIResources()
        {
            if (SettingsHelper.GetSettingString("Core.UI") == null)
            {
                SettingsHelper.SetSetting("Core.UI", "WinUI");
            }
            if (SettingsHelper.GetSettingString("Core.Theme") == "Acrylic Glass")
            {
                var res = new ResourceDictionary();
                res.Source = new Uri("ms-appx:///UI/AcrylicGlassUI.xaml");
                Application.Current.Resources.MergedDictionaries.Add(res);
                Application.Current.FocusVisualKind = FocusVisualKind.DottedLine;
                return;
            }
            if (SettingsHelper.GetSettingString("Core.Theme") == "Win32 Light")
            {
                var res = new ResourceDictionary();
                res.Source = new Uri("ms-appx:///UI/Win32 Light.xaml");
                Application.Current.Resources.MergedDictionaries.Add(res);
                Application.Current.FocusVisualKind = FocusVisualKind.DottedLine;
                return;
            }
            if (SettingsHelper.GetSettingString("Core.Theme") == "Win32 Dark")
            {
                var res = new ResourceDictionary();
                res.Source = new Uri("ms-appx:///UI/Win32 Dark.xaml");
                Application.Current.Resources.MergedDictionaries.Add(res);
                Application.Current.FocusVisualKind = FocusVisualKind.DottedLine;
                return;
            }
            if (SettingsHelper.GetSettingString("Core.UI") == "WinUI")
            {
                var res = new ResourceDictionary();
                res.Source = new Uri("ms-appx:///UI/WinUI.xaml");
                Application.Current.FocusVisualKind = FocusVisualKind.HighVisibility;
                Application.Current.Resources.MergedDictionaries.Add(res);
            }
            if (SettingsHelper.GetSettingString("Core.UI") == "CrimsonUI")
            {
                var res = new ResourceDictionary();
                res.Source = new Uri("ms-appx:///UI/CrimsonUI.xaml");
                Application.Current.FocusVisualKind = FocusVisualKind.Reveal;
                Application.Current.Resources.MergedDictionaries.Add(res);
            }
            if (SettingsHelper.GetSettingString("Core.UI") == "10 Light")
            {
                var res = new ResourceDictionary();
                res.Source = new Uri("ms-appx:///UI/10 Light.xaml");
                Application.Current.FocusVisualKind = FocusVisualKind.HighVisibility;
                Application.Current.Resources.MergedDictionaries.Add(res);
            }
            if (SettingsHelper.GetSettingString("Core.UI") == "10 Dark")
            {
                var res = new ResourceDictionary();
                res.Source = new Uri("ms-appx:///UI/10 Dark.xaml");
                Application.Current.FocusVisualKind = FocusVisualKind.HighVisibility;
                Application.Current.Resources.MergedDictionaries.Add(res);
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
