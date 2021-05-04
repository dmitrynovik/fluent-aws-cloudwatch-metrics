namespace GetAwsMetric.RDS
{
    public class Load
    {
        public double? CPUUtilization { get; internal set; }
        public double? FreeableMemory { get; internal set; }
        public double? DatabaseConnections { get; internal set; }
        public double? ReadLatency { get; internal set; }
        public double? ReadIOPS { get; internal set; }
        public double? ReadThroughput { get; internal set; }
        public double? WriteIOPS { get; internal set; }
        public double? WriteLatency { get; internal set; }
        public double? WriteThroughput { get; internal set; }
    }
}
