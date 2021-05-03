using System;
using System.Linq;

namespace GetAwsMetric
{
    class Program
    {
        static void Main()
        {
            var request = new EC2CpuUtlizationRequest("i-0af87ffc0d4062cfc")
                .AddStatistics(AwsMetricRequest.Statistic.Average)
                .LastMinutes(5);

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
