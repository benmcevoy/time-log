using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Media.TextFormatting;
using TimeLog.Data;
using TimeLog.Lexer;
using TimeLog.Parser;

namespace TimeLog
{
    public partial class MainWindow : Window
    {
        private TextLayout? _textBoxLayout;
        private ScrollViewer? _textBoxScrollViewer;


        public MainWindow()
        {
            InitializeComponent();

            Closing += MainWindow_Closing;
            Loaded += MainWindow_Loaded;

            // TODO: inject - needs config too
            ViewModel = new MainViewModel(
                new Repository("timesheet.txt", "timesheet_todo.txt"),
                new LogParser(new LineLexer()),
                new KeywordExtractor(),
                new State(this, tb));

            DataContext = ViewModel;
        }

        private void ViewModel_Synchronize(object? sender, EventArgs e)
            => FindDay();

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "SelectedDate") return;


            var first = GetFirstVisibleLine();
            var last = GetLastVisibleLine();
            var scrollToLine = ViewModel.LineNumber;
            var offset = (last - first) / 2;
            var currentline = first + offset;
            var totalLines = _textBoxLayout!.TextLines.Count;

            // scroll up/down - try to offset it to the middle of the view
            scrollToLine = (scrollToLine <= currentline)
                ? scrollToLine - offset
                : scrollToLine + offset;

            scrollToLine = (scrollToLine > totalLines) ? totalLines : scrollToLine;
            scrollToLine = (scrollToLine < 0) ? 0 : scrollToLine;

            tb.ScrollToLine(scrollToLine);
        }

        private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            Loaded -= MainWindow_Loaded;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            var sv = tb.GetTemplateDescendants().OfType<ScrollViewer>().First();
            var tp = tb.GetTemplateDescendants().OfType<TextPresenter>().First();

            _textBoxLayout = tp.TextLayout;
            _textBoxScrollViewer = sv;

            tb.ScrollToLine(tb.GetLineCount() - 1);

            ViewModel.Synchronize += ViewModel_Synchronize;
        }

        void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            stats.Focus();
            ViewModel.SaveCommand.Execute(null);
        }

        protected void SyncButton_Click(object sender, RoutedEventArgs e)
            => FindDay();

        private int GetFirstVisibleLine()
        {
            var top = new Point(0, _textBoxScrollViewer!.Offset.Y);
            var hit = _textBoxLayout!.HitTestPoint(top);

            return _textBoxLayout.GetLineIndexFromCharacterIndex(hit.CharacterHit.FirstCharacterIndex, false);
        }

        private int GetLastVisibleLine()
        {
            var top = new Point(0, _textBoxScrollViewer!.Offset.Y);
            var bottom = new Point(0, top.Y + _textBoxScrollViewer!.Viewport.Height);
            var hit = _textBoxLayout!.HitTestPoint(bottom);

            return _textBoxLayout.GetLineIndexFromCharacterIndex(hit.CharacterHit.FirstCharacterIndex, false);
        }

        private void FindDay()
        {
            ViewModel.FindDay(GetFirstVisibleLine(), GetLastVisibleLine());
        }

        public MainViewModel ViewModel { get; set; }
    }
}
