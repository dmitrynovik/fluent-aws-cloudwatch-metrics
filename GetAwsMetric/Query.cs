using Amazon.CloudWatch.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetAwsMetric
{
    public static class Query
    {
        public static async Task<GetMetricStatisticsResponse> Execute(AwsMetricRequest request) => await new AwsMetricClient().GetMetric(request);

        public static async Task<GetMetricStatisticsResponse[]> ExecuteAll(IEnumerable<AwsMetricRequest> requests)
        {
            var tasks = requests.Select(r => new AwsMetricClient().GetMetric(r));
            return await Task.WhenAll(tasks);
        }
    }
}
