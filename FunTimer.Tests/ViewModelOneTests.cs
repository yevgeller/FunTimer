
using System;
using FunTimer.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace FunTimer.Tests
{
    [TestClass]
    public class UnitTest1
    {
        ViewModelOne vm;// = new ViewModelOne();

        string newTimerExpectedValue = "0 hours 0 min 0 sec";

        //[UITestMethod]
        [TestInitialize]
        public void SetUp()
        {
            vm = new ViewModelOne();
        }

        [UITestMethod]
        public void InitializeVM_ExpectAllTimersToBeReset()
        {
            ViewModelOne vm2 = new ViewModelOne();
            Assert.AreEqual(newTimerExpectedValue, vm2.FunTimerDisplayedValue, "When ViewModelOne is initialized, FunTimerValue is not set to 0");
            Assert.AreEqual(newTimerExpectedValue, vm2.WorkTimerDisplayedValue, "When ViewModelOne is initialized, WorkTimerValue is not set to 0");
            Assert.IsTrue(vm2.StartFunTimerCommand.CanExecute(null), "Cannot start fun timer when ViewModelOne is initialized");
            Assert.IsTrue(vm2.StartWorkTimerCommand.CanExecute(null), "Cannot start work timer when ViewModelOne is initialized");
        }

        [UITestMethod]
        public void CanExecute_FunTimer_When_WorkTimerIsStarted()
        {
            Assert.IsTrue(vm.StartWorkTimerCommand.CanExecute(null), "Error during setup: cannot start work timer");
            Assert.IsTrue(vm.StartFunTimerCommand.CanExecute(null), "Error during setup: cannot start fun timer");

            vm.StartFunTimerCommand.Execute(null);

            Assert.IsFalse(vm.StartFunTimerCommand.CanExecute(null), "Should NOT be able to start fun timer once it is started");
            Assert.IsTrue(vm.StartWorkTimerCommand.CanExecute(null), "Should be able to start work timer once the fun time is started");
        }

        [UITestMethod]
        public void CanExecute_WorkTimer_WhenWorkTimerIsStarted()
        {
            Assert.IsTrue(vm.StartWorkTimerCommand.CanExecute(null), "Error during setup: cannot start work timer");
            Assert.IsTrue(vm.StartFunTimerCommand.CanExecute(null), "Error during setup: cannot start fun timer");

            vm.StartWorkTimerCommand.Execute(null);

            Assert.IsTrue(vm.StartFunTimerCommand.CanExecute(null), "Should be able to start fun timer once it is started");
            Assert.IsFalse(vm.StartWorkTimerCommand.CanExecute(null), "Should NOT be able to start work timer once the fun time is started");
        }

        [UITestMethod]
        public void ResetTimers_BothTimersReset()
        {
            bool atLeastOneTimerWasStarted = false;

            vm.StartWorkTimerCommand.Execute(null);
            vm.StartFunTimerCommand.Execute(null);

            atLeastOneTimerWasStarted = vm.GetFunTimerStarted() || vm.GetWorkTimerIsStarted();

            Assert.IsTrue(atLeastOneTimerWasStarted);
            Assert.IsTrue(vm.ResetBothTimersCommand.CanExecute(null));

            vm.ResetBothTimersCommand.Execute(null);

            Assert.IsTrue(vm.ResetBothTimersCommand.CanExecute(null));
            Assert.AreEqual(newTimerExpectedValue, vm.FunTimerDisplayedValue, "When ViewModelOne is initialized, FunTimerValue is not set to 0");
            Assert.AreEqual(newTimerExpectedValue, vm.WorkTimerDisplayedValue, "When ViewModelOne is initialized, WorkTimerValue is not set to 0");

        }
    }
}
