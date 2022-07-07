using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsAgent.Models;
using MetricsAgent.Services.Interfaces;
using Quartz;

#pragma warning disable CA1416 

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private readonly INetworkMetricsRepository _networkMetricsRepository;
        private readonly PerformanceCounterCategory _networkCounterCategory;

        public NetworkMetricJob(
            INetworkMetricsRepository networkMetricsRepository)
        {
            _networkMetricsRepository = networkMetricsRepository;

            _networkCounterCategory = new PerformanceCounterCategory("Network Interface");
        }

        public Task Execute(IJobExecutionContext context)
        {
            string[] instanceName = _networkCounterCategory.GetInstanceNames();

            foreach (var i in instanceName)
            {
                PerformanceCounter networkSentCounter = new PerformanceCounter(
                    "Network Interface", "Bytes Received/sec", i);

                PerformanceCounter networkReceivedCounter = new PerformanceCounter(
                    "Network Interface", "Bytes Sent/sec", i);

                var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

                _networkMetricsRepository.CreateSentCounter(new NetworkMetric()
                {
                    Time = time.TotalSeconds,
                    Value = (int)networkSentCounter.NextValue()
                });

                _networkMetricsRepository.Create(new NetworkMetric()
                {
                    Time = time.TotalSeconds,
                    Value = (int)networkReceivedCounter.NextValue()
                });
            }


            //var networkSentCounter = _networkSentCounter.NextValue();

            //var timeNetworkSent = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            //_networkMetricsRepository.CreateSentCounter(new NetworkMetric()
            //{
            //    Time = timeNetworkSent.TotalSeconds,
            //    Value = (int)networkSentCounter
            //});


            //var networkReceivedCounter = _networkReceivedCounter.NextValue();

            //var timeNetworkReceived = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            //_networkMetricsRepository.Create(new NetworkMetric()
            //{
            //    Time = timeNetworkReceived.TotalSeconds,
            //    Value = (int)networkReceivedCounter
            //});


            return Task.CompletedTask;
        }
    }
}
