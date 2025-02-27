using System.Windows;

namespace StudentsPayment
{
    public partial class AddEditPaymentWindow : Window
    {
        public Payments Payment { get; private set; }

        public AddEditPaymentWindow(Payments payment = null)
        {
            InitializeComponent();
            Payment = payment ?? new Payments();
            DataContext = Payment;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}