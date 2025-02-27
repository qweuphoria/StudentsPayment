using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StudentsPayment
{
    public partial class AuthorizationPage : Page
    {
        private databaseEntities _context;

        public AuthorizationPage()
        {
            InitializeComponent();
            _context = new databaseEntities();
        }

        private void Enter_Button_Click(object sender, RoutedEventArgs e)
        {
            string login = Login_TextBox.Text;
            string password = Password_PasswordBox.Password;

            // Проверка заполнения полей
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Логин и пароль не могут быть пустыми!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Оставаться на странице авторизации
            }

            // Поиск пользователя в таблице Admins
            var admin = _context.Admins.FirstOrDefault(a => a.username == login && a.password_hash == password);

            if (admin != null)
            {
                MessageBox.Show("Авторизация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                // Создаем новое главное окно и передаем роль пользователя
                var mainWindow = new MainWindow(admin.role);
                mainWindow.Show();
                // Закрываем текущее окно авторизации
                Window.GetWindow(this)?.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль! Попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                // Очистка полей для повторного ввода
                Password_PasswordBox.Clear();
            }
        }

        private void Registration_Button_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу регистрации
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}