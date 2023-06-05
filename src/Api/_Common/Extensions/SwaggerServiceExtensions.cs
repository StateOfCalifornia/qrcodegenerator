namespace Api.Common.Extensions;

public static class SwaggerServiceExtensions
{
    #region Public static Methods
    public static IServiceCollection AddMySwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
    {
        // (1) Bind Settings
        // (2) Explicitly register setting objects by delegating to the IOptions object to allow settings DI directly
        services.Configure<SwaggerSettings>(o => configuration.GetSection("SwaggerSettings").Bind(o));
        services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<SwaggerSettings>>().Value);

        // (3) Add Swagger
        services.AddSwaggerGen(s =>
        {
            var swaggerSettings = services.BuildServiceProvider().GetRequiredService<SwaggerSettings>();
            s.SwaggerDoc(swaggerSettings.DocumentName, new OpenApiInfo
            {
                Version = swaggerSettings.Version,
                Title = swaggerSettings.Title,
                Description = $"{swaggerSettings.Description}.<br /><br />Environment: {configuration["Serilog:Properties:Environment"]}<br />Build Number: {configuration["Serilog:Properties:Version"]}<br />Running on: {RuntimeInformation.FrameworkDescription}",
                TermsOfService = new Uri(swaggerSettings.TermsOfServiceUri),
                Contact = new OpenApiContact
                {
                    Name = swaggerSettings.Contact.Name,
                    Email = swaggerSettings.Contact.Email,
                    Url = new Uri(swaggerSettings.Contact.Url)
                },
                License = new OpenApiLicense
                {
                    Name = swaggerSettings.License.Name,
                    Url = new Uri(swaggerSettings.License.Url)
                },
            });

            // (3c) Add xml documentation
            var basePath = AppContext.BaseDirectory;
            s.IncludeXmlComments(Path.Combine(basePath, "Api.xml"));
            s.IncludeXmlComments(Path.Combine(basePath, "Application.xml"));
        });

        return services;
    }

    public static IApplicationBuilder UseMySwaggerDocumentation(this IApplicationBuilder app, IConfiguration configuration)
    {
        // (1) Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // (2) Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        app.UseSwaggerUI(c =>
        {
            var swaggerOptions = new SwaggerSettings();
            configuration.GetSection("SwaggerSettings").Bind(swaggerOptions);
            var endpoint = $"/swagger/{swaggerOptions.DocumentName}/swagger.json";
            c.SwaggerEndpoint(endpoint, swaggerOptions.ProjectName);

            // (2a) This tells the api to serve swagger at root url
            c.RoutePrefix = string.Empty;
            
            // (2b) Set Document/Page Title
            c.DocumentTitle = swaggerOptions.ProjectName;
            c.DisplayRequestDuration();
            c.EnableFilter();

            // (2c) Inject UI elements
            c.InjectStylesheet("/swagger-ui/custom.css");
            c.InjectJavascript("/swagger-ui/custom.js");
        });
        return app;
    }
    #endregion
}
