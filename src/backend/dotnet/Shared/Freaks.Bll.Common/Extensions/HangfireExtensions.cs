using Freaks.Bll.Common.Hangfire;
using Freaks.Options.Common;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.Bll.Common.Extensions;

public static class HangfireExtensions
{
    public static IServiceCollection AddHangfireCommon(this IServiceCollection services, IConfiguration configuration)
    {
        var dbOptions =
            configuration.GetSection(nameof(DbOptions))
                         .Get<DbOptions>();

        if (dbOptions is null)
        {
            throw new ArgumentNullException();
        }

        services.AddSingleton<HangfireHttpContextSnapshotAttribute>();

        services.AddHangfire((serviceProvider, config) =>
        {
            config.UsePostgreSqlStorage(
                options =>
                {
                    options.UseNpgsqlConnection(dbOptions.ConnectionString);
                },
                new PostgreSqlStorageOptions
                {
                    SchemaName = "hangfire",
                    QueuePollInterval = TimeSpan.FromSeconds(5),
                    InvisibilityTimeout = TimeSpan.FromMinutes(5),
                });

            config.UseFilter(serviceProvider.GetRequiredService<HangfireHttpContextSnapshotAttribute>());
        });

        services.AddHangfireServer(options =>
        {
            options.WorkerCount = 1;
        });

        return services;
    }
}