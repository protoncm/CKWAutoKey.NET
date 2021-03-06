﻿using CoinAutoKeyweight.NET.Services;
using System.ComponentModel;
using System.Windows;
using System;
using System.Collections.Generic;

namespace CoinAutoKeyweight.NET
{
    public class FormDataSource : PropertyChanges
    {
        private string _messageText = "Ready.";
        private bool _isRunning = false;
        private bool _isPaused = false;
        private const string STATUS = "Status";
        private const string  APPLICATION_TITLE = "Bear Macro";
        private string _formTitle = string.Empty;
        private string _runningTime = "00:00:00";
        private string _logMessage = string.Empty;
        private const int MAX_MESSAGE_LOG = 100;
        private Queue<string> messageStore = new Queue<string>(MAX_MESSAGE_LOG);
        public ConfigurationData Config
        {
            get;
            private set;
        }

        public FormDataSource()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Config = new ConfigurationData(null);
                _messageText = STATUS;
                _formTitle = APPLICATION_TITLE;
            }
            else
            {
                Config = new ConfigurationData(XmlServices.Load());
                _formTitle = Config.CurrentProfile.Name;
            }
        }

        public bool EnablePauseButton
        {
            get
            {
                return IsRunning && Config.NeedInitialConfig;
            }
        }
        public string FormTitle
        {
            get
            {
                return string.Format("{0} ({1})", APPLICATION_TITLE, _formTitle);
            }
            set
            {
                _formTitle = value;
                OnPropertyChanged("FormTitle");
            }
        }
        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                _isPaused = value;
                OnPropertyChanged("IsPaused");
            }
        }
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                OnPropertyChanged("IsRunning");
                OnPropertyChanged("EnablePauseButton");
            }
        }

        public string RunningTime
        {
            get
            {
                return string.Format("Running Time: {0}", _runningTime);
            }
            set
            {
                _runningTime = value;
                OnPropertyChanged("RunningTime");
            }
        }

        public string MessageText
        {
            get { return _messageText; }
            set
            {
                _messageText = string.Format("{0}: {1}", STATUS, value);
                OnPropertyChanged("MessageText");
            }
        }

        public string LogMessage
        {
            get
            {
                return string.Join("\r\n", messageStore);
            }
        }

        public void SetMessageLog(string message)
        {
            if (messageStore.Count == MAX_MESSAGE_LOG)
                messageStore.Dequeue();
            messageStore.Enqueue(message);
            OnPropertyChanged("LogMessage");
        }

        public void SetStatusText(string text)
        {
            MessageText = text;
            SetMessageLog(text);
        }

        public void SetRunningTime(double totalSeconds = 0)
        {
            if(totalSeconds != 0)
            {
                TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
                RunningTime = time.ToString(@"hh\:mm\:ss");
            }
            else
            {
                RunningTime = "00:00:00";
            }
        }
    }
}
