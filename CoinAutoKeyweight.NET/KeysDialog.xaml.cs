﻿using CoinAutoKeyweight.NET.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoinAutoKeyweight.NET
{
    /// <summary>
    /// Interaction logic for KeysDialog.xaml
    /// </summary>
    public partial class KeysDialog : Window
    {
        private FormDataSource _formDataSource;
        public KeysDialog()
        {
            InitializeComponent();
            
        }

        private void btnAddKey_Click(object sender, RoutedEventArgs e)
        {
            OpenKeyCaptureDialog();
        }

        private void dgKeys_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            var displayIndex = grid.CurrentCell.Column.DisplayIndex;
            if(grid != null && displayIndex == 1)
            {
                var selectedItem = (AssignedKey)grid.SelectedItem;
                OpenKeyCaptureDialog(selectedItem);
            }
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
                    _formDataSource.Config.AssignedKeys.Add(new AssignedKey()
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

        private void btnClearKey_Click(object sender, RoutedEventArgs e)
        {
            _formDataSource = (FormDataSource)DataContext;
            _formDataSource.Config.AssignedKeys.Clear();
            dgKeys.Items.Refresh();
        }

        private void btnRemoveKey_Click(object sender, RoutedEventArgs e)
        {
            _formDataSource = (FormDataSource)DataContext;
            var removedItems = _formDataSource.Config.AssignedKeys.Where(a => a.IsChecked).ToList();
            if(removedItems != null && removedItems.Count > 0)
            {
                foreach (var item in removedItems)
                {
                    _formDataSource.Config.AssignedKeys.Remove(item);
                }
            }
            dgKeys.ItemsSource = null;
            dgKeys.ItemsSource = _formDataSource.Config.AssignedKeys;
        }
    }
}
