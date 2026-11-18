using Avalonia.Controls;
using Avalonia.Input;

namespace TimeLog.Commands.Formatting
{
    public class FormatSelectionToUpper : Command
    {
        public FormatSelectionToUpper(State state) : base(state) { }

        public override void Execute(object? parameter)
        {
            var raw = State.TextBox.SelectedText;

            State.TextBox.SelectedText = raw.ToUpper();
        }

        public override string Name { get; } = "Format selection as upper case";
        public override KeyBinding[] ApplicationInputBindings()
        {
            return new[] {new KeyBinding {Command = this, Gesture = KeyGesture.Parse("Ctrl+Shift+U")},};
        }

        public override string ContextMenuParentName { get; } = "Format";
        public override bool CanExecute(object? parameter) => !string.IsNullOrEmpty(State.TextBox.SelectedText);
        public override MenuItem ContextMenuItem
        {
            get => new MenuItem {Header = "To upper", Command = this, InputGesture = KeyGesture.Parse("Ctrl+Shift+U")};
        }
    }
}
