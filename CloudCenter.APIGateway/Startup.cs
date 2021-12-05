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
            // ��Ϊ��Ocelot.json�ļ��������˷���ServiceName=CloudCenter.APiӦ���еĽӿڱ���Ҫ������Ȩ��
            // ���Կͻ���Ҫ�����CloudCenter.APiӦ���еĽӿڣ��ͱ������AccessToken��
            // Ȼ��Ocelot�������AccessTokenȥIds4Center��֤�Ϸ��ԣ�
            // �����֤ͨ���Ͱ�����ת����CloudCenter.APiӦ�ô���
            #region Identity4
            var authenticationProviderKey = "APIGateway";//AuthenticationProviderKey": "APIGateway"
            services.AddAuthentication("Bearer")
                   .AddIdentityServerAuthentication(authenticationProviderKey, O =>
                   {
                       O.Authority = "http://localhost:5001";
                       O.ApiName = "CloudCenter.APi";//��Դ���ƣ�����֤������ע�����Դ�б������е�apiResourceһ��
                       O.SupportedTokens = SupportedTokens.Both;
                       O.ApiSecret = "CloudCenter";
                       O.RequireHttpsMetadata = false;//�Ƿ�����https
                   });
            #endregion
            services.AddOcelot()
                .AddConsul();
            //services.AddOcelot().AddConsul()
            //    .AddCacheManager(m =>
            //    {
            //        m.WithDictionaryHandle();//Ĭ���ֵ�洢
            //    })
            //    .AddPolly();
            ////services.AddControllers();

            ////�����IOcelotCache<CachedResponse>��Ĭ�ϻ����Լ��--׼���滻���Զ����
            //services.AddSingleton<IOcelotCache<CachedResponse>, CustomeCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //��Ĭ�ϵ�����ܵ�ȫ������
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
