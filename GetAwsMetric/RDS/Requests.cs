namespace GetAwsMetric.RDS
{
    public abstract class RdsMetricsRequest : AwsMetricRequest
    {
        protected RdsMetricsRequest(string metric, string instanceId) : base("AWS/RDS", metric)
        {
            AddDimension("DBInstanceIdentifier", instanceId);
        }
    }

    //request.Copy("FreeableMemory", StandardUnit.Bytes),
    //            request.Copy("DatabaseConnections", StandardUnit.Count),
    //            request.Copy("WriteLatency", StandardUnit.Seconds),
    //            request.Copy("ReadThroughput", StandardUnit.BytesSecond),
    //            request.Copy("ReadIOPS", StandardUnit.CountSecond),
    //            request.Copy("WriteThroughput", StandardUnit.BytesSecond),
    //            request.Copy("WriteIOPS", StandardUnit.CountSecond),

    public class CpuUtlizationRequest : RdsMetricsRequest
    {
        public CpuUtlizationRequest(string instanceId) : base("CPUUtilization", instanceId) {  }
    }
}
