using Quartz;
using System.Threading.Tasks;
using MetricsAgent.Services.Interfaces;
using System;
using System.Diagnostics;
using MetricsAgent.Models;

#pragma warning disable CA1416 

namespace MetricsAgent.Jobs
{
    public class HddMetricJob : IJob
    {
        private readonly IHddMetricsRepository _hddMetricsRepository;
        private readonly PerformanceCounter _hddCounter;

        public HddMetricJob(IHddMetricsRepository hddMetricsRepository)
        {
            _hddMetricsRepository = hddMetricsRepository;
            _hddCounter = 
                new PerformanceCounter(
                    "PhysicalDisk",
                    "% Disk Time",
                    "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var cpuUsageInPercents = _hddCounter.NextValue();

            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _hddMetricsRepository.Create(new HddMetric()
            {
                Time = time.TotalSeconds,
                Value = (int)cpuUsageInPercents
            });

            return Task.CompletedTask;
        }
    }
}
