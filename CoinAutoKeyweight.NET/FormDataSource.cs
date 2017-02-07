using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Config = new ConfigurationData(DatabaseServices.Instance.LoadConfiguration());
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
