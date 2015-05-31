using System;

namespace TimeLog
{
    public class FindEventArgs : EventArgs
    {
        public FindEventArgs(string findText)
        {
            FindText = findText;
        }
        public string FindText { get; set; }
    }
}