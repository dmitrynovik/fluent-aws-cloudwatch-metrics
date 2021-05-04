using Amazon.CloudWatch;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GetAwsMetric.RDS
{
    public class LoadClient
    {
        private readonly AwsMetricClient awsMetricClient;

        public LoadClient() 
        {
            awsMetricClient = new AwsMetricClient();
        }

        public LoadClient(string key, string secret)
        {
            awsMetricClient = new AwsMetricClient(key, secret);
        }

        public async Task<Load> GetCurrent(string dbInstance, int dataPointEachSeconds = 60, int periodSeconds = 120)
        {
            if (string.IsNullOrWhiteSpace(dbInstance))
                throw new ArgumentException($"'{nameof(dbInstance)}' cannot be null or whitespace.", nameof(dbInstance));

            var request = new RdsMetricsRequest("CPUUtilization", dbInstance)
                .AddStatistics(AwsMetricRequest.Statistic.Average)
                .AddStatistics(AwsMetricRequest.Statistic.Minimum)
                .AddStatistics(AwsMetricRequest.Statistic.Maximum)
                .DataPointEach(TimeSpan.FromSeconds(dataPointEachSeconds))
                .Recent(TimeSpan.FromSeconds(periodSeconds));

            var requests = new[]
            {
                request,
                request.Copy("DatabaseConnections", StandardUnit.Count),
                request.Copy("FreeableMemory", StandardUnit.Bytes),
                request.Copy("ReadIOPS", StandardUnit.CountSecond),
                request.Copy("ReadLatency", StandardUnit.Seconds),
                request.Copy("ReadThroughput", StandardUnit.BytesSecond),
                request.Copy("WriteIOPS", StandardUnit.CountSecond),
                request.Copy("WriteLatency", StandardUnit.Seconds),
                request.Copy("WriteThroughput", StandardUnit.BytesSecond),
            };

            var responses = (await awsMetricClient.ExecuteAll(requests)).ToDictionary(x => x.Label, x => x);

            return new Load
            {
                CPUUtilization = responses.GetOrDefault("CPUUtilization")?.Datapoints?.Last().Average,
                DatabaseConnections = responses.GetOrDefault("DatabaseConnections")?.Datapoints?.Last().Maximum,
                FreeableMemory = responses.GetOrDefault("FreeableMemory")?.Datapoints?.Last().Minimum,
                ReadIOPS = responses.GetOrDefault("ReadIOPS")?.Datapoints?.Last().Average,
                ReadLatency = responses.GetOrDefault("ReadLatency")?.Datapoints?.Last().Average,
                ReadThroughput = responses.GetOrDefault("ReadThroughput")?.Datapoints?.Last().Average,
                WriteIOPS = responses.GetOrDefault("WriteIOPS")?.Datapoints?.Last().Average,
                WriteLatency = responses.GetOrDefault("WriteLatency")?.Datapoints?.Last().Average,
                WriteThroughput = responses.GetOrDefault("WriteThroughput")?.Datapoints?.Last().Average,
            };
        }
    }
}
