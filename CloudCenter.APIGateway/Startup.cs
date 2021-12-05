using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.Cache;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace CloudCenter.APIGateway
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // 因为在Ocelot.json文件中配置了访问ServiceName=CloudCenter.APi应用中的接口必须要经过授权，
            // 所以客户端要想访问CloudCenter.APi应用中的接口，就必须带上AccessToken，
            // 然后Ocelot拿着这个AccessToken去Ids4Center验证合法性，
            // 如果验证通过就把请求转发到CloudCenter.APi应用处理
            #region Identity4
            var authenticationProviderKey = "APIGateway";//AuthenticationProviderKey": "APIGateway"
            services.AddAuthentication("Bearer")
                   .AddIdentityServerAuthentication(authenticationProviderKey, O =>
                   {
                       O.Authority = "http://localhost:5001";
                       O.ApiName = "CloudCenter.APi";//资源名称，跟认证服务中注册的资源列表名称中的apiResource一致
                       O.SupportedTokens = SupportedTokens.Both;
                       O.ApiSecret = "CloudCenter";
                       O.RequireHttpsMetadata = false;//是否启用https
                   });
            #endregion
            services.AddOcelot()
                .AddConsul();
            //services.AddOcelot().AddConsul()
            //    .AddCacheManager(m =>
            //    {
            //        m.WithDictionaryHandle();//默认字典存储
            //    })
            //    .AddPolly();
            ////services.AddControllers();

            ////这里的IOcelotCache<CachedResponse>是默认缓存的约束--准备替换成自定义的
            //services.AddSingleton<IOcelotCache<CachedResponse>, CustomeCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //将默认的请求管道全部丢掉
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseHsts();
            //}

            //app.UseHttpsRedirection();
            //app.UseMvc();
            app.UseOcelot();
        }
    }
}
