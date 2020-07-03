using System.Windows.Controls;
using System.Windows.Input;
using TimeLog.Infrastructure;

namespace TimeLog.Commands.Find
{
    public class FindNext : Command
    {
        public FindNext(State state) : base(state) { }
        public override void Execute(object parameter) => Find.FindNext(State);
        public override string Name { get; } = "Find next";
        public override InputBinding[] ApplicationInputBindings() => new[] { new KeyBinding(this, Key.F3, ModifierKeys.None) };
        public override MenuItem ContextMenuItem { get; }
        public override bool CanExecute(object parameter) => !string.IsNullOrEmpty(State.TextBox.Text);
    }
}
