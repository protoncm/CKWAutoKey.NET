using CoinAutoKeyweight.NET.Models;
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
    /// Interaction logic for BuffDialog.xaml
    /// </summary>
    public partial class BuffDialog : Window
    {
        private FormDataSource _formDataSource;
        public BuffDialog()
        {
            InitializeComponent();
        }

        private void dgKeys_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            var displayIndex = grid.CurrentCell.Column.DisplayIndex;
            if (grid != null && displayIndex == 1)
            {
                var selectedItem = (AssignedKey)grid.SelectedItem;
                OpenKeyCaptureDialog(selectedItem);
            }
        }

        private void btnAddKey_Click(object sender, RoutedEventArgs e)
        {
            OpenKeyCaptureDialog();
        }

        private void btnRemoveKey_Click(object sender, RoutedEventArgs e)
        {
            _formDataSource = (FormDataSource)DataContext;
            var removedItems = _formDataSource.Config.AssignedBuffKeys.Where(a => a.IsChecked).ToList();
            if (removedItems != null && removedItems.Count > 0)
            {
                foreach (var item in removedItems)
                {
                    _formDataSource.Config.AssignedBuffKeys.Remove(item);
                }
            }
            dgKeys.ItemsSource = null;
            dgKeys.ItemsSource = _formDataSource.Config.AssignedBuffKeys;
        }

        private void btnClearKey_Click(object sender, RoutedEventArgs e)
        {
            _formDataSource = (FormDataSource)DataContext;
            _formDataSource.Config.AssignedBuffKeys.Clear();
            dgKeys.Items.Refresh();
        }

        private void OpenKeyCaptureDialog(AssignedKey intitalKey = null)
        {
            KeyCapturedDialog keyCapturedDialog = new KeyCapturedDialog();
            _formDataSource = (FormDataSource)DataContext;
            keyCapturedDialog.DataContext = _formDataSource;
            if (intitalKey == null)
            {
                _formDataSource.Config.CurrentKey = "Any Key!";
                keyCapturedDialog.Closed += (o, args) =>
                {
                    _formDataSource.Config.AssignedBuffKeys.Add(new AssignedKey()
                    {
                        Key = _formDataSource.Config.CurrentKey
                    });
                    dgKeys.Items.Refresh();
                };
            }
            else
            {
                _formDataSource.Config.CurrentKey = intitalKey.Key;
                keyCapturedDialog.Closed += (o, args) =>
                {
                    intitalKey.Key = _formDataSource.Config.CurrentKey;
                };
            }


            keyCapturedDialog.ShowDialog();
        }
    }
}
