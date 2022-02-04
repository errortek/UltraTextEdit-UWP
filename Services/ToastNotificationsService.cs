using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;

using UltraTextEdit_UWP.Activation;

using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;

namespace UltraTextEdit_UWP.Services
{
    internal partial class ToastNotificationsService : ActivationHandler<ToastNotificationActivatedEventArgs>
    {
        public void ShowToastNotification(ToastNotification toastNotification)
        {
            if (SystemInformation.Instance.IsFirstRun)
            {
                try
                {
                    ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
                }
                catch (Exception)
                {
                    // TODO WTS: Adding ToastNotification can fail in rare conditions, please handle exceptions as appropriate to your scenario.
                }
            }
        }

        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
        {
            //// TODO WTS: Handle activation from toast notification
            //// More details at https://docs.microsoft.com/windows/uwp/design/shell/tiles-and-notifications/send-local-toast

            await Task.CompletedTask;
        }
    }
}
