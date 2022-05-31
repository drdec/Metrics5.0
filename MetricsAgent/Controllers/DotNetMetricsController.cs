using System;
using System.Collections.Generic;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MetricsAgent.Services;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/[controller]")]
    [ApiController]
    public class DotNetMetricsController : Controller
    {
        private IDotNetMetricsRepository _dotNetMetricsRepository;
        private ILogger<DotNetMetricsController> _logger;

        public DotNetMetricsController(IDotNetMetricsRepository dotNetMetricsRepository, ILogger<DotNetMetricsController> logger)
        {
            _dotNetMetricsRepository = dotNetMetricsRepository;
            _logger = logger;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetMetricsCreateRequest request)
        {
            DotNetMetric metrics = new DotNetMetric()
            {
                Time = request.Time,
                Value = request.Value
            };

            _dotNetMetricsRepository.Create(metrics);

            if (_logger != null)
            {
                _logger.LogDebug("Успешно добавили новую dotNet метрику: {0}", metrics);
            }

            return Ok();
        }

        [HttpPut("Update")]
        public IActionResult Update(DotNetMetric dotNetMetric)
        {
            _dotNetMetricsRepository.Update(dotNetMetric);

            if (_logger != null)
            {
                _logger.LogDebug($"cpu метрика успешно обновлена : {dotNetMetric}");
            }

            return Ok();
        }


        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            _dotNetMetricsRepository.Delete(id);

            if (_logger != null)
            {
                _logger.LogDebug($"Успешно удалили метрику dotNet с id - {id}");
            }

            return Ok();
        }


        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
        public IActionResult GetDotNetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok(_dotNetMetricsRepository.GetErrorsCount(fromTime, toTime));
        }


        [HttpGet("get-all")]
        public IActionResult GetAllItems()
        {
            var metrics = _dotNetMetricsRepository.GetAll();
            var response = new AllDotNetMetricsResponse()
            {
                Metrics = new List<DotNetMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new DotNetMetricDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id
                });
            }

            if (_logger != null)
                _logger.LogDebug("Успешно вернули данные dotNet метрики");

            return response.IsEmpty() ? Ok("empty") : Ok(response);
        }

        [HttpGet("get-by-id")]
        public IActionResult GetById(int id)
        {
            var result = _dotNetMetricsRepository.GetById(id);

            if (_logger != null)
            {
                _logger.LogDebug($"Успешно вернули данне метрики по id : metrics - {result}, id - {id}");
            }

            return Ok(result);
        }

    }
}