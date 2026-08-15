using System.Collections.ObjectModel;
using System.Windows.Input;
using LifeTracker.Models;

namespace LifeTracker.ViewModels;

public class ActivitiesViewModel : BaseViewModel
{
    private string _newActivityName = string.Empty;
    private Activity? _selectedActivity;

    public ObservableCollection<Activity> ActivitiesList { get; set; } = new();

    public string NewActivityName
    {
        get => _newActivityName;
        set { _newActivityName = value; OnPropertyChanged(); }
    }

    public Activity? SelectedActivity
    {
        get => _selectedActivity;
        set
        {
            if (_selectedActivity != value)
            {
                _selectedActivity = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand AddActivityCommand { get; }
    public ICommand SelectActivityCommand { get; }

    public ActivitiesViewModel()
    {
        AddActivityCommand = new Command(async () => await AddActivityAsync());
        SelectActivityCommand = new Command(SelectActivity);
    }


    public async void LoadActivities()
    {
        try
        {
            var dbActivities = await App.Database.GetAllActivitiesAsync();

            ActivitiesList.Clear();
            foreach (var act in dbActivities)
            {
                ActivitiesList.Add(act);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading activities: {ex.Message}");
        }
    }

    private async Task AddActivityAsync()
    {
        if (string.IsNullOrWhiteSpace(NewActivityName)) return;

        await App.Database.GetOrCreateActivityAsync(NewActivityName.Trim());
        NewActivityName = string.Empty;
        LoadActivities();

    }


    private void SelectActivity()
    {
        if (SelectedActivity == null) return;

        AppState.SetCurrentActivity(SelectedActivity);
        Shell.Current.GoToAsync("//MainPage");
    }
}