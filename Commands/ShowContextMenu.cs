using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TimeLog.Infrastructure;

namespace TimeLog.Commands
{
    public class ShowContextMenu : Command
    {
        public ShowContextMenu(State state) : base(state) { }

        public override void Execute(object parameter)
        {
            var rect = State.TextBox.GetRectFromCharacterIndex(State.TextBox.CaretIndex);
            var point = rect.BottomRight;

            State.TextBox.ContextMenu.PlacementTarget = State.TextBox;
            State.TextBox.ContextMenu.Placement = PlacementMode.RelativePoint;
            State.TextBox.ContextMenu.HorizontalOffset = point.X + State.TextBox.ContextMenu.ActualWidth;
            State.TextBox.ContextMenu.VerticalOffset = point.Y;
            State.TextBox.ContextMenu.IsOpen = true;
        }

        public override string Name { get; } = "Show context menu";
        public override InputBinding[] ApplicationInputBindings() => new[] { new KeyBinding(this, Key.Space, ModifierKeys.Control) };
        public override MenuItem ContextMenuItem { get; }
    }
}
