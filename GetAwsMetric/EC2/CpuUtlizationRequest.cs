namespace GetAwsMetric.EC2
{
    public class CpuUtlizationRequest : AwsMetricRequest
    {
        public CpuUtlizationRequest(string instanceId) : base("AWS/EC2", "CPUUtilization")
        {
            AddDimension("InstanceId", instanceId);
        }
    }
}
