namespace GetAwsMetric
{
    public class EC2CpuUtlizationRequest : AwsMetricRequest
    {
        public EC2CpuUtlizationRequest(string instanceId) : base("AWS/EC2", "CPUUtilization")
        {
            AddDimension("InstanceId", instanceId);
        }
    }
}
