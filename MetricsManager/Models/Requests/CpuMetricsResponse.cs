using System.Collections.Generic;

namespace MetricsManager.Models.Requests
{
    public class CpuMetricsResponse
    {
        public List<CpuMetric> Metrics { get; set; }
    }
}
