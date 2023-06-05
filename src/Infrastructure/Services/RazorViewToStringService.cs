namespace Infrastructure.Services;

public class RazorViewToStringService : IRazorViewToStringService
{
    private readonly IRazorViewEngine _viewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IServiceProvider _serviceProvider;

    #region Constructor
    public RazorViewToStringService(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
    {
        _viewEngine = viewEngine;
        _tempDataProvider = tempDataProvider;
        _serviceProvider = serviceProvider;
    }
    #endregion

    #region IRazorViewToStringRenderer Implementation
    public async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
    {
        var actionContext = GetActionContext();
        var view = FindView(actionContext, viewName);

        using var output = new StringWriter();
        var viewContext = new ViewContext(
            actionContext,
            view,
            new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary()){ Model = model },
            new TempDataDictionary(actionContext.HttpContext,  _tempDataProvider),
            output,
            new HtmlHelperOptions());

        await view.RenderAsync(viewContext);

        return output.ToString();
    }
    #endregion

    #region Private Methods
    private IView FindView(ActionContext actionContext, string viewName)
    {
        var getViewResult = _viewEngine.GetView(null, viewName, true);
        if (getViewResult.Success) return getViewResult.View;

        var findViewResult = _viewEngine.FindView(actionContext, viewName, true);
        if (findViewResult.Success) return findViewResult.View;

        var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
        var errorMessage = string.Join(
            Environment.NewLine,
            new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations));
        throw new InvalidOperationException(errorMessage);
    }

    private ActionContext GetActionContext()
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };
        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }
    #endregion
}
