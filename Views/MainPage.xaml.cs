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

namespace UltraTextEdit_UWP.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private bool saved = true;
        public MainPage()
        {
            InitializeComponent();
            var appViewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            appViewTitleBar.BackgroundColor = Color.FromArgb(100, 242, 242, 242);
            appViewTitleBar.ButtonForegroundColor = (Color)Resources["SystemAccentColor"];
            appViewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            UpdateTitleBarLayout(coreTitleBar);

            Window.Current.SetTitleBar(AppTitleBar);

            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
            Window.Current.Activated += Current_Activated;


            if (BuildInfo.BeforeWin11)
            {
                string[] fonts = {
                "Arial",
                "Blackadder ITC",
                "Calibri",
                "Cambria",
                "Century Gothic",
                "Comic Sans MS",
                "Consolas",
                "Franklin Gothic",
                "Gill Sans MT",
                "Impact",
                "Ink Free",
                "Lucida Console",
                "Lucida Sans Unicode",
                "Segoe UI",
                "Sitka Display",
                "Symbol",
                "Trebuchet MS",
                "Verdana"
            };
                fontbox.ItemsSource = fonts;
            } else
            {
                string[] fonts = {
                "Arial",
                "Blackadder ITC",
                "Calibri",
                "Cambria",
                "Century Gothic",
                "Comic Sans MS",
                "Consolas",
                "Franklin Gothic",
                "Gill Sans MT",
                "Impact",
                "Ink Free",
                "Lucida Console",
                "Lucida Sans Unicode",
                "Segoe UI",
                "Segoe UI Variable Display",
                "Segoe UI Variable Small",
                "Segoe UI Variable Text",
                "Sitka Display",
                "Symbol",
                "Trebuchet MS",
                "Verdana"
            };
                fontbox.ItemsSource = fonts;
            }
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
            SolidColorBrush defaultForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];
            SolidColorBrush inactiveForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorDisabledBrush"];
            string themedark = ApplicationTheme.Dark.ToString();
            string themelight = ApplicationTheme.Light.ToString();

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

        private void FindBoxHighlightMatches()
        {
            FindBoxRemoveHighlights();

            Color highlightBackgroundColor = (Color)App.Current.Resources["SystemColorHighlightColor"];
            Color highlightForegroundColor = (Color)App.Current.Resources["SystemColorHighlightTextColor"];

            string textToFind = Find.Text;
            if (textToFind != null)
            {
                ITextRange searchRange = box.Document.GetRange(0, 0);
                while (searchRange.FindText(textToFind, TextConstants.MaxUnitCount, FindOptions.None) > 0)
                {
                    searchRange.CharacterFormat.BackgroundColor = highlightBackgroundColor;
                    searchRange.CharacterFormat.ForegroundColor = highlightForegroundColor;
                }
            }
        }
        private void Combo3_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Combo3.SelectedIndex = 2;

            if ((ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7)))
            {
                Combo3.TextSubmitted += Combo3_TextSubmitted;
            }
        }

        private void Combo3_TextSubmitted(ComboBox sender, ComboBoxTextSubmittedEventArgs args)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                bool isDouble = double.TryParse(sender.Text, out double newValue);

                // Set the selected item if:
                // - The value successfully parsed to double AND
                // - The value is in the list of sizes OR is a custom value between 8 and 100
                if (isDouble && (FontSizes.Contains(newValue) || (newValue < 100 && newValue > 8)))
                {
                    // Update the SelectedItem to the new value. 
                    sender.SelectedItem = newValue;
                    box.Document.Selection.CharacterFormat.Size = (float)newValue;
                }
                else
                {
                    // If the item is invalid, reject it and revert the text. 
                    sender.Text = sender.SelectedValue.ToString();

                    var dialog = new ContentDialog
                    {
                        Content = "The font size must be a number between 8 and 100.",
                        CloseButtonText = "Close",
                        DefaultButton = ContentDialogButton.Close
                    };
                    var task = dialog.ShowAsync();
                }
            }

            // Mark the event as handled so the framework doesn’t update the selected item automatically. 
            args.Handled = true;
        }
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

        private void FindBoxRemoveHighlights()
        {
            ITextRange documentRange = box.Document.GetRange(0, TextConstants.MaxUnitCount);
            SolidColorBrush defaultBackground = box.Background as SolidColorBrush;
            SolidColorBrush defaultForeground = box.Foreground as SolidColorBrush;

            documentRange.CharacterFormat.BackgroundColor = defaultBackground.Color;
            documentRange.CharacterFormat.ForegroundColor = defaultForeground.Color;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            // Extract the color of the button that was clicked.
            Button clickedColor = (Button)sender;
            var rectangle = (Windows.UI.Xaml.Shapes.Rectangle)clickedColor.Content;
            var color = ((Windows.UI.Xaml.Media.SolidColorBrush)rectangle.Fill).Color;

            box.Document.Selection.CharacterFormat.ForegroundColor = color;

            fontColorButton.Flyout.Hide();
            box.Focus(Windows.UI.Xaml.FocusState.Keyboard);
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

        private void ListStyleButton_IsCheckedChanged(Microsoft.UI.Xaml.Controls.ToggleSplitButton sender, Microsoft.UI.Xaml.Controls.ToggleSplitButtonIsCheckedChangedEventArgs args)
        {
            // Use the toggle button to turn the selected list style on or off.
            if (sender.IsChecked == true)
            {
                // On. Apply the list style selected in the drop down to the selected text.
                var listStyle = ((FrameworkElement)ListStylesListView.SelectedItem).Tag.ToString();
                ApplyListStyle(listStyle);
            }
            else
            {
                // Off. Make the selected text not a list,
                // but don't change the list style selected in the drop down.
                ApplyListStyle("none");
            }
        }

        private void ListStylesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listStyle = ((FrameworkElement)(e.AddedItems[0])).Tag.ToString();

            if (ListButton.IsChecked == true)
            {
                // Toggle button is on. Turn it off...
                if (listStyle == "none")
                {
                    ListButton.IsChecked = false;
                }
                else
                {
                    // or apply the new selection.
                    ApplyListStyle(listStyle);
                }
            }
            else
            {
                // Toggle button is off. Turn it on, which will apply the selection
                // in the IsCheckedChanged event handler.
                ListButton.IsChecked = true;
            }
        }

        private void ApplyListStyle(string listStyle)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                // Apply the list style to the selected text.
                var paragraphFormatting = selectedText.ParagraphFormat;
                if (listStyle == "none")
                {
                    paragraphFormatting.ListType = Windows.UI.Text.MarkerType.None;
                }
                else if (listStyle == "bullet")
                {
                    paragraphFormatting.ListType = Windows.UI.Text.MarkerType.Bullet;
                }
                else if (listStyle == "numeric")
                {
                    paragraphFormatting.ListType = Windows.UI.Text.MarkerType.Arabic;
                }
                else if (listStyle == "alpha")
                {
                    paragraphFormatting.ListType = Windows.UI.Text.MarkerType.UppercaseEnglishLetter;
                }
                selectedText.ParagraphFormat = paragraphFormatting;
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window.Current.Content is Frame rootFrame)
            {
                rootFrame.Navigate(typeof(SettingsPage));
            }
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                // Get the instance of ComboBox
                ComboBox fontbox = sender as ComboBox;

                // Get the ComboBox selected item text
                string selectedItems = fontbox.SelectedItem.ToString();
                box.Document.Selection.CharacterFormat.Name = selectedItems;
            }
        }
        private void ListButton2_IsCheckedChanged(Microsoft.UI.Xaml.Controls.ToggleSplitButton sender, Microsoft.UI.Xaml.Controls.ToggleSplitButtonIsCheckedChangedEventArgs args)
        {
            // Use the toggle button to turn the selected list style on or off.
            if ((sender).IsChecked == true)
            {
                // On. Apply the list style selected in the drop down to the selected text.
                var listStyle2 = ((FrameworkElement)ListStylesListView2.SelectedItem).Tag.ToString();
                ApplyListStyle(listStyle2);
            }
            else
            {
                // Off. Make the selected text not a list,
                // but don't change the list style selected in the drop down.
                ApplyListStyle("none");
            }

        }


        private void ListStylesListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var listStyle2 = ((FrameworkElement)e.AddedItems[0]).Tag.ToString();

            if (ListButton2.IsChecked == true)
            {
                // Toggle button is on. Turn it off...
                if (listStyle2 == "none")
                {
                    ListButton2.IsChecked = false;
                }
                else
                {
                    // or apply the new selection.
                    ApplyListStyle2(listStyle2);
                }
            }
            else
            {
                // Toggle button is off. Turn it on, which will apply the selection
                // in the IsCheckedChanged event handler.
                ListButton2.IsChecked = true;
            }
        }
        private void ApplyListStyle2(string listStyle2)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                // Apply the list style to the selected text.
                var paragraphFormatting = selectedText.ParagraphFormat;
                if (listStyle2 == "left")
                {
                    paragraphFormatting.Alignment = Windows.UI.Text.ParagraphAlignment.Left;
                }
                else if (listStyle2 == "right")
                {
                    paragraphFormatting.Alignment = Windows.UI.Text.ParagraphAlignment.Right;
                }
                else if (listStyle2 == "center")
                {
                    paragraphFormatting.Alignment = Windows.UI.Text.ParagraphAlignment.Center;
                }
                selectedText.ParagraphFormat = paragraphFormatting;
            }
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
                Save(true);
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
                Content = $"Version 1.0 (Build 19044.493)\n\n© 2021-2022 jpb",
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
                using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(FileAccessMode.Read))
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    var properties = await file.Properties.GetImagePropertiesAsync();
                    int width = (int)properties.Width;
                    int height = (int)properties.Height;

                    // Load the file into the Document property of the RichEditBox.
                    box.Document.Selection.InsertImage(width, height, 0, Windows.UI.Text.VerticalCharacterAlignment.Baseline, "img", randAccStream);
                }
            }
        }

        private void link(object sender, RoutedEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.FrameworkElement", "AllowFocusOnInteraction"))
                hyperlinkText.AllowFocusOnInteraction = true;
            box.Document.Selection.Link = $"\"{hyperlinkText.Text}\"";
            box.Document.Selection.CharacterFormat.ForegroundColor = (Color)XamlBindingHelper.ConvertValue(typeof(Color), "#6194c7");
            linkbutton.Flyout.Hide();
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

        private void hidebottombar(object sender, RoutedEventArgs e)
        {
            bottombartabitem.Visibility = Visibility.Visible;
            bottombar.Visibility = Visibility.Collapsed;
        }

        private void showbottombar(object sender, RoutedEventArgs e)
        {
            bottombartabitem.Visibility = Visibility.Collapsed;
            bottombar.Visibility = Visibility.Visible;
        }

        private void box_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer ScrollViewer = UIHelper.FindVisualChild<ScrollViewer>(box);
            if (ScrollViewer != null)
            {
                ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            }

        }
    private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
        //TODO
    }
}
    public static class UIHelper
    {
        public static childItem FindVisualChild<childItem>(this DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
