using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FunTimer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer funTimer;
        DispatcherTimer workTimer;

        TimeSpan totalFunTime;// = new TimeSpan(0, 0, 0);
        TimeSpan totalWorkTime;// = new TimeSpan(0, 0, 0);

        public MainPage()
        {
            this.InitializeComponent();
            InitTimers();
            funTimes.Text = "0:00";
            workTimes.Text = "0:00";

            totalFunTime = new TimeSpan(0, 0, 0);
            totalWorkTime = new TimeSpan(0, 0, 0);
        }

        private void InitTimers()
        {
            funTimer = new DispatcherTimer();
            funTimer.Tick += FunTimer_Tick;
            funTimer.Interval = new TimeSpan(0, 0, 1);

            workTimer = new DispatcherTimer();
            workTimer.Tick += WorkTimer_Tick;
            workTimer.Interval = new TimeSpan(0, 0, 1);

        }

        private void WorkTimer_Tick(object sender, object e)
        {
            totalWorkTime += new TimeSpan(0, 0, 1);
            workTimes.Text = DisplaySpan(totalWorkTime);// + ;
        }

        private void FunTimer_Tick(object sender, object e)
        {
            totalFunTime += new TimeSpan(0, 0, 1);
            funTimes.Text = DisplaySpan(totalFunTime);
        }

        private void stopWorkStartFun_Click(object sender, RoutedEventArgs e)
        {
            workTimer.Stop();
            funTimer.Start();
        }

        private void stopFunStartWork_Click(object sender, RoutedEventArgs e)
        {
            funTimer.Stop();
            workTimer.Start();
        }

        private string DisplaySpan(TimeSpan input)
        {
            return String.Format("{0} hours {1} min {2} sec", input.Hours,
                           input.Minutes,
                           input.Seconds);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            funTimer.Stop();
            workTimer.Stop();
            totalWorkTime = new TimeSpan(0, 0, 0);
            totalFunTime = new TimeSpan(0, 0, 0);
        }
    }
}
