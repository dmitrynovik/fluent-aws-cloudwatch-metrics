using Amazon.CloudWatch;
using GetAwsMetric.RDS;
using System;
using System.Linq;

namespace GetAwsMetric
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbInstance = args.FirstOrDefault() ?? 
                Environment.GetEnvironmentVariable("RDS_DB_INSTANCE") ?? 
                throw new ArgumentException("no database instance passed!");

            var request = new CpuUtlizationRequest(dbInstance)
                .AddStatistics(AwsMetricRequest.Statistic.Average)
                .AddStatistics(AwsMetricRequest.Statistic.Minimum)
                .AddStatistics(AwsMetricRequest.Statistic.Maximum)
                .WithPeriod(TimeSpan.FromSeconds(60))
                .Last(TimeSpan.FromMinutes(2));

            var requests = new[]
            {
                request,
                request.Copy("FreeableMemory", StandardUnit.Bytes),
                request.Copy("DatabaseConnections", StandardUnit.Count),
                request.Copy("WriteLatency", StandardUnit.Seconds),
                request.Copy("ReadThroughput", StandardUnit.BytesSecond),
                request.Copy("ReadIOPS", StandardUnit.CountSecond),
                request.Copy("WriteThroughput", StandardUnit.BytesSecond),
                request.Copy("WriteIOPS", StandardUnit.CountSecond),
            };

            var result = Query.ExecuteAll(requests).GetAwaiter().GetResult();
            result.ToList().ForEach(x =>
            {
                Console.WriteLine(x.Label);
                foreach(var pt in x.Datapoints)
                {
                    Console.WriteLine($"\t{pt.Timestamp} => Average: {pt.Average}, Max: {pt.Maximum}, Min: {pt.Minimum}");
                }
                Console.WriteLine();
            });

            Console.WriteLine("Press any key to exit ...");
            Console.Read();
        }
    }
}
