namespace LifeTracker.Views;

public partial class ActivitiesPage : ContentPage
{
    public ActivitiesPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ViewModels.ActivitiesViewModel vm)
        {
            vm.LoadActivities();
        }
    }
}