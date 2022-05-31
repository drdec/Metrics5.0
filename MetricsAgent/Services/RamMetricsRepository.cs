using System;
using System.Collections.Generic;
using MetricsAgent.Models;
using MetricsAgent.Services.Interfaces;
using MySql.Data.MySqlClient;

namespace MetricsAgent.Services
{
    public class RamMetricsRepository : IRamMetricsRepository
    {
        private const string ConnectionString = "server=localhost; user=root; database = metrics; password = 123456;";

        public void Create(RamMetric item)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText =
                $"insert into ram_metrics(value, time)  values({item.Value}, {item.Time.TotalSeconds})";

            var cmd = new MySqlCommand(cmdText, connection);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText = $"delete from ram_metrics where id = {id}";

            using var cmd = new MySqlCommand(cmdText, connection);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public IList<RamMetric> GetAll()
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText = "select * from ram_metrics";

            using var cmd = new MySqlCommand(cmdText, connection);

            var returnList = new List<RamMetric>();

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new RamMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    });
                }
            }
            return returnList;
        }

        public IList<RamMetric> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText = $"select * from ram_metrics where time >= {fromTime.TotalSeconds} and time <= {toTime.TotalSeconds}";

            using var cmd = new MySqlCommand(cmdText, connection);

            var result = new List<RamMetric>();

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new RamMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    });
                }
            }

            return result;
        }

        public RamMetric GetById(int id)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText = $"select * from ram_metrics where id = {id}";

            using var cmd = new MySqlCommand(cmdText, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new RamMetric
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

        public void Update(RamMetric item)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText =
                $"update ram_metrics set value = {item.Value}, time = {item.Time.TotalSeconds} where id = {item.Id}";

            using var cmd = new MySqlCommand(cmdText, connection);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
