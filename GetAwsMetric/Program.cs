using System;
using System.Linq;

namespace GetAwsMetric
{
    class Program
    {
        static void Main()
        {
            var request = new MetricRequest("AWS/EC2", "CPUUtilization")
                .AddStatistics(Statistic.Average)
                .AddDimension("InstanceId", "i-0af87ffc0d4062cfc")
                .Last5Minutes();

            var client = new AwsMetricClient();
            Console.WriteLine(request);
            var result = client.GetMetric(request).GetAwaiter().GetResult();
            result.Datapoints.ToList().ForEach(x =>
            {
                Console.WriteLine($"{x.Timestamp}: {x.Average}");
            });

            Console.Read();
        }
    }
}
