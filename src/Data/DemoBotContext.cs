using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Reflection;
using System.Text;
using Zs.Bot.Data.SQLite;
using Zs.Common.Extensions;

namespace DemoBot.Data
{
    public class DemoBotContext : DbContext
    {

        public DemoBotContext()
        {
        }

        public DemoBotContext(DbContextOptions<DemoBotContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SQLiteBotContext.ConfigureEntities(modelBuilder);
            SQLiteBotContext.SeedData(modelBuilder);

            ConfigureEntities(modelBuilder);
            SeedData(modelBuilder);
        }
        private void ConfigureEntities(ModelBuilder modelBuilder)
        {
        }

        protected void SeedData(ModelBuilder modelBuilder)
        {
        }

        public static string GetOtherSqlScripts(string configPath)
        {
            var configuration = new ConfigurationBuilder()
                  .AddJsonFile(System.IO.Path.GetFullPath(configPath))
                  .Build();

            var connectionStringBuilder = new DbConnectionStringBuilder()
            {
                ConnectionString = configuration.GetSecretValue("ConnectionStrings:Default")
            };
            var dbName = connectionStringBuilder["Database"] as string;

            var resources = new[]
            {
                "Priveleges.sql",
                "StoredFunctions.sql",
                "Triggers.sql"
            };

            var sb = new StringBuilder();
            foreach (var resourceName in resources)
            {
                var sqlScript = Assembly.GetExecutingAssembly().ReadResource(resourceName);
                sb.Append(sqlScript + Environment.NewLine);
            }

            if (!string.IsNullOrWhiteSpace(dbName))
                sb.Replace("DefaultDbName", dbName);

            return sb.ToString();
        }
    }
}
