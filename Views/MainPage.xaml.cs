using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.Notifications;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UltraTextEdit_UWP.Services;
using System.Reflection;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Markup;
using Microsoft.Toolkit.Uwp.Helpers;
using UltraTextEdit_UWP.Helpers;
using Microsoft.Graphics.Canvas.Text;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using System.Text;
using UltraTextEdit_UWP.Dialogs;
using MicaForUWP.Media;

namespace UltraTextEdit_UWP.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {


        private bool saved = true;
        private string fileNameWithPath = "";

        public List<string> fonts
        {
            get
            {
                return CanvasTextFormat.GetSystemFontFamilies().OrderBy(f => f).ToList();
            }
        }

        public MainPage()
        {
            InitializeComponent();
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

            }
            else
            {

            }
            var appViewTitleBar = ApplicationView.GetForCurrentView().TitleBar;                           
            appViewTitleBar.ButtonForegroundColor = (Color)Resources["SystemAccentColor"];
            appViewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            UpdateTitleBarLayout(coreTitleBar);

            Window.Current.SetTitleBar(AppTitleBar);

            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
            Window.Current.Activated += Current_Activated;

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += OnCloseRequest;

            NavigationCacheMode = NavigationCacheMode.Required;

            ShareSourceLoad();
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
        }

        public List<double> FontSizes { get; } = new List<double>()
            {
                8,
                9,
                10,
                11,
                12,
                14,
                16,
                18,
                20,
                24,
                28,
                36,
                48,
                72,
                96};

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        
        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Bold = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Italic = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == Windows.UI.Text.UnderlineType.None)
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.Single;
                }
                else
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
        }

        
        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            // Extract the color of the button that was clicked.
            Button clickedColor = (Button)sender;
            var borderone = (Windows.UI.Xaml.Controls.Border)clickedColor.Content;
            var bordertwo = (Windows.UI.Xaml.Controls.Border)borderone.Child;
            var rectangle = (Windows.UI.Xaml.Shapes.Rectangle)bordertwo.Child;
            var color = (rectangle.Fill as SolidColorBrush).Color;
            box.Document.Selection.CharacterFormat.ForegroundColor = color;
            //FontColorMarker.SetValue(ForegroundProperty, new SolidColorBrush(color));
            box.Focus(FocusState.Keyboard);
        }

        private void fontcolorsplitbutton_Click(Microsoft.UI.Xaml.Controls.SplitButton sender, Microsoft.UI.Xaml.Controls.SplitButtonClickEventArgs args)
        {
            // If you see this, remind me to look into the splitbutton color applying logic
        }

        private void ConfirmColor_Click(object sender, RoutedEventArgs e)
        {
            // Confirm color picker choice and apply color to text
            Color color = myColorPicker.Color;
            box.Document.Selection.CharacterFormat.ForegroundColor = color;

            // Hide flyout
            colorPickerButton.Flyout.Hide();
        }

        private void CancelColor_Click(object sender, RoutedEventArgs e)
        {
            // Cancel flyout
            colorPickerButton.Flyout.Hide();
        }


        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new WhatsNewDialog();
            await dialog.ShowAsync();
        }
        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a text file.
            Windows.Storage.Pickers.FileOpenPicker open =
                new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".rtf");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Load the file into the Document property of the RichEditBox.
                    box.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, randAccStream);
                }
                AppTitle.Text = file.Name + " - " + "UTE UWP";
                UnsavedTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Save(true);


        }
        private async void Save(bool isCopy)
        {
            string fileName = AppTitle.Text.Replace(" - " + "UTE UWP", "");
            if (isCopy || fileName == "Untitled")
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

                // Dropdown of file types the user can save the file as
                savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });
                savePicker.FileTypeChoices.Add("Office Open XML Document", new List<string>() { ".docx" });
                savePicker.FileTypeChoices.Add("Plain Text File", new List<string>() { ".txt" });

                // Default file name if the user does not type one in or select a file to replace
                savePicker.SuggestedFileName = "New Document";

                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    // Prevent updates to the remote version of the file until we
                    // finish making changes and call CompleteUpdatesAsync.
                    CachedFileManager.DeferUpdates(file);
                    // write to file
                    using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
                        await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                    {
                        box.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, randAccStream);
                    }

                    // Let Windows know that we're finished changing the file so the
                    // other app can update the remote version of the file.
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                    if (status != FileUpdateStatus.Complete)
                    {
                        Windows.UI.Popups.MessageDialog errorBox =
                            new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
                        await errorBox.ShowAsync();
                    }
                    AppTitle.Text = file.Name + " - " + "UTE UWP";
                    saved = true;
                }
            }
            else if (!isCopy || fileName != "Untitled")
            {
                string path = fileNameWithPath.Replace("\\" + fileName, "");
                try
                {
                    StorageFile file = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFileAsync("CurrentlyOpenFile");
                    if (file != null)
                    {
                        // Prevent updates to the remote version of the file until we
                        // finish making changes and call CompleteUpdatesAsync.
                        CachedFileManager.DeferUpdates(file);
                        // write to file
                        using (IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                            if (file.Name.EndsWith(".txt"))
                            {
                                box.Document.SaveToStream(TextGetOptions.None, randAccStream);
                            }
                            else
                            {
                                box.Document.SaveToStream(TextGetOptions.FormatRtf, randAccStream);
                            }


                        // Let Windows know that we're finished changing the file so the
                        // other app can update the remote version of the file.
                        FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                        if (status != FileUpdateStatus.Complete)
                        {
                            Windows.UI.Popups.MessageDialog errorBox = new("File " + file.Name + " couldn't be saved.");
                            await errorBox.ShowAsync();
                        }
                        saved = true;
                        AppTitle.Text = file.Name + " -  UTE UWP";
                        Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Remove("CurrentlyOpenFile");
                    }
                }
                catch (Exception)
                {
                    Save(true);
                }
            }
        }

        private void OnCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            if (saved == false) { e.Handled = true; ShowUnsavedDialog(); }
        }

        private void BoldButton_Off(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextCharacterFormat selectedText = box.Document.Selection.CharacterFormat;
            if (selectedText != null)
            {
                selectedText.FontStyle = FontStyle.Normal;
            }
        }

        private void ItalicButton_Off(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextCharacterFormat selectedText = box.Document.Selection.CharacterFormat;
            if (selectedText != null)
            {
                selectedText.FontStyle = FontStyle.Normal;
            }
        }
        private void UnderlineButton_Off(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextCharacterFormat selectedText = box.Document.Selection.CharacterFormat;
            if (selectedText != null)
            {
                selectedText.FontStyle = FontStyle.Normal;
            }
        }

        private void SuperscriptCheck(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Superscript = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void SubscriptCheck(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Subscript = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private async void ShowUnsavedDialog()
        {
            string fileName = AppTitle.Text.Replace(" - " + "UTE UWP", "");
            ContentDialog aboutDialog = new ContentDialog()
            {
                Title = "Do you want to save changes to " + fileName + "?",
                Content = "There are unsaved changes to your document, want to save them?",
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Save changes",
                SecondaryButtonText = "No",
                DefaultButton = ContentDialogButton.Primary
            };

            ContentDialogResult result = await aboutDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Save(false);
            }
            else if (result == ContentDialogResult.Secondary)
            {
                await ApplicationView.GetForCurrentView().TryConsolidateAsync();
            }
            else
            {
                // Do nothing
            }
        }

        private void helpnoti(object sender, RoutedEventArgs e)
        {
            void ShowToastNotificationSample()
            {
                // Create the toast content
                var content = new ToastContent()
                {
                    // More about the Launch property at https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.notifications.toastcontent
                    Launch = "ToastContentActivationParams",

                    Visual = new ToastVisual()
                    {
                        BindingGeneric = new ToastBindingGeneric()
                        {
                            Children =
                        {
                            new AdaptiveImage()
                            {
                                Source = "ms-appx:///Assets/Square44x44Logo.altform-lightunplated_targetsize-48.png"
                            },
                            new AdaptiveText()
                            {
                                Text = "Welcome to UltraTextEdit UWP!"
                            },

                            new AdaptiveText()
                            {
                                 Text = @"If you need any help on how to use this app, just go to the jpb website! You can find the link to the website in Settings."
                            }
                        }
                        }
                    },

                    Actions = new ToastActionsCustom()
                    {
                        Buttons =
                    {
                        // More about Toast Buttons at https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.notifications.toastbutton
                        new ToastButtonDismiss("OK")
                    }
                    }
                };

                // Add the content to the toast
                var toast = new ToastNotification(content.GetXml())
                {
                    // TODO WTS: Set a unique identifier for this notification within the notification group. (optional)
                    // More details at https://docs.microsoft.com/uwp/api/windows.ui.notifications.toastnotification.tag
                    Tag = "UTE UWP HelpNoti"
                };

                // And show the toast
                try
                {
                    ToastNotificationManager.CreateToastNotifier().Show(toast);
                }
                catch (Exception)
                {
                    // TODO WTS: Adding ToastNotification can fail in rare conditions, please handle exceptions as appropriate to your scenario.
                }
            }
            ShowToastNotificationSample();
        }

        private async void DisplayAboutDialog()
        {
            ContentDialog aboutDialog = new ContentDialog()
            {
                Title = "UltraTextEdit UWP",
                Content = $"Version 10.0 (Build 22000.2777)\n\n© 2021-2024 jpb",
                CloseButtonText = "OK",
                DefaultButton = ContentDialogButton.Close
            };

            await aboutDialog.ShowAsync();
        }

        private void aboutclick(object sender, RoutedEventArgs e)
        {
            DisplayAboutDialog();
        }

        private void StrikeButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Strikethrough = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void StrikeButton_Off(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextCharacterFormat selectedText = box.Document.Selection.CharacterFormat;
            if (selectedText != null)
            {
                selectedText.FontStyle = FontStyle.Normal;
            }
        }

        private void edittextchanged(object sender, RoutedEventArgs e)
        {
            box.Document.GetText(TextGetOptions.UseObjectText, out string textStart);

            if (textStart == "" || string.IsNullOrWhiteSpace(textStart))
            {
                saved = true;
            }
            else
            {
                saved = false;
            }

            if (!saved) UnsavedTextBlock.Visibility = Visibility.Visible;
            else UnsavedTextBlock.Visibility = Visibility.Collapsed;

        }

        private async void utever(object sender, RoutedEventArgs e)
        {
            utever dialog = new utever();

            dialog.DefaultButton = ContentDialogButton.Primary;


            var result = await dialog.ShowAsync();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            box.Document.Undo();
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            box.Document.Redo();
        }

        private async void image(object sender, RoutedEventArgs e)
        {
            // Open an image file.
            Windows.Storage.Pickers.FileOpenPicker open =
                new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".png");
            open.FileTypeFilter.Add(".jpg");
            open.FileTypeFilter.Add(".jpeg");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                using IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.Read);
                var properties = await file.Properties.GetImagePropertiesAsync();
                int width = (int)properties.Width;
                int height = (int)properties.Height;

                ImageOptionsDialog dialog = new()
                {
                    DefaultWidth = width,
                    DefaultHeight = height
                };

                ContentDialogResult result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    box.Document.Selection.InsertImage((int)dialog.DefaultWidth, (int)dialog.DefaultHeight, 0, VerticalCharacterAlignment.Baseline, string.IsNullOrWhiteSpace(dialog.Tag) ? "Image" : dialog.Tag, randAccStream);
                    return;
                }

                // Insert an image
                box.Document.Selection.InsertImage(width, height, 0, VerticalCharacterAlignment.Baseline, "Image", randAccStream);
            }
        }

        private void link(object sender, RoutedEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.FrameworkElement", "AllowFocusOnInteraction"))
                hyperlinkText.AllowFocusOnInteraction = true;
            box.Document.Selection.Link = $"\"{hyperlinkText.Text}\"";
            box.Document.Selection.CharacterFormat.ForegroundColor = (Color)XamlBindingHelper.ConvertValue(typeof(Color), "#6194c7");
            AddLinkButton.Flyout.Hide();
        }

        private void Dash(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == Windows.UI.Text.UnderlineType.None)
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.Dash;
                }
                else
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void DashDot(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == Windows.UI.Text.UnderlineType.None)
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.DashDot;
                }
                else
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void wordunderline(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == Windows.UI.Text.UnderlineType.None)
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.Words;
                }
                else
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void doublewaveunderline(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == Windows.UI.Text.UnderlineType.None)
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.DoubleWave;
                }
                else
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void heavywaveunderline(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == Windows.UI.Text.UnderlineType.None)
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.HeavyWave;
                }
                else
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
        }

        

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window.Current.Content is Frame rootFrame)
            {
                rootFrame.Navigate(typeof(HomePage));
            }
        }

        private async void NewDoc_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView currentAV = ApplicationView.GetForCurrentView();
            CoreApplicationView newAV = CoreApplication.CreateNewView();
            await newAV.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var newWindow = Window.Current;
                var newAppView = ApplicationView.GetForCurrentView();
                newAppView.Title = $"Untitled - UTE UWP";

                var frame = new Frame();
                frame.Navigate(typeof(MainPage));
                newWindow.Content = frame;
                newWindow.Activate();

                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newAppView.Id,
                    ViewSizePreference.UseMinimum, currentAV.Id, ViewSizePreference.UseMinimum);
            });
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            box.Document.Selection.Copy();
        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            box.Document.Selection.Paste(0);
        }

        private void CutButton_Click(object sender, RoutedEventArgs e)
        {
            box.Document.Selection.Cut();
        }

        private void ShareSourceLoad()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.DataRequested);
        }

        private void DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = "UltraTextEdit Share Service";
            request.Data.Properties.Description = "Text sharing for the UTE UWP app";
            request.Data.SetText(box.TextDocument.ToString());
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            ShareSourceLoad();
            DataTransferManager.ShowShareUI();
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            commentsplitview.IsPaneOpen = true;
            commentstabitem.Visibility = Visibility.Visible;
        }

        private void closecomments(object sender, RoutedEventArgs e)
        {
            commentsplitview.IsPaneOpen = false;
            commentstabitem.Visibility = Visibility.Collapsed;
        }

        // Method to create a table format string which can directly be set to 
        // RichTextBox Control

        private async void AddTableButton_Click(object sender, RoutedEventArgs e)
        {
            var dialogtable = new TableDialog();
            await dialogtable.ShowAsync();
            InsertTableInRichTextBox(dialogtable.rows, dialogtable.columns, 1000);
        }

        private void AddSymbolButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SymbolButton_Click(object sender, RoutedEventArgs e)
        {
                // Extract the color of the button that was clicked.
                Button clickedSymbol = (Button)sender;
                string rectangle = clickedSymbol.Content.ToString();
                string text = rectangle;

                var myDocument = box.Document;
                string oldText;
                myDocument.GetText(TextGetOptions.None, out oldText);
                myDocument.SetText(TextSetOptions.None, oldText + text);

                symbolbut.Flyout.Hide();
                box.Focus(FocusState.Keyboard);
            
        }

        private async void DateInsertionAsync(object sender, RoutedEventArgs e)
        { // Create a ContentDialog
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Insert current date and time";

            // Create a ListView for the user to select the date format
            ListView listView = new ListView();
            listView.SelectionMode = ListViewSelectionMode.Single;

            // Create a list of date formats to display in the ListView
            List<string> dateFormats = new List<string>();
            dateFormats.Add(DateTime.Now.ToString("dd.M.yyyy"));
            dateFormats.Add(DateTime.Now.ToString("M.dd.yyyy"));
            dateFormats.Add(DateTime.Now.ToString("dd MMM yyyy"));
            dateFormats.Add(DateTime.Now.ToString("dddd, dd MMMM yyyy"));
            dateFormats.Add(DateTime.Now.ToString("dd MMMM yyyy"));
            dateFormats.Add(DateTime.Now.ToString("hh:mm:ss tt"));
            dateFormats.Add(DateTime.Now.ToString("HH:mm:ss"));
            dateFormats.Add(DateTime.Now.ToString("dddd, dd MMMM yyyy, HH:mm:ss"));
            dateFormats.Add(DateTime.Now.ToString("dd MMMM yyyy, HH:mm:ss"));
            dateFormats.Add(DateTime.Now.ToString("MMM dd, yyyy"));

            // Set the ItemsSource of the ListView to the list of date formats
            listView.ItemsSource = dateFormats;

            // Set the content of the ContentDialog to the ListView
            dialog.Content = listView;

            // Make the insert button colored
            dialog.DefaultButton = ContentDialogButton.Primary;

            // Add an "Insert" button to the ContentDialog
            dialog.PrimaryButtonText = "OK";
            dialog.PrimaryButtonClick += (s, args) =>
            {
                string selectedFormat = listView.SelectedItem as string;
                string formattedDate = dateFormats[listView.SelectedIndex];
                box.Document.Selection.Text = formattedDate;
            };

            // Add a "Cancel" button to the ContentDialog
            dialog.SecondaryButtonText = "Cancel";

            // Show the ContentDialog
            await dialog.ShowAsync();
        }

        private String InsertTableInRichTextBox(int rows, int cols, int width)
        {
            //Create StringBuilder Instance
            StringBuilder strTableRtf = new StringBuilder();

            //beginning of rich text format
            strTableRtf.Append(@"{\rtf1 ");

            //Variable for cell width
            int cellWidth;

            //Start row
            strTableRtf.Append(@"\trowd");

            //Loop to create table string
            for (int i = 0; i < rows; i++)
            {
                strTableRtf.Append(@"\trowd");

                for (int j = 0; j < cols; j++)
                {
                    //Calculate cell end point for each cell
                    cellWidth = (j + 1) * width;

                    //A cell with width 1000 in each iteration.
                    strTableRtf.Append(@"\cellx" + cellWidth.ToString());
                }

                //Append the row in StringBuilder
                strTableRtf.Append(@"\intbl \cell \row");
            }
            strTableRtf.Append(@"\pard");
            strTableRtf.Append(@"}");
            var strTableString = strTableRtf.ToString();
            box.Document.Selection.SetText(TextSetOptions.FormatRtf, strTableString);
            return strTableString;

        }

        private void showinsiderinfo(object sender, RoutedEventArgs e)
        {
            ToggleThemeTeachingTip1.IsOpen = true;
        }

        private void AlignLeftButton_Click(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = box.Document.Selection;
            selectedText.ParagraphFormat.Alignment = ParagraphAlignment.Left;
        }

        private void AlignCenterButton_Click(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = box.Document.Selection;
            selectedText.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        }

        private void AlignRightButton_Click(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = box.Document.Selection;
            selectedText.ParagraphFormat.Alignment = ParagraphAlignment.Right;
        }

        private void NoneNumeral_Click(object sender, RoutedEventArgs e)
        {
            box.Document.Selection.ParagraphFormat.ListType = MarkerType.None;
            myListButton.IsChecked = false;
            myListButton.Flyout.Hide();
            box.Focus(FocusState.Keyboard);
        }

        private void DottedNumeral_Click(object sender, RoutedEventArgs e)
        {
            box.Document.Selection.ParagraphFormat.ListType = MarkerType.Bullet;
            myListButton.IsChecked = true;
            myListButton.Flyout.Hide();
            box.Focus(FocusState.Keyboard);
        }

        private void NumberNumeral_Click(object sender, RoutedEventArgs e)
        {
            box.Document.Selection.ParagraphFormat.ListType = MarkerType.Arabic;
            myListButton.IsChecked = true;
            myListButton.Flyout.Hide();
            box.Focus(FocusState.Keyboard);
        }

        private void LetterSmallNumeral_Click(object sender, RoutedEventArgs e)
        {
            box.Document.Selection.ParagraphFormat.ListType = MarkerType.LowercaseEnglishLetter;
            myListButton.IsChecked = true;
            myListButton.Flyout.Hide();
            box.Focus(FocusState.Keyboard);
        }

        private void LetterBigNumeral_Click(object sender, RoutedEventArgs e)
        {
            box.Document.Selection.ParagraphFormat.ListType = MarkerType.UppercaseEnglishLetter;
            myListButton.IsChecked = true;
            myListButton.Flyout.Hide();
            box.Focus(FocusState.Keyboard);
        }

        private void SmalliNumeral_Click(object sender, RoutedEventArgs e)
        {
            box.Document.Selection.ParagraphFormat.ListType = MarkerType.LowercaseRoman;
            myListButton.IsChecked = true;
            myListButton.Flyout.Hide();
            box.Focus(FocusState.Keyboard);
        }

        private void BigINumeral_Click(object sender, RoutedEventArgs e)
        {
            box.Document.Selection.ParagraphFormat.ListType = MarkerType.UppercaseRoman;
            myListButton.IsChecked = true;
            myListButton.Flyout.Hide();
            box.Focus(FocusState.Keyboard);
        }

        private void FindButton2_Click(object sender, RoutedEventArgs e)
        {
            textsplitview.IsPaneOpen = true;
        }

        private void closepane(object sender, RoutedEventArgs e)
        {
            textsplitview.IsPaneOpen = false;
        }

        #region Find and Replace

        private void RepAllBTN_Click(object Sender, RoutedEventArgs EvArgs)
        {
            if (ReplaceBox.Text == FindTextBox.Text)
            {
            }
            else if (ReplaceBox.Text.ToLower() == FindTextBox.Text.ToLower() && CaseSensBox.IsChecked == true && FullWordsBox.IsChecked == true)
            {

            }
            else if (ReplaceBox.Text.ToLower() == FindTextBox.Text.ToLower() && CaseSensBox.IsChecked != true)
            {

            }
            else
            {
                Replace(box, FindTextBox.Text, ReplaceBox.Text, true, CaseSensBox.IsChecked, FullWordsBox.IsChecked, true, ReplaceBox);
            }
        }

        public static void Replace(RichEditBox RichEditBox, string TextToFind, string TextToReplace, bool ReplaceAll, bool? caseSensitive, bool? matchWords, bool? none, TextBox replaceBox)
        {
            int i = 0;

            if (ReplaceAll == true)
            {
                string Value = GetText(RichEditBox);
                if (!(string.IsNullOrWhiteSpace(Value) && string.IsNullOrWhiteSpace(TextToFind) && string.IsNullOrWhiteSpace(TextToReplace)))
                {
                    RichEditBox.Document.Selection.SetRange(0, GetText(RichEditBox).Length);
                    if (caseSensitive == true)
                    {
                        i = FindAsInt(TextToFind, FindOptions.Case, RichEditBox);
                        _ = RichEditBox.Document.Selection.FindText(TextToFind, GetText(RichEditBox).Length, FindOptions.Case);
                        if (i > j)
                        {
                            try
                            {
                                RichEditBox.Document.Selection.SetText(TextSetOptions.FormatRtf, replaceBox.Text);
                            }
                            catch (StackOverflowException)
                            {
                                return;
                            }
                            RichEditBox.Document.Selection.SetText(TextSetOptions.FormatRtf, replaceBox.Text);
                            _ = RichEditBox.Focus(FocusState.Pointer);
                            Replace(RichEditBox, TextToFind, TextToReplace, true, true, false, false, replaceBox);
                        }
                        else
                        {
                            j = 0;
                            return;
                        }
                    }
                    if (matchWords == true)
                    {
                        i = FindAsInt(TextToFind, FindOptions.Word, RichEditBox);
                        _ = RichEditBox.Document.Selection.FindText(TextToFind, GetText(RichEditBox).Length, FindOptions.Word);
                        if (i > j)
                        {
                            try
                            {
                                RichEditBox.Document.Selection.SetText(TextSetOptions.FormatRtf, replaceBox.Text);
                            }
                            catch (StackOverflowException)
                            {
                                return;
                            }
                            RichEditBox.Document.Selection.SetText(TextSetOptions.FormatRtf, replaceBox.Text);
                            _ = RichEditBox.Focus(FocusState.Pointer);
                            Replace(RichEditBox, TextToFind, TextToReplace, true, false, true, false, replaceBox);
                        }
                        else
                        {
                            j = 0;
                            return;
                        }
                    }
                    if (none == true)
                    {
                        i = FindAsInt(TextToFind, FindOptions.None, RichEditBox);
                        _ = RichEditBox.Document.Selection.FindText(TextToFind, GetText(RichEditBox).Length, FindOptions.None);
                        if (i > j)
                        {
                            try
                            {
                                RichEditBox.Document.Selection.SetText(TextSetOptions.FormatRtf, replaceBox.Text);
                            }
                            catch (StackOverflowException)
                            {
                                return;
                            }
                            _ = RichEditBox.Focus(FocusState.Pointer);
                            Replace(RichEditBox, TextToFind, TextToReplace, true, false, false, true, replaceBox);
                        }
                        else
                        {
                            j = 0;
                            return;
                        }
                    }
                    _ = RichEditBox.Focus(FocusState.Pointer);
                }
            }
            else
            {
                RichEditBox.Document.Selection.SetRange(0, GetText(RichEditBox).Length);
                if (caseSensitive == true)
                {
                    _ = RichEditBox.Document.Selection.FindText(TextToFind, GetText(RichEditBox).Length, FindOptions.Case);
                    RichEditBox.Document.Selection.SetText(TextSetOptions.FormatRtf, replaceBox.Text);
                    _ = RichEditBox.Focus(FocusState.Pointer);
                }
                if (matchWords == true)
                {
                    _ = RichEditBox.Document.Selection.FindText(TextToFind, GetText(RichEditBox).Length, FindOptions.Word);
                    RichEditBox.Document.Selection.SetText(TextSetOptions.FormatRtf, replaceBox.Text);
                    _ = RichEditBox.Focus(FocusState.Pointer);
                }
                if (none == true)
                {
                    _ = RichEditBox.Document.Selection.FindText(TextToFind, GetText(RichEditBox).Length, FindOptions.None);
                    RichEditBox.Document.Selection.SetText(TextSetOptions.FormatRtf, replaceBox.Text);
                    _ = RichEditBox.Focus(FocusState.Pointer);
                }
                _ = RichEditBox.Focus(FocusState.Pointer);
            }
        }

        public static int FindAsInt(string textToFind, FindOptions options, RichEditBox FindREB)
        {
            ITextRange searchRange = FindREB.Document.GetRange(0, 0);
            int x = 0;
            while (searchRange.FindText(textToFind, TextConstants.MaxUnitCount, options) > 0)
            {
                x++;
            }
            return x;
        }

        private static int j = 0;

        public static string GetText(RichEditBox Richbox)
        {
            Richbox.Document.GetText(TextGetOptions.FormatRtf, out string Text);
            ITextRange Range = Richbox.Document.GetRange(0, Text.Length);
            Range.GetText(TextGetOptions.FormatRtf, out string Value);
            return Value;
        }

        private void FindBTN_Click(object Sender, RoutedEventArgs EvArgs)
        {
            box.Document.Selection.SetRange(0, GetText(box).Length);
            if (CaseSensBox.IsChecked == true)
            {
                _ = box.Document.Selection.FindText(FindTextBox.Text, GetText(box).Length, FindOptions.Case);
                _ = box.Focus(FocusState.Pointer);
            }
            if (FullWordsBox.IsChecked == true)
            {
                _ = box.Document.Selection.FindText(FindTextBox.Text, GetText(box).Length, FindOptions.Word);
                _ = box.Focus(FocusState.Pointer);
            }
            if (!CaseSensBox.IsChecked == true && !FullWordsBox.IsChecked == true)
            {
                _ = box.Document.Selection.FindText(FindTextBox.Text, GetText(box).Length, FindOptions.None);
                _ = box.Focus(FocusState.Pointer);
            }
        }

        private void RepBTN_Click(object Sender, RoutedEventArgs EvArgs)
        {
            Replace(box, FindTextBox.Text, ReplaceBox.Text, true, CaseSensBox.IsChecked, FullWordsBox.IsChecked, true, ReplaceBox);
        }

        private void CancelFindRepBTN_Click(object Sender, RoutedEventArgs EvArgs)
        {
            _ = box.Focus(FocusState.Pointer);
        }

        #endregion Find and Replace

        private void FontsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FontSizeBox_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args)
        {

        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }
    }
}
