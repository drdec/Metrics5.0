using System;
using MetricsAgent.Models;
using MetricsAgent.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace MetricsAgent.Services
{
    public class HddMetricsRepository : IHddMetricsRepository
    {
        private readonly IOptions<DatabaseOptions> _databaseOptions;

        public HddMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public void Create(HddMetric item)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            connection.Execute($"insert into hdd_metrics(value, time)  values({item.Value}, {item.Time})");
        }

        public void Delete(int id)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute($"delete from hdd_metrics where id = {id}");
        }

        public IList<HddMetric> GetAll()
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            return connection.Query<HddMetric>("select * from hdd_metrics").ToList();
        }

        public IList<HddMetric> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            return connection.Query<HddMetric>($"select * from hdd_metrics where " +
                                               $"time >= {fromTime.TotalSeconds} and time <= {toTime.TotalSeconds}")
                .ToList();
        }

        public HddMetric GetById(int id)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            return connection.QuerySingle<HddMetric>($"select * from hdd_metrics where id = {id}");
        }

        public void Update(HddMetric item)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute($"update hdd_metrics set value = {item.Value}, time = {item.Time} where id = {item.Id}");
        }
    }
}
