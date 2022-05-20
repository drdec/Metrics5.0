using System;

namespace MetricsAgent.Models.Requests
{
    public class DotNetMetricsCreateRequest
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }
    }
}
