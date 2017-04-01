﻿using CoinAutoKeyweight.NET.Models;
using CoinAutoKeyweight.NET.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        private Stopwatch runningTime;
        private System.Timers.Timer timer;
        public MainWindow()
        {
            InitializeComponent();
            _formDataSource = (FormDataSource)FindResource("formDataSource");
            _inputService = new InputServices();
        }

        private void btnAssignKey_Click(object sender, RoutedEventArgs e)
        {
            _formDataSource.SetStatusText("Open Key Dialog.");
            KeysDialog keyDialog = new KeysDialog();
            keyDialog.DataContext = _formDataSource;
            keyDialog.Closed += (o, args) =>
            {
                ApplyChanged();
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
                XmlServices.Save(_formDataSource.Config.GetDataDic(), SourceChanged.Profile);
                _formDataSource.SetStatusText("Saved Changes.");
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            string processName = _formDataSource.Config.Settings.AssignedWindowName;
            runningTime = new Stopwatch();
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
#if DEBUG
            processName = "Notepad";
#endif
            if (!_formDataSource.IsRunning)
            {
                timer.Elapsed += Timer_Elapsed;

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
                    bool firstTimeBuff = true;
                    // capture running time
                    runningTime.Start();
                    timer.Start();

                    do
                    {
                        //check buff before start
                        var buff = _formDataSource.Config.CurrentProfile.Buff;
                        if (_formDataSource.Config.AssignedBuffKeys.Count > 0 && buff.AutoBuff)
                        {
                            if ((buff.StartIn == 0 && firstTimeBuff) || (buff.StartIn <= runningTime.Elapsed.TotalSeconds && firstTimeBuff))
                            {
                                DoBuff(_formDataSource.Config.AssignedBuffKeys, runningTime);
                                firstTimeBuff = false;
                            }
                            else if(!firstTimeBuff)
                            {
                                var currentTime = runningTime.Elapsed.TotalMinutes;
                                var buffInThisTime = _formDataSource.Config.AssignedBuffKeys.Where(a => currentTime - a.Timestamp >= a.Duration).ToList();
                                if (buffInThisTime != null && buffInThisTime.Count > 0)
                                {
                                    DoBuff(buffInThisTime, runningTime);
                                }
                            }
                        }
                       
                        
                        for (int i = 0; i < _formDataSource.Config.AssignedKeys.Count; i++)
                        {
                            var activeKeyStroke = _formDataSource.Config.AssignedKeys[i];
                            _formDataSource.Config.DisplayAssignedKey = activeKeyStroke;
                            _formDataSource.SetStatusText(string.Format("Holding Key {0} in {1} sec.", _formDataSource.Config.DisplayAssignedKey.Key, _formDataSource.Config.DisplayAssignedKey.Duration));
                            // do action
                            DoAction(activeKeyStroke);
                        }
                    }
                    while (_formDataSource.IsRunning);
                }));

                thread.Start();
            }
            else
            {
                runningTime.Stop();
                timer.Elapsed -= Timer_Elapsed;
                timer.Stop();
                timer.Enabled = false;
                _formDataSource.IsRunning = false;
                _formDataSource.SetStatusText("Stopped / Waiting for next request.");
                _formDataSource.SetRunningTime();
                WindowsAPI.SwitchWindow(WindowHandle);
                thread.Abort();
                //make sure current key was released
                InputServices.ReleaseKey(_formDataSource.Config.DisplayAssignedKey.Key);
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _formDataSource.SetRunningTime(runningTime.Elapsed.TotalSeconds);
        }

        public void DoBuff(List<AssignedKey> keys, Stopwatch runningStopwatch)
        {
            foreach (var buffKey in keys)
            {
                _formDataSource.Config.DisplayAssignedKey = buffKey;
                _formDataSource.SetStatusText(string.Format("Buffing Key {0} for every {1} min.", _formDataSource.Config.DisplayAssignedKey.Key, _formDataSource.Config.DisplayAssignedKey.Duration));
                // do action
                DoAction(buffKey, true);
                buffKey.Timestamp = runningStopwatch.Elapsed.TotalMinutes;
            }
        }

        private void DoAction(AssignedKey ak, bool isBuff = false)
        {
            int timeOffset = 10; //ms
            Stopwatch timer = new Stopwatch();
            timer.Start();
            double duration = isBuff ? 0.5 : ak.Duration;
            while (timer.Elapsed < TimeSpan.FromSeconds(duration))
            {
                InputServices.PressKey(ak.Key, true);
                Thread.Sleep(timeOffset); // waiting time
            }
            timer.Stop();
            // release key
            InputServices.ReleaseKey(ak.Key);
            // delay
            if (ak.Delay != 0)
            {
                int delay = Convert.ToInt32(ak.Delay * 1000);
                Thread.Sleep(delay);
                if (delay >= 1000)
                {
                    _formDataSource.SetStatusText(string.Format("Waiting in {0} sec", ak.Delay));
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(thread != null && WindowHandle != null)
            {
                runningTime.Stop();
                timer.Elapsed -= Timer_Elapsed;
                timer.Stop();
                timer.Enabled = false;
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
            _formDataSource.SetStatusText("Open Key Dialog.");
            BuffDialog BuffDialog = new BuffDialog();
            BuffDialog.DataContext = _formDataSource;
            BuffDialog.Closed += (o, args) =>
            {
                ApplyChanged();
            };
            BuffDialog.ShowDialog();
        }

        private void btnChangeName_Click(object sender, RoutedEventArgs e)
        {
            ChangeNameDialog changeNamedialog = new ChangeNameDialog();
            changeNamedialog.tbName.Text = _formDataSource.Config.CurrentProfile.Name;
            changeNamedialog.Closing += (s, arg) => {
                if (string.IsNullOrEmpty(changeNamedialog.tbName.Text) && !changeNamedialog.isCancel)
                {
                    MessageBox.Show("Please input your name!");
                    arg.Cancel = true;
                }
                else if (!changeNamedialog.isCancel)
                {
                    string newName = changeNamedialog.tbName.Text;
                    string oldName = _formDataSource.Config.CurrentProfile.Name;
                    if (_formDataSource.Config.Profiles.Where(a => a.Name != oldName).Any(a => a.Name == newName))
                    {
                        MessageBox.Show(string.Format("\"{0}\" is already exist.", newName), "Error.");
                        arg.Cancel = true;
                    }
                    else
                    {
                        _formDataSource.Config.CurrentProfile.Name = newName;
                        _formDataSource.FormTitle = newName;
                        ApplyChanged();
                    }
                }
            };
            changeNamedialog.ShowDialog();
        }

        private void btnNewProfile_Click(object sender, RoutedEventArgs e)
        {
            ChangeNameDialog changeNamedialog = new ChangeNameDialog();
            changeNamedialog.Closing += (s, arg) => {
                if (string.IsNullOrEmpty(changeNamedialog.tbName.Text) && !changeNamedialog.isCancel)
                {
                    MessageBox.Show("Please input your name!");
                    arg.Cancel = true;
                }
                else if (!changeNamedialog.isCancel)
                {
                    string newName = changeNamedialog.tbName.Text;
                    if (_formDataSource.Config.Profiles.Any(a => a.Name == newName))
                    {
                        MessageBox.Show(string.Format("\"{0}\" is already exist.", newName), "Error.");
                        arg.Cancel = true;
                    }
                    else
                    {
                        _formDataSource.Config.CreateNewProfile(newName);
                        _formDataSource.FormTitle = newName;
                        btnopen.Items.Refresh();
                        ApplyChanged();
                    }
                }
            };
            changeNamedialog.ShowDialog();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnopen_Click(object sender, RoutedEventArgs e)
        {
            var selectedProfile = (e.OriginalSource as MenuItem).DataContext as Profile;
            if(selectedProfile != null)
            {
                _formDataSource.Config.LoadProfile(selectedProfile);
                _formDataSource.FormTitle = selectedProfile.Name;
                _formDataSource.SetStatusText(string.Format("{0} is loaded.", selectedProfile.Name));
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettiingDialog settingDialog = new SettiingDialog();
            settingDialog.Closing += (s, arg) =>
            {
                XmlServices.Save(_formDataSource.Config.GetDataDic(), SourceChanged.Settings);
                _formDataSource.SetStatusText("Save Settings.");
            };
            settingDialog.ShowDialog();
        }
    }
}
