using System;
using MetricsAgent.Models;

namespace MetricsAgent.Services.Interfaces
{
    public interface IDotNetMetricsRepository : IRepository<DotNetMetric>
    {
        int GetErrorsCount(TimeSpan fromTime, TimeSpan toTime);

        void CreateWithErrors(DotNetMetric item);
    }
}
