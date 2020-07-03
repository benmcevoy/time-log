using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using TimeLog.Infrastructure;

namespace TimeLog.Commands
{
    public sealed class SpellCheck : Command
    {
        private readonly MenuItem _speller;

        public SpellCheck(State state) : base(state)
        {
            _speller = new MenuItem { Name = "Timelog_Commands_SpellCheck", Header = Name, Command = this };

            State.TextBox.ContextMenu.Opened += Suggest;
        }

        private void Suggest(object sender, RoutedEventArgs args)
        {
            MenuItem spellingMenu = null;

            foreach (MenuItem contextMenuItem in State.TextBox.ContextMenu.Items)
            {
                if (contextMenuItem.Header.Equals("Spelling"))
                {
                    spellingMenu = contextMenuItem;
                    break;
                }
            }

            if (spellingMenu == null) return;
            spellingMenu.Items.Clear();

            var spellingError = State.TextBox.GetSpellingError(State.TextBox.CaretIndex);

            if (spellingError == null) return;

            var cmdIndex = 0;

            foreach (var suggestion in spellingError.Suggestions)
            {
                var menuItem = new MenuItem
                {
                    Header = suggestion,
                    FontWeight = FontWeights.Bold,
                    Command = EditingCommands.CorrectSpellingError,
                    CommandParameter = suggestion,
                    CommandTarget = State.TextBox
                };

                spellingMenu.Items.Insert(cmdIndex, menuItem);

                cmdIndex++;
            }
        }

        public override void Execute(object parameter) { }
        public override string Name { get; } = "Spelling";
        public override InputBinding[] ApplicationInputBindings() => null;
        public override MenuItem ContextMenuItem { get => _speller; }
    }
}
