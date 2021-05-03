using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using System;
using System.Threading.Tasks;

namespace GetAwsMetric
{

    public class AwsMetricClient
    {
        // aws cloudwatch get-metric-statistics --namespace AWS/EC2 --metric-name CPUUtilization  --period 60 --statistics Maximum
        // --dimensions Name=InstanceId,Value=i-0af87ffc0d4062cfc --start-time 2021-05-03T03:20:00 --end-time 2021-05-03T03:21:00
        public async Task<GetMetricStatisticsResponse> GetMetric(AwsMetricRequest request)
        {
            if (!request.IsValid())
                throw new ArgumentException(nameof(request));

            var rq = request.ToGetMetricStatisticsRequest();
            using var awsClient = new AmazonCloudWatchClient();
            return await awsClient.GetMetricStatisticsAsync(rq);
        }
    }
}
