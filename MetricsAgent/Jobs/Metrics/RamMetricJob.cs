using System;
using System.Diagnostics;
using Quartz;
using System.Threading.Tasks;
using MetricsAgent.Models;
using MetricsAgent.Services.Interfaces;

#pragma warning disable CA1416 

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        private readonly IRamMetricsRepository _ramMetricsRepository;
        private readonly PerformanceCounter _ramCounter;

        public RamMetricJob(
            IRamMetricsRepository ramMetricsRepository)
        {
            _ramMetricsRepository = ramMetricsRepository;
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var cpuUsageInPercents = _ramCounter.NextValue();

            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _ramMetricsRepository.Create(new RamMetric()
            {
                Time = time.TotalSeconds,
                Value = (int)cpuUsageInPercents
            });

            return Task.CompletedTask;
        }
    }
}
