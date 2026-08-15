namespace LifeTracker.Views;

public partial class NavBarView : ContentView
{
    public NavBarView()
    {
        InitializeComponent();
    }

    private async void OnTimerIconClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnActivitiesIconClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ActivitiesPage");
    }

    private async void OnHistoryIconClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HistoryPage");
    }
}