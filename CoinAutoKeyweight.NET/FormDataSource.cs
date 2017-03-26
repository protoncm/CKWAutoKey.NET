using CoinAutoKeyweight.NET.Models;
using CoinAutoKeyweight.NET.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CoinAutoKeyweight.NET
{
    public class FormDataSource : PropertyChanges
    {
        private string _messageText = "Ready.";
        private bool _isRunning = false;
        private const string STATUS = "Status";
        private const string  APPLICATION_TITLE = "Bear Macro";
        private string _formTitle = string.Empty;
        
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
                _formTitle = Config.CurrentProfile.Name;
            }
            else
            {
                Config = new ConfigurationData(XmlServices.Load());
                _formTitle = Config.CurrentProfile.Name;
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
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                OnPropertyChanged("IsRunning");
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

        public void SetStatusText(string text)
        {
            MessageText = text;
        }
    }
}
