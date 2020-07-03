using System.Windows.Controls;
using System.Windows.Input;
using TimeLog.Infrastructure;

namespace TimeLog.Commands.Formatting
{
    public class FormatSelectionToLower : Command
    {
        public FormatSelectionToLower(State state) : base(state) { }

        public override void Execute(object parameter)
        {
            var raw = State.TextBox.SelectedText;

            State.TextBox.SelectedText = raw.ToLower();
        }

        public override string Name { get; } = "Format selection as lower case";
        public override InputBinding[] ApplicationInputBindings()
        {
            return new InputBinding[]
            {
                new KeyBinding(this, Key.U, ModifierKeys.Control )
            };
        }

        public override string ContextMenuParentName { get; } = "Format";
        public override bool CanExecute(object parameter) => !string.IsNullOrEmpty(State.TextBox.SelectedText);
        public override MenuItem ContextMenuItem
        {
            get => new MenuItem { Header = "To lower", Command = this, InputGestureText = "Ctrl+U" };
        }
    }
}
