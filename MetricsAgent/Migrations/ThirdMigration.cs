using FluentMigrator;

namespace MetricsAgent.Migrations
{
    [Migration(4)]
    public class ThirdMigration : Migration
    {
        public override void Up()
        {
            Create.Table("dotnet_metrics_error").WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("value").AsInt64()
                .WithColumn("time").AsInt64();
        }

        public override void Down()
        {
            Delete.Table("dotnet_metrics_error");
        }
    }
}
