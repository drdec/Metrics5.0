using System;
using System.Collections.Generic;
using System.Linq;
using MetricsAgent.Controllers.Interfaces;
using MetricsAgent.Models;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.Extensions.Options;

namespace MetricsAgent.Services
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private readonly IOptions<DatabaseOptions> _databaseOptions;

        public CpuMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public void Create(CpuMetric item)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            connection.Execute($"insert into cpu_metrics(value, time)  values({item.Value}, {item.Time})");
        }

        public void Delete(int id)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            connection.Execute($"delete from cpu_metrics where id = {id}");
        }

        public void Update(CpuMetric item)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            connection.Execute(
                $"update cpu_metrics set value = {item.Value}, time = {item.Time} " +
                $"where id = {item.Id}");
        }

        public IList<CpuMetric> GetAll()
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            List<CpuMetric> result = connection.Query<CpuMetric>("select * from cpu_metrics").ToList();
            return result;
        }

        public IList<CpuMetric> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            return connection
                .Query<CpuMetric>($"select * from cpu_metrics where time >= {fromTime.TotalSeconds} " +
                                  $"and time <= {toTime.TotalSeconds}").ToList();
        }

        public CpuMetric GetById(int id)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            return connection.QuerySingle<CpuMetric>($"select * from cpu_metrics where id = {id}");
        }
    }
}