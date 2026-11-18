namespace TimeLog.Controls.Dialog
{
    public enum MessageBoxImage
    {
        Asterisk = 64, // The message box contains a symbol consisting of a lowercase letter i in a circle.
        Error = 16, // The message box contains a symbol consisting of white X in a circle with a red background.
        Exclamation =
            48, // The message box contains a symbol consisting of an exclamation point in a triangle with a yellow background.
        Hand = 16, // The message box contains a symbol consisting of a white X in a circle with a red background.
        Information = 64, // The message box contains a symbol consisting of a lowercase letter i in a circle.
        None = 0, // The message box contains no symbols.
        Question = 32, // The message box contains a symbol consisting of a question mark in a circle. The question mark message icon is no longer recommended because it does not clearly represent a specific type of message and because the phrasing of a message as a question could apply to any message type. In addition, users can confuse the question mark symbol with a help information symbol. Therefore, do not use this question mark symbol in your message boxes. The system continues to support its inclusion only for backward compatibility.
        Stop = 16, // The message box contains a symbol consisting of white X in a circle with a red background.
        Warning = 48 // The message box contains a symbol consisting of an exclamation point in a triangle with a yellow background.
    }
}
