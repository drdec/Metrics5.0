using System.Collections.Generic;
using MetricsAgent.Models.ModelsDto;

namespace MetricsAgent.Models.Requests
{
    public class AllHddMetricsResponse
    {
        public List<HddMetricDto> Metrics { get; set; }

        public bool IsEmpty() => Metrics.Count == 0;
    }
}
