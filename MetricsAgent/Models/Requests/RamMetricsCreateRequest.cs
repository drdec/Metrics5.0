using System;

namespace MetricsAgent.Models.Requests
{
    public class RamMetricsCreateRequest
    {
        public int Value { get; set; }
        public TimeSpan Time { get; set; }
    }
}
