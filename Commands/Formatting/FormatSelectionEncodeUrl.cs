using System;
using System.Windows.Controls;
using System.Windows.Input;
using TimeLog.Infrastructure;

namespace TimeLog.Commands.Formatting
{
    public class FormatSelectionEncodeUrl : Command
    {
        public FormatSelectionEncodeUrl(State state) : base(state) { }

        public override void Execute(object parameter)
        {
            var raw = State.TextBox.SelectedText;

            State.TextBox.SelectedText = Uri.EscapeDataString(raw);
        }

        public override string Name { get; } = "Format selection as url encoded";
        public override InputBinding[] ApplicationInputBindings() => null;
        public override string ContextMenuParentName { get; } = "Format";
        public override bool CanExecute(object parameter) => !string.IsNullOrEmpty(State.TextBox.SelectedText);
        public override MenuItem ContextMenuItem
        {
            get => new MenuItem { Header = "Url encode", Command = this };
        }
    }
}
