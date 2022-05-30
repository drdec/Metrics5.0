using System;
using System.Collections.Generic;
using MetricsAgent.Controllers.Interfaces;
using MetricsAgent.Models;
using MySql.Data.MySqlClient;

namespace MetricsAgent.Services
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {

        private const string ConnectionString = "server=localhost; user=root; database = metrics; password = 123456;";


        public void Create(CpuMetric item)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText =
                $"insert into cpu_metrics(value, time)  values({item.Value}, {item.Time.TotalSeconds})";

            using var cmd = new MySqlCommand(cmdText, connection);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText = $"delete from cpu_metrics where id = {id}";

            using var cmd = new MySqlCommand(cmdText, connection);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(CpuMetric item)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText =
                $"update cpu_metrics set value = {item.Value}, time = {item.Time.TotalSeconds} where id = {item.Id}";

            using var cmd = new MySqlCommand(cmdText, connection);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public IList<CpuMetric> GetAll()
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText = "select * from cpu_metrics";

            using var cmd = new MySqlCommand(cmdText, connection);
            
            var returnList = new List<CpuMetric>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new CpuMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    });
                }
            }
            return returnList;
        }

        public IList<CpuMetric> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText = $"select * from cpu_metrics where time >= {fromTime.TotalSeconds} and time <= {toTime.TotalSeconds}";

            using var cmd = new MySqlCommand(cmdText, connection);

            var result = new List<CpuMetric>();

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new CpuMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    });
                }
            }

            return result;
        }

        public CpuMetric GetById(int id)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText = $"select * from cpu_metrics where id = {id}";

            using var cmd = new MySqlCommand(cmdText, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new CpuMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    };
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
