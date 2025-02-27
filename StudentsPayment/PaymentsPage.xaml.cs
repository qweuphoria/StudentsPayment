using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;

namespace StudentsPayment
{
    public partial class PaymentsPage : Page
    {
        private databaseEntities _context = new databaseEntities();

        public PaymentsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _context.Payments.Load();
            PaymentsDataGrid.ItemsSource = _context.Payments.Local;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditWindow = new AddEditPaymentWindow();
            if (addEditWindow.ShowDialog() == true)
            {
                _context.Payments.Add(addEditWindow.Payment);
                _context.SaveChanges();
                LoadData();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPayment = PaymentsDataGrid.SelectedItem as Payments;
            if (selectedPayment != null)
            {
                var addEditWindow = new AddEditPaymentWindow(selectedPayment);
                if (addEditWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPayment = PaymentsDataGrid.SelectedItem as Payments;
            if (selectedPayment != null)
            {
                _context.Payments.Remove(selectedPayment);
                _context.SaveChanges();
                LoadData();
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}