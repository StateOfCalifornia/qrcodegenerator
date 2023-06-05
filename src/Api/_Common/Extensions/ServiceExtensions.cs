namespace Api.Common.Extensions;

public static class ServiceExtensions
{
    public static void AddMyApi(this IServiceCollection services)
    {
        //Add Startup Validation Filter in order to validate all config settings
        services.AddTransient<IStartupFilter, OptionsValidationStartupFilter>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
    }

    public static void AddMyControllers(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services
            .AddControllers(options =>
            {
                //All Api routes possibly can throw these errors by default, 
                //so lets document them so they are added to swagger to each route by default
                options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
                options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status404NotFound));
                options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                //Add Exception Attribute
                options.Filters.Add<ApiExceptionFilterAttribute>();
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Formatting = !environment.IsProduction() ? Formatting.Indented : Formatting.None;
            })
            .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        // Customise default API behaviour to supress modlestate as FluentValidation takes care of this
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
    }

    public static void UseMyDevelopmentServices(this IApplicationBuilder app, IServiceCollection services)
    {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();

        //This maps all our services to a cshtml template using existing service
        app.Map("/services", builder => builder.Run(async context =>
        {
            var viewRenderer = services.BuildServiceProvider().GetService<IRazorViewToStringService>();
            var htmlString = await viewRenderer.RenderViewToStringAsync("/Common/services.cshtml", services);
            await context.Response.WriteAsync(htmlString);
        }));
    }
}
