using FunTimer.Lib.Data;
using FunTimer.Lib.Models;
using FunTimer.ServiceClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace FunTimer.ViewModels
{
    public class ViewModelOne : BaseViewModel
    {
        DispatcherTimer _funTimer, _workTimer;
        TimeSpan _totalFunTime, _totalWorkTime;
        IDataLayerAsync _db;
        TimeRecord tr;

        #region Initializers and private methods
        public ViewModelOne()
        {
            tr = new TimeRecord { StartTime = DateTime.MinValue, EndTime = DateTime.MinValue };

            _db = new TextDataLayerNonStatic("lablabla.txt");
            //List<TimeRecord> trs = new List<TimeRecord>();
            //for (int i = -1; i < 2; i++)
            //{
            //    TimeRecord tr = new TimeRecord
            //    {
            //        TimeRecordType = i % 2 == 0 ? TimeRecordTypeEnum.FunTimePeriod : TimeRecordTypeEnum.WorkTimePeriod,
            //        StartTime = DateTime.Now.Subtract(new TimeSpan(i, 0, 0)),
            //        EndTime = DateTime.Now.Add(new TimeSpan(i + 2, 0, 0))
            //    };

            //    trs.Add(tr);
            //}

            //_db.SaveTimeRecordListAsync(trs);

            //List<TimeRecord> trsOut = GetRecordsFromStorage().ConfigureAwait(false);

            List<TimeRecord> list = _db.GetAllTimeRecordsAsync().Result;

            _startFunTimerCommand = new DelegateCommand(this.StartFunTimerCommandAction, this.CanStartFunTimer);
            _startWorkTimerCommand = new DelegateCommand(StartWorkTimerCommandAction, this.CanStartWorkTimer); //add this line to wherever you initialize your commands
            _resetBothTimersCommand = new DelegateCommand(this.ResetBothTimersCommandAction, this.CanResetBothTimers); //add this line to wherever you initialize your commands

            InitializeTimers();
            InitializeTimeSpans();
            AttachTimerEvents();

            CanStartWorkTimerProperty = true;
            CanStartFunTimerPropery = true;
        }

        private void InitializeTimeSpans()
        {
            WorkTimerDisplayedValue = DisplaySpan(_totalWorkTime);
            FunTimerDisplayedValue = DisplaySpan(_totalFunTime);
        }

        private void InitializeTimers()
        {
            _funTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            _workTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
        }

        private void AttachTimerEvents()
        {
            _funTimer.Tick += FunTimer_Tick;
            _workTimer.Tick += WorkTimer_Tick;
        }

        private void WorkTimer_Tick(object sender, object e)
        {
            _totalWorkTime += new TimeSpan(0, 0, 1);
            WorkTimerDisplayedValue = DisplaySpan(_totalWorkTime);
        }

        private void FunTimer_Tick(object sender, object e)
        {
            _totalFunTime += new TimeSpan(0, 0, 1);
            FunTimerDisplayedValue = DisplaySpan(_totalFunTime);
        }

        private string DisplaySpan(TimeSpan input)
        {
            return String.Format("{0} hours {1} min {2} sec", input.Hours,
                           input.Minutes,
                           input.Seconds);
        }

        private void CheckIfCommandsCanRun()
        {
            _startWorkTimerCommand.RaiseCanExecuteChanged();
            _startFunTimerCommand.RaiseCanExecuteChanged();
        }

        private async Task ProcessTimeRecord(TimeRecordTypeEnum typeParam)
        {
            if (tr.StartTime != DateTime.MinValue)// tr.EndTime != DateTime.MinValue)
            {
                tr.EndTime = DateTime.Now;
                await _db.SaveTimeRecordAsync(tr);
                tr = new TimeRecord { StartTime = DateTime.Now, TimeRecordType = typeParam };
            }
            else
            {
                //starting the app
                tr.StartTime = DateTime.Now;
                tr.TimeRecordType = typeParam;
            }

            await RefreshTimeRecords();
        }

        private async Task RefreshTimeRecords()
        {
            List<TimeRecord> allRecords = await _db.GetAllTimeRecordsAsync();
            List<TimeRecord> todaysRecords = allRecords
                .Where(x => x.StartTime.DayOfYear == DateTime.Now.DayOfYear && x.StartTime.Year == DateTime.Now.Year)
                .OrderByDescending(x=>x.StartTime)
                .ToList();

            string result = string.Empty;
            foreach (TimeRecord tr in todaysRecords)
                result += tr.ForDisplay() + Environment.NewLine;

            Records = result;
        }

        #endregion

        #region Properties 

        private string _funTimerVal;
        public string FunTimerDisplayedValue
        {
            get { return _funTimerVal; }
            set
            {
                if (value != this._funTimerVal)
                {
                    this._funTimerVal = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _workTimerValue;
        public string WorkTimerDisplayedValue
        {
            get { return _workTimerValue; }
            set
            {
                if (value != this._workTimerValue)
                {
                    this._workTimerValue = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _records;
        public string Records
        {
            get { return _records; }
            set
            {
                if (value != this._records)
                {
                    this._records = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private bool _canStartWorkTimer;

        public bool CanStartWorkTimerProperty
        {
            get { return _canStartWorkTimer; }
            set { _canStartWorkTimer = value; }
        }

        private bool _canStartFunTimer;

        public bool CanStartFunTimerPropery
        {
            get { return _canStartFunTimer; }
            set { _canStartFunTimer = value; }
        }

        public bool GetFunTimerStarted()
        {
            return _funTimer.IsEnabled;
        }

        public bool GetWorkTimerIsStarted()
        {
            return _workTimer.IsEnabled;
        }

        #endregion

        #region DelegateCommands
        DelegateCommand _startFunTimerCommand;
        public ICommand StartFunTimerCommand { get { return _startFunTimerCommand; } }
        private void StartFunTimerCommandAction(object obj)
        {
            _funTimer.Start();
            _workTimer.Stop();
            CanStartFunTimerPropery = false;
            CanStartWorkTimerProperty = true;
            CheckIfCommandsCanRun();
            ProcessTimeRecord(TimeRecordTypeEnum.WorkTimePeriod).ConfigureAwait(false); ; //this feels like a crutch, but I need to get smarter on async/await
        }

        private bool CanStartFunTimer(object obj)
        {
            return CanStartFunTimerPropery;
        }


        DelegateCommand _startWorkTimerCommand;
        public ICommand StartWorkTimerCommand { get { return _startWorkTimerCommand; } }
        private void StartWorkTimerCommandAction(object obj)
        {
            _workTimer.Start();
            _funTimer.Stop();
            CanStartFunTimerPropery = true;
            CanStartWorkTimerProperty = false;
            CheckIfCommandsCanRun();
            ProcessTimeRecord(TimeRecordTypeEnum.FunTimePeriod).ConfigureAwait(false);
        }

        private bool CanStartWorkTimer(object obj)
        {
            //this was hard to test -- return _workTimer.IsEnabled == false || (_totalWorkTime == new TimeSpan(0,0,0) && _totalFunTime == new TimeSpan(0,0,0));
            return CanStartWorkTimerProperty;
        }


        DelegateCommand _resetBothTimersCommand;
        public ICommand ResetBothTimersCommand { get { return _resetBothTimersCommand; } }
        private void ResetBothTimersCommandAction(object obj)
        {
            _funTimer.Stop();
            _workTimer.Stop();
            CanStartFunTimerPropery = true;
            CanStartWorkTimerProperty = true;
            InitializeTimers();
        }

        private bool CanResetBothTimers(object obj)
        {
            return true;
        }

        #endregion
    }
}
