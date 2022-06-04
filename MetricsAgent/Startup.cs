using MetricsAgent.Converters;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System;
using AutoMapper;
using FluentMigrator.Runner;
using MetricsAgent.Controllers.Interfaces;
using MetricsAgent.Jobs;
using MetricsAgent.Services.Interfaces;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace MetricsAgent
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
                    .ScanIn(typeof(Startup).Assembly).For.Migrations())
                    .AddLogging(lb => lb
                    .AddFluentMigratorConsole());

            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<IJobFactory, SingletonJobFactory>();

            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton(new JobSchedule(
                typeof(CpuMetricJob),
                "0/5 * * ? * * *"));

            services.AddSingleton<RamMetricJob>();
            services.AddSingleton(new JobSchedule(
                typeof(RamMetricJob),
                "0/5 * * ? * * *"));

            services.AddSingleton<HddMetricJob>();
            services.AddSingleton(new JobSchedule(
                typeof(HddMetricJob),
                "0/5 * * ? * * *"));

            services.AddHostedService<QuartzHostedService>();

            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(
                new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter()));

            services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>()
                .Configure<DatabaseOptions>(options =>
            {
                Configuration.GetSection("Settings:DatabaseOptions").Bind(options);
            });

            services.AddScoped<IDotNetMetricsRepository, DotNetMetricsRepository>()
                .Configure<DatabaseOptions>(options =>
                {
                    Configuration.GetSection("Settings:DatabaseOptions").Bind(options);
                });

            services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>().Configure<DatabaseOptions>(options =>
            {
                Configuration.GetSection("Settings:DatabaseOptions").Bind(options);
            });

            services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>()
                .Configure<DatabaseOptions>(options =>
                {
                    Configuration.GetSection("Settings:DatabaseOptions").Bind(options);
                });

            services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>().Configure<DatabaseOptions>(options =>
            {
                Configuration.GetSection("Settings:DatabaseOptions").Bind(options);
            });



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "MetricsAgent", Version = "v1"});

                // Поддержка TimeSpan
                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("00:00:00")
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetricsAgent v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            migrationRunner.MigrateUp();
        }
    }
}
