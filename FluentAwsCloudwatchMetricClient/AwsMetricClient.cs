using Amazon;
using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetAwsMetric
{
    public class AwsMetricClient
    {
        public AwsMetricClient() { }
        public AwsMetricClient(string key, string secret, RegionEndpoint endpoint)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            if (string.IsNullOrWhiteSpace(secret)) throw new ArgumentException($"'{nameof(secret)}' cannot be null or whitespace.", nameof(secret));
            Key = key;
            Secret = secret;
            Endpoint = endpoint;
        }

        public string Key { get; }
        public string Secret { get; }
        public RegionEndpoint Endpoint { get; }

        public async Task<GetMetricStatisticsResponse> Execute(AwsMetricRequest request)
        {
            if (!request.IsValid()) throw new ArgumentException(nameof(request));
            using var awsClient = Key == null ? new AmazonCloudWatchClient() : new AmazonCloudWatchClient(Key, Secret, Endpoint);
            return await awsClient.GetMetricStatisticsAsync(request.ToGetMetricStatisticsRequest());
        }

        public async Task<GetMetricStatisticsResponse[]> ExecuteAll(IEnumerable<AwsMetricRequest> requests)
        {
            var tasks = requests.Select(r => new AwsMetricClient().Execute(r));
            return await Task.WhenAll(tasks);
        }
    }
}
