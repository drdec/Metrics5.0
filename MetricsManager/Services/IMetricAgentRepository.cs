using System.Collections.Generic;
using MetricsManager.Models;
using Microsoft.OpenApi.Services;

namespace MetricsManager.Services
{
    public interface IMetricAgentRepository
    {
        void Create(AgentInfo agentInfo);

        void Enable(int id);

        void Disable(int id);

        IList<AgentInfo> Get();
    }
}
