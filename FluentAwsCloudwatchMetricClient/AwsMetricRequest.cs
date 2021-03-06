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
            Minimum,
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
        private IDictionary<string, string> dimensions = new Dictionary<string, string>();
        private readonly string metricName;
        private readonly string ns;
        private HashSet<Statistic> stats = new HashSet<Statistic>();
        private StandardUnit unit = StandardUnit.Percent;

        public AwsMetricRequest(string ns, string name)
        {
            metricName = name;
            this.ns = ns;
        }

        public AwsMetricRequest(string name, AwsMetricRequest other, StandardUnit u = null, Statistic? stat = null)
        {
            ns = other.ns;
            metricName = name;
            unit = u == null ? other.unit : u;
            utcFrom = other.utcFrom;
            utcTo = other.utcTo;
            stats = new HashSet<Statistic>(other.stats);
            dimensions = new Dictionary<string, string>(other.dimensions);
            if (stat.HasValue)
            {
                SetStatistics(stat.Value);
            }
        }

        public AwsMetricRequest Copy(string name, StandardUnit u = null, Statistic? stat = null) => new AwsMetricRequest(name, this, u, stat);

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

        public AwsMetricRequest DataPointEach(TimeSpan ts)
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

        public AwsMetricRequest SetStatistics(Statistic stat)
        {
            stats.Clear();
            return AddStatistics(stat);
        }

        public AwsMetricRequest AddDimension(string name, string value)
        {
            dimensions[name] = value;
            return this;
        }

        public AwsMetricRequest Recent(TimeSpan period)
        {
            var now = DateTime.UtcNow;
            utcFrom = now.Add(-period);
            utcTo = now;
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
