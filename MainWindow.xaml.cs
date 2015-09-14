using System;
using System.Windows;

namespace TimeLog
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
            Loaded += MainWindow_Loaded;
        }

        void ViewModel_Synchronize(object sender, EventArgs e)
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

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainWindow_Loaded;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            tb.ScrollToEnd();

            _viewModel.Synchronize += ViewModel_Synchronize;
            _viewModel.FindNextText += ViewModel_FindNextText;
            _viewModel.FindTextBox = tb;

            _viewModel.InsertText += ViewModel_InsertText;
        }

        private void ViewModel_InsertText(object sender, ValueEventArgs<string> e)
        {
            tb.SelectedText = e.Value;
            tb.CaretIndex += e.Value.Length;
            tb.SelectionLength = 0;
        }

        void ViewModel_FindNextText(object sender, ValueEventArgs<string> e)
        {
            tb.Find(e.Value);
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
