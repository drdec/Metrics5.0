using System.Collections.Generic;
using MetricsAgent.Models.Dto;

namespace MetricsAgent.Models.Requests
{
    public class AllDotNetMetricsResponse
    {
        public List<DotNetMetricDto> Metrics { get; set; }

        public bool IsEmpty() => Metrics.Count == 0;
    }
}
