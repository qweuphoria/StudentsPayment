using System.Windows;
using System.Windows.Controls;

namespace StudentsPayment
{
    public partial class AddEditUserWindow : Window
    {
        public Admins User { get; private set; }
        private const string AdminSecretCode = "ADMIN123"; // Секретный код для регистрации администратора

        public AddEditUserWindow(Admins user = null)
        {
            InitializeComponent();
            User = user ?? new Admins();
            DataContext = User;
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Показываем поле для ввода секретного кода, если выбрана роль "Администратор"
            if (RoleComboBox.SelectedItem != null)
            {
                string selectedRole = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                SecretCodeLabel.Visibility = selectedRole == "Администратор" ? Visibility.Visible : Visibility.Collapsed;
                SecretCodeTextBox.Visibility = selectedRole == "Администратор" ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(User.username))
            {
                MessageBox.Show("Логин не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(User.full_name))
            {
                MessageBox.Show("ФИО не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Получаем текст выбранной роли
            if (RoleComboBox.SelectedItem is ComboBoxItem selectedRoleItem)
            {
                User.role = selectedRoleItem.Content.ToString();
            }
            else
            {
                MessageBox.Show("Выберите роль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Если пароль не был введен, устанавливаем значение по умолчанию
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Пароль не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                User.password_hash = PasswordBox.Password;
            }

            // Если выбрана роль администратора, проверяем секретный код
            if (User.role == "Администратор" && SecretCodeTextBox.Text != AdminSecretCode)
            {
                MessageBox.Show("Неверный секретный код для регистрации администратора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}