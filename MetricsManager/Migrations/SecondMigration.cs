using FluentMigrator;

namespace MetricsManager.Migrations
{
    [Migration(2)]
    public class SecondMigration : Migration
    {
        public override void Up()
        {
            Create.Table("agent_info").WithColumn("agent_id").AsInt64().PrimaryKey().Identity()
                .WithColumn("agent_address").AsString()
                .WithColumn("enabled").AsBoolean();
        }

        public override void Down()
        {
            Delete.Table("agent_info");
        }
    }
}
