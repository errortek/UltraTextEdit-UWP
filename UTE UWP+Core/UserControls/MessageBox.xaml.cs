using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UltraTextEdit_UWP.UserControls
{
    public sealed partial class MessageBox : UserControl
    {
        public MessageBox()
        {
            this.InitializeComponent();
            this.DataContext = this;
            if (Parent is MessageBoxesGrid || Parent is null)
            {

            }
            else
            {
                throw new ArgumentException("The parent of MessageBox must be a MessageBoxesGrid");
            }
        }

        private bool IsDesignTime
        {
            get
            {
#if DEBUG
                return Windows.ApplicationModel.DesignMode.DesignModeEnabled;
#else
            return false;
#endif
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Close();
            if (IsDesignTime == false)
            {
                SetValue(MessageBoxVisibilityProperty, Visibility.Collapsed);
            }
            else
            {
                SetValue(MessageBoxVisibilityProperty, Visibility.Visible);
            }
        }

        public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
        "Title", // The name of the property
        typeof(string), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata("Title") // Default value
        );

        [Browsable(true)]
        [Category("Common")]
        [Description("The title of the MessageBox")]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty FirstButtonTextProperty =
        DependencyProperty.Register(
        "FirstButtonText", // The name of the property
        typeof(string), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata("Yes") // Default value
        );

        [Browsable(true)]
        [Category("Common")]
        public string FirstButtonText
        {
            get { return (string)GetValue(FirstButtonTextProperty); }
            set { SetValue(FirstButtonTextProperty, value); }
        }

        public static readonly DependencyProperty SecondButtonTextProperty =
        DependencyProperty.Register(
        "SecondButtonText", // The name of the property
        typeof(string), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata("No") // Default value
        );

        [Browsable(true)]
        [Category("Common")]
        public string SecondButtonText
        {
            get { return (string)GetValue(SecondButtonTextProperty); }
            set { SetValue(SecondButtonTextProperty, value); }
        }

        public static readonly DependencyProperty CancelButtonTextProperty =
        DependencyProperty.Register(
        "CancelButtonText", // The name of the property
        typeof(string), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata("Cancel") // Default value
        );

        [Browsable(true)]
        [Category("Common")]
        public string CancelButtonText
        {
            get { return (string)GetValue(CancelButtonTextProperty); }
            set { SetValue(CancelButtonTextProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxWidthProperty =
        DependencyProperty.Register(
        "MessageBoxWidth", // The name of the property
        typeof(int), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata(0) // Default value
        );

        [Browsable(true)]
        [Category("Layout")]
        public int MessageBoxWidth
        {
            get { return (int)GetValue(MessageBoxWidthProperty); }
            set { SetValue(MessageBoxWidthProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxHeightProperty =
        DependencyProperty.Register(
        "MessageBoxHeight", // The name of the property
        typeof(int), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata(0) // Default value
        );

        [Browsable(true)]
        [Category("Layout")]
        public int MessageBoxHeight
        {
            get { return (int)GetValue(MessageBoxHeightProperty); }
            set { SetValue(MessageBoxHeightProperty, value); }
        }

        public static new readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(
        "Content", // The name of the property
        typeof(UIElement), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata(null) // Default value
        );

        public new UIElement Content
        {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public event RoutedEventHandler FirstButtonClick;

        public event RoutedEventHandler SecondButtonClick;

        public event RoutedEventHandler CancelButtonClick;

        public event RoutedEventHandler CloseButtonClick;

        public enum ButtonStyle
        {
            Win32,
            UWP
        };

        public static readonly DependencyProperty CloseButtonStyleProperty =
        DependencyProperty.RegisterAttached(
        "CloseButtonStyleClick", // The name of the property
        typeof(ButtonStyle), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata(ButtonStyle.Win32, OnCloseButtonStyleChanged) // Default value
        );

        private static void OnCloseButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MessageBox)d).SetValue();
        }

        private void SetValue()
        {
            if (CloseButtonStyle == ButtonStyle.Win32)
            {
                CloseButton.Width = 35;
            }
            if (CloseButtonStyle == ButtonStyle.UWP)
            {
                CloseButton.Width = 50;
            }
        }

        [Browsable(true)]
        [Category("Common")]
        public ButtonStyle CloseButtonStyle
        {
            get { return (ButtonStyle)GetValue(CloseButtonStyleProperty); }
            set { SetValue(CloseButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty IsCloseButtonEnabledProperty =
        DependencyProperty.RegisterAttached(
        "IsCloseButtonEnabled", // The name of the property
        typeof(bool), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata(true, CloseButtonEnabled) // Default value
        );

        private static void CloseButtonEnabled(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MessageBox)d).SetEnabledValue();
        }

        private void SetEnabledValue()
        {
            if (IsCloseButtonEnabled != false)
            {
                CloseButton.IsEnabled = true;
            }
            else
            {
                CloseButton.IsEnabled = false;
            }
        }

        [Browsable(true)]
        [Category("Common")]
        public bool IsCloseButtonEnabled
        {
            get { return (bool)GetValue(IsCloseButtonEnabledProperty); }
            set { SetValue(IsCloseButtonEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsSingleButtonProperty =
        DependencyProperty.RegisterAttached(
        "IsSingleButton", // The name of the property
        typeof(bool), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata(false, IsSingleButtonChanged) // Default value
        );

        private static void IsSingleButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MessageBox)d).SetSingleButtonValue();
        }

        private void SetSingleButtonValue()
        {
            if (IsSingleButton == true)
            {
                BTN2.Visibility = Visibility.Collapsed;
                BTN3.Visibility = Visibility.Collapsed;
            }
            else
            {
                BTN2.Visibility = Visibility.Visible;
                BTN3.Visibility = Visibility.Visible;
            }
        }

        [Browsable(true)]
        [Category("Common")]
        public bool IsSingleButton
        {
            get { return (bool)GetValue(IsSingleButtonProperty); }
            set { SetValue(IsSingleButtonProperty, value); }
        }

        public static readonly DependencyProperty RememberPositionProperty =
        DependencyProperty.RegisterAttached(
        "RememberPosition", // The name of the property
        typeof(bool), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata(true) // Default value
        );

        [Browsable(true)]
        [Category("Common")]
        public bool RememberPosition
        {
            get { return (bool)GetValue(RememberPositionProperty); }
            set { SetValue(RememberPositionProperty, value); }
        }

        public static readonly DependencyProperty PlaySoundProperty =
        DependencyProperty.RegisterAttached(
        "PlaySound", // The name of the property
        typeof(bool), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata(false) // Default value
        );

        [Browsable(true)]
        [Category("Common")]
        public bool PlaySound
        {
            get { return (bool)GetValue(PlaySoundProperty); }
            set { SetValue(PlaySoundProperty, value); }
        }

        public static readonly DependencyProperty BlockUserInputProperty =
        DependencyProperty.RegisterAttached(
        "BlockUserInput", // The name of the property
        typeof(bool), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata(true) // Default value
        );

        [Browsable(true)]
        [Category("Common")]
        public bool BlockUserInput
        {
            get { return (bool)GetValue(BlockUserInputProperty); }
            set { SetValue(BlockUserInputProperty, value); }
        }

        public static readonly DependencyProperty HasIconProperty =
        DependencyProperty.RegisterAttached(
        "HasIcon", // The name of the property
        typeof(bool), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata(false, IconChanged) // Default value
        );

        [Browsable(true)]
        [Category("Common")]
        public bool HasIcon
        {
            get { return (bool)GetValue(HasIconProperty); }
            set { SetValue(HasIconProperty, value); }
        }

        private static void IconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MessageBox)d).DetectIconChange();
        }

        private void DetectIconChange()
        {
            if (HasIcon == true)
            {
                TitleBarIcon.Visibility = Visibility.Visible;
            }
            else
            {
                TitleBarIcon.Visibility = Visibility.Collapsed;
            }
        }

        private static void BitmapIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MessageBox)d).DetectBitmapIconChange();
        }

        private void DetectBitmapIconChange()
        {
            if (Icon != null)
            {
                TitleBarIcon.Source = Icon;
            }
            else
            {

            }
        }

        public static readonly DependencyProperty IconProperty =
        DependencyProperty.RegisterAttached(
        "Icon", // The name of the property
        typeof(ImageSource), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata(null, BitmapIconChanged) // Default value
        );

        [Browsable(true)]
        [Category("Common")]
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxVisibilityProperty =
        DependencyProperty.RegisterAttached(
        "MessageBoxVisibility", // The name of the property
        typeof(Visibility), // The type of the property
        typeof(MessageBox), // The type of the owner class
        new PropertyMetadata(Visibility.Visible) // Default value
        );

        [Browsable(true)]
        [Category("Common")]
        public Visibility MessageBoxVisibility
        {
            get { return (Visibility)GetValue(MessageBoxVisibilityProperty); }
            set { SetValue(MessageBoxVisibilityProperty, value); }
        }

        private Point lastPosition;
        private bool isDragging;

        private void MessageBox_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            lastPosition = e.GetCurrentPoint(this).Position;
            (sender as Border).CapturePointer(e.Pointer);
            isDragging = true;
            var owner = Parent;
            if (owner is MessageBoxesGrid messageBoxesGrid)
            {
                uint lastIndex = (uint)(messageBoxesGrid.Children.Count - 1);
                uint currentIndex = (uint)messageBoxesGrid.Children.IndexOf(this);

                messageBoxesGrid.Children.Move(currentIndex, lastIndex);
            }
        }

        private void MessageBox_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            (sender as Border).ReleasePointerCapture(e.Pointer);
            isDragging = false;
            if (MessageBoxBorder.Margin.Left < 0)
            {
                MessageBoxBorder.Margin = new Thickness(0, MessageBoxBorder.Margin.Top, 0, 0);
            }
            if (MessageBoxBorder.Margin.Top < 0)
            {
                MessageBoxBorder.Margin = new Thickness(MessageBoxBorder.Margin.Left, 0, 0, 0);
            }
            if (MessageBoxBorder.Margin.Left > ActualWidth - MessageBoxBorder.ActualWidth)
            {
                MessageBoxBorder.Margin = new Thickness(ActualWidth - MessageBoxBorder.ActualWidth, MessageBoxBorder.Margin.Top, 0, 0);
            }
            if (MessageBoxBorder.Margin.Top > ActualHeight - 35)
            {
                MessageBoxBorder.Margin = new Thickness(MessageBoxBorder.Margin.Left, ActualHeight - 35, 0, 0);
            }
        }

        private void MessageBox_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (e.Pointer.IsInContact && isDragging == true)
            {
                var newPosition = e.GetCurrentPoint(this).Position;
                var deltaX = newPosition.X - lastPosition.X;
                var deltaY = newPosition.Y - lastPosition.Y;

                var margin = MessageBoxBorder.Margin;
                margin.Left += deltaX;
                margin.Top += deltaY;
                MessageBoxBorder.Margin = margin;

                lastPosition = newPosition;
            }
        }

        private void Border_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            isDragging = false;
        }

        private void Border_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            isDragging = false;
        }

        private void MessageBoxBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MessageBoxWidth > MessageBoxBorder.MinWidth)
            {
                BTN1.Width = (MessageBoxWidth - 55) / 3;
                BTN2.Width = (MessageBoxWidth - 55) / 3;
                BTN3.Width = (MessageBoxWidth - 55) / 3;
            }
            else if (MessageBoxBorder.Width <= MessageBoxBorder.MinWidth)
            {
                BTN1.Width = (300 - 55) / 3;
                BTN2.Width = (300 - 55) / 3;
                BTN3.Width = (300 - 55) / 3;
            }
        }

        protected void FirstButton_Click(object sender, RoutedEventArgs e)
        {
            if (FirstButtonClick != null)
                FirstButtonClick(this, new RoutedEventArgs());
            else
            {
                AllButtonsClickDefault();
            }
        }

        private void AllButtonsClickDefault()
        {
            Close();
        }

        private bool firstOpen = false;
        private bool isOn;

        public bool IsOn
        {
            get { return isOn; }
            private set
            {
                if (isOn != value)
                {
                    isOn = value;
                    UpdateParentBlockBorderVisibility();
                }
            }
        }

        public void SetMyProperty(bool value)
        {
            IsOn = value;
        }

        private void UpdateParentBlockBorderVisibility()
        {
            var owner = Parent;
            if (owner is MessageBoxesGrid messageBoxesGrid)
            {
                messageBoxesGrid.SetBlockBorder();
            }
        }

        public void Open()
        {
            MessageBoxVisibility = Visibility.Visible;
            if (RememberPosition == true && firstOpen == false)
            {
                double X = FullBox.ActualWidth;
                double X2 = Math.Round((X / 2) - (MessageBoxBorder.ActualWidth / 2));
                double Y = FullBox.ActualHeight;
                double Y2 = Math.Round((Y / 2) - (MessageBoxBorder.ActualHeight / 2));

                MessageBoxBorder.Margin = new Thickness(X2, Y2, 0, 0);
                MessageBoxBorder.HorizontalAlignment = HorizontalAlignment.Left;
                MessageBoxBorder.VerticalAlignment = VerticalAlignment.Top;
                firstOpen = true;
            }
            if (RememberPosition == false)
            {
                double X = FullBox.ActualWidth;
                double X2 = Math.Round((X / 2) - (MessageBoxBorder.ActualWidth / 2));
                double Y = FullBox.ActualHeight;
                double Y2 = Math.Round((Y / 2) - (MessageBoxBorder.ActualHeight / 2));

                MessageBoxBorder.Margin = new Thickness(X2, Y2, 0, 0);
                MessageBoxBorder.HorizontalAlignment = HorizontalAlignment.Left;
                MessageBoxBorder.VerticalAlignment = VerticalAlignment.Top;
                firstOpen = true;
            }
            if (PlaySound == true)
            {
                var mediaPlayer = new MediaPlayer();
                mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("C:\\Windows\\Media\\Windows Background.wav"));
                mediaPlayer.Play();
            }
            IsOn = true;
            var owner = Parent;
            if (owner is MessageBoxesGrid messageBoxesGrid)
            {
                uint lastIndex = (uint)(messageBoxesGrid.Children.Count - 1);
                uint currentIndex = (uint)messageBoxesGrid.Children.IndexOf(this);

                messageBoxesGrid.Children.Move(currentIndex, lastIndex);
            }
        }

        public void Close()
        {
            MessageBoxVisibility = Visibility.Collapsed;
            IsOn = false;
        }

        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            if (SecondButtonClick != null)
                SecondButtonClick(this, new RoutedEventArgs());
            else
            {
                AllButtonsClickDefault();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (CancelButtonClick != null)
                CancelButtonClick(this, new RoutedEventArgs());
            else
            {
                AllButtonsClickDefault();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (CloseButtonClick != null)
                CloseButtonClick(this, new RoutedEventArgs());
            else
            {
                AllButtonsClickDefault();
            }
        }

        private void UserControl_LayoutUpdated(object sender, object e)
        {

        }

        private void FullBox_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var owner = Parent;
            if (owner is MessageBoxesGrid messageBoxesGrid)
            {
                uint lastIndex = (uint)(messageBoxesGrid.Children.Count - 1);
                uint currentIndex = (uint)messageBoxesGrid.Children.IndexOf(this);

                messageBoxesGrid.Children.Move(currentIndex, lastIndex);
            }
        }
    }
}
