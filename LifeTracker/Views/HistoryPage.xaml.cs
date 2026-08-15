namespace LifeTracker.Views;

public partial class HistoryPage : ContentPage
{
    public HistoryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ViewModels.HistoryViewModel vm)
        {
            await vm.LoadHistoryAsync();
        }
    }
}