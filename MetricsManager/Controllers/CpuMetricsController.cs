using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using MetricsManager.Models;
using MetricsManager.Models.Requests;
using MetricsManager.Services;
using Newtonsoft.Json;

namespace MetricsManager.Controllers
{
    [Route("api/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        #region Services
        private readonly IMetricsAgentClient _metricsAgentClient;
        
        #endregion

        public CpuMetricsController(IMetricsAgentClient metricsAgentClient)
        {
            _metricsAgentClient = metricsAgentClient;
        }

        //[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        //public IActionResult GetMetricsFromAgent(
        //    [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        //{
        //    AgentInfo agentInfo = _agentPool.Get().FirstOrDefault(agent => agent.AgentId == agentId);

        //    if (agentInfo == null)
        //    {
        //        return BadRequest();
        //    }

        //    string requestQuery =
        //        $"{agentInfo.AgentAddress}api/metrics/cpu" +
        //        $"/from/{fromTime:dd\\.hh\\:mm\\:ss}/to/{toTime:dd\\.hh\\:mm\\:ss}";

        //    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
        //    httpRequestMessage.Headers.Add("Accept", "application/json");

        //    var httpClient = _httpClientFactory.CreateClient();
        //    HttpResponseMessage response = httpClient.SendAsync(httpRequestMessage).Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string responseString = response.Content.ReadAsStringAsync().Result;

        //        CpuMetricsResponse metric = JsonConvert.DeserializeObject<CpuMetricsResponse>(responseString);
        //        metric.AgentId = agentId;

        //        return Ok(metric);
        //    }

        //    return BadRequest();
        //}

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            CpuMetricsResponse response =  _metricsAgentClient.GetCpuMetrics(new CpuMetricRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }
    }
}
