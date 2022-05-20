using System;

namespace MetricsAgent.Models.Requests
{
    public class HddMetricsCreateRequest
    {
        public int Value { get; set; }
        public TimeSpan Time { get; set; }
    }
}
