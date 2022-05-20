using System.Collections.Generic;
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
    public class RamMetricsController : Controller
    {

        private IRamMetricsRepository _ramMetricsRepository;
        private ILogger<RamMetricsController> _logger;


        public RamMetricsController(
            ILogger<RamMetricsController> logger,
            IRamMetricsRepository ramMetricsRepository)
        {
            _ramMetricsRepository = ramMetricsRepository;
            _logger = logger;
        }


        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricsCreateRequest request)
        {
            RamMetric ramMetric = new RamMetric
            {
                Time = request.Time,
                Value = request.Value
            };

            _ramMetricsRepository.Create(ramMetric);

            if (_logger != null)
                _logger.LogDebug("Успешно добавили новую network метрику: {0}", ramMetric);

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _ramMetricsRepository.GetAll();
            var response = new AllRamMetricsResponse
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new RamMetricDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id
                });
            }

            if (_logger != null)
                _logger.LogDebug("Успешно вернули данные network метрики");

            return response.IsEmpty() ? Ok("empty") : Ok(response);
        }

        [HttpGet("get-by-id")]
        public IActionResult GetById(int id)
        {
            var result = _ramMetricsRepository.GetById(id);

            if (_logger != null)
            {
                _logger.LogDebug($"Успешно вернули данне метрики по id : metrics - {result}, id - {id}");
            }

            return Ok(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            _ramMetricsRepository.Delete(id);

            if (_logger != null)
            {
                _logger.LogDebug($"network метрика успешно удалена : {id}");
            }
            return Ok();
        }

        [HttpPut("Update")]
        public IActionResult Update(RamMetric ramMetric)
        {
            _ramMetricsRepository.Update(ramMetric);

            if (_logger != null)
            {
                _logger.LogDebug($"network метрика успешно обновлена : {ramMetric}");
            }

            return Ok();
        }

        [HttpGet("available")]
        public IActionResult GetRamMetrics()
        {
            return Ok();
        }
    }
}