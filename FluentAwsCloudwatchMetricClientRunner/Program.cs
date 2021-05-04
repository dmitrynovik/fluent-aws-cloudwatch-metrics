using GetAwsMetric.RDS;
using System;
using System.Linq;
using System.Text.Json;

namespace GetAwsMetric
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbInstance = args.FirstOrDefault() ?? 
                Environment.GetEnvironmentVariable("RDS_DB_INSTANCE") ?? 
                throw new ArgumentException("no database instance passed!");

            var load = new LoadClient().GetCurrent(dbInstance).GetAwaiter().GetResult();

            Console.WriteLine($"Current RDS instance {dbInstance} load:\n");
            Console.WriteLine(JsonSerializer.Serialize(load, new JsonSerializerOptions { WriteIndented = true }));

            Console.WriteLine("\nPress any key to exit ...");
            Console.Read();
        }
    }
}
