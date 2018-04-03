using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VoiceOnlineStats.Extensions;
using VoiceOnlineStats.Services;

namespace VoiceOnlineStats
{
    public class Startup
    {
        public static IHostingEnvironment CurrentEnvironment { get; set; }
        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            CurrentEnvironment = env;
            Configuration = configuration;
        }        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
            });
            services.AddMvc(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));
            });
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "_af";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.HeaderName = "X-XSRF-TOKEN";
            });
            services.AddLiteDBService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (CurrentEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            });
            app.UseCors("AllowSpecificOrigin");
            app.UseMvc();
        }
    }
}
