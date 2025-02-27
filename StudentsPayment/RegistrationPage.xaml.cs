using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StudentsPayment
{
    public partial class RegistrationPage : Page
    {
        private databaseEntities _context;
        private const string AdminSecretCode = "ADMIN123"; // Секретный код для регистрации администратора

        public RegistrationPage()
        {
            InitializeComponent();
            _context = new databaseEntities();
        }

        private void Registration_Button_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullName_TextBox.Text;
            string login = Login_TextBox.Text;
            string password = Password_PasswordBox.Password;
            string confirmPassword = PasswordConfirm_PasswordBox.Password;
            string role = (Role_ComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string secretCode = SecretCode_TextBox.Text;

            // Проверка заполнения полей
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Поле 'ФИО' не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Поле 'Логин' не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Поле 'Пароль' не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Выберите роль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка совпадения паролей
            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка существования пользователя с таким логином
            if (_context.Admins.Any(a => a.username == login))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Если выбрана роль администратора, проверяем секретный код
            if (role == "Администратор" && secretCode != AdminSecretCode)
            {
                MessageBox.Show("Неверный секретный код для регистрации администратора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создание нового пользователя
            var admin = new Admins
            {
                username = login,
                password_hash = password,
                full_name = fullName, // Сохраняем ФИО
                role = role // Роль (администратор или пользователь)
            };

            // Добавление пользователя в базу данных
            _context.Admins.Add(admin);
            _context.SaveChanges();

            MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new AuthorizationPage());
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            // Возврат на страницу авторизации
            NavigationService.Navigate(new AuthorizationPage());
        }

        private void Role_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Показываем поле для ввода секретного кода, если выбрана роль администратора
            if (Role_ComboBox.SelectedItem != null)
            {
                string selectedRole = (Role_ComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                SecretCode_TextBox.Visibility = selectedRole == "Администратор" ? Visibility.Visible : Visibility.Collapsed;
                SecretCode_Label.Visibility = selectedRole == "Администратор" ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}