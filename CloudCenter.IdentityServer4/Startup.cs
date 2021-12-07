using Autofac;
using Autofac.Extensions.DependencyInjection;
using CloudCenter.IdentityServer4.Configuration;
using CloudCenter.IdentityServer4.Extendsions;
using CloudCenter.IdentityServer4.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AspectCore;
using AspectCore.Extensions.Autofac;
using Microsoft.AspNetCore.HttpsPolicy;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CloudCenter.UnitOfWork;
using CloudCenter.Infrastructure;

namespace CloudCenter.IdentityServer4
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
            services.TryAddSingleton<UnitOfWork.IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<ICookie, Cookie>();//IOC配置 方便项目中使用

            #region 注入cookie
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #endregion
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            IContext.Accessor = accessor;//全局使用
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
            //添加任何Autofac模块或注册。
            //这是在ConfigureServices之后调用的，所以
            //在此处注册将覆盖在ConfigureServices中注册的内容。
            //在构建主机时必须调用“UseServiceProviderFactory（new AutofacServiceProviderFactory（））”`否则将不会调用此。

            //builder.RegisterModule(new AutofacModuleRegister(Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath,
            //    new List<string>()
            //       { //批量构造函数注入
            //                   "RongKang.Repository.dll","RongKang.UnitOfWork.dll"
            //       }));


            builder.RegisterDynamicProxy();
        }
    }
}
