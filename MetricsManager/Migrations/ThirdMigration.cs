using FluentMigrator;

namespace MetricsManager.Migrations
{
    [Migration(3)]
    public class ThirdMigration : Migration
    {
        public override void Up()
        {
            Delete.Table("agent_info");

            Create.Table("agent_info").WithColumn("agent_id").AsInt64().PrimaryKey()
                .WithColumn("agent_address").AsString()
                .WithColumn("enabled").AsBoolean();
        }

        public override void Down()
        {
            Delete.Table("agent_info");
        }
    }
}
