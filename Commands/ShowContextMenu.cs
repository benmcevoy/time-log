using Avalonia.Input;

namespace TimeLog.Commands
{
    public class ShowContextMenu : Command
    {
        public ShowContextMenu(State state) : base(state) { }

        public override void Execute(object? parameter)
            => State.TextBox.ContextMenu?.Open();

        public override string Name { get; } = "Show context menu";

        public override KeyBinding[] ApplicationInputBindings()
            => [new KeyBinding { Command = this, Gesture = KeyGesture.Parse("Ctrl+Space") }];
    }
}
