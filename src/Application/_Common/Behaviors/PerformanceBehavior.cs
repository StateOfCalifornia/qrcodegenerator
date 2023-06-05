using Application.Common.Interfaces;
using Application.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly AppSettings _appSettings;

        #region Constructor
        public PerformanceBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService, AppSettings appSettings)
        {
            _timer = new Stopwatch();
            _logger = logger;
            _currentUserService = currentUserService;
            _appSettings = appSettings;
        }
        #endregion

        #region IPipelineBehavior Implementation
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();
            var response = await next();
            _timer.Stop();

            if (_timer.ElapsedMilliseconds > _appSettings.PerformanceThresholdInMilliseconds)
            {
                var name = typeof(TRequest).Name;
                _logger.LogWarning(
                    "Long Running Request: {RequestName} {@CurrentUser} {@Request} (Limit Threshold is {LimitThresholdInMilliseconds} milliseconds):  ({ElapsedMilliseconds} milliseconds)",
                    name,
                    _currentUserService,
                    request,
                    _appSettings.PerformanceThresholdInMilliseconds,
                    _timer.ElapsedMilliseconds);
            }
            return response;
        }
        #endregion
    }
}