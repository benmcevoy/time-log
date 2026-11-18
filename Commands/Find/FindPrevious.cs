using Avalonia.Input;

namespace TimeLog.Commands.Find
{
    public class FindPrevious : Command
    {
        public FindPrevious(State state) : base(state) { }
        public override void Execute(object? parameter)=> Find.FindPrevious(State);

        public override string Name { get; } = "Find previous";
        public override KeyBinding[] ApplicationInputBindings() =>
            [new KeyBinding { Command = this, Gesture = KeyGesture.Parse("Shift+F3") }];

        public override bool CanExecute(object? parameter) => !string.IsNullOrEmpty(State.TextBox.Text);
    }
}
