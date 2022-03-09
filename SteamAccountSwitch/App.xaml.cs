using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

using SteamAccountSwitch.Interfaces;
using SteamAccountSwitch.Models;
using SteamAccountSwitch.Services.Windows;
using SteamAccountSwitch.ViewModels;

using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

#nullable disable

namespace SteamAccountSwitch
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider Container { get; private set; }

        public Window Window { get; private set; }
        

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            Container = ConfigureServices();
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
        }

        private static IServiceProvider ConfigureServices()
        {            
            var services = new ServiceCollection();
            
            // Services
            services.AddSingleton<IConfiguration>(provider => new ConfigurationBuilder()
                                                                .AddUserSecrets<App>().Build());
            services.AddSingleton<IConfig, WindowsConfigService>();
            services.AddSingleton<IMediaStorage, WindowsMediaStorageService>();
            services.AddSingleton<ISteam, WindowsSteamService>();

            return services.BuildServiceProvider();
        }
    }
}
