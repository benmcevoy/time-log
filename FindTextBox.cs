using System;
using System.Windows.Controls;
using System.Windows;

namespace TimeLog
{
    public class FindTextBox : TextBox
    {
        public void Find(string findText)
        {
            FindText = findText;
            Find();
        }

        public void Find()
        {
            var start = CaretIndex;

            if (SelectedText.Length > 0)
            {
                start = start + SelectedText.Length;
            }

            Select(start, Text.Length);

            var selectedText = SelectedText;
            var startindex = selectedText.IndexOf(FindText, StringComparison.CurrentCultureIgnoreCase);

            if (startindex > 0)
            {
                Select(start + startindex, FindText.Length);
                ScrollToLine(GetLineIndexFromCharacterIndex(start + startindex));
            }
            else
            {
                Select(start, 0);
            }
        }

        public string FindText
        {
            get { return (string)GetValue(FindTextProperty); }
            set { SetValue(FindTextProperty, value); }
        }

        public static readonly DependencyProperty FindTextProperty =
            DependencyProperty.Register("FindText", typeof(string), typeof(FindTextBox), new UIPropertyMetadata(""));


    }
}
