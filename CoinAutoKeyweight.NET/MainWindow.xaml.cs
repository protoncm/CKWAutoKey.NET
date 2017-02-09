using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoinAutoKeyweight.NET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FormDataSource _formDataSource;
        private InputServices _inputService;
        public MainWindow()
        {
            InitializeComponent();
            _formDataSource = (FormDataSource)FindResource("formDataSource");
            _inputService = new InputServices();
        }

        private void btnAssignKey_Click(object sender, RoutedEventArgs e)
        {
            _formDataSource.MessageText = "Please press any key.";
            KeyCapturedDialog keyCapturedDialog = new KeyCapturedDialog();
            keyCapturedDialog.DataContext = _formDataSource;
            keyCapturedDialog.Closed += (o, args) =>
            {
                Saved();
                _formDataSource.MessageText = "Saved Key.";
            };
            keyCapturedDialog.ShowDialog();
        }

        private void chkSnipping_Checked(object sender, RoutedEventArgs e)
        {
            Saved();
            if(_formDataSource != null)
            {
                _formDataSource.MessageText = string.Format("Updated Snapping = {0}.", chkSnipping.IsChecked?.ToString());
            }
        }

        private void Saved()
        {
            if(_formDataSource != null)
            {
                DatabaseServices.Instance.SaveConfiguration(
                    _formDataSource.Config.AssignedKey,
                    _formDataSource.Config.AssignedActiveWindow,
                    _formDataSource.Config.IsSnapping.Value
                );
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("MapleStory");

            if (processes.Length == 0)
            {
                MessageBox.Show("Please open MapleStory.", "Error", MessageBoxButton.OK);
                return;
            }

            IntPtr WindowHandle = processes[0].MainWindowHandle;
            WindowsAPI.SwitchWindow(WindowHandle);
            System.Threading.Thread.Sleep(500);
            InputServices.PressKey('A', true);
            //InputServices.PressKey('A', false);
        }
    }
}
