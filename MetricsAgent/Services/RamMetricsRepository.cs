using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MetricsAgent.Models;
using MetricsAgent.Services.Interfaces;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace MetricsAgent.Services
{
    public class RamMetricsRepository : IRamMetricsRepository
    {
        private readonly IOptions<DatabaseOptions> _databaseOptions;

        public RamMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public void Create(RamMetric item)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute($"insert into ram_metrics(value, time)  values({item.Value}, {item.Time})");
        }

        public void Delete(int id)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute($"delete from ram_metrics where id = {id}");
        }

        public IList<RamMetric> GetAll()
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            return connection.Query<RamMetric>("select * from ram_metrics").ToList();
        }

        public IList<RamMetric> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            return connection.Query<RamMetric>($"select * from ram_metrics" +
                                               $" where time >= {fromTime.TotalSeconds} and time <= {toTime.TotalSeconds}")
                .ToList();
        }

        public RamMetric GetById(int id)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            return connection.QuerySingle<RamMetric>($"select * from ram_metrics where id = {id}");
        }

        public void Update(RamMetric item)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute($"update ram_metrics set value = {item.Value}, time = {item.Time} where id = {item.Id}");
        }
    }
}
