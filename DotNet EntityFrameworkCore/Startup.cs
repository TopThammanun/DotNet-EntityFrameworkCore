using DotNet_EntityFrameworkCore.Core;
using DotNet_EntityFrameworkCore.Domain;
using DotNet_EntityFrameworkCore.WebAPICore;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AutoMapper;
using DotNet_EntityFrameworkCore.DataCore;
using DotNet_EntityFrameworkCore.Service;

namespace DotNet_EntityFrameworkCore
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
            services.AddHttpContextAccessor();

            services.AddCookiePolicy(option =>
            {
                option.MinimumSameSitePolicy = SameSiteMode.None;
                option.Secure = CookieSecurePolicy.Always;
            });

            services
                .AddControllers(option =>
                {
                    option.Filters.Add(typeof(HttpResponseExceptionFilter));
                    option.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider(System.Globalization.DateTimeStyles.AssumeLocal));
                })
                .AddJsonOptions(option =>
                {
                    option.JsonSerializerOptions.Converters.Add(new WebAPICore.TimeSpanConverter());
                    option.JsonSerializerOptions.Converters.Add(new WebAPICore.DateTimeConverter(DateTimeStyles.AssumeLocal));
                });

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddHttpContextAccessor();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            services.AddSingleton<ServiceInfo>(new ServiceInfo("01", "PCU Microservice", null, environment));

            services.AddDbContext<ITDBContext, TDBContext>();
            services.AddSingleton<IDbModelConfigure, DBConfigure>();
            services.AddScoped<TUnitOfWork, TUnitOfWork>();

            services.AddScoped<IPlanService, PlanService>();
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

            appLifetime.ApplicationStarted.Register(() =>
            {
                Console.WriteLine($"Running as environment {env.EnvironmentName}.\n");
                var addressFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
                foreach (var address in addressFeature.Addresses)
                {
                    Console.WriteLine("Now listening on: " + address);
                }
            });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name");
                });
            }

            SessionData.Init(() =>
            {
                var contextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
                return contextAccessor.HttpContext.Items;
            });
            app.UseCors("CORSAllowLocalHost3000");
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("healthcheck");
            });
        }
    }
}
