using MetricsAgent.Models;
using MetricsAgent.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using MetricsAgent.Controllers.Interfaces;
using MetricsAgent.Models.Dto;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {

        private ICpuMetricsRepository _cpuMetricsRepository;
        private ILogger<CpuMetricsController> _logger;


        public CpuMetricsController(
            ILogger<CpuMetricsController> logger,
            ICpuMetricsRepository cpuMetricsRepository)
        {
            _cpuMetricsRepository = cpuMetricsRepository;
            _logger = logger;
        }


        [HttpPost("create")]
        public IActionResult Create([FromBody] CpuMetricCreateRequest request)
        {
            CpuMetric cpuMetric = new CpuMetric
            {
                Time = request.Time,
                Value = request.Value
            };

            _cpuMetricsRepository.Create(cpuMetric);

            if (_logger != null)
                _logger.LogDebug("Успешно добавили новую cpu метрику: {0}", cpuMetric);

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _cpuMetricsRepository.GetAll();
            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new CpuMetricDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id
                });
            }

            if (_logger != null)
                _logger.LogDebug("Успешно вернули данные cpu метрики");

            return response.IsEmpty() ? Ok("empty") : Ok(response);
        }

        [HttpGet("get-by-id")]
        public IActionResult GetById(int id)
        {
            var result = _cpuMetricsRepository.GetById(id);

            if (_logger != null)
            {
                _logger.LogDebug($"Успешно вернули данне метрики по id : metrics - {result}, id - {id}");
            }

            return result == null ? Ok("sorry, data not found") : Ok(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            _cpuMetricsRepository.Delete(id);
            if (_logger != null)
            {
                _logger.LogDebug($"cpu метрика успешно удалена : {id}");
            }
            return Ok();
        }

        [HttpPut("Update")]
        public IActionResult Update(CpuMetric cpuMetric)
        {
            _cpuMetricsRepository.Update(cpuMetric);

            if (_logger != null)
            {
                _logger.LogDebug($"cpu метрика успешно обновлена : {cpuMetric}");
            }

            return Ok();
        }


        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok(_cpuMetricsRepository.GetByPeriod(fromTime, toTime));
        }
    }
}
