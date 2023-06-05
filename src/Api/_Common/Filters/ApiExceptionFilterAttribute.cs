namespace Api.Common.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private const string VALIDATION_TITLE = "Validation Errors";
    private const string VALIDATION_CONTENT_TYPE = "application/problem+json";
    private const string VALIDATION_DETAILS = "Please refer to the errors property for validation details.";
    private const string VALIDATION_TYPE = "https://tools.ietf.org/html/rfc4918#section-11.2";

    #region Constructors
    public ApiExceptionFilterAttribute()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            { typeof(JsonPatchException), HandleJsonPatchException }
        };
    }
    #endregion

    #region OnException Override
    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }
    #endregion

    #region Private Handlers
    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        HandleUnknownException(context);
    }

    private static void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Oops, Unhandled Exception",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Detail = "An unexpected error has occured. The appropriate system administrators have been contacted.",
            Instance = context.HttpContext.Request.Path
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            ContentTypes = { VALIDATION_CONTENT_TYPE }
        };

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var validationException = context.Exception as ValidationException;
        var details = new ValidationProblemDetails(validationException.Errors)
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = VALIDATION_TITLE,
            Type = VALIDATION_TYPE,
            Detail = VALIDATION_DETAILS,
            Instance = context.HttpContext.Request.Path
        };
        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity,
            ContentTypes = { VALIDATION_CONTENT_TYPE }
        };
        context.ExceptionHandled = true;
    }

    private static void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState)
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = VALIDATION_TITLE,
            Type = VALIDATION_TYPE,
            Detail = VALIDATION_DETAILS,
            Instance = context.HttpContext.Request.Path
        };
        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity,
            ContentTypes = { VALIDATION_CONTENT_TYPE }
        };
        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "Not Found",
            Detail = "The specified resource cannot be found.",
            Instance = context.HttpContext.Request.Path
        };

        context.Result = new NotFoundObjectResult(details)
        {
            ContentTypes = { VALIDATION_CONTENT_TYPE }
        };
        context.ExceptionHandled = true;
    }

    private static void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            Detail = "Invalid authorization token or unauthorized to use this resource.",
            Instance = context.HttpContext.Request.Path
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status401Unauthorized,
            ContentTypes = { VALIDATION_CONTENT_TYPE }
        };

        context.ExceptionHandled = true;
    }

    private static void HandleForbiddenAccessException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            Instance = context.HttpContext.Request.Path,
            Detail = "Forbidden to use this resource."
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status403Forbidden,
            ContentTypes = { VALIDATION_CONTENT_TYPE },
        };

        context.ExceptionHandled = true;
    }

    private static void HandleJsonPatchException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid JsonPatch Document",
            Type = "https://tools.ietf.org/html/rfc5789#section-2.2",
            Instance = context.HttpContext.Request.Path,
            Detail = context.Exception.Message
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ContentTypes = { VALIDATION_CONTENT_TYPE },
        };
        context.ExceptionHandled = true;
    }
    #endregion
}
