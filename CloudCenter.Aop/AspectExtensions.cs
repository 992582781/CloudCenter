using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CloudCenter.Aop
{
	public static class AspectExtensions
	{
		public static void ConfigAspect(this IServiceCollection services)
		{
			services.ConfigureDynamicProxy(config =>
			{
				//TestInterceptor拦截器类
				//拦截代理所有Service结尾的类
				//config.Interceptors.AddTyped<TransactionalAttribute>();
				//config.Interceptors.AddTyped<TransactionalAttribute>(Predicates.ForService("UserRepository"));
				//config.Interceptors.AddTyped<TransactionalAttribute>(Predicates.ForService("*Controller"));
			});
			services.BuildDynamicProxyProvider();
		}
	}
}
