using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Threading.Tasks;
using CloudCenter.APi.Middlewares;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CloudCenter.APi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddConsul(Configuration);
            services.AddCustomSwagger(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddMiniProfiler(options =>
                      options.RouteBasePath = "/profiler");
            services.AddCustomAuthentication(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "CloudCenter.APi V1");
                c.OAuthClientId("CloudCenter.APi");
                c.OAuthAppName("CloudCenter.APi Swagger UI");
                c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("CloudCenter.APi.index.html");
                //c.IndexStream = () => GetType().GetTypeInfo()
                //.Assembly.GetManifestResourceStream("CloudCenter.APi.index.html");
                //////设置首页为Swagger
                //c.RoutePrefix = string.Empty;
                ////自定义页面 集成性能分析
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "SN博客API");
                //////设置为none可折叠所有方法
                //c.DocExpansion(DocExpansion.None);
                //////设置为-1 可不显示models
                //c.DefaultModelsExpandDepth(-1);
            });
            app.UseRouting();
            app.UseCors("CorsPolicy");
            ConfigureAuth(app);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                //endpoints.MapDefaultControllerRoute().RequireAuthorization("ApiScope"); // adds scope
            });
            //Consul注册
            app.UseMiniProfiler();
            app.UseConsul();
        }

        protected virtual void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
