using CoinAutoKeyweight.NET.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CoinAutoKeyweight.NET
{
    /// <summary>
    /// Interaction logic for SettiingDialog.xaml
    /// </summary>
    public partial class SettiingDialog : Window
    {
        public bool isCancel { get; set; }
        public SettiingDialog()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            isCancel = true;
            this.Close();
        }
    }
}
