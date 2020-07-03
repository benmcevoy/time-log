using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TimeLog.Infrastructure
{
    public class State : NotifyObject
    {
        internal State(Window owner, TextBox textBox)
        {
            Owner = owner;
            TextBox = textBox;
        }

        public Window Owner { get; }

        public TextBox TextBox { get; }

        private FileInfo _file;
        public FileInfo File
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
        public string PluginFolder { get; internal set; }
        
        private string _status;
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
