namespace Api.Controllers;

[ApiController]
[Route("customer")]
public class CustomerController : BaseController
{
    /// <summary>Gets Customers</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IList<CustomerViewModel>), StatusCodes.Status200OK)]
    [ResponseCache(Duration = 300)]
    public async Task<ActionResult<IList<CustomerViewModel>>> Customers()
    {
        var list = await Mediator.Send(new GetAllCustomersQuery());
        return Ok(list);
    }
}