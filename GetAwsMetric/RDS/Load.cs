namespace GetAwsMetric.RDS
{
    public class Load
    {
        public double? CPUUtilization { get; set; }
        public double? FreeableMemory { get; set; }
        public double? DatabaseConnections { get; set; }
        public double? ReadLatency { get; internal set; }
        public double? ReadIOPS { get; set; }
        public double? ReadThroughput { get; set; }
        public double? WriteIOPS { get; set; }
        public double? WriteLatency { get; set; }
        public double? WriteThroughput { get; set; }
    }
}
