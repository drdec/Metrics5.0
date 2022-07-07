using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using MetricsManager.Models;
using MetricsManager.Models.Requests;
using Newtonsoft.Json;

namespace MetricsManager.Controllers
{
    [Route("api/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAgentPool<AgentInfo> _agentPool;

        public CpuMetricsController(
            IHttpClientFactory httpClientFactory,
            IAgentPool<AgentInfo> agentPool)
        {
            _agentPool = agentPool;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            AgentInfo agentInfo = _agentPool.Get().FirstOrDefault(agent => agent.AgentId == agentId);

            if (agentInfo == null)
            {
                return BadRequest();
            }

            string requestQuery =
                $"{agentInfo.AgentAddress}api/metrics/cpu" +
                $"/from/{fromTime:dd\\.hh\\:mm\\:ss}/to/{toTime:dd\\.hh\\:mm\\:ss}";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
            httpRequestMessage.Headers.Add("Accept", "application/json");
            
            var httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response =  httpClient.SendAsync(httpRequestMessage).Result;
            
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;

                CpuMetricsResponse metric =
                    (CpuMetricsResponse)JsonConvert.DeserializeObject(responseString, typeof(CpuMetricsResponse));
                
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }
    }
}
