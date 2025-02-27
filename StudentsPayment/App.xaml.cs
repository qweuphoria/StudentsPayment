using System;
using System.Linq;
using System.Windows;

namespace StudentsPayment
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создаем экземпляр контекста базы данных
            using (var context = new databaseEntities())
            {
                // Проверяем, есть ли данные в базе
                if (!context.Students.Any())
                {
                    // Если база данных пуста, заполняем её
                    var seeder = new DataSeeder(context);
                    seeder.SeedData();
                }
            }
        }
    }
}