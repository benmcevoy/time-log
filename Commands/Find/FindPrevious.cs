using System;
using System.Windows.Controls;
using System.Windows.Input;
using TimeLog.Infrastructure;

namespace TimeLog.Commands.Find
{
    public class FindPrevious : Command
    {
        public FindPrevious(State state) : base(state) { }
        public override void Execute(object parameter)
        {
            if (Find.FindText == null) return;

            var textToFind = Find.FindText;
            var textToFindLength = textToFind.Length;
            // adding the SelectedText.Length means we always move backward
            var start = State.TextBox.CaretIndex - State.TextBox.SelectedText.Length;
            // this is very inefficient, should be paging the text in and searching the page
            // but so should so the textbox, so sue me.
            var textToSearch = State.TextBox.Text;
            var foundIndex = textToSearch.LastIndexOf(textToFind, start, StringComparison.CurrentCultureIgnoreCase);

            // not found
            if (foundIndex <= 0)
            {
                // try from end of text
                foundIndex = textToSearch.LastIndexOf(textToFind, textToSearch.Length, StringComparison.CurrentCultureIgnoreCase);
            }

            // really not found
            if (foundIndex <= 0) return;

            State.TextBox.Select(foundIndex, textToFindLength);
            State.TextBox.ScrollToLine(State.TextBox.GetLineIndexFromCharacterIndex(foundIndex));
        }

        public override string Name { get; } = "Find previous";
        public override InputBinding[] ApplicationInputBindings() => new[] {new KeyBinding(this, Key.F3, ModifierKeys.Shift)};
        public override MenuItem ContextMenuItem { get; }
        public bool CanExecute(object parameter) => !string.IsNullOrEmpty(State.TextBox.Text);
    }
}
