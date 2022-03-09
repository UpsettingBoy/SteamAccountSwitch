using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;

using SteamAccountSwitch.Interfaces;
using SteamAccountSwitch.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAccountSwitch.ViewModels
{
    public partial class SettingsViewModel: ObservableObject
    {
        [ObservableProperty]
        private SettingsModel _settings;
        [ObservableProperty]
        private bool _steamExeBrowseEnabled;

        public SettingsViewModel()
        {
            var container = ((App)Application.Current).Container;

            _settings = new SettingsModel(container.GetService<IConfig>()!);
            _steamExeBrowseEnabled = false;
        }
    }
}
