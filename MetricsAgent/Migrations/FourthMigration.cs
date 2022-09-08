using FluentMigrator;

namespace MetricsAgent.Migrations
{
    [Migration(5)]
    public class FourthMigration : Migration
    {
        public override void Up()
        {
            Create.Table("network_metrics_sent").WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("value").AsInt64()
                .WithColumn("time").AsInt64();
        }

        public override void Down()
        {
            Delete.Table("network_metrics_sent");
        }

    }
}
