using CloudCenter.IdentityServer4.CommandAndQueries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CloudCenter.IdentityServer4.Handlers
{
    public class UserQueryHandler : IRequestHandler<UserQuery, bool>,
                                    IRequestHandler<UserIdQuery, string>
    {
        private readonly ILogger<UserQueryHandler> _logger;
        public UserQueryHandler(ILogger<UserQueryHandler> logger)
        {
            _logger = logger;
        }
        public Task<bool> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogError("mediator过程开始");
            return Task.FromResult(true);
        }

        public Task<string> Handle(UserIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogError("mediator过程开始");
            return Task.FromResult("根据ID查询user");
        }
    }
}
