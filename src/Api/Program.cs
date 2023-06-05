namespace Northwind.Api;

public class Program
{
    protected Program() { }

    #region Public Statics
    public static IConfiguration Configuration { get; } = GetConfiguration();
    public static IHost BuildWebHost(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseConfiguration(Configuration)
            .UseStartup<Startup>();
        })
        .Build();
    #endregion

    #region Main Entry
    public static int Main(string[] args)
    {
        var START_UP_APPLICATION_VALUE = typeof(Program).Namespace;
        Log.Logger = RegisterLogger().CreateLogger();
        Log.Verbose($"{START_UP_APPLICATION_VALUE} Logger Initialized");
        try
        {
            Log.Verbose($"Starting {START_UP_APPLICATION_VALUE} Web Host");
            BuildWebHost(args).Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
    #endregion

    #region Private Statics
    private static IConfiguration GetConfiguration()
    {
        //Get base config
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        //If were in development, get Secrets
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development"))
        {
            configuration.AddUserSecrets<Program>(true);
        }

        return configuration.Build();
    }

    private static LoggerConfiguration RegisterLogger()
    {
        //Configure the logger
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration);
        return logger;
    }
    #endregion
}
