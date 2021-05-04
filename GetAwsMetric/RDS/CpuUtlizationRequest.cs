namespace GetAwsMetric.RDS
{
    public class CpuUtlizationRequest : AwsMetricRequest
    {
        public CpuUtlizationRequest(string instanceId) : base("AWS/RDS", "CPUUtilization")
        {
            AddDimension("DBInstanceIdentifier", instanceId);
        }
    }
}
