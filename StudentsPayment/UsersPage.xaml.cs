using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StudentsPayment
{
    public partial class UsersPage : Page
    {
        private databaseEntities _context;

        public UsersPage()
        {
            InitializeComponent();
            _context = new databaseEntities();
            LoadData();
        }

        private void LoadData()
        {
            // Загрузка всех пользователей из таблицы Admins
            _context.Admins.Load();
            UsersDataGrid.ItemsSource = _context.Admins.Local;
        }

        // Добавление нового пользователя
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditWindow = new AddEditUserWindow();
            if (addEditWindow.ShowDialog() == true)
            {
                try
                {
                    _context.Admins.Add(addEditWindow.User);
                    _context.SaveChanges();
                    LoadData();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    // Сбор сообщений об ошибках валидации
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    // Объединение сообщений в одну строку
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Вывод сообщения об ошибке
                    MessageBox.Show("Ошибка валидации: " + fullErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Редактирование выбранного пользователя
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersDataGrid.SelectedItem as Admins;
            if (selectedUser != null)
            {
                var addEditWindow = new AddEditUserWindow(selectedUser);
                if (addEditWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Удаление выбранного пользователя
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersDataGrid.SelectedItem as Admins;
            if (selectedUser != null)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _context.Admins.Remove(selectedUser);
                    _context.SaveChanges();
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обновление данных
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}