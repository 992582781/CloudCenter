using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CloudCenter.IdentityServer4.Configuration;
using CloudCenter.IdentityServer4.Extendsions;
using CloudCenter.IdentityServer4.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CloudCenter.IdentityServer4
{
    public class Startup
    {
        public static IContainer AutofacContainer;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                // in-memory, code config
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)//4.x新加
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryClients(Config.Clients);
            // not recommended for production - you need to store your key material somewhere secure
            //builder.AddDeveloperSigningCredential();
            services.ConfigureNonBreakingSameSiteCookies();
            services.AddMediatR(typeof(Startup));

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            //身份认证
            app.UseAuthentication();
            //批准授权
            app.UseAuthorization();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        /// <summary>
        /// 定义此方法，框架自动调用注入所有系统服务   
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // 在这里添加服务注册
            //builder.RegisterType<TopicService>();
        }
    }
}
