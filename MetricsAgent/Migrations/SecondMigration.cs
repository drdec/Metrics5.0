using FluentMigrator;

namespace MetricsAgent.Migrations
{
    [Migration(2)]
    public class SecondMigration : Migration
    {
        public override void Up()
        {
            Create.Table("dotnet_metrics").WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("value").AsInt64()
                .WithColumn("time").AsInt64();

            Create.Table("hdd_metrics").WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("value").AsInt64()
                .WithColumn("time").AsInt64();

            Create.Table("network_metrics").WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("value").AsInt64()
                .WithColumn("time").AsInt64();

            Create.Table("ram_metrics").WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("value").AsInt64()
                .WithColumn("time").AsInt64();

        }

        public override void Down()
        {
            Delete.Table("dotnet_metrics");
            Delete.Table("hdd_metrics");
            Delete.Table("network_metrics");
            Delete.Table("ram_metrics");
        }
    }
}
