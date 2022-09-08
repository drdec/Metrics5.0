using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsAgent.Models;
using MetricsAgent.Services.Interfaces;
using Quartz;

#pragma warning disable CA1416 

namespace MetricsAgent.Jobs
{
    public class DotnetMetricJob : IJob
    {
        private readonly IDotNetMetricsRepository _dotNetMetricsRepository;
        private readonly PerformanceCounter _dotNetCounterErrors;
        private readonly PerformanceCounter _dotNetCounterMemory;

        public DotnetMetricJob(
            IDotNetMetricsRepository dotNetMetricsRepository)
        {
            _dotNetMetricsRepository = dotNetMetricsRepository;

            _dotNetCounterErrors =
                new PerformanceCounter(
                    ".NET CLR Exceptions", 
                    "# of Exceps Thrown / sec", 
                    "_Global_");

            _dotNetCounterMemory = new PerformanceCounter(
                ".NET CLR Memory",
                "# Bytes in all heaps",
                "_Global_");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var numberDotNetErrors = _dotNetCounterErrors.NextValue();

            var timeDotNetErrors = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _dotNetMetricsRepository.CreateWithErrors(new DotNetMetric()
            {
                Time = timeDotNetErrors.TotalSeconds,
                Value = (int)numberDotNetErrors
            });


            var dotNetUsageInPercents = _dotNetCounterMemory.NextValue();

            var timeDotNetUSage = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _dotNetMetricsRepository.Create(new DotNetMetric()
            {
                Time = timeDotNetUSage.TotalSeconds,
                Value = (int)dotNetUsageInPercents
            });


            return Task.CompletedTask;
        }
    }
}
