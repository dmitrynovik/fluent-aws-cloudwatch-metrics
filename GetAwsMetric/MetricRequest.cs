using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GetAwsMetric
{
    public class MetricRequest
    {
        private DateTime utcFrom;
        private DateTime utcTo;
        private readonly IDictionary<string, string> dimensions = new Dictionary<string, string>();
        private readonly string metricName;
        private readonly string ns;
        private readonly HashSet<Statistic> stats = new HashSet<Statistic>();
        private StandardUnit unit = StandardUnit.Percent;

        public MetricRequest(string ns, string name)
        {
            metricName = name;
            this.ns = ns;
        }

        public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(60);

        public MetricRequest FromUtc(DateTime ts)
        {
            utcFrom = ts;
            return this;
        }

        public MetricRequest ToUtc(DateTime ts)
        {
            utcTo = ts;
            return this;
        }

        public MetricRequest WithPeriod(TimeSpan ts)
        {
            return this;
        }

        public MetricRequest Unit(StandardUnit unit)
        {
            this.unit = unit;
            return this;
        }

        public MetricRequest AddStatistics(Statistic stat)
        {
            stats.Add(stat);
            return this;
        }
        public MetricRequest AddDimension(string name, string value)
        {
            dimensions[name] = value;
            return this;
        }

        public MetricRequest LastMinutes(int minutes)
        {
            var now = DateTime.UtcNow;
            utcFrom = now.AddMinutes(-minutes);
            utcTo = now;
            Period = TimeSpan.FromSeconds(60);
            return this;
        }

        public MetricRequest ForEC2Instance(string name)
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
