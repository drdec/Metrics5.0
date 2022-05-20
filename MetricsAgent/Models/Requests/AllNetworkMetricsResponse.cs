using System.Collections.Generic;
using MetricsAgent.Models.ModelsDto;

namespace MetricsAgent.Models.Requests
{
    public class AllNetworkMetricsResponse
    {
        public List<NetworkMetricDto> Metrics { get; set; }
        public bool IsEmpty() => Metrics.Count == 0;
    }
}
