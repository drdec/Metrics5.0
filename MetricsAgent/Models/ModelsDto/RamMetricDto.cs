﻿using System;

namespace MetricsAgent.Models.ModelsDto
{
    public class RamMetricDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public TimeSpan Time { get; set; }
    }
}
}
