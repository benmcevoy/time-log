using System;
using System.Globalization;
using System.Windows;

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

            var vm = new MainViewModel(new Repository(), new LogParser2(), new KeywordExtractor());
            var w = new MainWindow { ViewModel = vm };

            w.Show();
        }

        //private void SetFirstDayOfWeek(DayOfWeek firstDayOfWeek)
        //{
        //    var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        //    var uiculture = (CultureInfo)CultureInfo.CurrentUICulture.Clone();

        //    culture.DateTimeFormat.FirstDayOfWeek = firstDayOfWeek;
        //    uiculture.DateTimeFormat.FirstDayOfWeek = firstDayOfWeek;

        //    System.Threading.Thread.CurrentThread.CurrentCulture = culture;
        //    System.Threading.Thread.CurrentThread.CurrentUICulture = uiculture;
        //}
    }
}
