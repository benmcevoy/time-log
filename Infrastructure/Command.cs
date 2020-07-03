using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace TimeLog.Infrastructure
{
    public abstract class Command : ICommand
    {
        protected readonly State State;
        protected Command(State state) => State = state;
        public abstract void Execute(object parameter);
        public abstract string Name { get; }
        public abstract InputBinding[] ApplicationInputBindings();
        public virtual string ContextMenuParentName { get; }
        public abstract MenuItem ContextMenuItem { get; }
        public virtual bool CanExecute(object parameter) => true;
        public virtual event EventHandler CanExecuteChanged;
    }
}
