using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI;
using Microsoft.UI.Xaml;

using StorefrontAccountSwitch.Interfaces;
using StorefrontAccountSwitch.Messages;
using StorefrontAccountSwitch.Services.Windows;

using System;

using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

#nullable disable

namespace StorefrontAccountSwitch
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application, IRecipient<UpdateAppThemeMessage>
    {
        public IServiceProvider Container { get; private set; }
        public Window Window { get; private set; }
        

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Container = ConfigureServices();
            SetElementsTheme();

            this.InitializeComponent();
            WeakReferenceMessenger.Default.Register(this);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Window = new MainWindow();
            Window.Activate();

            WinUIEx.WindowExtensions.CenterOnScreen(Window);

            WeakReferenceMessenger.Default.Send(new UpdateAppThemeMessage((ElementTheme)Container.GetService<IConfig>().GetConfig<byte>("AppTheme")));
        }

        private void SetElementsTheme()
        {
            // Settings initial setup
            _ =  new Models.SettingsModel(Container.GetService<IConfig>());
            
            var elementTheme = (ElementTheme)Container.GetService<IConfig>().GetConfig<byte>("AppTheme");
            var requestedTheme = elementTheme switch
            {
                ElementTheme.Light => ApplicationTheme.Light,
                ElementTheme.Dark => ApplicationTheme.Dark,
                _ => RequestedTheme,
            };
            RequestedTheme = requestedTheme;
        }

        private static IServiceProvider ConfigureServices()
        {            
            var services = new ServiceCollection();
            
            services.AddSingleton<IConfiguration>(provider => new ConfigurationBuilder()
                                                                .AddUserSecrets<App>().Build());
            services.AddSingleton<IConfig, WindowsConfigService>();
            services.AddSingleton<IMediaStorage, WindowsMediaStorageService>();
            services.AddSingleton<ISteam, WindowsSteamService>();

            return services.BuildServiceProvider();
        }

        public void Receive(UpdateAppThemeMessage message)
        {
            var lightGray = new Color() { A = 255, R = 240, G = 240, B = 240 };
            var titleColor = message.Value switch
            {
                ElementTheme.Light => lightGray,
                ElementTheme.Dark => Colors.Black,
                _ => RequestedTheme switch
                {
                    ApplicationTheme.Light => lightGray,
                    ApplicationTheme.Dark => Colors.Black,
                    _ => lightGray,
                },
            };
            
            if (Window.Content is FrameworkElement fe)
            {
                fe.RequestedTheme = message.Value;
                WinUIEx.WindowExtensions.SetTitleBarBackgroundColors(Window, titleColor);
            }
        }
    }
}
