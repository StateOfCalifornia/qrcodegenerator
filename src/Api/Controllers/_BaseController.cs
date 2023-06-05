namespace Api.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    private ISender _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
}
