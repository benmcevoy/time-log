using System.Windows.Controls;
using System.Windows.Input;
using TimeLog.Infrastructure;

namespace TimeLog.Commands
{
    public class Goto : Command
    {
        public Goto(State state) : base(state) { }

        public override void Execute(object parameter)
        {
            var dialog = new Dialog();
            var result = dialog.Prompt(State.Owner, "Enter line number:", null,"Notepad");

            if (!result.Result) return;
            if (!int.TryParse(result.Value, out var lineNumber)) return;

            State.TextBox.ScrollToLine(lineNumber);
            State.TextBox.Select(State.TextBox.GetCharacterIndexFromLineIndex(lineNumber), 0);
        }

        public override string Name { get; } = "Go to line";

        public override InputBinding[] ApplicationInputBindings() =>
            new[] {new KeyBinding(this, Key.G, ModifierKeys.Control),};
        
        public override MenuItem ContextMenuItem { get; }

        public override bool CanExecute(object parameter) => !string.IsNullOrEmpty(State.TextBox.Text);
    }
}
