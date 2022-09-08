using System.Collections.Generic;
using MetricsAgent.Models.ModelsDto;

namespace MetricsAgent.Models.Requests
{
    public class AllCpuMetricsResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }

        public bool IsEmpty() => Metrics.Count == 0;
    }
}
