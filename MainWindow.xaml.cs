using System;
using System.Windows;
using TimeLog.Data;
using TimeLog.Infrastructure;
using TimeLog.Lexer;
using TimeLog.Parser;

namespace TimeLog
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
            Loaded += MainWindow_Loaded;

            ViewModel = new MainViewModel(new Repository(), new LogParser(new LineLexer()), new KeywordExtractor(), new State(this, tb));
        }

        private void ViewModel_Synchronize(object sender, EventArgs e)
        {
            FindDay();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "SelectedDate") return;

            var scrollToLine = ViewModel.LineNumber;

            if (scrollToLine > 20)
            {
                scrollToLine = scrollToLine - 2;
            }

            tb.ScrollToLine(scrollToLine);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainWindow_Loaded;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            tb.ScrollToEnd();
            _viewModel.Synchronize += ViewModel_Synchronize;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            stats.Focus();
            ViewModel.SaveCommand.Execute(null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FindDay();
        }

        private void FindDay()
        {
            ViewModel.FindDay(tb.GetFirstVisibleLineIndex(), tb.GetLastVisibleLineIndex());
        }

        private MainViewModel _viewModel;
        public MainViewModel ViewModel { get { return _viewModel; } set { _viewModel = value; DataContext = _viewModel; } }
    }
}
