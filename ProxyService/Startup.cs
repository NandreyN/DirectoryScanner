﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentScheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProxyService.Classes;

namespace ProxyService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            FolderRecordContext.ConnectionString = TaskItemContext.ConnectionString =
                PoolItemContext.ConnectionString = Configuration.GetConnectionString("Default");

            services.AddDbContext<PoolItemContext>(x => x.UseSqlite(Configuration.GetConnectionString("Default")));
            services.AddDbContext<TaskItemContext>(x => x.UseSqlite(Configuration.GetConnectionString("Default")));
            services.AddDbContext<FolderRecordContext>(x => x.UseSqlite(Configuration.GetConnectionString("Default")));

            JobManager.AddJob<BackgroundDistributor>(x => x.ToRunNow());
            services.AddSingleton<HttpClient>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddConsole();
            app.UseMvc();
        }
    }
}
