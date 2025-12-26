using System.Windows;
using System.Windows.Media;

namespace UI.Views
{
    public enum MessageType
    {
        Info,
        Success,
        Warning,
        Error
    }

    public partial class CustomMessageBox : Window
    {
        public bool Result { get; private set; }

        public CustomMessageBox()
        {
            InitializeComponent();
        }

        private void SetIcon(MessageType type)
        {
            switch (type)
            {
                case MessageType.Success:
                    IconBorder.Background = new SolidColorBrush(Color.FromArgb(0x1A, 0x22, 0xC5, 0x5E));
                    IconText.Text = "✓";
                    IconText.Foreground = new SolidColorBrush(Color.FromRgb(0x22, 0xC5, 0x5E));
                    break;
                case MessageType.Warning:
                    IconBorder.Background = new SolidColorBrush(Color.FromArgb(0x1A, 0xF5, 0x9E, 0x0B));
                    IconText.Text = "⚠";
                    IconText.Foreground = new SolidColorBrush(Color.FromRgb(0xF5, 0x9E, 0x0B));
                    break;
                case MessageType.Error:
                    IconBorder.Background = new SolidColorBrush(Color.FromArgb(0x1A, 0xEF, 0x44, 0x44));
                    IconText.Text = "✕";
                    IconText.Foreground = new SolidColorBrush(Color.FromRgb(0xEF, 0x44, 0x44));
                    break;
                default:
                    IconBorder.Background = new SolidColorBrush(Color.FromArgb(0x1A, 0x63, 0x66, 0xF1));
                    IconText.Text = "ℹ";
                    IconText.Foreground = new SolidColorBrush(Color.FromRgb(0x63, 0x66, 0xF1));
                    break;
            }
        }

        private void PrimaryButton_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            DialogResult = true;
            Close();
        }

        private void SecondaryButton_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            DialogResult = false;
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            DialogResult = false;
            Close();
        }

        public static void Show(string message, string title = "Message", MessageType type = MessageType.Info)
        {
            var msgBox = new CustomMessageBox();
            msgBox.TitleText.Text = title;
            msgBox.MessageText.Text = message;
            msgBox.SetIcon(type);
            msgBox.Owner = System.Windows.Application.Current.MainWindow;
            msgBox.ShowDialog();
        }

        public static bool ShowConfirm(string message, string title = "Confirm", MessageType type = MessageType.Warning)
        {
            var msgBox = new CustomMessageBox();
            msgBox.TitleText.Text = title;
            msgBox.MessageText.Text = message;
            msgBox.SetIcon(type);
            msgBox.SecondaryButton.Content = "No";
            msgBox.SecondaryButton.Visibility = Visibility.Visible;
            msgBox.PrimaryButton.Content = "Yes";
            msgBox.Owner = System.Windows.Application.Current.MainWindow;
            msgBox.ShowDialog();
            return msgBox.Result;
        }

        public static void ShowSuccess(string message, string title = "Success")
        {
            Show(message, title, MessageType.Success);
        }

        public static void ShowError(string message, string title = "Error")
        {
            Show(message, title, MessageType.Error);
        }

        public static void ShowWarning(string message, string title = "Warning")
        {
            Show(message, title, MessageType.Warning);
        }
    }
}

