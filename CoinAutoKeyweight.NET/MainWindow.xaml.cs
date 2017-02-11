using CoinAutoKeyweight.NET.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoinAutoKeyweight.NET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FormDataSource _formDataSource;
        private InputServices _inputService;
        private IntPtr WindowHandle;
        private Thread thread;
        public MainWindow()
        {
            InitializeComponent();
            _formDataSource = (FormDataSource)FindResource("formDataSource");
            _inputService = new InputServices();
        }

        private void btnAssignKey_Click(object sender, RoutedEventArgs e)
        {
            _formDataSource.MessageText = "Please press any key.";
            KeyCapturedDialog keyCapturedDialog = new KeyCapturedDialog();
            keyCapturedDialog.DataContext = _formDataSource;
            keyCapturedDialog.Closed += (o, args) =>
            {
                ApplyChanged();
                _formDataSource.MessageText = "Saved Key.";
            };
            keyCapturedDialog.ShowDialog();
        }

        private void chkSnipping_Checked(object sender, RoutedEventArgs e)
        {
            ApplyChanged();
            if(_formDataSource != null)
            {
                _formDataSource.MessageText = string.Format("Updated Snapping = {0}.", chkSnipping.IsChecked?.ToString());
            }
        }

        private void ApplyChanged()
        {
            if(_formDataSource != null)
            {
                XmlServices.Save(_formDataSource.Config.GetDataDic());
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!_formDataSource.IsRunning)
            {
                Process[] processes = Process.GetProcessesByName("MapleStory");
                if (processes.Length == 0)
                {
                    MessageBox.Show("Please open MapleStory.", "Error", MessageBoxButton.OK);
                    return;
                }
                _formDataSource.IsRunning = true;
                WindowHandle = processes[0].MainWindowHandle;
                WindowsAPI.SwitchWindow(WindowHandle);
                _formDataSource.MessageText = string.Format("Holding Key {0} in {1} sec.", _formDataSource.Config.AssignedKey, _formDataSource.Config.HoldTime);
                thread = new Thread(new ThreadStart(() =>
                {
                    int timeOffset = 10; //ms
                    do
                    {
                        Stopwatch timer = new Stopwatch();
                        timer.Start();
                        while (timer.Elapsed < TimeSpan.FromSeconds(_formDataSource.Config.HoldTime))
                        {
                            InputServices.PressKey(_formDataSource.Config.AssignedKey, true);
                            Thread.Sleep(timeOffset); // waiting time
                        }
                        timer.Stop();
                        // release key
                        InputServices.ReleaseKey(_formDataSource.Config.AssignedKey);
                    }
                    while (_formDataSource.IsRunning);
                }));

                thread.Start();
            }
            else
            {
                _formDataSource.IsRunning = false;
                _formDataSource.MessageText = "Stopped / Waiting for next request.";
                WindowsAPI.SwitchWindow(WindowHandle);
                thread.Abort();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(thread != null && WindowHandle != null)
            {
                _formDataSource.IsRunning = false;
                WindowsAPI.SwitchWindow(WindowHandle);
                thread.Abort();
            }
        }

        private void tbHoldTime_KeyUp(object sender, KeyEventArgs e)
        {
            ApplyChanged();
        }
    }
}
