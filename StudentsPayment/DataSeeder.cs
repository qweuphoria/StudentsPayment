using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentsPayment
{
    public class DataSeeder
    {
        private readonly databaseEntities _context;

        public DataSeeder(databaseEntities context)
        {
            _context = context;
        }

        public void SeedData()
        {
            // Заполняем таблицу Rooms
            SeedRooms();

            // Генерация случайных данных для студентов
            SeedStudents();

            // Генерация случайных данных для пользователей
            SeedUsers();

            // Генерация случайных данных для оплат
            SeedPayments();

            // Генерация случайных данных для долгов
            SeedDebts();

            // Сохранение изменений в базе данных
            _context.SaveChanges();
        }

        private void SeedRooms()
        {
            var random = new Random();

            for (int i = 1; i <= 100; i++) // Создаем 100 комнат
            {
                var room = new Rooms
                {
                    room_number = $"Room {i}",
                    capacity = random.Next(1, 5), // Вместимость от 1 до 4 человек
                    price_per_month = random.Next(1000, 5001) // Цена за месяц от 1000 до 5000
                };

                _context.Rooms.Add(room);
            }
        }

        private void SeedStudents()
        {
            var random = new Random();
            var firstNames = new List<string> { "Иван", "Петр", "Алексей", "Мария", "Анна", "Дмитрий", "Елена", "Сергей", "Ольга", "Николай" };
            var lastNames = new List<string> { "Иванов", "Петров", "Сидоров", "Кузнецова", "Смирнова", "Васильев", "Попов", "Михайлов", "Новиков", "Федоров" };

            // Получаем список всех комнат
            var rooms = _context.Rooms.ToList();

            for (int i = 0; i < 100; i++)
            {
                var student = new Students
                {
                    first_name = firstNames[random.Next(firstNames.Count)],
                    last_name = lastNames[random.Next(lastNames.Count)],
                    phone_number = GeneratePhoneNumber(random),
                    email = GenerateEmail(random),
                    room_id = rooms[random.Next(rooms.Count)].room_id // Случайный room_id из существующих
                };

                _context.Students.Add(student);
            }
        }

        private void SeedUsers()
        {
            var random = new Random();
            var firstNames = new List<string> { "Иван", "Петр", "Алексей", "Мария", "Анна", "Дмитрий", "Елена", "Сергей", "Ольга", "Николай" };
            var lastNames = new List<string> { "Иванов", "Петров", "Сидоров", "Кузнецова", "Смирнова", "Васильев", "Попов", "Михайлов", "Новиков", "Федоров" };

            for (int i = 0; i < 100; i++)
            {
                var user = new Admins
                {
                    username = $"user{i + 1}", // Уникальный логин
                    password_hash = "password", // Простой пароль для примера
                    full_name = $"{firstNames[random.Next(firstNames.Count)]} {lastNames[random.Next(lastNames.Count)]}",
                    role = i < 10 ? "Администратор" : "Пользователь" // Первые 10 пользователей - администраторы
                };

                _context.Admins.Add(user);
            }
        }

        private void SeedPayments()
        {
            var random = new Random();
            var students = _context.Students.ToList(); // Получаем всех студентов

            foreach (var student in students)
            {
                // Генерация 1-5 оплат для каждого студента
                for (int i = 0; i < random.Next(1, 6); i++)
                {
                    var payment = new Payments
                    {
                        student_id = student.student_id,
                        payment_date = DateTime.Now.AddMonths(-random.Next(1, 13)), // Дата оплаты за последние 12 месяцев
                        amount = random.Next(1000, 10001), // Сумма оплаты от 1000 до 10000
                        month = random.Next(1, 13), // Месяц от 1 до 12
                        year = random.Next(2020, 2024) // Год от 2020 до 2023
                    };

                    _context.Payments.Add(payment);
                }
            }
        }

        private void SeedDebts()
        {
            var random = new Random();
            var students = _context.Students.ToList(); // Получаем всех студентов

            foreach (var student in students)
            {
                // Генерация 0-3 долгов для каждого студента
                for (int i = 0; i < random.Next(0, 4); i++)
                {
                    var debt = new Debts
                    {
                        student_id = student.student_id,
                        debt_amount = random.Next(1000, 5001), // Сумма долга от 1000 до 5000
                        month = random.Next(1, 13), // Месяц от 1 до 12
                        year = random.Next(2020, 2024) // Год от 2020 до 2023
                    };

                    _context.Debts.Add(debt);
                }
            }
        }

        private string GeneratePhoneNumber(Random random)
        {
            return $"+7{random.Next(900, 1000)}{random.Next(1000000, 10000000)}";
        }

        private string GenerateEmail(Random random)
        {
            var domains = new List<string> { "gmail.com", "yandex.ru", "mail.ru", "outlook.com" };
            return $"user{random.Next(1000, 10000)}@{domains[random.Next(domains.Count)]}";
        }
    }
}