using System.Windows;

namespace StudentsPayment
{
    public partial class MainWindow : Window
    {
        private string _currentUserRole;

        public MainWindow(string userRole = null)
        {
            InitializeComponent();
            _currentUserRole = userRole;

            // Показываем кнопку "Пользователи" только для администраторов
            if (_currentUserRole == "Администратор")
            {
                UsersButton.Visibility = Visibility.Visible;
            }

            // Переход на страницу студентов по умолчанию
            MainFrame.Navigate(new StudentsPage());
        }

        private void StudentsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StudentsPage());
        }

        private void RoomsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RoomsPage());
        }

        private void PaymentsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PaymentsPage());
        }

        private void DebtsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DebtsPage());
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsersPage());
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Очистка текущих данных сессии (если они есть)
            _currentUserRole = null; // Сбрасываем роль пользователя

            // Открываем окно авторизации
            var authorizationPage = new AuthorizationPage();
            var authorizationWindow = new Window
            {
                Content = authorizationPage,
                Title = "Авторизация",
                Width = 400,
                Height = 350,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            // Закрываем текущее окно
            this.Close();

            // Открываем окно авторизации
            authorizationWindow.Show();
        }
    }
}