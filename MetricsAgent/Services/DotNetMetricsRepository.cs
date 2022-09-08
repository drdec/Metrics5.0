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

    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private readonly IOptions<DatabaseOptions> _databaseOptions;

        public DotNetMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public void Create(DotNetMetric item)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute(
                $"insert into dotnet_metrics(value, time)  values({item.Value}, {item.Time})");
        }

        public void CreateWithErrors(DotNetMetric item)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute(
                $"insert into dotnet_metrics_error(value, time)  values({item.Value}, {item.Time})");
        }


        public IList<DotNetMetric> GetAll()
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            return connection.Query<DotNetMetric>("select * from dotnet_metrics").ToList();
        }

        public IList<DotNetMetric> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            return connection.Query<DotNetMetric>($"select * from dotnet_metrics " +
                                                  $"where time >= {fromTime.TotalSeconds} " +
                                                  $"and time <= {toTime.TotalSeconds}").ToList();
        }
        
        public int GetErrorsCount(TimeSpan fromTime, TimeSpan toTime)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            return connection.Query<int>(
                $"select value from dotnet_metrics_error " +
                $"where time >= {fromTime.TotalSeconds} " +
                $"and time <= {toTime.TotalSeconds}").Sum();
        }
    }
}
