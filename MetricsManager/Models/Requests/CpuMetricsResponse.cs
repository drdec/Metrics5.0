using System.Collections.Generic;

namespace MetricsManager.Models.Requests
{
    public class CpuMetricsResponse
    {
        public int AgentId { get; set; }

        public List<CpuMetric> Metrics { get; set; }
    }
}
