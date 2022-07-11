using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UltraTextEdit_UWP.Views
{
    public sealed partial class utever : ContentDialog
    {

        private bool isCopying;

        public utever()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (isCopying)
                return;

            if (!isCopying)
                CopyButtonLabel.Text = "Copy";

            isCopying = true;
            DataPackage package = new DataPackage();

            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "Version ", Version.Text },
                { "Version/Build semester ", Version2.Text },
                { "Build Date ", InstalledOn.Text },
                { "Build", Build.Text },
            };

            int maxLength = data.Keys.Max(f => f.Length + 5);

            var lines = data.Select(f => string.Format($"{{0,-{maxLength}}}", f.Key) + f.Value);

            string targetText = string.Join(Environment.NewLine, lines);

            package.SetText(targetText);
            Clipboard.SetContent(package);

            bool center = ApiInformation.IsPropertyPresent("Windows.UI.Xaml.UIElement", "CenterPoint");
            bool vectorkey = ApiInformation.IsMethodPresent("Windows.UI.Composition.Compositor", "CreateVector3KeyFrameAnimation");
            bool spring = ApiInformation.IsMethodPresent("Windows.UI.Composition.Compositor", "CreateSpringVector3Animation");
            bool scalar = ApiInformation.IsTypePresent("Windows.UI.Xaml.ScalarTransition");

            if (center && vectorkey && spring && scalar)
            {
                CopyButtonLabel.CenterPoint = new Vector3((float)CopyButtonLabel.ActualWidth / 2, (float)CopyButtonLabel.ActualHeight / 2, (float)CopyButtonLabel.ActualWidth / 2);
                var _springAnimation = Window.Current.Compositor.CreateVector3KeyFrameAnimation();
                _springAnimation.Target = "Scale";
                _springAnimation.InsertKeyFrame(1f, new Vector3(0f));
                _springAnimation.Duration = TimeSpan.FromMilliseconds(100);
                var test = Window.Current.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                CopyButtonLabel.StartAnimation(_springAnimation);
                test.End();
                test.Completed += async (s1, e1) =>
                {
                    CopyButtonLabel.Text = "Copied";
                    CopyButtonLabel.CenterPoint = new Vector3((float)CopyButtonLabel.ActualWidth / 2, (float)CopyButtonLabel.ActualHeight / 2, (float)CopyButtonLabel.ActualWidth / 2);

                    var springAnimation = Window.Current.Compositor.CreateSpringVector3Animation();
                    springAnimation.Target = "Scale";
                    springAnimation.FinalValue = new Vector3(1f);
                    springAnimation.DampingRatio = 0.4f;
                    springAnimation.Period = TimeSpan.FromMilliseconds(20);
                    CopyButtonLabel.StartAnimation(springAnimation);
                    await Task.Delay(1020);
                    CopyButtonLabel.Opacity = 0;
                    await Task.Delay(200);
                    CopyButtonLabel.Text = "Copy";
                    CopyButtonLabel.Opacity = 1;
                    await Task.Delay(200);
                    isCopying = false;
                };
            }
            else
            {
                CopyButtonLabel.Text = "Copied";
                await Task.Delay(1000);
                CopyButtonLabel.Text = "Copy";
                isCopying = false;
            }
        }
    }
}
