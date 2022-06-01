using System;
using System.Collections.Generic;
using AutoMapper;
using MetricsAgent.Controllers.Interfaces;
using MetricsAgent.Models;
using MetricsAgent.Models.ModelsDto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/[controller]")]
    [ApiController]
    public class NetworkMetricsController : Controller
    {
        private readonly INetworkMetricsRepository _networkMetricsRepository;
        private readonly ILogger<NetworkMetricsController> _logger;
        private readonly IMapper _mapper;

        public NetworkMetricsController(
            ILogger<NetworkMetricsController> logger,
            INetworkMetricsRepository networkMetricsRepository,
            IMapper mapper)
        {
            _networkMetricsRepository = networkMetricsRepository;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpPost("create")]
        public IActionResult Create([FromBody] NetworkMetricsCreateRequest request)
        {
            NetworkMetric networkMetric = new()
            {
                Time = request.Time.TotalSeconds,
                Value = request.Value
            };

            _networkMetricsRepository.Create(networkMetric);

            if (_logger != null)
                _logger.LogDebug("Успешно добавили новую network метрику: {0}", networkMetric);

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _networkMetricsRepository.GetAll();
            var response = new AllNetworkMetricsResponse
            {
                Metrics = new List<NetworkMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));
            }

            if (_logger != null)
                _logger.LogDebug("Успешно вернули данные network метрики");

            return response.IsEmpty() ? Ok("empty") : Ok(response);
        }

        [HttpGet("get-by-id")]
        public IActionResult GetById(int id)
        {
            var result = _networkMetricsRepository.GetById(id);

            if (_logger != null)
            {
                _logger.LogDebug($"Успешно вернули данне метрики по id : metrics - {result}, id - {id}");
            }

            return Ok(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            _networkMetricsRepository.Delete(id);

            if (_logger != null)
            {
                _logger.LogDebug($"network метрика успешно удалена : {id}");
            }
            return Ok();
        }

        [HttpPut("Update")]
        public IActionResult Update(NetworkMetric networkMetric)
        {
            _networkMetricsRepository.Update(networkMetric);

            if (_logger != null)
            {
                _logger.LogDebug($"network метрика успешно обновлена : {networkMetric}");
            }

            return Ok();
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetNetworkMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok(_networkMetricsRepository.GetByPeriod(fromTime, toTime));
        }
    }
}