# fluent-aws-cloudwatch-metrics
Getting a bunch of metrics from AWS made easier!

## Single metric - an example
Get single CloudWatch metric for last 2 minutes CPU Utilization with data point each minute:
```
var request = new RdsMetricsRequest("CPUUtilization", dbInstance)
    .AddStatistics(AwsMetricRequest.Statistic.Average)
    .AddStatistics(AwsMetricRequest.Statistic.Minimum)
    .AddStatistics(AwsMetricRequest.Statistic.Maximum)
    .DataPointEach(TimeSpan.FromSeconds(60))
    .Recent(TimeSpan.FromMinutes(2));

var response = await awsMetricClient.Execute(request);
```

## Multiple metrics (or "load")
```
var load = await new LoadClient().GetCurrent(dbInstance);

```

This produces the output object:
```

{
  "CPUUtilization": 8,
  "FreeableMemory": 22941003776,
  "DatabaseConnections": 45,
  "ReadLatency": 0,
  "ReadIOPS": 0,
  "ReadThroughput": 2516130.864485592,
  "WriteIOPS": 9.865186888633371,
  "WriteLatency": 0.0006730937773882559,
  "WriteThroughput": 9903894.668422192
}


```
