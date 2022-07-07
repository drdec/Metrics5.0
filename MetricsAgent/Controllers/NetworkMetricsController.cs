using System;
using System.Collections.Generic;
using AutoMapper;
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

            _logger?.LogDebug("Успешно вернули данные network метрики");

            return response.IsEmpty() ? Ok("empty") : Ok(response);
        }


        [HttpGet("get-by-period-network-received/from/{fromTime}/to/{toTime}")]
        public IActionResult GetByPeriodNetworkReceived([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger?.LogDebug("Успешно вернули количество отправленных данных network метрики за период времени");
            return Ok(_networkMetricsRepository.GetByPeriod(fromTime, toTime));
        }

        [HttpGet("get-by-perid-network-sent")]
        public IActionResult GetByPeriodNetworkSent([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger?.LogDebug("Успешно вернули количесво отправленных данных network метрики за период времени");
            return Ok(_networkMetricsRepository.GetByPeriodSent(fromTime, toTime));
        }

    }
}