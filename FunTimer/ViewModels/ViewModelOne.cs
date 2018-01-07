using FunTimer.ServiceClasses;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace FunTimer.ViewModels
{
    public class ViewModelOne : BaseViewModel
    {
        DispatcherTimer _funTimer, _workTimer;
        TimeSpan _totalFunTime, _totalWorkTime;
        #region Initializers and private methods
        public ViewModelOne()
        {
            _startFunTimerCommand = new DelegateCommand(this.StartFunTimerCommandAction, this.CanStartFunTimer);
            _startWorkTimerCommand = new DelegateCommand(this.StartWorkTimerCommandAction, this.CanStartWorkTimer); //add this line to wherever you initialize your commands

            InitializeTimers();
            InitializeTimeSpans();
        }

        private void InitializeTimeSpans()
        {
            _totalFunTime = new TimeSpan(0, 0, 0);
            _totalWorkTime = new TimeSpan(0, 0, 0);
        }

        private void InitializeTimers()
        {
            _funTimer = new DispatcherTimer();
            _funTimer.Interval = new TimeSpan(0, 0, 1);
            _funTimer.Tick += FunTimer_Tick;

            _workTimer = new DispatcherTimer();
            _workTimer.Interval = new TimeSpan(0, 0, 1);
            _workTimer.Tick += WorkTimer_Tick;
        }

        private void WorkTimer_Tick(object sender, object e)
        {
            _totalWorkTime += new TimeSpan(0, 0, 1);
            WorkTimerValue = DisplaySpan(_totalWorkTime);
        }

        private void FunTimer_Tick(object sender, object e)
        {
            _totalFunTime += new TimeSpan(0, 0, 1);
            FunTimerValue = DisplaySpan(_totalFunTime);
        }

        private string DisplaySpan(TimeSpan input)
        { //change here -- remove leading zeroes
            return String.Format("{0} hours {1} min {2} sec", input.Hours,
                           input.Minutes,
                           input.Seconds);
        }

        private void CheckIfCommandsCanRun()
        {
            _startWorkTimerCommand.RaiseCanExecuteChanged();
            _startFunTimerCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Properties 

        private string _funTimerVal;
        public string FunTimerValue
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
        public string WorkTimerValue
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
        #endregion

        #region DelegateCommands
        DelegateCommand _startFunTimerCommand;
        public ICommand StartFunTimerCommand { get { return _startFunTimerCommand; } }
        private void StartFunTimerCommandAction(object obj)
        {
            _funTimer.Start();
            _workTimer.Stop();
            CheckIfCommandsCanRun();
        }

        private bool CanStartFunTimer(object obj)
        {
            return _funTimer.IsEnabled == false ||
                (_totalWorkTime == new TimeSpan(0,0,0) && _totalFunTime == new TimeSpan(0,0,0));
        }


        DelegateCommand _startWorkTimerCommand;
        public ICommand StartWorkTimerCommand { get { return _startWorkTimerCommand; } }
        private void StartWorkTimerCommandAction(object obj)
        {
            _workTimer.Start();
            _funTimer.Stop();
            CheckIfCommandsCanRun();
        }

        private bool CanStartWorkTimer(object obj)
        {
            return _workTimer.IsEnabled == false || 
                (_totalWorkTime == new TimeSpan(0,0,0) && _totalFunTime == new TimeSpan(0,0,0));
        }


        #endregion
    }
}
