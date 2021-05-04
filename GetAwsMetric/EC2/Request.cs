namespace GetAwsMetric.EC2
{
    public class EC2Request : AwsMetricRequest
    {
        public EC2Request(string metricName, string instanceId) : base("AWS/EC2", metricName)
        {
            AddDimension("InstanceId", instanceId);
        }
    }
}
