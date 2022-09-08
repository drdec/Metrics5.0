using MetricsManager.Models;
using MetricsManager.Models.Requests;

namespace MetricsManager.Services
{
    public interface IMetricsAgentClient
    {
        CpuMetricsResponse GetCpuMetrics(CpuMetricRequest cpuMetricsRequest);


    }
}
