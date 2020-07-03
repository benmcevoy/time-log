using System;
using System.Windows.Controls;
using System.Windows.Input;
using TimeLog.Infrastructure;

namespace TimeLog.Commands.Formatting
{
    public class FormatSelectionDecodeUrl : Command
    {
        public FormatSelectionDecodeUrl(State state) : base(state) { }

        public override void Execute(object parameter)
        {
            var raw = State.TextBox.SelectedText;

            State.TextBox.SelectedText = Uri.UnescapeDataString(raw);
        }
        public override bool CanExecute(object parameter) => !string.IsNullOrEmpty(State.TextBox.SelectedText);
        public override string Name { get; } = "Format selection from url encoded";
        public override InputBinding[] ApplicationInputBindings() => null;
        public override string ContextMenuParentName { get; } = "Format";
        public override MenuItem ContextMenuItem
        {
            get => new MenuItem { Header = "Url decode", Command = this };
        }
    }
}
