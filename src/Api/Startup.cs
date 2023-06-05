namespace Api;

public class Startup
{
    private IConfiguration Configuration { get; }
    private IWebHostEnvironment Environment { get; }
    private IServiceCollection _services;

    #region Constructor
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }
    #endregion

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add Project Dependencies
        services.AddMyApi();
        services.AddMyApplication();
        services.AddMyInfrastructure(Configuration);
        services.AddMyControllers(Environment);
        // Only add Swagger to non-production sites.
        if (!Environment.IsProduction()) services.AddMySwaggerDocumentation(Configuration);

        //The middleware defaults to sending a Status307TemporaryRedirect with all redirects
        //In Production, we want to enable a permanent redirect
        if (!Environment.IsDevelopment())
        {
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                options.HttpsPort = 443;
            });
        }

        if (Environment.IsDevelopment()) services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton(services);
        _services = services;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IMapper autoMapper)
    {
        //Set serilog as logger
        loggerFactory.AddSerilog();

        //Verify Automapper Configuration
        autoMapper.ConfigurationProvider.AssertConfigurationIsValid();

        if (Environment.IsDevelopment()) app.UseMyDevelopmentServices(_services);
        else app.UseHsts();

        app.UseStaticFiles();

        app.UseHealthChecks("/health");
        app.UseHttpsRedirection();
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor |
            ForwardedHeaders.XForwardedProto
        });

        if (!Environment.IsProduction()) app.UseMySwaggerDocumentation(Configuration);

        // (A)
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        // (B)
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
