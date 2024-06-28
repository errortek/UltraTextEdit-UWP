using UTE_UWP_.Helpers;
using Windows.UI.Xaml;
using System;

namespace UTE_UWP_.UI
{
    public class UI
    {
        public UI()
        {

        }

        public static void ConfigureUIResources()
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
    }
}
