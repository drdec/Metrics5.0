using System;
using System.Collections.Generic;
using MetricsAgent.Models;
using Org.BouncyCastle.Asn1.BC;

namespace MetricsAgent.Services.Interfaces
{
    public interface INetworkMetricsRepository : IRepository<NetworkMetric>
    {
        void CreateSentCounter(NetworkMetric item);

        IList<NetworkMetric> GetByPeriodSent(TimeSpan fromTime, TimeSpan toTime);
    }
}
