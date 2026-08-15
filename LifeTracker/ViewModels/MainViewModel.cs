using LifeTracker.Models;
using System.Windows.Input;

namespace LifeTracker.ViewModels;

public class MainViewModel : BaseViewModel
{
    private int _currentActivityId;
    private ActivitySession? _currentSession;
    private string _elapsedTime = "00:00:00";
    private string _actionButtonText = "Start";
    private string _currentActivity = "Estudiando DAM";
    private bool _isRunning = false;

    private readonly IDispatcherTimer _timer;
    private DateTime _startTime;
    private TimeSpan _accumulatedTime = TimeSpan.Zero;

    public string ElapsedTime
    {
        get => _elapsedTime;
        set { _elapsedTime = value; OnPropertyChanged(); }
    }

    public string ActionButtonText
    {
        get => _actionButtonText;
        set { _actionButtonText = value; OnPropertyChanged(); }
    }

    public string CurrentActivity
    {
        get => _currentActivity;
        set { _currentActivity = value; OnPropertyChanged(); }
    }

    public ICommand ToggleTimerCommand { get; }

    public MainViewModel()
    {
        ToggleTimerCommand = new Command(async () => await ToggleTimerAsync());

        _timer = Application.Current!.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += OnTimerTick;

        AppState.OnActivityChanged += async (newActivity) =>
        {
            if (_isRunning)
            {
                ToggleTimerCommand.Execute(null);
            }

            CurrentActivity = newActivity.Name;
            _currentActivityId = newActivity.Id;

            DateTime currentLogicalDay = DateTime.Now.AddHours(-5).Date;
            var existingSession = await App.Database.GetSessionAsync(_currentActivityId, currentLogicalDay);

            if (existingSession != null)
            {
                _currentSession = existingSession;
                _accumulatedTime = existingSession.Duration;
            }
            else
            {
                _currentSession = null;
                _accumulatedTime = TimeSpan.Zero;
            }

            ElapsedTime = _accumulatedTime.ToString(@"hh\:mm\:ss");
            ActionButtonText = "Start";
        };

        _ = LoadCurrentDaySessionAsync();
    }

    public async Task LoadCurrentDaySessionAsync()
    {
        if (_isRunning) return;

        DateTime currentLogicalDay = DateTime.Now.AddHours(-5).Date;

        if (_currentActivityId == 0)
        {
            var activity = await App.Database.GetOrCreateActivityAsync(CurrentActivity);
            _currentActivityId = activity.Id;
        }

        var existingSession = await App.Database.GetSessionAsync(_currentActivityId, currentLogicalDay);

        if (existingSession != null)
        {
            _currentSession = existingSession;
            _accumulatedTime = existingSession.Duration;
        }
        else
        {
            _currentSession = null;
            _accumulatedTime = TimeSpan.Zero;
        }

        ElapsedTime = _accumulatedTime.ToString(@"hh\:mm\:ss");
    }

    private async Task ToggleTimerAsync()
    {
        DateTime currentLogicalDay = DateTime.Now.AddHours(-5).Date;

        if (!_isRunning)
        {
            if (_currentActivityId == 0)
            {
                var activity = await App.Database.GetOrCreateActivityAsync(CurrentActivity);
                _currentActivityId = activity.Id;
            }

            if (_currentSession == null || _currentSession.Date != currentLogicalDay)
            {
                _currentSession = await App.Database.GetSessionAsync(_currentActivityId, currentLogicalDay);

                if (_currentSession != null)
                {
                    _accumulatedTime = _currentSession.Duration;
                }
                else
                {
                    _accumulatedTime = TimeSpan.Zero;
                    _currentSession = new ActivitySession
                    {
                        ActivityId = _currentActivityId,
                        Date = currentLogicalDay,
                        Duration = _accumulatedTime
                    };
                    await App.Database.SaveSessionAsync(_currentSession);
                }
            }

            _isRunning = true;
            ActionButtonText = "Pause";
            _startTime = DateTime.Now;
            _timer.Start();
        }
        else
        {
            _isRunning = false;
            ActionButtonText = "Resume";
            _timer.Stop();
            _accumulatedTime += DateTime.Now - _startTime;

            if (_currentSession != null)
            {
                _currentSession.Duration = _accumulatedTime;
                await App.Database.SaveSessionAsync(_currentSession);
            }
        }
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        TimeSpan currentSession = DateTime.Now - _startTime;
        TimeSpan totalTime = _accumulatedTime + currentSession;
        ElapsedTime = totalTime.ToString(@"hh\:mm\:ss");
    }
}