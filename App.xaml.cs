using System.Windows;
using TimeLog.Data;
using TimeLog.Lexer;
using TimeLog.Parser;

namespace TimeLog
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            
            var w = new MainWindow {  };

            w.Show();
        }
    }
}
