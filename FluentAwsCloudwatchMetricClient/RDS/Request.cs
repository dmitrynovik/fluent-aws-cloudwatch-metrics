namespace GetAwsMetric.RDS
{
    public class RdsMetricsRequest : AwsMetricRequest
    {
        public RdsMetricsRequest(string metric, string instanceId) : base("AWS/RDS", metric)
        {
            AddDimension("DBInstanceIdentifier", instanceId);
        }
    }
}
