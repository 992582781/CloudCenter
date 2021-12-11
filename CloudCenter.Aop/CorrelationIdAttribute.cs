using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorrelationId.Abstractions;
using Microsoft.Extensions.Logging;
using CloudCenter.Log;

namespace CloudCenter.Aop
{
    public class CorrelationIdsAttribute : AbstractInterceptorAttribute
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                NLogHelper.Default.Info($"链路ID_[{IContext.GetContext().TraceIdentifier}]_方法：{(context.ProxyMethod.Name)}_Begin");
                await next(context);
                NLogHelper.Default.Info($"链路ID_[{IContext.GetContext().TraceIdentifier}]_方法：{(context.ProxyMethod.Name)}_End");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
