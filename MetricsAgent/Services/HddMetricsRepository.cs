using System;
using MetricsAgent.Models;
using MetricsAgent.Services.Interfaces;
using System.Collections.Generic;

namespace MetricsAgent.Services
{
    //public class HddMetricsRepository : IHddMetricsRepository
    //{
    //    private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";

    //    public void Create(HddMetric item)
    //    {
    //        using var connection = new SQLiteConnection(ConnectionString);
    //        connection.Open();

    //        using var cmd = new SQLiteCommand(connection);
    //        cmd.CommandText = "INSERT INTO hddmetrics(value, time) VALUES(@value, @time)";
    //        cmd.Parameters.AddWithValue("@value", item.Value);
    //        cmd.Parameters.AddWithValue("@time", item.Time);

    //        cmd.Prepare();
    //        cmd.ExecuteNonQuery();
    //    }

    //    public void Delete(int id)
    //    {
    //        using var connection = new SQLiteConnection(ConnectionString);
    //        connection.Open();

    //        using var cmd = new SQLiteCommand(connection);
    //        cmd.CommandText = "DELETE FROM hddmetrics WHERE id=@id";
    //        cmd.Parameters.AddWithValue("@id", id);
    //        cmd.Prepare();
    //        cmd.ExecuteNonQuery();
    //    }

    //    public IList<HddMetric> GetAll()
    //    {
    //        using var connection = new SQLiteConnection(ConnectionString);
    //        connection.Open();

    //        using var cmd = new SQLiteCommand(connection);
    //        cmd.CommandText = "SELECT * FROM hddmetrics";
    //        var returnList = new List<HddMetric>();

    //        using (SQLiteDataReader reader = cmd.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                returnList.Add(new HddMetric
    //                {
    //                    Id = reader.GetInt32(0),
    //                    Value = reader.GetInt32(1),
    //                    Time = TimeSpan.FromSeconds(reader.GetInt32(2))
    //                });
    //            }
    //        }
    //        return returnList;
    //    }

    //    public IList<HddMetric> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
    //    {
    //        using var connection = new SQLiteConnection(ConnectionString);
    //        connection.Open();
    //        using var cmd = new SQLiteCommand(connection);

    //        cmd.CommandText = "SELECT * FROM hddmetrics WHERE time >=@fromTime and time<=@toTime";
    //        cmd.Parameters.AddWithValue("fromTime", fromTime.TotalSeconds);
    //        cmd.Parameters.AddWithValue("toTime", toTime.TotalSeconds);

    //        var result = new List<HddMetric>();

    //        using (SQLiteDataReader reader = cmd.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                result.Add(new HddMetric
    //                {
    //                    Id = reader.GetInt32(0),
    //                    Value = reader.GetInt32(1),
    //                    Time = TimeSpan.FromSeconds(reader.GetInt32(2))
    //                });
    //            }
    //        }

    //        return result;
    //    }

    //    public HddMetric GetById(int id)
    //    {
    //        using var connection = new SQLiteConnection(ConnectionString);
    //        connection.Open();

    //        using var cmd = new SQLiteCommand(connection);
    //        cmd.CommandText = $"SELECT * FROM hddmetrics WHERE id={id}";

    //        using (SQLiteDataReader reader = cmd.ExecuteReader())
    //        {
    //            if (reader.Read())
    //            {
    //                return new HddMetric
    //                {
    //                    Id = reader.GetInt32(0),
    //                    Value = reader.GetInt32(1),
    //                    Time = TimeSpan.FromSeconds(reader.GetInt32(2))
    //                };
    //            }
    //            else
    //            {
    //                return null;
    //            }
    //        }
    //    }

    //    public void Update(HddMetric item)
    //    {
    //        using var connection = new SQLiteConnection(ConnectionString);
    //        connection.Open();

    //        using var cmd = new SQLiteCommand(connection);
    //        cmd.CommandText = "UPDATE hddmetrics SET value = @value, time = @time WHERE id = @id; ";
    //        cmd.Parameters.AddWithValue("@id", item.Id);
    //        cmd.Parameters.AddWithValue("@value", item.Value);
    //        cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);

    //        cmd.Prepare();
    //        cmd.ExecuteNonQuery();
    //    }
    //}
}
