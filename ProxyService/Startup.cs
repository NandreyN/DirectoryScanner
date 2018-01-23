using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
           
            //connectionString = Configuration.GetConnectionString("Tasks") ?? throw new ArgumentNullException("Invalid connection string block");
            //services.AddDbContext<TaskItemContext>(x => x.UseSqlite(Configuration.GetConnectionString("Default")));
            services.AddMvc();
            services.AddDbContext<PoolItemContext>(x => x.UseSqlite(Configuration.GetConnectionString("Default")));
            services.AddDbContext<TaskItemContext>(x => x.UseSqlite(Configuration.GetConnectionString("Default")));
            services.AddDbContext<QueryItemContext>(x => x.UseSqlite(Configuration.GetConnectionString("Default")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
