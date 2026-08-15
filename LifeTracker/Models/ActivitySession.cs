using SQLite;

namespace LifeTracker.Models;

public class ActivitySession
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int ActivityId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Duration { get; set; }
}