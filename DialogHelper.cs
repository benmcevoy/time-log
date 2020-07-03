using System.Windows;

namespace TimeLog
{
    public static class DialogHelper
    {
        public static MessageBoxResult YesNo(Window owner, string message)
        {
            var dialog = new Dialog();
            
            return dialog.ShowDialog(owner, message,"Notepad", DialogButton.YesNo, MessageBoxImage.Question);
        }

        public static MessageBoxResult SaveDontSave(Window owner, string message)
        {
            var dialog = new Dialog();

            return dialog.ShowDialog(owner, message, "Notepad", DialogButton.SaveDontSaveCancel, MessageBoxImage.Exclamation);
        }

        public static MessageBoxResult Error(Window owner, string message)
        {
            var dialog = new Dialog();

            return dialog.ShowDialog(owner, message, "Notepad", DialogButton.OK, MessageBoxImage.Error);
        }
    }
}
