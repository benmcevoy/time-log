using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Timers;
using System.Diagnostics;
using System.Windows.Input;
using TimeLog.Commands;
using TimeLog.Commands.Find;
using TimeLog.Commands.Formatting;
using TimeLog.Data;
using TimeLog.Infrastructure;
using TimeLog.Model;
using TimeLog.Parser;

namespace TimeLog
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Timer _timer;
        private readonly IParser _parser;
        private readonly KeywordExtractor _keywordExtractor;
        private readonly State _state;
        private readonly Repository _repository;
        private Log _log;

        public MainViewModel(Repository repository, IParser parser, KeywordExtractor keywordExtractor, State state)
        {
            _repository = repository;
            _parser = parser;
            _keywordExtractor = keywordExtractor;
            _state = state;

            SaveCommand = new RelayCommand(_ => Save());
            SyncCommand = new RelayCommand(_ => Sync());
            NextDayCommand = new RelayCommand(_ => NextDay());
            PreviousDayCommand = new RelayCommand(_ => PreviousDay());
            InsertCommand = new RelayCommand(_ => Insert());

            _timer = new Timer(1000) { Enabled = false };
            _timer.Elapsed += Timer_Elapsed;

            _text = _repository.Load();
            _todo = _repository.LoadToDo();

            Find = new Find(state);
            FindNext = new FindNext(state);
            FindPrevious = new FindPrevious(state);
            Goto = new Goto(state);
            ShowContextMenu = new ShowContextMenu(state);
            FormatSelectionBase64Decode = new FormatSelectionBase64Decode(state);
            FormatSelectionBase64Encode = new FormatSelectionBase64Encode(state);
            FormatSelectionDecodeUrl = new FormatSelectionDecodeUrl(state);
            FormatSelectionEncodeUrl = new FormatSelectionEncodeUrl(state);
            FormatSelectionToLower = new FormatSelectionToLower(state);
            FormatSelectionToUpper = new FormatSelectionToUpper(state);
            SpellCheck = new SpellCheck(state);

            EnsureDateLine();
        }

        private void Insert()
        {
            var text = "\r\n" + DateTime.Today.ToString(LogParser.LogDateFormat) + "\r\n" + LogParser.TheIdealLine + "\r\n";

            _state.TextBox.SelectedText = text;
            _state.TextBox.CaretIndex += text.Length;
            _state.TextBox.SelectionLength = 0;
        }

        private static int RoundToNearest(int value, int roundTo, bool useCeiling = false)
        {
            if (useCeiling) return (int)Math.Ceiling(value / (double)roundTo) * roundTo;

            return (value / roundTo) * roundTo;
        }

        public void FindDay(int startLine, int endLine)
        {
            if (_log != null)
            {
                var date = (from d in _log.Days where d.LineNumber >= startLine && d.LineNumber < endLine select d.Date).FirstOrDefault();

                if (date != DateTime.MinValue)
                {
                    SelectedDate = date;
                }
            }
        }

        private void EnsureDateLine()
        {
            Parse();

            if (_log.Days.All(d => d.Date != DateTime.Today))
            {
                _text = _text + string.Format("\r\n\r\n{0}\r\n{1}\r\n\r\n\r\n", DateTime.Today.ToString(LogParser.LogDateFormat), LogParser.TheIdealLine);
            }
        }

        private void Parse()
        {
            var lines = _text.Split('\n');
            _log = _parser.Parse(lines);
            Keywords = _keywordExtractor.Extract(_log);
            Statistics = _log.GetStatistics(SelectedDate);
            Status = "Statistics generated";

            RemainToday = _log.GetRemainToday();
        }

        private void NextDay()
        {
            SelectedDate = SelectedDate.AddDays(1);
        }

        private void PreviousDay()
        {
            SelectedDate = SelectedDate.AddDays(-1);
        }

        private void Sync()
        {
            if (Synchronize != null)
            {
                Synchronize(this, new EventArgs());
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Debug.WriteLine("Timer elapsed");
            SetDirty(false);
            Save();
            Parse();
        }

        private void Save()
        {
            _repository.Save(Text);
            _repository.SaveToDo(ToDo);
            Status = "Saved";
        }

        public ICommand SyncCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand NextDayCommand { get; private set; }
        public ICommand PreviousDayCommand { get; private set; }
        public ICommand InsertCommand { get; private set; }


        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text == value) return;

                _text = value;
                SetDirty(true);
                OnPropertyChanged("Text");
            }
        }

        private string _todo;
        public string ToDo
        {
            get
            {
                return _todo;
            }
            set
            {
                if (_todo == value) return;

                _todo = value;
                Save();
                OnPropertyChanged("ToDo");
            }
        }

        private IEnumerable<string> _keywords;
        public IEnumerable<string> Keywords
        {
            get { return _keywords; }
            set
            {
                if (_keywords == value) return;

                _keywords = value;
                OnPropertyChanged("Keywords");
            }
        }

        private string _status;
        public string Status { get { return string.Format("{0} {1}", _status, DateTime.Now.ToLongTimeString()); } set { _status = value; OnPropertyChanged("Status"); } }

        private double _remainToday;
        public double RemainToday { get { return _remainToday; } set { _remainToday = value; OnPropertyChanged("RemainToday"); } }

        private DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                if (_selectedDate == value) return;

                _selectedDate = value;
                Statistics = _log.GetStatistics(_selectedDate);
                LineNumber = _log.SelectedLineNumber;
                OnPropertyChanged("SelectedDate");
            }
        }

        public int LineNumber { get; set; }

        private void SetDirty(bool isDirty)
        {
            _timer.Stop();
            _timer.Enabled = isDirty;
            if (isDirty)
            {
                _timer.Start();
            }
        }

        private string _statistics;
        public string Statistics { get { return _statistics; } set { _statistics = value; OnPropertyChanged("Statistics"); } }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;

            Debug.WriteLine("OnPropertyChanged: " + propertyName);
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Synchronize;

        public Find Find { get; }
        public FindNext FindNext { get; }
        public FindPrevious FindPrevious { get; }
        public Goto Goto { get; }
        public ShowContextMenu ShowContextMenu { get; }
        public FormatSelectionBase64Decode FormatSelectionBase64Decode { get; }
        public FormatSelectionBase64Encode FormatSelectionBase64Encode { get; }
        public FormatSelectionDecodeUrl FormatSelectionDecodeUrl { get; }
        public FormatSelectionEncodeUrl FormatSelectionEncodeUrl { get; }
        public FormatSelectionToLower FormatSelectionToLower { get; }
        public FormatSelectionToUpper FormatSelectionToUpper { get; }
        public SpellCheck SpellCheck { get; }
    }
}

