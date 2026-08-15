using SQLite;
using LifeTracker.Models;

namespace LifeTracker.Data;

public class LocalDatabase
{
    private SQLiteAsyncConnection? _connection;
    private readonly string _dbPath;

    public LocalDatabase()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "LifeTracker.db3");
        System.Diagnostics.Debug.WriteLine($"LA BASE DE DATOS ESTÁ AQUÍ: {_dbPath}");
    }

    public async Task InitAsync()
    {
        if (_connection is not null)
            return;

        _connection = new SQLiteAsyncConnection(_dbPath);

        await _connection.CreateTableAsync<Activity>();
        await _connection.CreateTableAsync<ActivitySession>();
    }

    public async Task<Activity> GetOrCreateActivityAsync(string activityName)
    {
        await InitAsync();

        var activity = await _connection.Table<Activity>()
                                        .FirstOrDefaultAsync(a => a.Name == activityName);
        if (activity == null)
        {
            activity = new Activity { Name = activityName };
            await _connection.InsertAsync(activity);
        }

        return activity;
    }

    public async Task SaveSessionAsync(ActivitySession session)
    {
        await InitAsync();

        if (session.Id != 0)
        {
            await _connection.UpdateAsync(session);
        }
        else
        {
            await _connection.InsertAsync(session);
        }
    }

    public async Task<List<Activity>> GetAllActivitiesAsync()
    {
        await InitAsync();
        return await _connection.Table<Activity>().OrderBy(a => a.Name).ToListAsync();
    }

    public async Task<ActivitySession?> GetSessionAsync(int activityId, DateTime date)
    {
        await InitAsync();
        return await _connection.Table<ActivitySession>()
                                .FirstOrDefaultAsync(s => s.ActivityId == activityId && s.Date == date);
    }

    public async Task<List<ActivitySession>> GetAllSessionsAsync()
    {
        await InitAsync();
        return await _connection.Table<ActivitySession>().OrderByDescending(s => s.Date).ToListAsync();
    }
}