using Avalonia.Input;
using TimeLog.Controls.Dialog;

namespace TimeLog.Commands.Find
{
    public class Find : Command
    {
        private static string FindText = "";
        public Find(State state) : base(state) { }

        public static void FindNext(State state)
        {
            // null or empty - whitespace is OK
            if (string.IsNullOrEmpty(FindText))
            {
                new Find(state).Execute(false);
                return;
            }

            var textToFind = FindText;
            var textToFindLength = FindText.Length;
            // adding the SelectedText.Length means we always move forward
            var start = state.TextBox.CaretIndex + state.TextBox.SelectedText.Length;
            // this is very inefficient, should be paging the text in and searching the page
            // but so should so the textbox, so sue me.
            var textToSearch = state.TextBox.Text ?? "";
            var foundIndex = textToSearch.IndexOf(textToFind, start, StringComparison.CurrentCultureIgnoreCase);

            // not found
            if (foundIndex <= 0)
            {
                // try from start of text
                foundIndex = textToSearch.IndexOf(textToFind, 0, StringComparison.CurrentCultureIgnoreCase);
            }

            // really not found
            if (foundIndex <= 0) return;

            state.TextBox.CaretIndex = foundIndex;
            state.TextBox.ClearSelection();

            state.TextBox.SelectionStart = foundIndex;
            state.TextBox.SelectionEnd = foundIndex + textToFindLength;
        }

        public static void FindPrevious(State state)
        {

            // null or empty - whitespace is OK
            if (string.IsNullOrEmpty(FindText))
            {
                new Find(state).Execute(true);
                return;
            }

            var textToFind = FindText;
            var textToFindLength = textToFind.Length;
            // adding the SelectedText.Length means we always move backward
            var start = state.TextBox.CaretIndex - state.TextBox.SelectedText.Length;
            // this is very inefficient, should be paging the text in and searching the page
            // but so should so the textbox, so sue me.
            var textToSearch = state.TextBox.Text ?? "";
            var foundIndex = textToSearch.LastIndexOf(textToFind, start, StringComparison.CurrentCultureIgnoreCase);

            // not found
            if (foundIndex <= 0)
            {
                // try from end of text
                foundIndex = textToSearch.LastIndexOf(textToFind, textToSearch.Length, StringComparison.CurrentCultureIgnoreCase);
            }

            // really not found
            if (foundIndex <= 0) return;

            state.TextBox.CaretIndex = foundIndex;
            state.TextBox.ClearSelection();
            state.TextBox.SelectionStart = foundIndex;
            state.TextBox.SelectionEnd = foundIndex + textToFindLength;
        }

        public override void Execute(object? parameter)
        {
            var dialogResult = DialogHelper.Prompt(State.Owner, "Find", "");

            if (dialogResult.MessageBoxResult == MessageBoxResult.Cancel) return;

            FindText = dialogResult.MessageBoxPromptValue;

            if (string.IsNullOrEmpty(FindText)) return;

            if (parameter is not null && parameter is bool)
            {
                if (((bool)parameter) == true)
                {
                    FindPrevious(State);
                    return;
                }
            }

            FindNext(State);

        }

        public override string Name { get; } = "Find";

        public override KeyBinding[] ApplicationInputBindings()
            => [new KeyBinding { Command = this, Gesture = KeyGesture.Parse("Ctrl+F") }];

        public override bool CanExecute(object? parameter) => !string.IsNullOrEmpty(State.TextBox.Text);
    }
}
