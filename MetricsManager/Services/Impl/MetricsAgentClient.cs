using System;
using System.Linq;
using System.Net.Http;
using MetricsManager.Models;
using MetricsManager.Models.Requests;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MetricsManager.Services.Impl
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        #region Services

        private readonly HttpClient _httpClient;
        private readonly ILogger<MetricsAgentClient> _logger;

        #endregion

        public MetricsAgentClient(
            HttpClient httpClient,
            ILogger<MetricsAgentClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public CpuMetricsResponse GetCpuMetrics(CpuMetricRequest cpuMetricsRequest)
        {
            try
            {
                AgentInfo agentInfo = new()
                {
                    AgentAddress = new Uri("string"),
                    AgentId = 1,
                    Enable = true
                };
                    //_agentPool.Get().FirstOrDefault(agent => agent.AgentId == cpuMetricsRequest.AgentId);

                if (agentInfo == null)
                {
                    throw new Exception($"AgentId {cpuMetricsRequest.AgentId} not found.");
                }

                string requestQuery =
                    $"{agentInfo.AgentAddress}api/metrics/cpu" +
                    $"/from/{cpuMetricsRequest.FromTime:dd\\.hh\\:mm\\:ss}/to/{cpuMetricsRequest.ToTime:dd\\.hh\\:mm\\:ss}";

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
                httpRequestMessage.Headers.Add("Accept", "application/json");

                HttpResponseMessage response = _httpClient.SendAsync(httpRequestMessage).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    CpuMetricsResponse cpuMetricsResponse =
                        JsonConvert.DeserializeObject<CpuMetricsResponse>(responseString);
                    cpuMetricsResponse.AgentId = cpuMetricsRequest.AgentId;

                    return cpuMetricsResponse;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return null;
        }
    }
}
