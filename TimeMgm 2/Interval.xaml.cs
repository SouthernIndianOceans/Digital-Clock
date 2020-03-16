using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TimeMgm_2
{
    /// <summary>
    /// Interaction logic for Interval.xaml
    /// </summary>
    public partial class Interval
    {
        private DispatcherTimer _tma = new DispatcherTimer();
        private int _interval = 300;
        private MediaElement _m = new MediaElement();
        public Interval()
        {
            InitializeComponent();
            _tma.Interval = new TimeSpan(0, 0, 1);
            _tma.Tick += Tma_Tick;
            var location = Directory.GetCurrentDirectory() + "\\alarms.mp3";
            _m.LoadedBehavior = MediaState.Manual;
            _m.UnloadedBehavior = MediaState.Manual;
            _m.Source = new Uri(location);
            _m.Volume = 100;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _tma.Start();
            _m.Play();
        }
        private void Tma_Tick(object sender, EventArgs e)
        {
            _interval--;
            var tms = TimeSpan.FromSeconds(_interval);
            Ha.Content = tms.ToString(@"hh\:mm\:ss");
            if (_interval != 0) return;
            _tma.Stop();
            _m.Stop();
            _tma = null;
            _m = null;
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _tma.Stop();
            _m.Stop();
            _tma = null;
            _m = null;
            Close();
        }
    }
}
