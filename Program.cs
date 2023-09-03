using DemoBot;
using Zs.Bot.Data.SQLite;
using Zs.Bot.Services;
using Zs.Bot.Telegram.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(static (context, services) =>
    {
        var settings = context.Configuration.Get<Settings>()!;
        services.Configure<Settings>(context.Configuration);

        services.AddTelegramBotClient(settings.BotToken);
        services.AddSqliteMessageDataStorage();
        services.AddCommandManager(settings.CliPath);

        services.AddHostedService<DemoBot.DemoBot>();
    })
    .Build();

using (var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<SQLiteBotContext>();
    await context.Database.EnsureCreatedAsync();
}

host.Run();