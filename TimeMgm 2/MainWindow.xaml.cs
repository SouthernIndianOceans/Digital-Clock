using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Windows.Media;

namespace TimeMgm_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DispatcherTimer _tma = new DispatcherTimer();
        private int _spent, _interval;
        private readonly string[] _list = new string[4];
        public MainWindow()
        {
            InitializeComponent();
            _tma.Interval = new TimeSpan(0, 0, 1);
            _tma.Tick += Tma_Tick;
        }

        private void Tma_Tick(object sender, EventArgs e)
        {
            _interval++;
            _spent++;
            var tms = TimeSpan.FromSeconds(_spent);
            Ha.Content = tms.ToString(@"hh\:mm\:ss");
            Hb.Content = DateTime.Now.ToShortTimeString();
            if (_interval > 1800)
            {
                _interval = 0;
                var i = new Interval();
                i.Show();
                i.Activate();
                i.Topmost = true;
            }
            if (_list[2] != DateTime.Now.ToShortTimeString()) return;
            Am.Foreground = Brushes.Red;
            var ix = new Interval {Msg = {Content = _list[3]}};
            ix.ShowDialog();
            ix.Activate();
            ix.Topmost = true;
            ix.Topmost = false;
            ix.Focus();  
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            File.AppendAllLines(_filed + "\\Session.log", GetSessionEnds());
        }
        private readonly string _filed = Directory.GetCurrentDirectory();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var sra = new StreamReader(_filed + "\\Alarm.txt");
                for (var i = 0; i < 4; i++)
                {
                    _list[i] = sra.ReadLine();
                }
                sra.Close();
                sra.Dispose();
                Left = Convert.ToDouble(_list[0]);
                Top = Convert.ToDouble(_list[1]);
                Am.Content = _list[2];  
                File.AppendAllLines(_filed + "\\Session.log", GetSessionStarts());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load User Settings." + ex.Message);                
            }
            _tma.Start();
        }
        private static string SendsA()
        {
            return DateTime.Now.ToLongDateString();
        }
        private static string SendsB()
        {
            return DateTime.Now.ToLongTimeString();
        }
        private static string SendsC()
        {
            return "=====Session starts here=====";
        }
        private static string SendsD()
        {
            return "===== Session ends here =====";
        }
        private static IEnumerable<string> GetSessionStarts()
        {
            var st = new[]
            {
                SendsC(),
                SendsA(),
                SendsB()
            };
            return st;
        }
        private static IEnumerable<string> GetSessionEnds()
        {
            var st = new[]
            {
                SendsA(),
                SendsB(),
                SendsD()
            };
            return st;
        }
    }
}