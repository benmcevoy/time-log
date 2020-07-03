using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace TimeLog
{
    public partial class Dialog : Window
    {
        private MessageBoxResult _result = MessageBoxResult.None;

        public Dialog()
        {
            InitializeComponent();
        }

        public MessageBoxResult ShowDialog(Window owner, string message, string caption = null, DialogButton button = DialogButton.OK,
            MessageBoxImage icon = MessageBoxImage.None)
        {
            Owner = owner;
            Title = caption;
            Message.Text = message;
            Value.Visibility = Visibility.Collapsed;
            Message.MaxWidth = Width - 20;

            SetIcon(icon);
            SetButtons(button);
            ShowDialog();

            return _result;
        }

        public ValueResult Prompt(Window owner, string message, string defaultValue, string caption)
        {
            Owner = owner;
            Title = caption;
            Message.Text = message;

            Value.Text = defaultValue;
            Value.Visibility = Visibility.Visible;
            Value.Focus();

            SetIcon(MessageBoxImage.Question);
            SetButtons(DialogButton.OKCancel);

            OK.IsDefault = true;

            ShowDialog();

            return new ValueResult
            {
                Value = Value.Text,
            };
        }

        private void SetIcon(MessageBoxImage icon)
        {
            Icon.Visibility = Visibility.Collapsed;

            switch (icon)
            {
                case MessageBoxImage.Error:
                    Icon.Source = new BitmapImage(new Uri("pack://application:,,,/TimeLog;component/Icons/error.ico"));
                    Icon.Visibility = Visibility.Visible;
                    break;

                case MessageBoxImage.Question:
                    Icon.Source = new BitmapImage(new Uri("pack://application:,,,/TimeLog;component/Icons/question.ico"));
                    Icon.Visibility = Visibility.Visible;
                    break;

                case MessageBoxImage.Exclamation:
                    Icon.Source = new BitmapImage(new Uri("pack://application:,,,/TimeLog;component/Icons/warning.ico"));
                    Icon.Visibility = Visibility.Visible;
                    break;

                case MessageBoxImage.Information:
                    Icon.Source = new BitmapImage(new Uri("pack://application:,,,/TimeLog;component/Icons/information.ico"));
                    Icon.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void SetButtons(DialogButton button)
        {
            ResetButtons(OK, Yes, No, Cancel, DontSave, Save);

            switch (button)
            {
                case DialogButton.OK:
                    OK.Visibility = Visibility.Visible;
                    OK.IsDefault = true;
                    OK.IsCancel = true;
                    break;

                case DialogButton.OKCancel:
                    OK.Visibility = Visibility.Visible;
                    Cancel.Visibility = Visibility.Visible;
                    Cancel.IsDefault = true;
                    Cancel.IsCancel = true;
                    break;

                case DialogButton.YesNoCancel:
                    Yes.Visibility = Visibility.Visible;
                    No.Visibility = Visibility.Visible;
                    Cancel.Visibility = Visibility.Visible;
                    Cancel.IsDefault = true;
                    Cancel.IsCancel = true;
                    break;

                case DialogButton.YesNo:
                    Yes.Visibility = Visibility.Visible;
                    No.Visibility = Visibility.Visible;
                    No.IsDefault = true;
                    No.IsCancel = true;
                    break;

                case DialogButton.SaveDontSaveCancel:
                    Save.Visibility = Visibility.Visible;
                    DontSave.Visibility = Visibility.Visible;
                    Cancel.Visibility = Visibility.Visible;
                    Cancel.IsDefault = true;
                    Cancel.IsCancel = true;
                    break;

                default:
                    OK.Visibility = Visibility.Visible;
                    OK.IsDefault = true;
                    OK.IsCancel = true;
                    break;
            }
        }

        private static void ResetButtons(params Button[] buttons)
        {
            foreach (var button in buttons)
            {
                button.Visibility = Visibility.Collapsed;
                button.IsDefault = false;
            }
        }

        private void SetResult(object sender, RoutedEventArgs e)
        {
            _result = (MessageBoxResult)Enum.Parse(typeof(MessageBoxResult), ((Button)sender).Tag.ToString());

            Close();
        }

        private void Value_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            _result = MessageBoxResult.OK;
            Close();
        }
    }

    public class ValueResult
    {
        public string Value { get; set; }
        public bool Result => !string.IsNullOrEmpty(Value);
    }

    public enum DialogButton
    {
        /// <summary>The message box displays an OK button.</summary>
        OK = 0,
        /// <summary>The message box displays OK and Cancel buttons.</summary>
        OKCancel = 1,
        /// <summary>The message box displays Yes, No, and Cancel buttons.</summary>
        YesNoCancel = 3,
        /// <summary>The message box displays Yes and No buttons.</summary>
        YesNo = 4,
        SaveDontSaveCancel = 5
    }
}
