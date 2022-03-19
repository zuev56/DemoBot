using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using Zs.Bot.Data.Enums;
using Zs.Bot.Services.Messaging;
using Zs.Common.Abstractions;
using Zs.Common.Enums;
using Zs.Common.Extensions;
using Zs.Common.Services.Abstractions;
using Zs.Common.Services.Scheduler;

namespace DemoBot
{
    internal class DemoBot : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IMessenger _messenger;
        private readonly IScheduler _scheduler;
        private readonly IDbClient _dbClient;
        private readonly ILogger<DemoBot>? _logger;


        public DemoBot(
            IConfiguration configuration,
            IMessenger messenger,
            IScheduler scheduler,
            IDbClient dbClient,
            ILogger<DemoBot>? logger = null)
        {
            try
            {
                _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
                _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
                _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
                _dbClient = dbClient ?? throw new ArgumentNullException(nameof(dbClient));
                _logger = logger;

                if (_configuration.GetSection("EnableDemoJobs").Get<bool>())
                    CreateJobs();
            }
            catch (Exception ex)
            {
                var tiex = new TypeInitializationException(typeof(DemoBot).FullName, ex);
               _logger?.LogError(tiex, $"{nameof(DemoBot)} initialization error");
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _scheduler.Start(dueTimeMs: 3000, periodMs: 1000);

                string startMessage = $"Bot '{nameof(DemoBot)}' started."
                    + Environment.NewLine + Environment.NewLine
                    + RuntimeInformationWrapper.GetRuntimeInfo();

                var rolesThatWillReceiveStartMessage = new[] { Role.Owner, Role.Admin };

                await _messenger.AddMessageToOutboxAsync(startMessage, rolesThatWillReceiveStartMessage);
                _logger?.LogInformation(startMessage);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Bot starting error");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _scheduler.Stop();
            _logger?.LogInformation("Bot stopped");

            return Task.CompletedTask;
        }

        private void CreateJobs()
        {
            var programJobExample = new ProgramJob<string>(
                period: TimeSpan.FromSeconds(60),
                method: SayHello,
                startUtcDate: DateTime.UtcNow + TimeSpan.FromSeconds(10),
                description: "programJobExample"
            );
            programJobExample.ExecutionCompleted += Job_ExecutionCompleted;
            _scheduler.Jobs.Add(programJobExample);

            var sqlJobExample = new SqlJob(
                period: TimeSpan.FromSeconds(60),
                resultType: QueryResultType.String,
                sqlQuery: $"SELECT 'Hello! It''s the SQL job result'",
                dbClient: _dbClient,
                startUtcDate: DateTime.UtcNow + TimeSpan.FromSeconds(20),
                description: "sqlJobExample"
            );
            sqlJobExample.ExecutionCompleted += Job_ExecutionCompleted;
            _scheduler.Jobs.Add(sqlJobExample);
        }

        private async Task<string> SayHello()
            => await Task.FromResult($"Hello! It's the program job result");// {DateTime.Now:dd.MM.yy HH:mm:ss}");

        private async void Job_ExecutionCompleted(IJob<string> job, IOperationResult<string> result)
        {
            _logger?.Log(LogLevel.Information, "Job \"{Job}\" execution completed", job.Description);

            await _messenger.AddMessageToOutboxAsync(result.Value, Role.Owner, Role.Admin);
        }

    }
}
