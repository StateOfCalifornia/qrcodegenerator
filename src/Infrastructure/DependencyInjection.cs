namespace Infrastructure;

public static class DependencyInjection
{
    #region Public Static Methods
    public static IServiceCollection AddMyInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddMyAppSettings(services, configuration);
        AddMyBusinessServices(services, configuration);
        AddMyRazorEmailServices(services);
        AddMyDateTimeServices(services);
        return services;
    }

    #endregion

    #region Private Static Methods
    private static void AddMyAppSettings(IServiceCollection services, IConfiguration configuration)
    {
        // (1) Bind AppSettings
        // (2) Register IValidateSettingsService in order to validate all settings
        // (3) Explicitly register setting objects by delegating to the IOptions object to allow settings DI directly
        services.Configure<AppSettings>(o => configuration.GetSection("AppSettings").Bind(o));
        services.AddSingleton<IValidateSettingsService>(resolver => resolver.GetRequiredService<IOptions<AppSettings>>().Value);
        services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppSettings>>().Value);
    }
    private static void AddMyBusinessServices(IServiceCollection services, IConfiguration configuration)
    {
        // (1) Configure your Database Context
        services.AddDbContext<NorthwindContext>(options =>
        {
            var businessConnString = configuration.GetConnectionString("NorthwindContext");
            options
                .UseSqlServer(businessConnString, x => x.EnableRetryOnFailure())
                .ConfigureWarnings(c => c.Log((RelationalEventId.CommandExecuting, Microsoft.Extensions.Logging.LogLevel.Debug)))
                .EnableSensitiveDataLogging()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        // (2) Add Health Checks for the Database Context
        services.AddHealthChecks().AddDbContextCheck<NorthwindContext>($"{nameof(NorthwindContext)} Database Health Check", HealthStatus.Unhealthy);

        // (3) Add Database Interface to Context
        services.AddScoped<INorthwindContext>(provider => provider.GetService<NorthwindContext>());
    }
    private static void AddMyDateTimeServices(IServiceCollection services)
    {
        var dateTimeService = new DateTimeService(DateTime.Now);
        services.AddTransient<IDateTimeService>(x => dateTimeService);
    }
    private static void AddMyRazorEmailServices(IServiceCollection services)
    {
        //Razor Pages are needed in order to inject Razor Rendering services
        services.AddRazorPages();
        services.AddScoped<IRazorViewToStringService, RazorViewToStringService>();
    }
    #endregion
}
