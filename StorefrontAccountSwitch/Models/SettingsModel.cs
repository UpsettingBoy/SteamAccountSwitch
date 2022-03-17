using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;

using StorefrontAccountSwitch.Interfaces;

namespace StorefrontAccountSwitch.Models
{
    public partial class SettingsModel: ObservableObject
    {
        private IConfig _config;
        
        [ObservableProperty]
        private string? _steamExePath;
        [ObservableProperty]
        private bool _steamAutoLaunch; // TODO: Remove someday
        [ObservableProperty]
        private ElementTheme _appTheme;

        public SettingsModel(IConfig config)
        {
            _config = config;
            
            _steamExePath = _config.InitConfig<string>("SteamExePath", null);
            _steamAutoLaunch = _config.InitConfig<bool>("SteamAutoLaunch", true);
            _appTheme = (ElementTheme)_config.InitConfig<byte>("AppTheme", (byte)ElementTheme.Default);
        }

        public void Save()
        {
            _config.SetConfig("SteamExePath", _steamExePath);
            _config.SetConfig("SteamAutoLaunch", _steamAutoLaunch);
            _config.SetConfig("AppTheme", (byte)_appTheme);
        }
    }
}
