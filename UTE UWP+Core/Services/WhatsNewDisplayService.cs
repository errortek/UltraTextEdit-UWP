﻿using System;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp.Helpers;

using UTE_UWP_.Views;

using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace UTE_UWP_.Services
{
    // For instructions on testing this service see https://github.com/microsoft/TemplateStudio/blob/main/docs/UWP/features/whats-new-prompt.md
    public static class WhatsNewDisplayService
    {
        private static bool shown = false;

        internal static async Task ShowIfAppropriateAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    if (SystemInformation.Instance.IsAppUpdated && !shown)
                    {
                        shown = true;
                        var dialog = new WhatsNewDialog();
                        await dialog.ShowAsync();
                    }
                });
        }
    }
}
