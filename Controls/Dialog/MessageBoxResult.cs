namespace TimeLog.Controls.Dialog
{
    public enum MessageBoxResult
    {
        Cancel = 2,
        No = 7,
        None = 0,
        OK = 1,
        Yes = 6
    }
    
    public class DialogResult
    {
        public DialogResult(MessageBoxResult result, string prompt)
        {
            MessageBoxResult = result;
            MessageBoxPromptValue = prompt;
        }
        
        public MessageBoxResult MessageBoxResult { get; }
        public string MessageBoxPromptValue { get; }
    }
}
