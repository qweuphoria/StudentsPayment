using System.Data.Entity;

namespace StudentsPayment
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<databaseEntities>
    {
        protected override void Seed(databaseEntities context)
        {
            // Создаем экземпляр DataSeeder
            var seeder = new DataSeeder(context);

            // Заполняем базу данных
            seeder.SeedData();

            base.Seed(context);
        }
    }
}