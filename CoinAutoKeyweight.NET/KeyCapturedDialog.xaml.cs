﻿using System;
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
    /// Interaction logic for KeyCapturedDialog.xaml
    /// </summary>
    public partial class KeyCapturedDialog : Window
    {
        public KeyCapturedDialog()
        {
            InitializeComponent();
            ShowInTaskbar = false;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            txtKey.Text = e.Key.ToString();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
