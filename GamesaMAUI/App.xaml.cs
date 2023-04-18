using Gamesa.Shared;

namespace GamesaMAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);
            if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                window.Title = "Gamesa";
                window.Width = Config.BoardSize + 50;
                window.Height = Config.BoardSize + 100;
            }
            return window;
        }
    }
}