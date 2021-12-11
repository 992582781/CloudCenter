using CloudCenter.Aop;
using CorrelationId.Abstractions;
using Microsoft.Extensions.Logging;

namespace CloudCenter.MVC.Controllers
{
    public  class CorrelationIdClass: ICorrelationIdClass
    {
        private readonly ILogger<CorrelationIdClass> _logger;
        private readonly ICorrelationContextAccessor _correlationContext;
        public CorrelationIdClass(ILogger<CorrelationIdClass> logger, ICorrelationContextAccessor correlationContext)
        {
            _logger = logger;
            _correlationContext = correlationContext;
        }
        [CorrelationIds]
        public  void  testc()
        {
            _logger.LogInformation($"[{_correlationContext.CorrelationContext.CorrelationId}]我是c");
        }
    }
}
