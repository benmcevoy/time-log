using Avalonia.Input;
using TimeLog.Controls.Dialog;

namespace TimeLog.Commands
{
    public class Goto : Command
    {
        public Goto(State state) : base(state) { }

        public override void Execute(object? parameter)
        {
            var dialog = DialogHelper.Prompt(State.Owner, "Enter line number:", "");

            if (dialog.MessageBoxResult == MessageBoxResult.Cancel) return;
            if (!int.TryParse(dialog.MessageBoxPromptValue, out var lineNumber)) return;

            var totalLines = State.TextBox_TextLayout!.TextLines.Count;

            lineNumber = (lineNumber >= totalLines) ? totalLines - 1 : lineNumber;
            lineNumber = (lineNumber < 0) ? 0 : lineNumber;

            State.TextBox.ScrollToLine(lineNumber);
        }

        public override string Name { get; } = "Go to line";

        public override KeyBinding[] ApplicationInputBindings() =>
            [new KeyBinding { Command = this, Gesture = KeyGesture.Parse("Ctrl+G") }];

        public override bool CanExecute(object? parameter) => !string.IsNullOrEmpty(State.TextBox.Text);
    }
}
