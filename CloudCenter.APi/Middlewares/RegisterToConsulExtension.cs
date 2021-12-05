using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CloudCenter.APi.Middlewares
{
    public static class RegisterToConsulExtension
    {
        /// <summary>
        /// 添加Consul功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            // 配置Consul服务注册地址
            services.Configure<ServiceDiscoveryOptions>(configuration.GetSection("ServiceDiscovery"));
            // 配置Consul客户端
            services.AddSingleton<IConsulClient>(sp => new ConsulClient(config =>
            {
                var consulOptions = sp.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                if (!string.IsNullOrWhiteSpace(consulOptions.Consul.HttpEndPoint))
                {
                    config.Address = new Uri(consulOptions.Consul.HttpEndPoint);
                }
            }));

            return services;
        }

        /// <summary>
        /// 使用Consul
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            IConsulClient consul = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var appLife = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            IOptions<ServiceDiscoveryOptions> serviceOptions = app.ApplicationServices.GetRequiredService<IOptions<ServiceDiscoveryOptions>>();
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));

            // 向Consul客户端注册RestApi服务
            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                // 提供健康检查的HTTP接口
                var httpCheck = new AgentServiceCheck()
                {
                    Timeout = TimeSpan.FromSeconds(5),//检测等待时间
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20),//失败后多久移除
                    Interval = TimeSpan.FromSeconds(12),//间隔12s一次 检查
                    HTTP = new Uri(address, "HealthCheck").OriginalString
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = address.Port
                };

                var result = consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

                // 服务应用停止后发注册RestApi服务
                appLife.ApplicationStopping.Register(() =>
                {
                    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                });
            }

            return app;
        }
    }
}
