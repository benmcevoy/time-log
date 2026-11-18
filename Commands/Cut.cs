using Avalonia.Controls;
using Avalonia.Input;

namespace TimeLog.Commands
{
    public class Cut : Command
    {
        public Cut(State state) : base(state) { }

        public override void Execute(object? parameter) => State.TextBox.Cut();

        public override string Name { get; } = "Cut";

        public override int Order { get; } = 90;

        public override KeyBinding[] ApplicationInputBindings() => [];
        public override MenuItem ContextMenuItem
        {
            get => new() {Header = "Cut", Command = this, InputGesture = KeyGesture.Parse("Ctrl+X")};
        }

       public override bool CanExecute(object? parameter) => !string.IsNullOrEmpty(State.TextBox.SelectedText);
    }
}
