using Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;

        #region Constructor
        public LoggingBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }
        #endregion

        #region IRequestPreProcessor Implementation
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var name = typeof(TRequest).Name;
            _logger.LogInformation("Request Information : {RequestName} {@CurrentUser} {@Request}", name, _currentUserService, request);
            return Task.CompletedTask;
        }
        #endregion
    }
}