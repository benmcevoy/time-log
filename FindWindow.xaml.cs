using System.Windows;

namespace TimeLog
{
    /// <summary>
    /// Interaction logic for FindWindow.xaml
    /// </summary>
    public partial class FindWindow : Window
    {
        public FindWindow()
        {
            InitializeComponent();
            FindText.Focus();
            Loaded += FindWindow_Loaded;
        }

        void FindWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= FindWindow_Loaded;
            FindText.SelectAll();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public string Find { get { return FindText.Text; } set { FindText.Text = value; } }
    }
}
