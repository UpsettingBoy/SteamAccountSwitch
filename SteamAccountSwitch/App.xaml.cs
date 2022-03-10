using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.UI;
using Microsoft.UI.Xaml;

using SteamAccountSwitch.Interfaces;
using SteamAccountSwitch.Messages;
using SteamAccountSwitch.Services.Windows;

using System;

using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

#nullable disable

namespace SteamAccountSwitch
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
            SetAppTheme((ElementTheme)Container.GetService<IConfig>().GetConfig<byte>("AppTheme"));

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

            WeakReferenceMessenger.Default.Send(new UpdateAppThemeMessage((ElementTheme)Container.GetService<IConfig>().GetConfig<byte>("AppTheme")));
        }

        private void SetAppTheme(ElementTheme elementTheme)
        {
            ApplicationTheme requestedTheme;
            switch (elementTheme)
            {
                case ElementTheme.Light:
                    requestedTheme = ApplicationTheme.Light;
                    break;
                case ElementTheme.Dark:
                    requestedTheme = ApplicationTheme.Dark;
                    break;
                case ElementTheme.Default:
                default:
                    requestedTheme = RequestedTheme;
                    break;
            }

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
            
            // App theme change
            // TODO: I hate this code bit, ugly
            Color titleColor;
            switch (message.Value)
            {
                case ElementTheme.Light:
                    titleColor = lightGray;
                    break;
                case ElementTheme.Dark:
                    titleColor = Colors.Black;
                    break;
                case ElementTheme.Default:
                default:
                    switch (RequestedTheme)
                    {
                        case ApplicationTheme.Light:
                            titleColor = lightGray;
                            break;
                        case ApplicationTheme.Dark:
                            titleColor = Colors.Black;
                            break;
                        default:
                            titleColor = lightGray;
                            break;
                    }
                    break;
            }

            if (Window.Content is FrameworkElement fe)
            {
                fe.RequestedTheme = message.Value;
                WinUIEx.WindowExtensions.SetTitleBarBackgroundColors(Window, titleColor);
            }
        }
    }
}
