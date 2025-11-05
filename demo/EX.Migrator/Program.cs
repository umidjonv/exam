using System;
using System.Threading.Tasks;

namespace EX.Migrator
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            Console.Title = "Exam Migrator";

            var factory = new AppDbContextFactory();

            await using (var db = factory.CreateDbContext(args))
            {
                if (await db.Database.CanConnectAsync())
                    await db.Database.EnsureCreatedAsync();
                
                Console.WriteLine("DB:ok ..."); 
            }

            Console.WriteLine("Done!");
        }
    }
}
