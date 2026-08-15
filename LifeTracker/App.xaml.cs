using LifeTracker.Data;

namespace LifeTracker;

public partial class App : Application
{
    public static LocalDatabase Database { get; private set; }

    public App()
    {
        InitializeComponent();

        Database = new LocalDatabase();
        _ = Database.InitAsync();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = new Window(new AppShell())
        {
            Title = "LifeTracker",
            Width = 400,
            Height = 450
        };

        window.Created += (sender, args) =>
        {
#if WINDOWS
            var nativeWindow = window.Handler?.PlatformView as Microsoft.UI.Xaml.Window;

            if (nativeWindow != null)
            {
                var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

                if (appWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter presenter)
                {
                    presenter.IsAlwaysOnTop = true; 
                }
                appWindow.Resize(new Windows.Graphics.SizeInt32(400, 450));
            }
#endif
        };

        return window;
    }
}