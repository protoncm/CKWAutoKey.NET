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
                DatabaseServices.Instance.SaveConfiguration(
                    _formDataSource.Config.AssignedKey,
                    _formDataSource.Config.AssignedActiveWindow,
                    _formDataSource.Config.IsSnapping.Value
                    );
            };
            keyCapturedDialog.ShowDialog();
        }
    }
}
