﻿using MetricsAgent.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using AutoMapper;
using MetricsAgent.Controllers.Interfaces;
using MetricsAgent.Models;
using MetricsAgent.Models.ModelsDto;
using Quartz;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ICpuMetricsRepository _cpuMetricsRepository;
        private readonly ILogger<CpuMetricsController> _logger;
        private readonly IMapper _mapper;

        public CpuMetricsController(
            IMapper mapper,
            ILogger<CpuMetricsController> logger,
            ICpuMetricsRepository cpuMetricsRepository)
        {
            _cpuMetricsRepository = cpuMetricsRepository;
            _logger = logger;
            _mapper = mapper;
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
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }

            _logger?.LogDebug("Успешно вернули данные cpu метрики");

            return response.IsEmpty() ? Ok("empty") : Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger?.LogDebug("Успешно вернули cpu метрику за период времени");
            //return Ok(_cpuMetricsRepository.GetByPeriod(fromTime, toTime));

            IList<CpuMetric> metrics = _cpuMetricsRepository.GetByPeriod(fromTime, toTime);

            AllCpuMetricsResponse response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                CpuMetricDto metricDto = _mapper.Map<CpuMetricDto>(metric);

                response.Metrics.Add(metricDto);
            }

            return Ok(response);
        }

        //[HttpGet("get-by-id")]
        //public IActionResult GetById(int id)
        //{
        //    var result = _cpuMetricsRepository.GetById(id);

        //    if (_logger != null)
        //    {
        //        _logger.LogDebug($"Успешно вернули данне метрики по id : metrics - {result}, id - {id}");
        //    }

        //    return result == null ? Ok("sorry, data not found") : Ok(result);
        //}

        //[HttpDelete("delete")]
        //public IActionResult Delete(int id)
        //{
        //    _cpuMetricsRepository.Delete(id);
        //    if (_logger != null)
        //    {
        //        _logger.LogDebug($"cpu метрика успешно удалена : {id}");
        //    }
        //    return Ok();
        //}

        //[HttpPut("Update")]
        //public IActionResult Update(CpuMetric cpuMetric)
        //{
        //    _cpuMetricsRepository.Update(cpuMetric);

        //    if (_logger != null)
        //    {
        //        _logger.LogDebug($"cpu метрика успешно обновлена : {cpuMetric}");
        //    }

        //    return Ok();
        //}
        //[HttpPost("create")]
        //public IActionResult Create([FromBody] CpuMetricCreateRequest request)
        //{
        //    CpuMetric cpuMetric = new ()
        //    {
        //        Time = request.Time.TotalSeconds,
        //        Value = request.Value
        //    };

        //    _cpuMetricsRepository.Create(cpuMetric);

        //    if (_logger != null)
        //        _logger.LogDebug("Успешно добавили новую cpu метрику: {0}", cpuMetric);
            

        //    return Ok();
        //}
    }
}
