using System;
using Dapper;
using MetricsManager.Models;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MetricsManager.Services.Impl
{
    public class MetricAgentRepository : IMetricAgentRepository
    {
        private readonly IOptions<DatabaseOptions> _databaseOptions;

        public MetricAgentRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public void Create(AgentInfo agentInfo)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            connection.Execute(
                "insert into agent_info(agent_id, agent_address, enabled) values(@agent_id, @agent_address, @enabled)",
                new
                {
                    agent_id = agentInfo.AgentId,
                    agent_address = agentInfo.AgentAddress.ToString(),
                    enabled = agentInfo.Enable
                });
        }

        public void Enable(int id)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            connection.Execute($"update agent_info set enabled = {true} where agent_id = {id}");
        }

        public void Disable(int id)
        {
            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            connection.Execute($"update agent_info set enabled = {false} where agent_id = {id}");
        }

        public IList<AgentInfo> Get()
        {
            //using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            //List<AgentInfo> result =  connection.Query<AgentInfo>
            //    ("select * from agent_info").ToList();

            //return result;

            using var connection = new MySqlConnection(_databaseOptions.Value.ConnectionString);

            connection.Open();

            string cmdText = "select * from agent_info";
            using var cmd = new MySqlCommand(cmdText, connection);

            var resultList = new List<AgentInfo>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    resultList.Add(new AgentInfo()
                    {
                        AgentId = reader.GetInt32(0),
                        AgentAddress = new Uri(reader.GetString(1)),
                        Enable = reader.GetBoolean(2)
                    });
                }
            }

            return resultList;
        }
    }
}
