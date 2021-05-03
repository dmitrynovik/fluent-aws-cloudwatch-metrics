using System;
using System.Linq;

namespace GetAwsMetric
{
    class Program
    {
        static void Main()
        {
            var request = new AwsMetricRequest("AWS/EC2", "CPUUtilization")
                .AddStatistics(AwsMetricRequest.Statistic.Average)
                .AddDimension("InstanceId", "i-0af87ffc0d4062cfc")
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
