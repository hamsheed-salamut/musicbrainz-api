using Microsoft.Extensions.Configuration;
using Serilog;

namespace MusicBrainz.SqlServer.Migrations
{
    class Program
    {
        private static ILogger _logger;
        private static IConfiguration _configuration;
        private readonly AutoResetEvent _closing = new AutoResetEvent(false);

        static int Main()
        {
            var configuration = BuildConfiguration();
            var logger = CreateLogger(configuration);

            return new Program(configuration, logger).Run();
        }

        private Program(IConfiguration configuration, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        private int Run()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            try
            {
                //Thread.Sleep(TimeSpan.FromSeconds(15));

                var databaseSettings = new ConnectionSetting();
                _configuration.Bind("Settings", databaseSettings);
                var upgrader = new DatabaseUpgrader(databaseSettings, _logger);
                return upgrader.Upgrade() ? 0 : 1;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error starting DatabaseDeployment");
                return 1;
            }
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
            Console.WriteLine(ex.ExceptionObject.ToString());
            Environment.Exit(1);
        }

        private static ILogger CreateLogger(IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Log.Logger = logger;
            return logger.ForContext<Program>();
        }

        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}