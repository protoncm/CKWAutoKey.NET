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

        private void tbName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                Close();
        }
    }
}
