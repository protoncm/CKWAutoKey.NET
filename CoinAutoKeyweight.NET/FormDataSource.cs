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
                _messageText = "{{Message}}";
            }
            else
            {
                Config = new ConfigurationData(XmlServices.Load());
            }
        }
        public string FormTitle
        {
            get
            {
#if DEBUG
                return string.Format("{0} {1}", APPLICATION_TITLE, Config.CurrentProfile != null ? "(" + Config.CurrentProfile.Name + ")" : string.Empty);
#endif
                return string.Format("{0} {1}", APPLICATION_TITLE, Config.CurrentProfile != null ? "(" + Config.CurrentProfile.Name + ")" : string.Empty);
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
                _messageText = value;
                OnPropertyChanged("MessageText");
            }
        }
    }
}
