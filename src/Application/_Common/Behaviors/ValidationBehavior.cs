﻿using Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;

        #region Constructor
        public ValidationBehavior(ILogger<TResponse> logger, IEnumerable<IValidator<TRequest>> validators, ICurrentUserService currentUserService)
        {
            _validators = validators;
            _logger = logger;
            _currentUserService = currentUserService;
        }
        #endregion

        #region IPipelineBehavior Implementation
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    var name = typeof(TRequest).Name;
                    _logger.LogWarning("Validation Warning: {RequestName} {@CurrentUser} {@Request} {@Failures} ", name, _currentUserService, request, failures);
                    throw new ValidationException(failures);
                }
            }
            return await next();
        }
        #endregion
    }
}