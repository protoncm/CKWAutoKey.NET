using CoinAutoKeyweight.NET.Models;
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
            KeysDialog keyDialog = new KeysDialog();
            keyDialog.DataContext = _formDataSource;
            keyDialog.Closed += (o, args) =>
            {
                ApplyChanged();
                _formDataSource.MessageText = "Saved Key.";
            };
            keyDialog.ShowDialog();
        }

        private void chkSnipping_Checked(object sender, RoutedEventArgs e)
        {
            ApplyChanged();
            if (_formDataSource != null)
            {
                if (_formDataSource.Config.AssignedKeys.Count == 0)
                {
                    _formDataSource.Config.DisplayAssignedKey = AssignedKey.Default;
                }
                //_formDataSource.MessageText = string.Format("Updated Snapping = {0}.", chkSnipping.IsChecked?.ToString());
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
            string processName = _formDataSource.Config.CurrentProfile.AssignedWindowName;
#if DEBUG
            processName = "Notepad";
#endif
            if (!_formDataSource.IsRunning)
            {
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length == 0)
                {
                    MessageBox.Show("Please open " + processName + ".", "Error", MessageBoxButton.OK);
                    return;
                }
                _formDataSource.IsRunning = true;
                WindowHandle = processes[0].MainWindowHandle;
                WindowsAPI.SwitchWindow(WindowHandle);
                thread = new Thread(new ThreadStart(() =>
                {
                    int timeOffset = 10; //ms
                    do
                    {
                        for (int i = 0; i < _formDataSource.Config.AssignedKeys.Count; i++)
                        {
                            _formDataSource.Config.DisplayAssignedKey = _formDataSource.Config.AssignedKeys[i];
                            _formDataSource.MessageText = string.Format("Holding Key {0} in {1} sec.", _formDataSource.Config.DisplayAssignedKey.Key, _formDataSource.Config.DisplayAssignedKey.Duration);
                            Stopwatch timer = new Stopwatch();
                            timer.Start();
                            while (timer.Elapsed < TimeSpan.FromSeconds(_formDataSource.Config.DisplayAssignedKey.Duration))
                            {
                                InputServices.PressKey(_formDataSource.Config.DisplayAssignedKey.Key, true);
                                Thread.Sleep(timeOffset); // waiting time
                            }
                            timer.Stop();
                            // release key
                            InputServices.ReleaseKey(_formDataSource.Config.DisplayAssignedKey.Key);
                        }
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
                //make sure current key was released
                InputServices.ReleaseKey(_formDataSource.Config.DisplayAssignedKey.Key);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(thread != null && WindowHandle != null)
            {
                _formDataSource.IsRunning = false;
                thread.Abort();
                //make sure current key was released
                InputServices.ReleaseKey(_formDataSource.Config.DisplayAssignedKey.Key);
            }
        }

        private void tbHoldTime_KeyUp(object sender, KeyEventArgs e)
        {
            ApplyChanged();
        }

        private void btnAssignBuffKey_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
