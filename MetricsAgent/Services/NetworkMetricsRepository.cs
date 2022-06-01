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
    public class NetworkMetricsRepository : INetworkMetricsRepository
    {
        private readonly IOptions<DatabaseOptions> _databaseOptions;

        public NetworkMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public void Create(NetworkMetric item)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute($"insert into network_metrics(value, time)  values({item.Value}, {item.Time})");
        }

        public void Delete(int id)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute($"delete from network_metrics where id = {id}");
        }

        public IList<NetworkMetric> GetAll()
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            return connection.Query<NetworkMetric>("select * from network_metrics").ToList();
        }

        public IList<NetworkMetric> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            return connection.Query<NetworkMetric>($"select * from network_metrics " +
                                            $"where time >= {fromTime.TotalSeconds} and time <= {toTime.TotalSeconds}")
                .ToList();
        }

        public NetworkMetric GetById(int id)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            return connection.QuerySingle<NetworkMetric>($"select * from network_metrics where id = {id}");
        }

        public void Update(NetworkMetric item)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute(
                $"update network_metrics set value = {item.Value}, time = {item.Time} where id = {item.Id}");
        }
    }
}
