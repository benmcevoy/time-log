using Avalonia.Controls;
using Avalonia.Input;

namespace TimeLog.Commands.Formatting
{
    public class FormatSelectionToLower : Command
    {
        public FormatSelectionToLower(State state) : base(state) { }

        public override void Execute(object? parameter)
        {
            var raw = State.TextBox.SelectedText;

            State.TextBox.SelectedText = raw.ToLower();
        }

        public override string Name { get; } = "Format selection as lower case";
        public override KeyBinding[] ApplicationInputBindings()
        {
            return [new KeyBinding {Command = this, Gesture = KeyGesture.Parse("Ctrl+U")}];
        }

        public override string ContextMenuParentName { get; } = "Format";
        public override bool CanExecute(object? parameter) => !string.IsNullOrEmpty(State.TextBox.SelectedText);
        public override MenuItem ContextMenuItem
        {
            get => new MenuItem
            {
                Header = "To lower",
                Command = this,
                InputGesture = KeyGesture.Parse("Ctrl+U")
            };
        }
    }
}
