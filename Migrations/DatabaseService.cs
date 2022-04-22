using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Migrations;

public class DatabaseService : IDatabaseService
{
    private readonly ILogger _logger;
    private readonly TournamentDbContext _dbContext;

    public DatabaseService(
        ILogger<DatabaseService> logger,
        TournamentDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async void CheckDatabaseConnection()
    {
        _logger.Log(LogLevel.Information, "Connecting database...");
        if (await _dbContext.Database.CanConnectAsync())
        {
            _logger.Log(LogLevel.Information, "Succesfully connected to database");
            IsDatabaseSchemaValid();
        }
        else
        {
            _logger.Log(LogLevel.Critical, "Cannot connect to database");
        }
    }

    private async void IsDatabaseSchemaValid()
    {
        var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            _logger.Log(LogLevel.Information, "Database schema updating...");
            await _dbContext.Database.MigrateAsync();
            _logger.Log(LogLevel.Information, "Database schema updated");
        }
    }
}

public interface IDatabaseService
{
    void CheckDatabaseConnection();
}