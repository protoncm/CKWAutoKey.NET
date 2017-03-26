using System.Windows;

namespace CoinAutoKeyweight.NET
{
    /// <summary>
    /// Interaction logic for ChangeNameDialog.xaml
    /// </summary>
    public partial class ChangeNameDialog : Window
    {
        public bool isCancel { get; set; }
        public ChangeNameDialog()
        {
            InitializeComponent();
            ShowInTaskbar = false;
            isCancel = false;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            isCancel = true;
            Close();
        }
    }
}
