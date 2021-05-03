using System;

namespace GetAwsMetric
{
    class Program
    {
        static void Main()
        {
            var client = new AwsMetricClient();
            var result = client.GetMetric(new Amazon.CloudWatch.Model.GetMetricDataRequest()).GetAwaiter().GetResult();
            Console.Read();
        }
    }
}
