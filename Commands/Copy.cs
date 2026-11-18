using Avalonia.Controls;
using Avalonia.Input;

namespace TimeLog.Commands
{
    public class Copy : Command
    {
        public Copy(State state) : base(state) { }

        public override void Execute(object? parameter) => State.TextBox.Copy();

        public override string Name { get; } = "Copy";
        
        public override int Order { get; } = 91;
        
        public override KeyBinding[] ApplicationInputBindings() => [];
        
        public override MenuItem ContextMenuItem
        {
            get => new() {Header = "Copy", Command = this, InputGesture = KeyGesture.Parse("Ctrl+C")};
        }

        public override bool CanExecute(object? parameter) => !string.IsNullOrEmpty(State.TextBox.SelectedText);
    }
}
