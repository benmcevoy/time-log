
using Avalonia.Controls;
using Avalonia.Input;

namespace TimeLog.Commands
{
    public class Paste : Command
    {
        public Paste(State state) : base(state) { }

        public override void Execute(object? parameter) => State.TextBox.Paste();

        public override string Name { get; } = "Paste";

        public override int Order { get; } = 92;

        public override KeyBinding[] ApplicationInputBindings() => [];

        public override MenuItem ContextMenuItem
        {
            get => new() {Header = "Paste", Command = this, InputGesture = KeyGesture.Parse("Ctrl+V")};
        }

        // public override bool CanExecute(object parameter)
        // {
        //     var text = ((IClipboard)AvaloniaLocator.Current.GetService(typeof(IClipboard))).GetTextAsync().Result;
        
        //     return !string.IsNullOrEmpty(text);
        // }
    }
}
