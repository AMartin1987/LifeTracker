namespace LifeTracker.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ViewModels.MainViewModel vm)
        {
            await vm.LoadCurrentDaySessionAsync();
        }
    }
}