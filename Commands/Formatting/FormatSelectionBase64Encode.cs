using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using TimeLog.Infrastructure;

namespace TimeLog.Commands.Formatting
{
    public class FormatSelectionBase64Encode : Command
    {
        public FormatSelectionBase64Encode(State state) : base(state) { }

        public override void Execute(object parameter)
        {
            var raw = State.TextBox.SelectedText;

            State.TextBox.SelectedText = Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));
        }

        public override string Name { get; } = "Format selection as base64";
        public override InputBinding[] ApplicationInputBindings() => null;
        public override string ContextMenuParentName { get; } = "Format";
        public override bool CanExecute(object parameter) => !string.IsNullOrEmpty(State.TextBox.SelectedText);
        public override MenuItem ContextMenuItem
        {
            get => new MenuItem {Header = "Base64 encode", Command = this};
        }
    }
}
