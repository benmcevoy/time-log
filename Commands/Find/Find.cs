using System;
using System.Windows.Controls;
using System.Windows.Input;
using TimeLog.Infrastructure;

namespace TimeLog.Commands.Find
{
    public class Find : Command
    {
        public static string FindText;

        public Find(State state) : base(state) { }

        public static void FindNext(State state)
        {
            if (FindText == null) return;

            var textToFind = FindText;
            var textToFindLength = FindText.Length;
            // adding the SelectedText.Length means we always move forward
            var start = state.TextBox.CaretIndex + state.TextBox.SelectedText.Length;
            // this is very inefficient, should be paging the text in and searching the page
            // but so should so the textbox, so sue me.
            var textToSearch = state.TextBox.Text;
            var foundIndex = textToSearch.IndexOf(textToFind, start, StringComparison.CurrentCultureIgnoreCase);

            // not found
            if (foundIndex <= 0)
            {
                // try from start of text
                foundIndex = textToSearch.IndexOf(textToFind, 0, StringComparison.CurrentCultureIgnoreCase);
            }

            // really not found
            if (foundIndex <= 0) return;

            state.TextBox.Select(foundIndex, textToFindLength);
            state.TextBox.ScrollToLine(state.TextBox.GetLineIndexFromCharacterIndex(foundIndex));
        }

        public override void Execute(object parameter)
        {
            var dialog = new Dialog();
            var result = dialog.Prompt(State.Owner, "Find?", State.TextBox.SelectedText, "Notepad");

            if (!result.Result) return;

            FindText = result.Value;

            FindNext(State);
        }

        public override string Name { get; } = "Find";
        public override InputBinding[] ApplicationInputBindings()
        {
            return new[] { new KeyBinding(this, Key.F, ModifierKeys.Control), };
        }

        public override MenuItem ContextMenuItem { get; }

        public override bool CanExecute(object parameter) => !string.IsNullOrEmpty(State.TextBox.Text);

    }
}
