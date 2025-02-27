using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;

namespace StudentsPayment
{
    public partial class StudentsPage : Page
    {
        databaseEntities _context = new databaseEntities();

        public StudentsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _context.Students.Load();
            StudentsDataGrid.ItemsSource = _context.Students.Local;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditWindow = new AddEditStudentWindow();
            if (addEditWindow.ShowDialog() == true)
            {
                _context.Students.Add(addEditWindow.Student);
                _context.SaveChanges();
                LoadData();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudent = StudentsDataGrid.SelectedItem as Students;
            if (selectedStudent != null)
            {
                var addEditWindow = new AddEditStudentWindow(selectedStudent);
                if (addEditWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudent = StudentsDataGrid.SelectedItem as Students;
            if (selectedStudent != null)
            {
                _context.Students.Remove(selectedStudent);
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