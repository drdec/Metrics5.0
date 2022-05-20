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
    public class HddMetricsController : Controller
    {

        private IHddMetricsRepository _hddMetricsRepository;
        private ILogger<HddMetricsController> _logger;


        public HddMetricsController(
            ILogger<HddMetricsController> logger,
            IHddMetricsRepository hddMetricsRepository)
        {
            _hddMetricsRepository = hddMetricsRepository;
            _logger = logger;
        }


        [HttpPost("create")]
        public IActionResult Create([FromBody] HddMetricsCreateRequest request)
        {
            var hddMetric = new HddMetric()
            {
                Time = request.Time,
                Value = request.Value
            };

            _hddMetricsRepository.Create(hddMetric);

            if (_logger != null)
                _logger.LogDebug("Успешно добавили новую hdd метрику: {0}", hddMetric);

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _hddMetricsRepository.GetAll();
            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new HddMetricDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id
                });
            }

            if (_logger != null)
                _logger.LogDebug("Успешно вернули данные hdd метрики");

            return response.IsEmpty() ? Ok("empty") : Ok(response);
        }

        [HttpGet("get-by-id")]
        public IActionResult GetById(int id)
        {
            var result = _hddMetricsRepository.GetById(id);

            if (_logger != null)
            {
                _logger.LogDebug($"Успешно вернули данные метрики по id : metrics - {result}, id - {id}");
            }

            return Ok(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            _hddMetricsRepository.Delete(id);
            if (_logger != null)
            {
                _logger.LogDebug($"hdd метрика успешно удалена : {id}");
            }
            return Ok();
        }

        [HttpPut("Update")]
        public IActionResult Update(HddMetric hddMetric)
        {
            _hddMetricsRepository.Update(hddMetric);

            if (_logger != null)
            {
                _logger.LogDebug($"hdd метрика успешно обновлена : {hddMetric}");
            }

            return Ok();
        }
    }
}