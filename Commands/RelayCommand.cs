using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace TimeLog.Commands
{
    public class RelayCommand : ICommand
    {
        readonly Action _execute;
        readonly Func<bool>? _canExecute;

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action execute) : this(execute, null) { }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action execute, Func<bool>? canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        // This event is never used - suppress CS00067 with the add/remove
        public event EventHandler? CanExecuteChanged  { add { } remove { } }  

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(object? parameter) => _canExecute?.Invoke() != false;
        
        public void Execute(object? parameter) => _execute();
    }
}