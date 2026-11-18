using Avalonia.Controls;
using Avalonia.Input;

namespace TimeLog.Commands
{
    public class InsertDateTime : Command
    {
        private readonly State _state;

        public InsertDateTime(State state) : base(state) => _state = state;

        public override void Execute(object? parameter)
        {
            InsertText(_state.TextBox, $"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}");
        }

        private static void InsertText(TextBox textBox, string text)
        {
            textBox.SelectedText = text;
            textBox.ClearSelection();
            textBox.CaretIndex += text.Length;
        }

        public override string Name { get; } = "Insert date";

        public override KeyBinding[] ApplicationInputBindings() =>
        [
            new KeyBinding {Command = this, Gesture = KeyGesture.Parse("F5")}
        ];

        public override string ContextMenuParentName { get; } = "Insert";

        public override MenuItem ContextMenuItem
        {
            get => new() { Header = "Date and time", Command = this, InputGesture = KeyGesture.Parse("F5")};
        }
    }
}
