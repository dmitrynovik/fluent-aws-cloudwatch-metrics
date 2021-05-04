using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using System;
using System.Threading.Tasks;

namespace GetAwsMetric
{
    public class AwsMetricClient
    {
        public async Task<GetMetricStatisticsResponse> GetMetric(AwsMetricRequest request)
        {
            if (!request.IsValid())
                throw new ArgumentException(nameof(request));

            using var awsClient = new AmazonCloudWatchClient();
            return await awsClient.GetMetricStatisticsAsync(request.ToGetMetricStatisticsRequest());
        }
    }
}
