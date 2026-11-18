using Avalonia.Controls;
using Avalonia.Threading;

namespace TimeLog.Controls.Dialog
{
    public static class DialogHelper
    {
        public static MessageBoxResult Show(Window owner, string message, string caption = "Time Log",
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.None)
        {
            using (var source = new CancellationTokenSource())
            {
                var dialog = new Dialog();

                dialog.ShowDialogAsync(owner, message, caption, button, icon)
                    .ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());

                Dispatcher.UIThread.MainLoop(source.Token);

                return dialog.DialogResult.MessageBoxResult;
            }
        }

        public static DialogResult Prompt(Window owner, string message, string defaultValue, string caption = "Time Log")
        {
            using (var source = new CancellationTokenSource())
            {
                var dialog = new Dialog();

                dialog.PromptAsync(owner, message, defaultValue, caption)
                    .ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());

                Dispatcher.UIThread.MainLoop(source.Token);

                return dialog.DialogResult;
            }
        }
    }
}
