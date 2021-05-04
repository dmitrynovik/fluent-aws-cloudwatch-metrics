using GetAwsMetric.RDS;
using System;
using System.Linq;

namespace GetAwsMetric
{
    class Program
    {
        static void Main()
        {
            var request = new CpuUtlizationRequest("solrad-develop")
                .AddStatistics(AwsMetricRequest.Statistic.Average)
                .WithPeriod(TimeSpan.FromSeconds(60))
                .Last(TimeSpan.FromMinutes(2));

            var client = new AwsMetricClient();
            Console.WriteLine(request);
            Console.WriteLine();

            var result = client.GetMetric(request).GetAwaiter().GetResult();
            result.Datapoints.ToList().ForEach(x =>
            {
                Console.WriteLine($"{x.Timestamp}: {x.Average}");
            });

            Console.Read();
        }
    }
}
