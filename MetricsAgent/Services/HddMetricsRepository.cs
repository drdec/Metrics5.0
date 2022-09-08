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
    }
}
