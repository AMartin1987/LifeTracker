using System.Collections.ObjectModel;

namespace LifeTracker.ViewModels;

public class SessionHistoryDisplay
{
    public string DateText { get; set; } = string.Empty;
    public string ActivityName { get; set; } = string.Empty;
    public string DurationText { get; set; } = string.Empty;
}

public class HistoryViewModel : BaseViewModel
{
    public ObservableCollection<SessionHistoryDisplay> HistoryList { get; set; } = new();


    public async Task LoadHistoryAsync()
    {
        try
        {
            var sessions = await App.Database.GetAllSessionsAsync();
            var activities = await App.Database.GetAllActivitiesAsync();
            var activityDictionary = activities.ToDictionary(a => a.Id, a => a.Name);

            HistoryList.Clear();

            foreach (var session in sessions)
            {
                activityDictionary.TryGetValue(session.ActivityId, out var name);
                name ??= "Activity name removed";

                HistoryList.Add(new SessionHistoryDisplay
                {
                    DateText = session.Date.ToString("dd/MM/yyyy"),
                    ActivityName = name,
                    DurationText = session.Duration.ToString(@"hh\:mm\:ss")
                });
            }

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading history: {ex.Message}");
        }
    }
}