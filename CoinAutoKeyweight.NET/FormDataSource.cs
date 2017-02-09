﻿using CoinAutoKeyweight.NET.Services;
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
        private string _messageText = string.Empty;
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
            }
            else
            {
                Config = new ConfigurationData(XmlServices.Load());
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
