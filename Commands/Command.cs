using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;

namespace TimeLog
{
    public abstract class Command : ICommand
    {
        protected readonly State State;
        protected Command(State state) => State = state;
        public abstract void Execute(object? parameter);
        public abstract string Name { get; }
        public abstract KeyBinding[] ApplicationInputBindings();
        public virtual string ContextMenuParentName { get; } = "";
        public virtual MenuItem? ContextMenuItem { get; }
        public virtual bool CanExecute(object? parameter) => true;
        // This event is never used - suppress CS00067 with the add/remove
        public event EventHandler? CanExecuteChanged  { add { } remove { } }  
        public virtual int Order { get; } = 100;
    }
}
