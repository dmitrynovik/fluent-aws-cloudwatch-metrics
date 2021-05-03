using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetAwsMetric
{
    public class MetricRequest
    {
        private DateTime utcFrom;
        private DateTime utcTo;
        private readonly IDictionary<string, string> dimensions = new Dictionary<string, string>();
        private readonly IDictionary<string, Amazon.CloudWatch.Statistic> metrics = new Dictionary<string, Amazon.CloudWatch.Statistic>();

        public int Period { get; set; } = 60;

        public MetricRequest FromUtc(DateTime ts)
        {
            utcFrom = ts;
            return this;
        }

        public MetricRequest ToUtc(DateTime ts)
        {
            utcTo = ts;
            return this;
        }

        public MetricRequest LastMinute()
        {
            utcTo = DateTime.UtcNow;
            utcFrom = utcTo.AddMinutes(-1);
            Period = 60;
            return this;
        }

        public MetricRequest AddMetric(MetricName name, Amazon.CloudWatch.Statistic stat)
        {
            metrics[name.ToString()] = stat;
            return this;
        }

        public MetricRequest ForEC2Instance(string name)
        {
            dimensions["InstanceId"] = name;
            return this;
        }

        public GetMetricDataRequest ToGetMetricDataRequest()
        {
            return new GetMetricDataRequest
            {
                EndTimeUtc = utcTo,
                StartTimeUtc = utcFrom,
                MetricDataQueries = new List<MetricDataQuery>(metrics.Select(m => new MetricDataQuery()))
            };
        }
    }

    public class AwsMetricClient
    {
        // aws cloudwatch get-metric-statistics --namespace AWS/EC2 --metric-name CPUUtilization  --period 60 --statistics Maximum
        // --dimensions Name=InstanceId,Value=i-0af87ffc0d4062cfc --start-time 2021-05-03T03:20:00 --end-time 2021-05-03T03:21:00
        public async Task<GetMetricDataResponse> GetMetric(MetricRequest request)
        {
            using var awsClient = new AmazonCloudWatchClient();
            return await awsClient.GetMetricDataAsync(request.ToGetMetricDataRequest());
        }
    }
}
