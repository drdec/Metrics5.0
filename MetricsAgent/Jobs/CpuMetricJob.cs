using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsAgent.Controllers.Interfaces;
using MetricsAgent.Models;
using Quartz;

namespace MetricsAgent.Jobs
{
    public class CpuMetricJob : IJob
    {
        private readonly ICpuMetricsRepository _cpuMetricsRepository;
        private readonly PerformanceCounter _cpuCounter;

        public CpuMetricJob(
            ICpuMetricsRepository cpuMetricsRepository)
        {
            _cpuMetricsRepository = cpuMetricsRepository;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var cpuUsageInPercents = _cpuCounter.NextValue();

            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _cpuMetricsRepository.Create(new CpuMetric()
            {
                Time = time.TotalSeconds,
                Value = (int)cpuUsageInPercents
            });

            return Task.CompletedTask;
        }
    }
}
