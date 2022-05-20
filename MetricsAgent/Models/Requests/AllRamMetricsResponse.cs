using System.Collections.Generic;
using MetricsAgent.Models.ModelsDto;

namespace MetricsAgent.Models.Requests
{
    public class AllRamMetricsResponse
    {
        public List<RamMetricDto> Metrics { get; set; }
        public bool IsEmpty() => Metrics.Count == 0;
    }
}
