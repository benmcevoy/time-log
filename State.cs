using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Media.TextFormatting;

namespace TimeLog
{
    public class State : NotifyObject
    {
        private TextLayout? _textBoxLayout;
        private ScrollViewer? _textBoxScrollViewer;

        internal State(Window owner, TextBox textBox)
        {
            Owner = owner;
            TextBox = textBox;
        }

        public Window Owner { get; }

        public TextBox TextBox { get; }

        public TextLayout? TextBox_TextLayout
        {
            get
            {
                if (_textBoxLayout is null) 
                    _textBoxLayout = TextBox.GetTemplateDescendants().OfType<TextPresenter>().First().TextLayout;

                return _textBoxLayout;
            }
        }

        public ScrollViewer? TextBox_ScrollViewer
        {
            get
            {
                if (_textBoxScrollViewer is null) 
                    _textBoxScrollViewer = TextBox.GetTemplateDescendants().OfType<ScrollViewer>().First();

                return _textBoxScrollViewer;
            }
        }

        private FileInfo? _file;
        public FileInfo? File
        {
            get => _file;
            internal set
            {
                _file = value;
                OnPropertyChanged(nameof(File));
                OnPropertyChanged(nameof(FileName));
            }
        }

        public string FileName => File?.Name ?? "Untitled.txt";
        public bool IsDirty { get; internal set; }
        public string PluginFolder { get; internal set; } = "";

        private string _status = "";
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
    }
}
