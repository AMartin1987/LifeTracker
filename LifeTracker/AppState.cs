using LifeTracker.Models;
using System;

namespace LifeTracker;

public static class AppState
{
    public static Activity? CurrentActivity { get; private set; }

    public static event Action<Activity>? OnActivityChanged;

    public static void SetCurrentActivity(Activity newActivity)
    {
        CurrentActivity = newActivity;

        OnActivityChanged?.Invoke(newActivity);
    }
}