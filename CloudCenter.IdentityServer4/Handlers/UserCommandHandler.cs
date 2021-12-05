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
    public class UserCommHandler : IRequestHandler<CreateUser, string>,
                                  IRequestHandler<RemoveUser, string>,
                                  IRequestHandler<ModifyUser, string>
    {
        private readonly ILogger<UserCommHandler> _logger;
        public UserCommHandler(ILogger<UserCommHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            _logger.LogError("mediator过程开始");
            return Task.FromResult("CreateUser created");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> Handle(RemoveUser request, CancellationToken cancellationToken)
        {
            _logger.LogError("mediator过程开始");
            return Task.FromResult("CreateUser created");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> Handle(ModifyUser request, CancellationToken cancellationToken)
        {
            _logger.LogError("mediator过程开始");
            return Task.FromResult("CreateUser created");
        }
    }
}
