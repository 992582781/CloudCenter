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
            services.TryAddSingleton<ICookie, Cookie>();//IOC���� ������Ŀ��ʹ��

            #region ע��cookie
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
                .AddInMemoryApiScopes(Config.ApiScopes)//4.x�¼�
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
            IContext.Accessor = accessor;//ȫ��ʹ��
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            //�����֤
            app.UseAuthentication();
            //��׼��Ȩ
            app.UseAuthorization();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        /// <summary>
        /// ����˷���������Զ�����ע������ϵͳ����   
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //����κ�Autofacģ���ע�ᡣ
            //������ConfigureServices֮����õģ�����
            //�ڴ˴�ע�Ὣ������ConfigureServices��ע������ݡ�
            //�ڹ�������ʱ������á�UseServiceProviderFactory��new AutofacServiceProviderFactory��������`���򽫲�����ôˡ�

            //builder.RegisterModule(new AutofacModuleRegister(Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath,
            //    new List<string>()
            //       { //�������캯��ע��
            //                   "RongKang.Repository.dll","RongKang.UnitOfWork.dll"
            //       }));


            builder.RegisterDynamicProxy();
        }
    }
}
