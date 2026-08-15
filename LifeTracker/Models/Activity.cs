using SQLite;

namespace LifeTracker.Models;

public class Activity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [SQLite.MaxLength(100), Unique] 
    public string Name { get; set; }

    // Columnas preparadas para el futuro
    public string? ColorHex { get; set; }
    public string? IconName { get; set; }
}