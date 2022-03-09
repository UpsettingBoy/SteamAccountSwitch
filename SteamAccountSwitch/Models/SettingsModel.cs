using Microsoft.Toolkit.Mvvm.ComponentModel;

using SteamAccountSwitch.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAccountSwitch.Models
{
    public partial class SettingsModel: ObservableObject
    {
        private IConfig _config;
        
        [ObservableProperty]
        private string? _steamExePath;
        [ObservableProperty]
        private bool _steamAutoLaunch;

        public SettingsModel(IConfig config)
        {
            _config = config;
            
            if (_config.Count == 0)
            {
                // First launch
                _config.SetConfig("SteamExePath", null);
                _config.SetConfig("SteamAutoLaunch", true);
            }

            _steamExePath = _config.GetConfig<string>("SteamExePath");
            _steamAutoLaunch = _config.GetConfig<bool>("SteamAutoLaunch");
        }

        public void Save()
        {
            _config.SetConfig("SteamExePath", _steamExePath);
            _config.SetConfig("SteamAutoLaunch", _steamAutoLaunch);
        }
    }
}
