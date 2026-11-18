using System.Reflection;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;

namespace TimeLog.Controls.Dialog
{
    public partial class Dialog : Window
    {
        private MessageBoxResult _result = MessageBoxResult.None;

        public Dialog()
        {
            InitializeComponent();

            PromptValue.AttachedToVisualTree += (s, e) =>
            {
                if (PromptValue.IsVisible) PromptValue.Focus();
            };

            Value = string.Empty;
        }

        public Task<DialogResult> ShowDialogAsync(Window owner, string message, string? caption = null,
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.None)
        {
            Owner = owner;
            Title = caption;
            Message.Text = message;
            PromptValue.IsVisible = false;
            Message.MaxWidth = Width - 20;

            SetDialogIcon(icon);
            SetButtons(button);

            Console.WriteLine("dialog opening");
            return ShowDialog<DialogResult>(owner);
        }

        public Task<DialogResult> PromptAsync(Window owner, string message, string defaultValue, string caption)
        {
            Owner = owner;
            Title = caption;
            Message.Text = message;

            this.Focus();

            PromptValue.Text = defaultValue;
            PromptValue.IsVisible = true;
            PromptValue.Focus();

            SetDialogIcon(MessageBoxImage.Question);
            SetButtons(MessageBoxButton.OKCancel);

            OK.IsDefault = true;

            return ShowDialog<DialogResult>(owner);
        }

        private void SetDialogIcon(MessageBoxImage icon)
        {
            DialogIcon.IsVisible = false;

            switch (icon)
            {
                case MessageBoxImage.Error:
                    DialogIcon.Source = new Bitmap(GetResource("TimeLog.Icons.error.ico"));
                    DialogIcon.IsVisible = true;
                    break;

                case MessageBoxImage.Question:
                    DialogIcon.Source = new Bitmap(GetResource("TimeLog.Icons.question.ico"));
                    DialogIcon.IsVisible = true;
                    break;

                case MessageBoxImage.Exclamation:
                    DialogIcon.Source = new Bitmap(GetResource("TimeLog.Icons.warning.ico"));
                    DialogIcon.IsVisible = true;
                    break;

                case MessageBoxImage.Information:
                    DialogIcon.Source = new Bitmap(GetResource("TimeLog.Icons.information.ico"));
                    DialogIcon.IsVisible = true;
                    break;
            }
        }

        private void SetButtons(MessageBoxButton button)
        {
            ResetButtons(OK, Yes, No, Cancel, DontSave, Save);

            switch (button)
            {
                case MessageBoxButton.OK:
                    OK.IsVisible = true;
                    OK.IsDefault = true;
                    OK.IsCancel = true;
                    break;

                case MessageBoxButton.OKCancel:
                    OK.IsVisible = true;
                    Cancel.IsVisible = true;
                    Cancel.IsDefault = true;
                    Cancel.IsCancel = true;
                    break;

                case MessageBoxButton.YesNoCancel:
                    Yes.IsVisible = true;
                    No.IsVisible = true;
                    Cancel.IsVisible = true;
                    Cancel.IsDefault = true;
                    Cancel.IsCancel = true;
                    break;

                case MessageBoxButton.YesNo:
                    Yes.IsVisible = true;
                    No.IsVisible = true;
                    No.IsDefault = true;
                    No.IsCancel = true;
                    break;

                case MessageBoxButton.SaveDontSaveCancel:
                    Save.IsVisible = true;
                    DontSave.IsVisible = true;
                    Cancel.IsVisible = true;
                    Cancel.IsDefault = true;
                    Cancel.IsCancel = true;
                    break;

                default:
                    OK.IsVisible = true;
                    OK.IsDefault = true;
                    OK.IsCancel = true;
                    break;
            }
        }

        private static void ResetButtons(params Button[] buttons)
        {
            foreach (var button in buttons)
            {
                button.IsVisible = false;
                button.IsDefault = false;
            }
        }

        private void SetResult(object? sender, RoutedEventArgs e)
        {
            if (sender is null) return;

            _result = Enum.Parse<MessageBoxResult>(((Button)sender).Tag?.ToString() ?? "");

            Close(new DialogResult(_result, PromptValue.Text ?? ""));
        }

        private void Value_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            _result = MessageBoxResult.OK;

            Close(new DialogResult(_result, PromptValue.Text ?? ""));
        }

        private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
        {
            Value = PromptValue.Text ?? "";
            DialogResult = new DialogResult(_result, Value);
        }

        private static Stream GetResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream(resourceName)!;
        }

        public string Value { get; set; }

        public DialogResult DialogResult { get; set; } = new DialogResult(MessageBoxResult.None, "");
    }
}
