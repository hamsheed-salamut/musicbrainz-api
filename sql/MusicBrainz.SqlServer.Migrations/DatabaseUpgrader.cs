using DbUp;
using DbUp.Engine;
using Polly;
using Serilog;
using System.Data.Common;
using System.Reflection;

namespace MusicBrainz.SqlServer.Migrations
{
    public class DatabaseUpgrader
    {
        private readonly ConnectionSetting _settings;
        private readonly Policy _connectionPolicy;
        private readonly ILogger _logger;

        public DatabaseUpgrader(ConnectionSetting settings, ILogger logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // retry policy when creating database connection
            _connectionPolicy = Policy
                .Handle<DbConnectionException>()
                .WaitAndRetry(settings.ConnectRetryCount,
                i => TimeSpan.FromSeconds(settings.ConnectRetryInterval),
                (exception, _) => { _logger.Error(exception, "Error connecting to the database."); });
        }

        public bool Upgrade()
        {
            _logger.Information("Starting Database Deployment");
            var policyResult = _connectionPolicy.ExecuteAndCapture(() => CreateUpgradeEngine());

            if (policyResult.Outcome != OutcomeType.Failure)
            {
                var upgrader = policyResult.Result;

                if (upgrader.IsUpgradeRequired())
                {
                    var result = upgrader.PerformUpgrade();
                    return result.Successful;
                }

                _logger.Information("No upgrade required");
                return true;
            }

            _logger.Error("Unable to connect to the database");
            return false;
        }

        private UpgradeEngine CreateUpgradeEngine()
        {
            EnsureDatabase.For.SqlDatabase(_settings.ConnectionString);

            // create upgrade engine with single transaction
            var upgradeEngine = DeployChanges.To.SqlDatabase(_settings.ConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithTransactionPerScript()
                .LogToAutodetectedLog()
                .Build();

            if (!upgradeEngine.TryConnect(out var error))
                throw new DbConnectionException(error);

            return upgradeEngine;
        }

        private sealed class DbConnectionException : DbException
        {
            public DbConnectionException(string message) : base(message)
            {

            }
        }
    }
}
