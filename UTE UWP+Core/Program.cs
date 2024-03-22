using System;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace UTE_UWP_
{
    public static class Program
    {
        // This project includes DISABLE_XAML_GENERATED_MAIN in the build properties,
        // which prevents the build system from generating the default Main method:
        // static void Main(string[] args)
        // {
        //     global::Windows.UI.Xaml.Application.Start((p) => new App());
        // }
        // TODO: Update the logic in this method if you want to control the launching of multiple instances.
        // You may find the `AppInstance.GetActivatedEventArgs()` useful for your app-defined logic.
        public static void Main(string[] args)
        {
            // If the platform indicates a recommended instance, use that.
            if (AppInstance.RecommendedInstance != null)
            {
                AppInstance.RecommendedInstance.RedirectActivationTo();
            }
            global::Microsoft.UI.Xaml.Application.Start((p) => new App());
        }
    }
}
