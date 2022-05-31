using System;
using MetricsAgent.Models;
using MetricsAgent.Services.Interfaces;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace MetricsAgent.Services
{

    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private const string ConnectionString = "server=localhost; user=root; database = metrics; password = 123456;";

        public void Create(DotNetMetric item)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText =
                $"insert into dotnet_metrics(value, time)  values({item.Value}, {item.Time.TotalSeconds})";

            var cmd = new MySqlCommand(cmdText, connection);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText = $"delete from dotnet_metrics where id = {id}";

            using var cmd = new MySqlCommand(cmdText, connection);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public IList<DotNetMetric> GetAll()
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText = "select * from dotnet_metrics";

            using var cmd = new MySqlCommand(cmdText, connection);

            var returnList = new List<DotNetMetric>();

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new DotNetMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    });
                }
            }
            return returnList;
        }

        public IList<DotNetMetric> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText = $"select * from dotnet_metrics where time >= {fromTime.TotalSeconds} and time <= {toTime.TotalSeconds}";

            using var cmd = new MySqlCommand(cmdText, connection);

            var result = new List<DotNetMetric>();

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new DotNetMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    });
                }
            }

            return result;
        }

        public DotNetMetric GetById(int id)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText = $"select * from dotnet_metrics where id = {id}";

            using var cmd = new MySqlCommand(cmdText, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new DotNetMetric
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

        public void Update(DotNetMetric item)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText =
                $"update dotnet_metrics set value = {item.Value}, time = {item.Time.TotalSeconds} where id = {item.Id}";

            using var cmd = new MySqlCommand(cmdText, connection);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public int GetErrorsCount(TimeSpan fromTime, TimeSpan toTime)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string cmdText = $"select value from dotnet_metrics where time >= {fromTime.TotalSeconds} and time <= {toTime.TotalSeconds}";

            using var cmd = new MySqlCommand(cmdText, connection);

            int result = 0;

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result += reader.GetInt32(0);
                }
            }

            return result;
        }
    }
}
