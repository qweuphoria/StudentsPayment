using System.Windows;

namespace StudentsPayment
{
    public partial class AddEditDebtWindow : Window
    {
        public Debts Debt { get; private set; }

        public AddEditDebtWindow(Debts debt = null)
        {
            InitializeComponent();
            Debt = debt ?? new Debts();
            DataContext = Debt;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}