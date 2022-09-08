using MetricsManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System;
using FluentMigrator.Runner;
using MetricsManager.Converter;
using MetricsManager.Migrations;
using MetricsManager.Services;
using MetricsManager.Services.Impl;
using Polly;

namespace MetricsManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddMySql5()
                    .WithGlobalConnectionString(
                        Configuration.GetSection("Settings:DatabaseOptions:ConnectionString").Value)
                    .ScanIn(typeof(FirstMigration).Assembly).For.Migrations())
                .AddLogging(lb => lb
                    .AddFluentMigratorConsole());

            services.AddHttpClient();

            services.AddHttpClient<IMetricsAgentClient, MetricsAgentClient>()
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
                    sleepDurationProvider: (attemptCount) => TimeSpan.FromMilliseconds(2000),
                    onRetry: (exception, sleepDuration, attemptNumber, context) =>
                    {

                    }));

            services.AddSingleton<IMetricAgentRepository, MetricAgentRepository>()
                .Configure<DatabaseOptions>(options =>
                {
                    Configuration.GetSection("Settings:DatabaseOptions").Bind(options);
                });

            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsManager", Version = "v1" });

                // Поддержка TimeSpan
                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("00:00:00")
                });
            });
        }
        
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IMigrationRunner migrationRunner)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetricsManager v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            migrationRunner.MigrateUp();
        }
    }
}
