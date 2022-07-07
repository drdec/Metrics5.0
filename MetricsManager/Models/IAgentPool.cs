using MetricsManager.Models;
using System.Collections.Generic;

namespace MetricsManager.Models
{
    public interface IAgentPool<T>
    {
        void Add(T value);
        T[] Get();
        Dictionary<int, T> Values { get; set; }
    }
}
