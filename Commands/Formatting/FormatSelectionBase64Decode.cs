using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using TimeLog.Infrastructure;

namespace TimeLog.Commands.Formatting
{
    public class FormatSelectionBase64Decode : Command
    {
        public FormatSelectionBase64Decode(State state) : base(state) { }

        public override void Execute(object parameter)
        {
            try
            {
                var raw = State.TextBox.SelectedText;

                State.TextBox.SelectedText = Encoding.UTF8.GetString(Convert.FromBase64String(raw));
            }
            catch (Exception ex)
            {
                State.Status = ex.Message;
            }
        }

        public override bool CanExecute(object parameter) => !string.IsNullOrEmpty(State.TextBox.SelectedText);
        public override string Name { get; } = "Format selection from base64";
        public override InputBinding[] ApplicationInputBindings() => null;
        public override string ContextMenuParentName { get; } = "Format";
        public override MenuItem ContextMenuItem
        {
            get => new MenuItem { Header = "Base64 decode", Command = this };
        }
    }
}
