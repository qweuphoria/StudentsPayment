using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace StudentsPayment
{
    public partial class PaymentsPage : Page
    {
        private readonly databaseEntities _context = new databaseEntities();
        private List<Payments> _paymentsCache = new List<Payments>();

        public PaymentsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _context.Payments.Load();
            _paymentsCache = _context.Payments.Local.ToList();
            FillMonthFilter();
            ApplyFilters();
        }

        private void FillMonthFilter()
        {
            var selectedValue = (MonthFilterComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            MonthFilterComboBox.Items.Clear();
            MonthFilterComboBox.Items.Add(new ComboBoxItem { Content = "Все" });

            foreach (var month in _paymentsCache
                .Select(p => p.month)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Distinct()
                .OrderBy(m => m))
            {
                MonthFilterComboBox.Items.Add(new ComboBoxItem { Content = month });
            }

            var toSelect = MonthFilterComboBox.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(i => (i.Content?.ToString() ?? "") == (selectedValue ?? "Все"));

            MonthFilterComboBox.SelectedItem = toSelect ?? MonthFilterComboBox.Items[0];
        }

        private void ApplyFilters()
        {
            var search = SearchTextBox.Text?.Trim().ToLower() ?? string.Empty;
            var month = (MonthFilterComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            var yearText = YearFilterTextBox.Text?.Trim();
            int? year = null;

            if (!string.IsNullOrWhiteSpace(yearText))
            {
                if (!int.TryParse(yearText, out var parsedYear))
                {
                    MessageBox.Show("Год должен быть числом.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                year = parsedYear;
            }

            var filtered = _paymentsCache.Where(p =>
                (string.IsNullOrWhiteSpace(search)
                 || p.student_id.ToString().Contains(search)
                 || (p.month ?? "").ToLower().Contains(search)
                 || p.amount.ToString().Contains(search))
                && (string.IsNullOrWhiteSpace(month) || month == "Все" || (p.month ?? string.Empty) == month)
                && (!year.HasValue || p.year == year.Value));

            PaymentsDataGrid.ItemsSource = filtered.ToList();
        }

        private bool ValidatePayment(Payments payment)
        {
            if (payment.student_id <= 0)
            {
                MessageBox.Show("ID студента должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!_context.Students.Any(s => s.student_id == payment.student_id))
            {
                MessageBox.Show("Студент с указанным ID не найден. Проверьте ссылочную целостность.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (payment.amount <= 0)
            {
                MessageBox.Show("Сумма оплаты должна быть больше 0.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(payment.month))
            {
                MessageBox.Show("Поле 'Месяц' не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (payment.year < 2000 || payment.year > 2100)
            {
                MessageBox.Show("Год должен быть в диапазоне 2000-2100.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditWindow = new AddEditPaymentWindow();
            if (addEditWindow.ShowDialog() == true)
            {
                if (!ValidatePayment(addEditWindow.Payment))
                {
                    return;
                }

                _context.Payments.Add(addEditWindow.Payment);
                _context.SaveChanges();
                LoadData();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPayment = PaymentsDataGrid.SelectedItem as Payments;
            if (selectedPayment == null)
            {
                MessageBox.Show("Выберите запись для редактирования.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var addEditWindow = new AddEditPaymentWindow(selectedPayment);
            if (addEditWindow.ShowDialog() == true)
            {
                if (!ValidatePayment(selectedPayment))
                {
                    return;
                }

                _context.SaveChanges();
                LoadData();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPayment = PaymentsDataGrid.SelectedItem as Payments;
            if (selectedPayment == null)
            {
                MessageBox.Show("Выберите запись для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Удалить выбранную оплату?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            _context.Payments.Remove(selectedPayment);
            _context.SaveChanges();
            LoadData();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void MonthFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void YearFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ExportDocButton_Click(object sender, RoutedEventArgs e)
        {
            ExportTable("doc");
        }

        private void ExportXlsButton_Click(object sender, RoutedEventArgs e)
        {
            ExportTable("xls");
        }

        private void ExportTable(string extension)
        {
            var rows = (PaymentsDataGrid.ItemsSource as IEnumerable<Payments>)?.ToList() ?? new List<Payments>();
            if (!rows.Any())
            {
                MessageBox.Show("Нет данных для экспорта.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var saveDialog = new SaveFileDialog
            {
                Filter = extension == "doc" ? "Word document (*.doc)|*.doc" : "Excel document (*.xls)|*.xls",
                FileName = $"PaymentsReport_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}"
            };

            if (saveDialog.ShowDialog() != true)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("ID\tID студента\tДата оплаты\tСумма\tМесяц\tГод");

            foreach (var payment in rows)
            {
                builder.AppendLine($"{payment.payment_id}\t{payment.student_id}\t{payment.payment_date:dd.MM.yyyy}\t{payment.amount}\t{payment.month}\t{payment.year}");
            }

            File.WriteAllText(saveDialog.FileName, builder.ToString(), Encoding.UTF8);
            MessageBox.Show($"Отчет успешно сохранен: {saveDialog.FileName}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
