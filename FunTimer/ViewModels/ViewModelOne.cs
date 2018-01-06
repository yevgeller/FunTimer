using FunTimer.ServiceClasses;
using System;
using System.Windows.Input;

namespace FunTimer.ViewModels
{
    public class ViewModelOne : BaseViewModel
    {
        public ViewModelOne()
        {
            _startFunTimerCommand = new DelegateCommand(this.StartFunTimerCommandAction, this.CanStartFunTimer); 
        }

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


        DelegateCommand _startFunTimerCommand;
        public ICommand StartFunTimerCommand { get { return _startFunTimerCommand; } }
        private void StartFunTimerCommandAction(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanStartFunTimer(object obj)
        {
            return true; //worktimer is enabled
        }
    }
}
