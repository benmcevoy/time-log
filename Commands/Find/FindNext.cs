using Avalonia.Input;

namespace TimeLog.Commands.Find
{
    public class FindNext : Command
    {
        public FindNext(State state) : base(state) { }
        public override void Execute(object? parameter) => Find.FindNext(State);
        public override string Name { get; } = "Find next";
        public override KeyBinding[] ApplicationInputBindings() =>
            [new KeyBinding {Command = this, Gesture = KeyGesture.Parse("F3")}];

        public override bool CanExecute(object? parameter) => !string.IsNullOrEmpty(State.TextBox.Text);
    }
}
