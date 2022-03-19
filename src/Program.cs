using DemoBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Zs.Bot.Data.Abstractions;
using Zs.Bot.Data.Repositories;
using Zs.Bot.Data.SQLite;
using Zs.Bot.Data.SQLite.Repositories;
using Zs.Bot.Messenger.Telegram;
using Zs.Bot.Services.Commands;
using Zs.Bot.Services.DataSavers;
using Zs.Bot.Services.Messaging;
using Zs.Common.Abstractions;
using Zs.Common.Exceptions;
using Zs.Common.Extensions;
using Zs.Common.Services.Abstractions;
using Zs.Common.Services.Scheduler;
using Zs.Common.Services.Shell;

namespace DemoBot
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                await CreateHostBuilder(args).RunConsoleAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                TrySaveFailInfo(ex.ToText());
                Console.WriteLine(ex.ToText());
                Console.Read();
            }
        }

        private static void TrySaveFailInfo(string text)
        {
            try
            {
                string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"Critical_failure_{DateTime.Now:yyyy.MM.dd HH:mm:ss.ff}.log");
                File.AppendAllText(path, text);
            }
            catch { }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, configurationBuilder) => configurationBuilder.AddConfiguration(CreateConfiguration(args)))
                .ConfigureLogging(logging => logging.AddConsole())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<SQLiteBotContext>(options =>
                        options.UseSqlite(hostContext.Configuration.GetSecretValue("ConnectionStrings:Default")));

                    services.AddSingleton<IDbContextFactory<DemoBotContext>, ContextFactory>();
                    services.AddSingleton<IDbContextFactory<SQLiteBotContext>, SQLiteBotContextFactory>();

                    services.AddScoped<ITelegramBotClient>(sp =>
                        new TelegramBotClient(hostContext.Configuration.GetSecretValue("Bot:Token"), new HttpClient()));

                    services.AddScoped<IMessenger, TelegramMessenger>();

                    services.AddScoped<ICommandsRepository, CommandsRepository<SQLiteBotContext>>();
                    services.AddScoped<IUserRolesRepository, UserRolesRepository<SQLiteBotContext>>();
                    services.AddScoped<IChatsRepository, ChatsRepository<SQLiteBotContext>>();
                    services.AddScoped<IUsersRepository, UsersRepository<SQLiteBotContext>>();
                    services.AddScoped<IMessagesRepository, MessagesRepository<SQLiteBotContext>>();

                    services.AddScoped<IScheduler, Scheduler>();
                    services.AddScoped<IMessageDataSaver, MessageDataDBSaver>();
                    services.AddScoped<IShellLauncher, ShellLauncher>(sp =>
                        new ShellLauncher(
                            bashPath: hostContext.Configuration.GetSecretValue("Bot:BashPath"),
                            powerShellPath: hostContext.Configuration.GetSecretValue("Bot:PowerShellPath")
                        ));
                    services.AddScoped<ICommandManager, CommandManager>();
                    services.AddScoped<IDbClient, DbClient>(sp =>
                        new DbClient(
                            hostContext.Configuration.GetSecretValue("ConnectionStrings:Default"),
                            sp.GetService<ILogger<DbClient>>())
                        );

                    
                    services.AddHostedService<DemoBot>();
                });
        }

        private static IConfiguration CreateConfiguration(string[] args)
        {
            var mainConfigPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (!File.Exists(mainConfigPath))
                throw new AppsettingsNotFoundException();

            var configuration = new ConfigurationManager();
            configuration.AddJsonFile(mainConfigPath, optional: false, reloadOnChange: true);

            foreach (var arg in args)
            {
                if (!File.Exists(arg))
                    throw new FileNotFoundException($"Wrong configuration path:\n{arg}");

                configuration.AddJsonFile(arg, optional: true, reloadOnChange: true);
            }

            if (configuration["SecretsPath"] != null)
                configuration.AddJsonFile(configuration["SecretsPath"]);

            return configuration;
        }

    }
}
