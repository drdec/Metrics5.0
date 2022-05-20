using MetricsAgent.Models;
using MetricsAgent.Services;
using MetricsAgent.Services.Interfaces;

namespace MetricsAgent.Controllers.Interfaces
{
    public interface ICpuMetricsRepository : IRepository<CpuMetric>
    {
    }
}
