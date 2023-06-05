using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;

        #region Constructor
        public UnhandledExceptionBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }
        #endregion

        #region IPipelineBehavior Implementation
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            //Do nothing with known type of exceptions
            //as these will be logged within their respective behavior
            catch (NotFoundException) { throw; }
            catch (ValidationException) { throw; }
            catch (ForbiddenAccessException) { throw; }
            //Log only unhandled exceptions
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, "Unhandled Exception for Request {Name} {@CurrentUser} {@Request}", requestName, _currentUserService, request);
                throw;
            }
        }
        #endregion
    }
}