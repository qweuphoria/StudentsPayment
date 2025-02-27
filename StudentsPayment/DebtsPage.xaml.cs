using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;

namespace StudentsPayment
{
    public partial class DebtsPage : Page
    {
        private databaseEntities _context = new databaseEntities();

        public DebtsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _context.Debts.Load();
            DebtsDataGrid.ItemsSource = _context.Debts.Local;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditWindow = new AddEditDebtWindow();
            if (addEditWindow.ShowDialog() == true)
            {
                _context.Debts.Add(addEditWindow.Debt);
                _context.SaveChanges();
                LoadData();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDebt = DebtsDataGrid.SelectedItem as Debts;
            if (selectedDebt != null)
            {
                var addEditWindow = new AddEditDebtWindow(selectedDebt);
                if (addEditWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDebt = DebtsDataGrid.SelectedItem as Debts;
            if (selectedDebt != null)
            {
                _context.Debts.Remove(selectedDebt);
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