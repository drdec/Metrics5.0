using System;
using System.Collections.Generic;
using MetricsAgent.Models;
using MetricsAgent.Models.ModelsDto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IMapper = AutoMapper.IMapper;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/[controller]")]
    [ApiController]
    public class HddMetricsController : Controller
    {
        private readonly IHddMetricsRepository _hddMetricsRepository;
        private readonly ILogger<HddMetricsController> _logger;
        private readonly IMapper _mapper;

        public HddMetricsController(
            ILogger<HddMetricsController> logger,
            IHddMetricsRepository hddMetricsRepository,
            IMapper mapper)
        {
            _hddMetricsRepository = hddMetricsRepository;
            _logger = logger;
            _mapper = mapper;
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
                response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
            }

            if (_logger != null)
                _logger.LogDebug("Успешно вернули данные hdd метрики");

            return response.IsEmpty() ? Ok("empty") : Ok(response);
        }

        [HttpGet("get-by-period/from/{fromTime}/to/{toTime}")]
        public IActionResult GetByPeriod([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok(_hddMetricsRepository.GetByPeriod(fromTime, toTime));
        }

        //[HttpPost("create")]
        //public IActionResult Create([FromBody] HddMetricsCreateRequest request)
        //{
        //    var hddMetric = new HddMetric()
        //    {
        //        Time = request.Time.TotalSeconds,
        //        Value = request.Value
        //    };

        //    _hddMetricsRepository.Create(hddMetric);

        //    if (_logger != null)
        //        _logger.LogDebug("Успешно добавили новую hdd метрику: {0}", hddMetric);

        //    return Ok();
        //}
        //[HttpGet("get-by-id")]
        //public IActionResult GetById(int id)
        //{
        //    var result = _hddMetricsRepository.GetById(id);

        //    if (_logger != null)
        //    {
        //        _logger.LogDebug($"Успешно вернули данные метрики по id : metrics - {result}, id - {id}");
        //    }

        //    return Ok(result);
        //}

        //[HttpDelete("delete")]
        //public IActionResult Delete(int id)
        //{
        //    _hddMetricsRepository.Delete(id);
        //    if (_logger != null)
        //    {
        //        _logger.LogDebug($"hdd метрика успешно удалена : {id}");
        //    }
        //    return Ok();
        //}

        //[HttpPut("Update")]
        //public IActionResult Update(HddMetric hddMetric)
        //{
        //    _hddMetricsRepository.Update(hddMetric);

        //    if (_logger != null)
        //    {
        //        _logger.LogDebug($"hdd метрика успешно обновлена : {hddMetric}");
        //    }

        //    return Ok();
        //}
    }
}