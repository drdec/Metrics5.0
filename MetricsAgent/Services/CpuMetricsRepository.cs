using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using MetricsAgent.Controllers.Interfaces;
using MetricsAgent.Models;
using MySql.Data.MySqlClient;
using Dapper;

namespace MetricsAgent.Services
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {

        private const string ConnectionString = "server=localhost; user=root; database = metrics; password = 123456;";


        public void Create(CpuMetric item)
        {
            using var connection = new MySqlConnection(ConnectionString);

            connection.Execute($"insert into cpu_metrics(value, time)  values({item.Value}, {item.Time})");
        }

        public void Delete(int id)
        {
            using var connection = new MySqlConnection(ConnectionString);

            connection.Execute($"delete from cpu_metrics where id = {id}");
        }

        public void Update(CpuMetric item)
        {
            using var connection = new MySqlConnection(ConnectionString);

            connection.Execute(
                $"update cpu_metrics set value = {item.Value}, time = {item.Time} " +
                $"where id = {item.Id}");
        }

        public IList<CpuMetric> GetAll()
        {
            using var connection = new MySqlConnection(ConnectionString);

            List<CpuMetric> result = connection.Query<CpuMetric>("select * from cpumetrics").ToList();
            return result;
        }

        public IList<CpuMetric> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            var connection = new MySqlConnection(ConnectionString);

            return connection
                .Query<CpuMetric>($"select * from cpumetrics where time >= {fromTime.TotalSeconds} " +
                                  $"and time <= {toTime.TotalSeconds}").ToList();
        }

        public CpuMetric GetById(int id)
        {
            using var connection = new MySqlConnection(ConnectionString);

            return connection.QuerySingle<CpuMetric>($"select * from cpumetric where id = {id}");
        }
    }
}