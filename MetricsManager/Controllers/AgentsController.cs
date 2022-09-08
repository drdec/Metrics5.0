using MetricsManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MetricsManager.Services;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly IMetricAgentRepository _metricAgentRepository;

        public AgentsController(
            IMetricAgentRepository metricAgentRepository)
        {
            _metricAgentRepository = metricAgentRepository;
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            _metricAgentRepository.Create(agentInfo);
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _metricAgentRepository.Enable(agentId);
            return Ok();
        }
        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _metricAgentRepository.Disable(agentId);
            return Ok();
        }

        [HttpGet("get")]
        public IActionResult GetAllAgents()
        {
            return Ok(_metricAgentRepository.Get());
        }

    }
}
