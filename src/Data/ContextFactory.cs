using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Zs.Common.Extensions;

namespace DemoBot.Data
{
    public class ContextFactory : IDbContextFactory<DemoBotContext>, IDesignTimeDbContextFactory<DemoBotContext>
    {
        private readonly DbContextOptions<DemoBotContext>? _options;

        public ContextFactory()
        {
        }

        public ContextFactory(DbContextOptions<DemoBotContext> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        // For repositories
        public DemoBotContext CreateDbContext() => new DemoBotContext(_options!);

        // For migrations
        public DemoBotContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetSecretValue("ConnectionStrings:Default");
            var optionsBuilder = new DbContextOptionsBuilder<DemoBotContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new DemoBotContext(optionsBuilder.Options);
        }
    }
}
