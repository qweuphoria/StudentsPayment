using System.Windows;

namespace StudentsPayment
{
    public partial class AddEditStudentWindow : Window
    {
        public Students Student { get; private set; }

        public AddEditStudentWindow(Students student = null)
        {
            InitializeComponent();
            Student = student ?? new Students();
            DataContext = Student;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}