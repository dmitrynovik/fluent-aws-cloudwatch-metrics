using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GetAwsMetric
{

    public class AwsMetricRequest
    {
        public enum Statistic
        {
            Minimium,
            Maximum,
            Average
        }

        public enum MetricName
        {
            CpuUtilised,
            MemoryUtilised
        }

        private DateTime utcFrom;
        private DateTime utcTo;
        private readonly IDictionary<string, string> dimensions = new Dictionary<string, string>();
        private readonly string metricName;
        private readonly string ns;
        private readonly HashSet<Statistic> stats = new HashSet<Statistic>();
        private StandardUnit unit = StandardUnit.Percent;

        public AwsMetricRequest(string ns, string name)
        {
            metricName = name;
            this.ns = ns;
        }

        public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(60);

        public AwsMetricRequest FromUtc(DateTime ts)
        {
            utcFrom = ts;
            return this;
        }

        public AwsMetricRequest ToUtc(DateTime ts)
        {
            utcTo = ts;
            return this;
        }

        public AwsMetricRequest WithPeriod(TimeSpan ts)
        {
            Period = ts;
            return this;
        }

        public AwsMetricRequest Unit(StandardUnit unit)
        {
            this.unit = unit;
            return this;
        }

        public AwsMetricRequest AddStatistics(Statistic stat)
        {
            stats.Add(stat);
            return this;
        }
        public AwsMetricRequest AddDimension(string name, string value)
        {
            dimensions[name] = value;
            return this;
        }

        public AwsMetricRequest Last(TimeSpan period)
        {
            var now = DateTime.UtcNow;
            utcFrom = now.Add(-period);
            utcTo = now;
            return this;
        }

        public AwsMetricRequest ForEC2Instance(string name)
        {
            dimensions["InstanceId"] = name;
            return this;
        }

        public GetMetricStatisticsRequest ToGetMetricStatisticsRequest()
        {
            return new GetMetricStatisticsRequest
            {
                Dimensions = dimensions.Select(kvp => new Dimension { Name = kvp.Key, Value = kvp.Value }).ToList(),
                EndTimeUtc = utcTo,
                MetricName = metricName,
                Namespace = ns,
                StartTimeUtc = utcFrom,
                Statistics = new List<string>(stats.Select(s => s.ToString())),
                Period = (int)Period.TotalSeconds,
                Unit = unit
            };
        }

        public bool IsValid() => dimensions.Any() &&
            stats.Any() &&
            !string.IsNullOrWhiteSpace(metricName) &&
            !string.IsNullOrWhiteSpace(ns) &&
            utcFrom != DateTime.MinValue &&
            utcTo != DateTime.MinValue &&
            Period.TotalSeconds >= 1;

        public override string ToString() => $"{ns}:{metricName}:{string.Join(',', stats.Select(s => s.ToString()))} {utcFrom} - {utcTo} sampling={Period.TotalSeconds} seconds";
    }
}
